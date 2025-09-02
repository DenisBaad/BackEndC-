using Enderecos.Domain.Repositories.Enderecos;
using Moq;

namespace CommonTestUtilities.Repositories.Enderecos;
public class EnderecoWriteOnlyRepositoryBuilder
{
    public static IEnderecoWriteOnlyRepository Build()
    {
        return new Mock<IEnderecoWriteOnlyRepository>().Object;
    }
}
