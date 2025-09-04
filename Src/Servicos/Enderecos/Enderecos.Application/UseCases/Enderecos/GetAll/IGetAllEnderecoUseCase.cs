using Aquiles.Communication.Responses.Enderecos;

namespace Enderecos.Application.UseCases.Enderecos.GetAll;
public interface IGetAllEnderecoUseCase
{
    public Task<IList<ResponseEnderecoJson>> Execute(Guid clienteId);
}
