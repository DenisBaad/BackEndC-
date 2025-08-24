using Aquiles.Application.UseCases.Planos.Update;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception;
using Aquiles.Exception.AquilesException;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Planos;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Planos.Update;
public class UpdatePlanoUseCaseTest
{
    [Fact]
    public async Task Sucesso()
    {
        (var user, _) = UsuarioBuilder.Build();
        var plano = PlanoBuilder.Build(user.Id);
        var request = RequestCreatePlanoJsonBuilder.Build();
        var updateOnlyPlanoRepositoryBuilder = new PlanoUpdateOnlyRepositoryBuilder().GetById(plano.Id, plano).Update().Build();  
        var useCase = CreateUseCase(updateOnlyPlanoRepositoryBuilder);

        Func<Task> act = async () => await useCase.Execute(request, plano.Id);

        await act.Should().NotThrowAsync();
    }

    private UpdatePlanoUseCase CreateUseCase(IPlanoUpdateOnlyRepository updateOnlyPlanoRepository)
    {
        var autoMapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new UpdatePlanoUseCase(updateOnlyPlanoRepository, unitOfWork, autoMapper);
    }

    [Fact]
    public async Task Erro_Descricao_Vazia()
    {
        (var user, _) = UsuarioBuilder.Build();
        var plano = PlanoBuilder.Build(user.Id);
        var request = RequestCreatePlanoJsonBuilder.Build();
        request.Descricao = string.Empty;
        var updateOnlyPlanoRepositoryBuilder = new PlanoUpdateOnlyRepositoryBuilder().GetById(plano.Id, plano).Update().Build();
        var useCase = CreateUseCase(updateOnlyPlanoRepositoryBuilder);

        Func<Task> act = async () => await useCase.Execute(request, plano.Id);

        (await act.Should().ThrowAsync<ValidationErrorException>())
        .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceMensagensDeErro.DESCRICAO_OBRIGATORIA));
    }
}
