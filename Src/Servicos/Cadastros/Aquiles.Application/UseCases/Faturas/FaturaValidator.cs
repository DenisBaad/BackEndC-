using Aquiles.Communication.Requests.Faturas;
using Aquiles.Exception;
using FluentValidation;

namespace Aquiles.Application.UseCases.Faturas;
public class FaturaValidator : AbstractValidator<RequestCreateFaturaJson>
{
    public FaturaValidator()
    {
        RuleFor(fatura => fatura.Status)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.STATUS_OBRIGATORIO);

        RuleFor(fatura => fatura.InicioVigencia)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.FATURA_INICIO_VIGENCIA_OBRIGATORIA);

        RuleFor(fatura => fatura.FimVigencia)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.FATURA_FIM_VIGENCIA_OBRIGATORIA);

        RuleFor(fatura => fatura.ValorTotal)
            .GreaterThanOrEqualTo(0).WithMessage(ResourceMensagensDeErro.FATURA_NEGATIVA);
        
        RuleFor(fatura => fatura.DataVencimento)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage(ResourceMensagensDeErro.FATURA_VENCIMENTO);
    }
}


