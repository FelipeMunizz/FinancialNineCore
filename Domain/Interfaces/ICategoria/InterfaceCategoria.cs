using Domain.Interfaces.Generics;
using Entities.Entidades;

namespace Domain.Interfaces.ICategoria;

public interface InterfaceCategoria : InterfaceGeneric<Categoria>
{
    Task<IList<Categoria>> ListarCategoriasUsuario(string emailUsuario, int idSistema);
}