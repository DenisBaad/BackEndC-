using Aquiles.Communication.Contracts;
using Aquiles.Communication.Requests.Clientes;
using Aquiles.Communication.Responses.Clientes;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Newtonsoft.Json;
using Confluent.Kafka;

namespace Aquiles.Application.UseCases.Clientes.Create;
public class CreateClienteUseCase : ICreateClienteUseCase
{
    private readonly IClienteWriteOnlyRepository _clienteWriteOnlyRepository;
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IProducer<Null, string> _producer;

    public CreateClienteUseCase(
        IClienteWriteOnlyRepository clienteWriteOnlyRepository,
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IMapper mapper, 
        IUnitOfWork unitOfWork,
        IUsuarioLogado usuarioLogado,
        IProducer<Null, string> producer)
    {
        _clienteWriteOnlyRepository = clienteWriteOnlyRepository;
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioLogado = usuarioLogado;
        _producer = producer;
    }

    public async Task<ResponseClientesJson> Execute(RequestCreateClientesJson request)
    {
        var usuario = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

        await Validate(request);
        var cliente = _mapper.Map<Cliente>(request);
        cliente.Id = Guid.NewGuid();
        cliente.UsuarioId = usuario;
        await _clienteWriteOnlyRepository.AddAsync(cliente);
        await _unitOfWork.CommitAsync();

        var evento = new ClienteEvent
        {
            ClienteId = cliente.Id,
            UsuarioId = cliente.UsuarioId,
            Endereco = request.Endereco
        };

        var message = new Message<Null, string>
        {
            Value = JsonConvert.SerializeObject(evento)
        };

        await _producer.ProduceAsync("clientes-criados", message);

        return _mapper.Map<ResponseClientesJson>(cliente);
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
