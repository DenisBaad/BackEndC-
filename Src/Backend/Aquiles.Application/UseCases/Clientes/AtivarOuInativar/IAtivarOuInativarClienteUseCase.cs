namespace Aquiles.Application.UseCases.Clientes.AtivarOuInativar;
public interface IAtivarOuInativarClienteUseCase
{
    public Task Execute(Guid id);
}
