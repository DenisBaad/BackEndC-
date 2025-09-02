using Aquiles.Application.UseCases.Usuarios.Create;
using Aquiles.Exception;
using FluentAssertions;
using CommonTestUtilities.Requests;

namespace Validators.Test.Usuarios.Create;
public class UsuarioValidatorTest
{
    [Fact]
    public void Sucesso()
    {
        var request = RequestCreateUsuariosJsonBuilder.Build();
        
        var validator = new UsuarioValidator().Validate(request);

        validator.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Erro_Nome_Vazio()
    {
        var request = RequestCreateUsuariosJsonBuilder.Build();
        request.Nome = string.Empty;
        
        var validator = new UsuarioValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Erro_Email_Vazio()
    {
        var request = RequestCreateUsuariosJsonBuilder.Build();
        request.Email = string.Empty;

        var validator = new UsuarioValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Erro_Email_Invalido()
    {
        var request = RequestCreateUsuariosJsonBuilder.Build();
        request.Email = "email.com";

        var validator = new UsuarioValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Erro_Senha_Invalida(int senha)
    {
        var request = RequestCreateUsuariosJsonBuilder.Build(senha);
        
        var validator = new UsuarioValidator().Validate(request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_TAMANHO_INVALIDO));
    }
}
