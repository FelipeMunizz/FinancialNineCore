using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;

namespace Domain.Servicos;

public class SistemaFinanceiroServico : ISistemaFinanceiroServico
{
    private readonly InterfaceSistemaFinanceiro _sFinanceiro;

    public SistemaFinanceiroServico(InterfaceSistemaFinanceiro sFinanceiro)
    {
        _sFinanceiro = sFinanceiro;
    }

    public async Task AdicionarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro)
    {
        bool valido = sistemaFinanceiro.ValidaString(sistemaFinanceiro.Nome, "Nome");

        if (valido)
        {
            var data = DateTime.Now;

            sistemaFinanceiro.DiaFechamento = 1;
            sistemaFinanceiro.Ano = data.Year;
            sistemaFinanceiro.Mes = data.Month;
            sistemaFinanceiro.AnoCopia = data.Year;
            sistemaFinanceiro.MesCopia = data.Month;
            sistemaFinanceiro.GerarCopiaDespesa = true;

            await _sFinanceiro.Add(sistemaFinanceiro);
        }
    }

    public async Task AtualizarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro)
    {
        bool valido = sistemaFinanceiro.ValidaString(sistemaFinanceiro.Nome, "Nome");

        if (valido)
        {
            sistemaFinanceiro.DiaFechamento = 1;
            await _sFinanceiro.Update(sistemaFinanceiro);
        }
    }
}
