using Entities.Entidades;

namespace Domain.Interfaces.InterfaceServicos;

public interface IDespesaServico
{
    Task AdicionarDespesa(Despesa despesa);
    Task AutalizarDespesa(Despesa despesa);
}
