using Domain.Interfaces.IDespesa;
using Entities.Entidades;
using Infra.Repositorio.Generics;

namespace Infra.Repositorio;

public class RepositorioDespesas : RepositorioGenerico<Despesa>, InterfaceDespesa
{
    public Task<IList<Despesa>> ListaDespesasUsuario(string emailUsuario)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Despesa>> ListaDespesasUsuarioNaoPagasMesesAnteriores(string emailUsuario)
    {
        throw new NotImplementedException();
    }
}
