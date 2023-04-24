using Entities.Entidades;

namespace Domain.Interfaces.InterfaceServicos;

public interface IDespesaServico
{
    Task AdicionarDespesa(Despesa despesa);
    Task AtualizarDespesa(Despesa despesa);
    Task<object> CarregaGraficos(string email);
}
