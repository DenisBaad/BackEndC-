namespace Aquiles.Application.UseCases.Clientes.Delete;
public interface IDeleteClienteUseCase
{
    public Task Execute(Guid id);
}
