using Aquiles.Exception;
using CommonTestUtilities.Requests;
using Enderecos.Application.UseCases.Enderecos;
using FluentAssertions;

namespace Validators.Test.Enderecos.Create;
public class EnderecoValidatorTest
{
    [Fact]
    public void Sucesso()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();

        var validator = new EnderecoValidator().Validate(request);

        validator.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Erro_Cep_Vazio()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        request.Cep = string.Empty;

        var validator = new EnderecoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.CEP_OBRIGATORIO));
    }

    [Fact]
    public void Erro_UF_Vazio()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        request.UF = string.Empty;

        var validator = new EnderecoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.ESTADO_OBRIGATORIO));
    }

    [Fact]
    public void Erro_Municipio_Vazio()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        request.Municipio = string.Empty;

        var validator = new EnderecoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.MUNICIPIO_OBRIGATORIO));
    }

    [Fact]
    public void Erro_Bairro_Vazio()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        request.Bairro = string.Empty;

        var validator = new EnderecoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.BAIRRO_OBRIGATORIO));
    }

    [Fact]
    public void Erro_Logradouro_Vazio()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        request.Logradouro = string.Empty;

        var validator = new EnderecoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.LOGRADOURO_OBRIGATORIO));
    }

    [Fact]
    public void Erro_Numero_Vazio()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        request.Numero = string.Empty;

        var validator = new EnderecoValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.NUMERO_OBRIGATORIO));
    }
}

