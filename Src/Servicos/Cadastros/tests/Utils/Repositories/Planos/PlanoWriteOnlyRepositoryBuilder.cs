using Aquiles.Domain.Repositories.Planos;
using Moq;

namespace CommonTestUtilities.Repositories.Planos;
public class PlanoWriteOnlyRepositoryBuilder
{
    public static IPlanoWriteOnlyRepository Build()
    {
        return new Mock<IPlanoWriteOnlyRepository>().Object;
    }
}
