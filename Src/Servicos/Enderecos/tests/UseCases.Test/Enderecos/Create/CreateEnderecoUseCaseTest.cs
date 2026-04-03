using Aquiles.Exception;
using Aquiles.Exception.AquilesException;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Enderecos;
using CommonTestUtilities.Requests;
using Enderecos.Application.UseCases.Enderecos.Create;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace UseCases.Test.Enderecos.Create;
public class CreateEnderecoUseCaseTest
{
    [Fact]
    public async Task Sucesso()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        useCase.Should().NotBeNull();
    }

    [Fact]
    public async Task Erro_Logradouro_Vazio()
    {
        var request = RequestCreateEnderecoJsonBuilder.Build();
        request.Logradouro = string.Empty;
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ValidationErrorException>())
            .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceMensagensDeErro.LOGRADOURO_OBRIGATORIO));
    }

    private CreateEnderecoUseCase CreateUseCase()
    {
        var autoMapper = MapperBuilder.Build();
        var writeOnlyRepository = EnderecoWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var logger = NullLogger<CreateEnderecoUseCase>.Instance; 

        return new CreateEnderecoUseCase(autoMapper, writeOnlyRepository, unitOfWork, logger);
    }
}
