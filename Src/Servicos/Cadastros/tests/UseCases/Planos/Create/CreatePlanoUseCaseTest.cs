using Aquiles.Application.UseCases.Planos.Create;
using Aquiles.Exception;
using Aquiles.Exception.AquilesException;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Planos;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Planos.Create;
public class CreatePlanoUseCaseTest
{
    [Fact]
    public async Task Sucesso()
    {
        var request = RequestCreatePlanoJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);
        
        useCase.Should().NotBeNull();
        result.Descricao.Should().Be(request.Descricao);
    }

    private CreatePlanoUseCase CreateUseCase()
    {
        var autoMapper = MapperBuilder.Build();
        var writeOnlyRepository = PlanoWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var usuarioLogado = UsuarioLogadoBuilder.Build();

        return new CreatePlanoUseCase(autoMapper, writeOnlyRepository, unitOfWork, usuarioLogado);
    }

    [Fact]
    public async Task Erro_Descricao_Vazia()
    {
        var request = RequestCreatePlanoJsonBuilder.Build();
        request.Descricao = string.Empty;
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ValidationErrorException>())
        .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceMensagensDeErro.DESCRICAO_OBRIGATORIA));
    }
}
