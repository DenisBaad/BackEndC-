using Aquiles.Communication.Requests.Usuarios;
using Aquiles.Exception;
using FluentValidation;

namespace Aquiles.Application.UseCases.Usuarios.Create;
public class UsuarioValidator : AbstractValidator<RequestCreateUsuariosJson>
{
    public UsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO);
        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO);
        });

        RuleFor(x => x.Senha.Length)
            .GreaterThanOrEqualTo(6).WithMessage(ResourceMensagensDeErro.SENHA_USUARIO_TAMANHO_INVALIDO);
    }
}
