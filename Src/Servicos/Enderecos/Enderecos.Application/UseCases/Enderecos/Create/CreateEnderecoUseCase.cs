using Aquiles.Communication.Requests.Enderecos;
using Aquiles.Communication.Responses.Enderecos;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Enderecos.Domain.Entities;
using Enderecos.Domain.Repositories;
using Enderecos.Domain.Repositories.Enderecos;

namespace Enderecos.Application.UseCases.Enderecos.Create;
public class CreateEnderecoUseCase : ICreateEnderecoUseCase
{
    private readonly IMapper _mapper;
    private readonly IEnderecoWriteOnlyRepository _enderecoWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioLogado _usuarioLogado;

    public CreateEnderecoUseCase(
        IMapper mapper,
        IEnderecoWriteOnlyRepository enderecoWriteRepository,
        IUnitOfWork unitOfWork,
        IUsuarioLogado usuarioLogado)
    {
        _enderecoWriteRepository = enderecoWriteRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ResponseEnderecoJson> Execute(RequestEnderecoJson request)
    {
        var usuario = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

        var endereco = _mapper.Map<Endereco>(request);
        endereco.Id = Guid.NewGuid();
        endereco.UsuarioId = usuario;
        await _enderecoWriteRepository.Create(endereco);
        await _unitOfWork.CommitAsync();

        return new ResponseEnderecoJson()
        {
            Id = endereco.Id,
        };
    }
}
