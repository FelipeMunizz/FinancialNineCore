using Entities.Entidades;
using NPOI.SS.UserModel;

namespace Domain.Interfaces.InterfaceServicos;

public interface IDespesaServico
{
    Task AdicionarDespesa(Despesa despesa);
    Task AtualizarDespesa(Despesa despesa);
    Task<object> CarregaGraficos(string email);
    Task ImportarDespeasExtratoBradescoCSV(StreamReader streamReader, int idCategoria);
    Task ImportarDespeasExtratoItauCSV(IWorkbook workbook, int idCategoria);
    Task ImportarNotaFiscal(string xml, int categoria);
}
