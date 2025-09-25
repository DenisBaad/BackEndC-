using Aquiles.Communication.Requests.Clientes;
using Aquiles.Communication.Responses.Clientes;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace Aquiles.Application.UseCases.Clientes.Create;
public class CreateClienteUseCase : ICreateClienteUseCase
{
    private readonly IClienteWriteOnlyRepository _clienteWriteOnlyRepository;
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<CreateClienteUseCase> _logger;

    public CreateClienteUseCase(
        IClienteWriteOnlyRepository clienteWriteOnlyRepository,
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IMapper mapper, 
        IUnitOfWork unitOfWork,
        IUsuarioLogado usuarioLogado,
        IProducer<Null, string> producer,
        ILogger<CreateClienteUseCase> logger)
    {
        _clienteWriteOnlyRepository = clienteWriteOnlyRepository;
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioLogado = usuarioLogado;
        _producer = producer;
        _logger = logger;
    }

    public async Task<ResponseClientesJson> Execute(RequestCreateClientesJson request)
    {
        try
        {
            await Validate(request);
            var cliente = _mapper.Map<Cliente>(request);
            cliente.Id = Guid.NewGuid();
            cliente.UsuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");
            await _clienteWriteOnlyRepository.AddAsync(cliente);
            await _unitOfWork.CommitAsync();
            request.Endereco.ClienteId = cliente.Id;
            
            var message = new Message<Null, string>{ Value = JsonConvert.SerializeObject(request.Endereco) };
            await _producer.ProduceAsync("clientes-criados", message);

            return _mapper.Map<ResponseClientesJson>(cliente);
        }
        catch (System.Exception ex)
        {
            var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogError(ex, "Erro ao criar cliente com request: {request}", jsonRequest);
            throw;
        }
    }

    private async Task Validate(RequestCreateClientesJson request)
    {
        var result = new ClienteValidator().Validate(request);
        var codeExist = await _clienteReadOnlyRepository.ExistClienteWithCode(request.Codigo);

        if (codeExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("código", "Cóodigo do cliente já cadastrado na base de dados"));
        }

        if (!result.IsValid)
        {
            var mensagensDeErro = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrorException(mensagensDeErro);
        }
    }
}
