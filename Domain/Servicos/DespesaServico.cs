using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Entities.Enums;
using NPOI.SS.UserModel;
using System.Transactions;

namespace Domain.Servicos;

public class DespesaServico : IDespesaServico
{
    private readonly InterfaceDespesa _despesa;

    public DespesaServico(InterfaceDespesa despesa)
    {
        _despesa = despesa;
    }

    public async Task AdicionarDespesa(Despesa despesa)
    {
        DateTime data = DateTime.UtcNow;
        despesa.DataCadastro = data;
        despesa.Ano = data.Year;
        despesa.Mes = data.Month;

        var valido = despesa.ValidaString(despesa.Nome, "Nome");
        if (valido)
            await _despesa.Add(despesa);
    }

    public async Task AtualizarDespesa(Despesa despesa)
    {
        DateTime data = DateTime.UtcNow;
        despesa.DataAlteração = data;

        if (despesa.Pago)
            despesa.DataPagamento = data;

        var valido = despesa.ValidaString(despesa.Nome, "Nome");
        if (valido)
            await _despesa.Update(despesa);
    }

    public async Task<object> CarregaGraficos(string email)
    {
        var despesasUsuario = await _despesa.ListaDespesasUsuario(email);
        var despesaAnterior = await _despesa.ListaDespesasUsuarioNaoPagasMesesAnteriores(email);

        var despesasNaoPagasMesAnterior = despesaAnterior.Any() ?
            despesaAnterior.ToList().Sum(x => x.Valor) : 0;

        var despesasPagas = despesasUsuario.Where(d => d.Pago && d.TipoDespesa == EnumTipoDespesa.Contas)
            .Sum(x => x.Valor);

        var despesasPendentes = despesasUsuario.Where(d => !d.Pago && d.TipoDespesa == EnumTipoDespesa.Contas)
            .Sum(x => x.Valor);

        var investimentos = despesasUsuario.Where(d => d.TipoDespesa == EnumTipoDespesa.Investimentos)
            .Sum(x => x.Valor);

        return new
        {
            sucesso = "Ok",
            despesasPagas,
            despesasPendentes,
            despesasNaoPagasMesAnterior,
            investimentos
        };
    }

    public async Task ImportarDespeasExtratoBradescoCSV(StreamReader streamReader, int idCategoria)
    {
        int n;
        List<Despesa> despesasContas = new List<Despesa>();
        List<Despesa> despesasInvestimentos = new List<Despesa>();
        using (TransactionScope scope = new TransactionScope())
        {
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                var values = line?.Split(';');

                string doc = values[3];
                if (!string.IsNullOrEmpty(doc) && int.TryParse(doc, out n))
                {
                    Despesa despesa = new Despesa();
                    despesa.Ano = Convert.ToDateTime(values?[0]).Year;
                    despesa.Mes = Convert.ToDateTime(values?[0]).Month;
                    despesa.DataCadastro = DateTime.Now;
                    despesa.DataPagamento = Convert.ToDateTime(values?[0]);
                    despesa.DataVencimento = Convert.ToDateTime(values?[0]);
                    despesa.Nome = values?[1] + values?[2];
                    string credito = values[4];
                    string debito = values[5].Replace("-", "");
                    if (!string.IsNullOrEmpty(credito))
                    {
                        despesa.TipoDespesa = EnumTipoDespesa.Investimentos;
                        despesa.Valor = Convert.ToDecimal(credito);
                        despesa.Pago = true;
                        despesa.DespesaAtrasada = false;
                        despesa.IdCategoria = idCategoria;
                        despesasInvestimentos.Add(despesa);
                    }
                    else
                    if (!string.IsNullOrEmpty(debito))
                    {
                        despesa.TipoDespesa = EnumTipoDespesa.Contas;
                        despesa.Valor = Convert.ToDecimal(debito);
                        despesa.Pago = true;
                        despesa.DespesaAtrasada = false;
                        despesa.IdCategoria = idCategoria;
                        despesasContas.Add(despesa);
                    }
                }
            }
            await _despesa.AdicionarListaDespesas(despesasInvestimentos);
            await _despesa.AdicionarListaDespesas(despesasContas);
            scope.Complete();
        }
    }

    public async Task ImportarDespeasExtratoItauCSV(IWorkbook workbook, int idCategoria)
    {
        List<string[]> data = new List<string[]>();
        // obter a primeira planilha
        ISheet sheet = workbook.GetSheetAt(0);

        // percorrer as linhas e colunas da planilha
        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) continue;

            var rowData = new List<string>();
            for (int j = row.FirstCellNum; j <= row.LastCellNum; j++)
            {
                ICell cell = row.GetCell(j);
                if (cell == null) continue;

                // adicionar o valor da célula à lista de dados da linha atual
                rowData.Add(cell.ToString());
            }
            // adicionar a linha atual à lista de dados
            data.Add(rowData.ToArray());
        }

        List<Despesa> despesas = new List<Despesa>();
        await _despesa.AdicionarListaDespesas(despesas);
    }
}
