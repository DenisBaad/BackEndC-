using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories.Planos;
using Moq;

namespace CommonTestUtilities.Repositories.Planos;
public class PlanoUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IPlanoUpdateOnlyRepository> _repository;

    public PlanoUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IPlanoUpdateOnlyRepository>();
    }

    public IPlanoUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }

    public PlanoUpdateOnlyRepositoryBuilder GetById(Guid id, Plano plano)
    {
        _repository.Setup(repository => repository.GetById(id)).ReturnsAsync(plano);
        return this;
    }

    public PlanoUpdateOnlyRepositoryBuilder Update()
    {
        _repository.Setup(repository => repository.Update(It.IsAny<Plano>()));
        return this;
    }
}
