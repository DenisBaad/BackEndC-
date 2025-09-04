using Enderecos.Domain.Entities;

namespace Enderecos.Domain.Repositories.Enderecos;
public interface IEnderecoReadOnlyRepository
{
    public Task<IList<Endereco>> GetAll(Guid clienteId);
}
