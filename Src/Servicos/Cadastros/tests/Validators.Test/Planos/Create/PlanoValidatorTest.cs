using Aquiles.Application.UseCases.Planos;
using Aquiles.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Planos.Create;
public class PlanoValidatorTest
{
    [Fact]
    public void Sucesso()
    {
        var request = RequestCreatePlanoJsonBuilder.Build();
        
        var validator = new PlanoValidator().Validate(request);

        validator.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Erro_Descricao_Vazia()
    {
        var request = RequestCreatePlanoJsonBuilder.Build();
        request.Descricao = string.Empty;
        
        var validator = new PlanoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.DESCRICAO_OBRIGATORIA));
    }

    [Fact]
    public void Erro_Valor_Plano_Negativo()
    {
        var request = RequestCreatePlanoJsonBuilder.Build();
        request.ValorPlano = 0;

        var validator = new PlanoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.VALOR_PLANO_NEGATIVO));
    }

    [Fact]
    public void Erro_Quantidade_Usuarios_Negativo()
    {
        var request = RequestCreatePlanoJsonBuilder.Build();
        request.QuantidadeUsuarios = 0;

        var validator = new PlanoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.QUANTIDADE_USUARIO_NEGATIVO));
    }

    [Fact]
    public void Erro_Vigencia_Meses_Negativo()
    {
        var request = RequestCreatePlanoJsonBuilder.Build();
        request.VigenciaMeses = 0;

        var validator = new PlanoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.VIGENCIA_MES_NEGATIVA));
    }
}
