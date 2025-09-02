using Aquiles.Application.UseCases.Usuarios.Create;
using Aquiles.Exception;
using Aquiles.Exception.AquilesException;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Usuarios;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Usuarios.Create;
public class CreateUsuarioUseCaseTest
{
    [Fact]
    public async Task Sucesso()
    {
        var request = RequestCreateUsuariosJsonBuilder.Build();
        var useCase = CreateUseCase();
        
        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Nome.Should().Be(request.Nome);
        result.Email.Should().Be(request.Email);
    }

    private CreateUsuarioUseCase CreateUseCase(string? email = null)
    {
        var usuarioWriteOnlyRepository = UsuarioWriteOnlyRepositoryBuilder.Build();
        var usuarioReadOnlyRepositoryBuilder = new UsuarioReadOnlyRepositoryBuilder();
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncryptBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (string.IsNullOrEmpty(email) == false)
        {
            usuarioReadOnlyRepositoryBuilder.ExistUserByEmail(email);
        }

        return new CreateUsuarioUseCase(usuarioWriteOnlyRepository, usuarioReadOnlyRepositoryBuilder.Build(), mapper, unitOfWork, passwordEncripter);
    }

    [Fact]
    public async Task Erro_Email_Ja_Registrado()
    {
        var request = RequestCreateUsuariosJsonBuilder.Build();
        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ValidationErrorException>())
        .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceMensagensDeErro.EMAIL_USUARIO_JA_CADASTRADO));
    }

    [Fact]
    public async Task Erro_Nome_Vazia()
    {
        var request = RequestCreateUsuariosJsonBuilder.Build();
        request.Nome = string.Empty;
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ValidationErrorException>())
        .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO));
    }
}
