using Domain.Interfaces.ILancamento;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Entities.Enums;
using Integration.Enel;
using NPOI.SS.UserModel;
using System.Transactions;
using System.Xml;

namespace Domain.Servicos;

public class LancamentoServico : ILancamentoServico
{
    private readonly InterfaceLancamento _lancamentos;

    public LancamentoServico(InterfaceLancamento lancamentos)
    {
        _lancamentos = lancamentos;
    }

    public async Task AdicionarLancamento(Lancamento lancamento)
    {
        DateTime data = DateTime.UtcNow;
        lancamento.DataCadastro = data;
        lancamento.Ano = data.Year;
        lancamento.Mes = data.Month;

        var valido = lancamento.ValidaString(lancamento.Nome, "Nome");
        if (valido)
            await _lancamentos.Add(lancamento);
    }

    public async Task AtualizarLancamento(Lancamento lancamento)
    {
        DateTime data = DateTime.UtcNow;
        lancamento.DataAlteração = data;

        if (lancamento.Pago)
            lancamento.DataPagamento = data;

        var valido = lancamento.ValidaString(lancamento.Nome, "Nome");
        if (valido)
            await _lancamentos.Update(lancamento);
    }

    public async Task<object> CarregaGraficos(string email)
    {
        var lancamentosUsuario = await _lancamentos.ListaLancamentosUsuario(email);
        var lancamentoAnterior = await _lancamentos.ListaLancamentosUsuarioNaoPagasMesesAnteriores(email);

        var despesasNaoPagasMesAnterior = lancamentoAnterior.Any() ?
            lancamentoAnterior.ToList().Sum(x => x.Valor) : 0;

        var despesasPagas = lancamentosUsuario.Where(d => d.Pago && d.TipoLancamento == EnumTipoLancamento.Despesa)
            .Sum(x => x.Valor);

        var despesasPendentes = lancamentosUsuario.Where(d => !d.Pago && d.TipoLancamento == EnumTipoLancamento.Despesa)
            .Sum(x => x.Valor);

        var receitas = lancamentosUsuario.Where(d => d.TipoLancamento == EnumTipoLancamento.Receita)
            .Sum(x => x.Valor);

        return new
        {
            sucesso = "Ok",
            despesasPagas,
            despesasPendentes,
            despesasNaoPagasMesAnterior,
            receitas
        };
    }

    public async Task ImportarContaLancamento(DadosEnel dadosEnel)
    {
        Enelintegracao enel = new Enelintegracao();
        await enel.ImportarContaLacamento(dadosEnel);
        return;
    }

    public async Task ImportarLancamentosExtratoBradescoCSV(StreamReader streamReader, int idCategoria)
    {
        int n;
        List<Lancamento> lancamentosDespesas = new List<Lancamento>();
        List<Lancamento> lancamentosReceitas = new List<Lancamento>();
        using (TransactionScope scope = new TransactionScope())
        {
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                var values = line?.Split(';');

                string doc = values[3];
                if (!string.IsNullOrEmpty(doc) && int.TryParse(doc, out n))
                {
                    Lancamento despesa = new Lancamento();
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
                        despesa.TipoLancamento = EnumTipoLancamento.Receita;
                        despesa.Valor = Convert.ToDecimal(credito);
                        despesa.Pago = true;
                        despesa.DespesaAtrasada = false;
                        despesa.IdCategoria = idCategoria;
                        lancamentosReceitas.Add(despesa);
                    }
                    else
                    if (!string.IsNullOrEmpty(debito))
                    {
                        despesa.TipoLancamento = EnumTipoLancamento.Despesa;
                        despesa.Valor = Convert.ToDecimal(debito);
                        despesa.Pago = true;
                        despesa.DespesaAtrasada = false;
                        despesa.IdCategoria = idCategoria;
                        lancamentosDespesas.Add(despesa);
                    }
                }
            }
            await _lancamentos.AdicionarListaLancamentos(lancamentosReceitas);
            await _lancamentos.AdicionarListaLancamentos(lancamentosDespesas);
            scope.Complete();
        }
    }

    public async Task ImportarLancamentosExtratoItauCSV(IWorkbook workbook, int idCategoria)
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

        List<Lancamento> lancamentos = new List<Lancamento>();
        await _lancamentos.AdicionarListaLancamentos(lancamentos);
    }

    public async Task ImportarNotaFiscal(string xmlContent, int categoria)
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlContent);

        Lancamento lancamento = new Lancamento
        {
            IdCategoria = categoria,
            Nome = xml.GetElementsByTagName("xProd")?.Item(0)?.InnerText,
            TipoLancamento = EnumTipoLancamento.Despesa,
            DataCadastro = DateTime.Now,
            DataPagamento = Convert.ToDateTime(xml.GetElementsByTagName("dhEmi")?.Item(0)?.InnerText),
            DataVencimento = Convert.ToDateTime(xml.GetElementsByTagName("dhEmi")?.Item(0)?.InnerText),
            Ano = Convert.ToDateTime(xml.GetElementsByTagName("dhEmi")?.Item(0)?.InnerText).Year,
            Mes = Convert.ToDateTime(xml.GetElementsByTagName("dhEmi")?.Item(0)?.InnerText).Month,
            Pago = true,
            DespesaAtrasada = false,
            Valor = Convert.ToDecimal(xml.GetElementsByTagName("vNF")?.Item(0)?.InnerText.Replace(".", ","))
        };


        await _lancamentos.Add(lancamento);
    }
}
