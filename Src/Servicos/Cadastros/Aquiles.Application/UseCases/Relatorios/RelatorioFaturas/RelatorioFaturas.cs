using Aquiles.Communication.Enums;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Domain.Repositories.Faturas;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Aquiles.Domain.Entities;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Utils.UsuarioLogado;

namespace Aquiles.Application.UseCases.Relatorios.RelatorioFaturas;

public class RelatorioFaturas : IRelatorioFaturas
{
    private readonly IFaturaReadOnlyRepository _faturaRepository;
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IPlanoReadOnlyRepository _planoReadOnlyRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public RelatorioFaturas(
        IFaturaReadOnlyRepository faturaRepository,
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IPlanoReadOnlyRepository planoReadOnlyRepository,
        IUsuarioLogado usuarioLogado)
    {
        _faturaRepository = faturaRepository;
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _usuarioLogado = usuarioLogado;
        _planoReadOnlyRepository = planoReadOnlyRepository;
    }

    public async Task<byte[]> Executar(
        string usuarioNome, 
        DateTime? dataAbertura, 
        DateTime? dataFechamento, 
        EnumStatusFatura? status, 
        List<string> clienteIds)
    {
        var usuario = await _usuarioLogado.GetUsuario();
        
        if (usuario == null)
            throw new System.Exception("Usuário não autenticado.");

        var faturas = await _faturaRepository.GetRelatorioFaturaPorCliente(
            usuario.Value, dataAbertura, dataFechamento, (int?)status, clienteIds);

        List<Guid> clienteIdsFiltrados;

        if (status.HasValue)
        {
            // Se está filtrando por status, pega só clientes que tem faturas naquele status
            clienteIdsFiltrados = faturas
                .Select(f => f.ClienteId)
                .Distinct()
                .ToList();
        }
        else
        {
            // Se não tem filtro de status, pega todos os clientes que vieram na lista inicial
            clienteIdsFiltrados = clienteIds.Select(id => Guid.Parse(id)).ToList();
        }

        var clientes = new List<Cliente>();
        
        foreach (var id in clienteIdsFiltrados)
        {
            var cliente = await _clienteReadOnlyRepository.GetById(id);
            if (cliente != null)
                clientes.Add(cliente);
        }

        // Buscar planos usados nas faturas
        var planoIds = faturas.Select(f => f.PlanoId).Distinct().ToList();
        var planos = new List<Plano>();
        
        foreach (var id in planoIds)
        {
            var plano = await _planoReadOnlyRepository.GetById(id);
            if (plano != null)
                planos.Add(plano);
        }
        var planoMap = planos.ToDictionary(p => p.Id, p => p);

        using (var stream = new MemoryStream())
        {
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            var fontPadrao = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var fontCabecalho = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            var fontCliente = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);

            document.Add(new Paragraph("Relatório de Faturas por Clientes", fontTitulo));
            document.Add(new Paragraph($"Usuário: {usuarioNome}", fontPadrao));
            document.Add(new Paragraph($"Período: {dataAbertura:dd/MM/yyyy} até {dataFechamento:dd/MM/yyyy}", fontPadrao));
            document.Add(new Paragraph("\n"));

            foreach (var cliente in clientes)
            {
                document.Add(new Paragraph($"Cliente: {cliente.Nome} - CPF/CNPJ: {cliente.CpfCnpj} - Contato: {cliente.Contato}", fontCliente));
                document.Add(new Paragraph("\n"));

                var faturasCliente = faturas.Where(f => f.ClienteId == cliente.Id).ToList();

                if (!faturasCliente.Any())
                {
                    document.Add(new Paragraph("Nenhuma fatura encontrada para este cliente.\n", fontPadrao));
                    continue;
                }

                var table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10, 10, 10, 25, 10, 10 });

                // Cabeçalhos das colunas 
                table.AddCell(new PdfPCell(new Phrase("Data Vencimento", fontCabecalho)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Valor Total", fontCabecalho)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Status", fontCabecalho)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Plano", fontCabecalho)) { HorizontalAlignment = Element.ALIGN_LEFT });  
                table.AddCell(new PdfPCell(new Phrase("Valor Plano", fontCabecalho)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Qtd. Usuários", fontCabecalho)) { HorizontalAlignment = Element.ALIGN_CENTER });

                foreach (var fatura in faturasCliente)
                {
                    if (!planoMap.TryGetValue(fatura.PlanoId, out var plano))
                        plano = null;

                    var planoDescricao = plano?.Descricao ?? "-";
                    var valorPlano = plano != null ? plano.ValorPlano.ToString("C") : "-";
                    var quantidadeUsuarios = plano != null ? plano.QuantidadeUsuarios.ToString() : "-";

                    table.AddCell(new PdfPCell(new Phrase(fatura.DataVencimento.ToString("dd/MM/yyyy"), fontPadrao)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(fatura.ValorTotal.ToString("C"), fontPadrao)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(fatura.Status.ToString(), fontPadrao)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(planoDescricao, fontPadrao)) { HorizontalAlignment = Element.ALIGN_LEFT });  
                    table.AddCell(new PdfPCell(new Phrase(valorPlano, fontPadrao)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(quantidadeUsuarios, fontPadrao)) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                document.Add(table);
                document.Add(new Paragraph("\n"));
            }

            document.Close();
            return stream.ToArray();
        }
    }
}
