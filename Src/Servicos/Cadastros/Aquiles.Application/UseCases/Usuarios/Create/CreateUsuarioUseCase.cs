using Aquiles.Communication.Requests.Usuarios;
using Aquiles.Communication.Responses.Usuarios;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories;
using Aquiles.Domain.Repositories.Usuarios;
using Aquiles.Exception;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.Services;
using AutoMapper;

namespace Aquiles.Application.UseCases.Usuarios.Create;
public class CreateUsuarioUseCase : ICreateUsuarioUseCase
{
    private readonly IUsuarioWriteOnlyRepository _usuarioWriteOnlyRepository;
    private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordEncrypt _passwordEncript;

    public CreateUsuarioUseCase(
        IUsuarioWriteOnlyRepository usuarioWriteOnlyRepository,
        IUsuarioReadOnlyRepository usuarioReadOnlyRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        PasswordEncrypt passwordEncript)
    {
        _usuarioWriteOnlyRepository = usuarioWriteOnlyRepository;
        _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _passwordEncript = passwordEncript;
    }

    public async Task<ResponseUsuariosJson> Execute(RequestCreateUsuariosJson request)
    {
        await Validate(request);
        var usuario = _mapper.Map<Usuario>(request);
        usuario.Senha = _passwordEncript.Encript(usuario.Senha);
        usuario.Id = Guid.NewGuid();
        await _usuarioWriteOnlyRepository.AddAsync(usuario);
        await _unitOfWork.CommitAsync();

        return new ResponseUsuariosJson
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }

    private async Task Validate(RequestCreateUsuariosJson request)
    {
        var result = new UsuarioValidator().Validate(request);
        var emailExist = await _usuarioReadOnlyRepository.ExistUserByEmail(request.Email);

        if (emailExist) 
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMensagensDeErro.EMAIL_USUARIO_JA_CADASTRADO));
        }

        if (!result.IsValid) 
        {
            var mensagensDeErro = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrorException(mensagensDeErro);
        }
    }
}
