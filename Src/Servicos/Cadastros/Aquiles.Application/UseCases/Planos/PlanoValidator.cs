using Aquiles.Communication.Requests.Planos;
using Aquiles.Exception;
using FluentValidation;

namespace Aquiles.Application.UseCases.Planos;
public class PlanoValidator : AbstractValidator<RequestCreatePlanoJson>
{
    public PlanoValidator()
    {
        RuleFor(plano => plano.Descricao)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.DESCRICAO_OBRIGATORIA);
        
        RuleFor(plano => plano.ValorPlano)
            .GreaterThan(0).WithMessage(ResourceMensagensDeErro.VALOR_PLANO_NEGATIVO);
        
        RuleFor(plano => plano.QuantidadeUsuarios)
            .GreaterThan(0).WithMessage(ResourceMensagensDeErro.QUANTIDADE_USUARIO_NEGATIVO);
        
        RuleFor(plano => plano.VigenciaMeses)
            .GreaterThan(0).WithMessage(ResourceMensagensDeErro.VIGENCIA_MES_NEGATIVA);
    }
}
