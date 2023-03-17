using Domain.Interfaces.ICategoria;
using Entities.Entidades;
using Infra.Repositorio.Generics;

namespace Infra.Repositorio;

public class RepositorioCategoria : RepositorioGenerico<Categoria>, InterfaceCategoria
{

    public Task<IList<Categoria>> ListarCategoriasUsuario(string emailUsuario)
    {
        throw new NotImplementedException();
    }
}
