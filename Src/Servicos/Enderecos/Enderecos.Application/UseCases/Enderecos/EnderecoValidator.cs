using Aquiles.Communication.Requests.Enderecos;
using Aquiles.Exception;
using FluentValidation;

namespace Enderecos.Application.UseCases.Enderecos;
public class EnderecoValidator : AbstractValidator<RequestEnderecoJson>
{
    public EnderecoValidator()
    {
        RuleFor(e => e.Cep)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.CEP_OBRIGATORIO);

        RuleFor(e => e.UF)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.ESTADO_OBRIGATORIO);

        RuleFor(e => e.Municipio)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.MUNICIPIO_OBRIGATORIO);

        RuleFor(e => e.Bairro)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.BAIRRO_OBRIGATORIO);

        RuleFor(e => e.Logradouro)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.LOGRADOURO_OBRIGATORIO);

        RuleFor(e => e.Numero)
            .NotEmpty().WithMessage(ResourceMensagensDeErro.NUMERO_OBRIGATORIO);
    }
}
