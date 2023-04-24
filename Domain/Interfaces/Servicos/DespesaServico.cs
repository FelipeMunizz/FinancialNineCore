using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Entities.Enums;

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
}
