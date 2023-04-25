using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Entities.Enums;
using System.Transactions;

namespace Domain.Interfaces.Servicos;

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
            despesasPagas = despesasPagas,
            despesasPendentes = despesasPendentes,
            despesasNaoPagasMesAnterior = despesasNaoPagasMesAnterior,
            investimentos = investimentos
        };
    }

    public async Task ImportarDespeasExtratoCSV(StreamReader streamReader, int idCategoria)
    {
        int n;
        List<Despesa> despesas = new List<Despesa>();
        using (TransactionScope scope = new TransactionScope())
        {
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                var values = line?.Split(';');

                string doc = values[2];
                if (!string.IsNullOrEmpty(doc) && int.TryParse(doc, out n))
                {
                    Despesa despesa = new Despesa();
                    despesa.Ano = Convert.ToDateTime(values?[0]).Year;
                    despesa.Mes = Convert.ToDateTime(values?[0]).Month;
                    despesa.DataCadastro = DateTime.Now;
                    despesa.DataPagamento = Convert.ToDateTime(values?[0]);
                    despesa.DataVencimento = Convert.ToDateTime(values?[0]);
                    despesa.Nome = values?[1];
                    string credito = values[3];
                    string debito = values[4].Replace("-", "");
                    if (!string.IsNullOrEmpty(credito))
                    {
                        despesa.TipoDespesa = EnumTipoDespesa.Investimentos;                        
                        despesa.Valor = Convert.ToDecimal(credito);
                    }
                    else
                    if (!string.IsNullOrEmpty(debito))
                    {
                        despesa.TipoDespesa = EnumTipoDespesa.Contas;
                        despesa.Valor = Convert.ToDecimal(debito);
                    }
                    despesa.Pago = true;
                    despesa.DespesaAtrasada = false;
                    despesa.IdCategoria = idCategoria;

                    despesas.Add(despesa);
                }
            }
            await _despesa.AdicionarListaDespesas(despesas);
            scope.Complete();
        }

    }
}
