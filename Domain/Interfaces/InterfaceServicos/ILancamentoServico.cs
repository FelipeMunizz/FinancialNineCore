using Entities.Entidades;
using NPOI.SS.UserModel;

namespace Domain.Interfaces.InterfaceServicos;

public interface ILancamentoServico
{
    Task AdicionarLancamento(Lancamento despesa);
    Task AtualizarLancamento(Lancamento despesa);
    Task<object> CarregaGraficos(string email);
    Task ImportarLancamentosExtratoBradescoCSV(StreamReader streamReader, int idCategoria);
    Task ImportarLancamentosExtratoItauCSV(IWorkbook workbook, int idCategoria);
    Task ImportarNotaFiscal(string xml, int categoria);
}
