namespace Enderecos.Domain.Repositories;
public interface IUnitOfWork
{
    public Task CommitAsync();
}