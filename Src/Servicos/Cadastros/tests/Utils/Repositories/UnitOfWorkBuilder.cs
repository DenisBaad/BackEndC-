using Aquiles.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        return new Mock<IUnitOfWork>().Object;
    }
}
