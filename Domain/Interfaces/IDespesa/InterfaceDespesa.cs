using Domain.Interfaces.Generics;
using Entities.Entidades;

namespace Domain.Interfaces.IDespesa;

public interface InterfaceDespesa : InterfaceGeneric<Despesa>
{
    Task<IList<Despesa>> ListaDespesasUsuario(string emailUsuario);
    Task<IList<Despesa>> ListaDespesasUsuarioNaoPagasMesesAnteriores(string emailUsuario);
    Task AdicionarListaDespesas(List<Despesa> despesas);
}