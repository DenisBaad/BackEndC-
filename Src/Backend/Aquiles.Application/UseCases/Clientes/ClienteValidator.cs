using Aquiles.Application.Helpers;
using Aquiles.Communication.Requests.Clientes;
using Aquiles.Exception;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Aquiles.Application.UseCases.Clientes;
public class ClienteValidator : AbstractValidator<RequestCreateClientesJson>
{
    public ClienteValidator()
    {
        When(p => !string.IsNullOrEmpty(p.CpfCnpj), () =>
        {
            RuleFor(p => p.CpfCnpj)
           .Length(11, 14)
           .WithMessage(ResourceMensagensDeErro.CNPJCPF_TAMANHO_INVALIDO);

            RuleFor(p => p.CpfCnpj).Custom((cpfCnpj, context) =>
            {
                if (cpfCnpj.Length == 11 && !ValidaCPF.IsCpf(cpfCnpj))
                    context.AddFailure(new ValidationFailure(nameof(cpfCnpj), ResourceMensagensDeErro.CPF_INVALIDO));
                else if (cpfCnpj.Length == 14 && !ValidaCNPJ.IsCnpj(cpfCnpj))
                    context.AddFailure(new ValidationFailure(nameof(cpfCnpj), ResourceMensagensDeErro.CNPJ_INVALIDO));
            });
        });

        RuleFor(p => p.Nome)
            .NotEmpty()
            .WithMessage(ResourceMensagensDeErro.NOME_CLIENTE_VAZIO);

        When(p => !string.IsNullOrEmpty(p.Nome), () =>
        {
            RuleFor(p => p.Nome)
                .Length(3, 45)
                .WithMessage(ResourceMensagensDeErro.NOME_CLIENTE_TAMANHO_INVALIDO);
        });

        When(p => !string.IsNullOrEmpty(p.Identidade), () =>
        {
            RuleFor(p => p.Identidade).Custom((identidade, context) =>
            {
                if (Regex.IsMatch(identidade, @"[^0-9\s]", RegexOptions.None, TimeSpan.FromSeconds(5)))
                {
                    context.AddFailure(new ValidationFailure(nameof(identidade), ResourceMensagensDeErro.IDENTIDADE_CARACTERES_ESPECIAIS));
                }
            });

            RuleFor(p => p.Identidade)
                .Length(6, 20)
                .WithMessage(ResourceMensagensDeErro.IDENTIDADE_TAMANHO_INVALIDO);

            RuleFor(p => p.OrgaoExpedidor)
                .NotEmpty()
                .WithMessage(ResourceMensagensDeErro.ORGAO_EXPEDIDOR_VAZIO);

            When(p => !string.IsNullOrEmpty(p.OrgaoExpedidor), () =>
            {
                RuleFor(p => p.OrgaoExpedidor).Custom((orgao, context) =>
                {
                    if (Regex.IsMatch(orgao, @"[^a-zA-Z0-9\s]", RegexOptions.None, TimeSpan.FromSeconds(5)))
                    {
                        context.AddFailure(new ValidationFailure(nameof(orgao), ResourceMensagensDeErro.ORGAO_EXPEDIDOR_CARACTERES_ESPECIAIS));
                    }
                });

                RuleFor(p => p.OrgaoExpedidor)
                    .Length(1, 10)
                    .WithMessage(ResourceMensagensDeErro.ORGAO_EXPEDIDOR_TAMANHO_INVALIDO);
            });
        });

        RuleFor(p => p.Tipo)
            .IsInEnum()
            .WithMessage(ResourceMensagensDeErro.TIPO_DEVE_SER_INFORMADO);

        When(p => !string.IsNullOrEmpty(p.NomeFantasia), () =>
        {
            RuleFor(p => p.NomeFantasia).Custom((fantasia, context) =>
            {
                if (Regex.IsMatch(fantasia, @"[^a-zA-Z0-9\s]", RegexOptions.None, TimeSpan.FromSeconds(5)))
                {
                    context.AddFailure(new ValidationFailure(nameof(fantasia), ResourceMensagensDeErro.NOME_FANTASIA_CARACTERES_ESPECIAIS));
                }
            });

            RuleFor(p => p.NomeFantasia)
                .Length(1, 45)
                .WithMessage(ResourceMensagensDeErro.NOME_FANTASIA_TAMANHO_INVALIDO);
        });
    }
}
