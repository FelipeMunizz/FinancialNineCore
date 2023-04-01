using Entities.Entidades;

namespace Domain.Interfaces.InterfaceServicos;

public interface ISistemaFinanceiroServico
{
    Task AdicionarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro);
    Task AtualizarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro);
}
