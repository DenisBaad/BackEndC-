using Enderecos.Domain.Entities;

namespace Enderecos.Domain.Repositories.Enderecos;
public interface IEnderecoWriteOnlyRepository
{
    public Task Create(Endereco endereco);
}
