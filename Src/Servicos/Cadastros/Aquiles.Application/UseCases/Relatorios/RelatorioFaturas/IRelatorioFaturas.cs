using Aquiles.Communication.Enums;

namespace Aquiles.Application.UseCases.Relatorios.RelatorioFaturas;
public interface IRelatorioFaturas
{
    public Task<byte[]> Executar(string usuarioNome, DateTime? dataAbertura, DateTime? dataFechamento, EnumStatusFatura? status, List<string> clienteId);
}
