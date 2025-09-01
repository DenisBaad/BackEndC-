using Aquiles.Communication.Requests.Enderecos;
using Aquiles.Communication.Responses.Enderecos;

namespace Enderecos.Application.UseCases.Enderecos.Create;
public interface ICreateEnderecoUseCase
{
    public Task<ResponseEnderecoJson> Execute(RequestEnderecoJson request);
}
