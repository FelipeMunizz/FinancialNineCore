using Domain.Interfaces.Generics;
using Entities.Entidades;

namespace Domain.Interfaces.ILancamento;

public interface InterfaceLancamento : InterfaceGeneric<Lancamento>
{
    Task<IList<Lancamento>> ListaLancamentosUsuario(string emailUsuario);
    Task<IList<Lancamento>> ListaLancamentosUsuarioNaoPagasMesesAnteriores(string emailUsuario);
    Task AdicionarListaLancamentos(List<Lancamento> lancamentos);
}