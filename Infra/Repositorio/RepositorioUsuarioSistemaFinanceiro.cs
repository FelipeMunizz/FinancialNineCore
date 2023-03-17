using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Infra.Repositorio.Generics;

namespace Infra.Repositorio;

public class RepositorioUsuarioSistemaFinanceiro : RepositorioGenerico<UsuarioSistemaFinanceiro>, InterfaceUsuarioSistemaFinanceiro
{
    public Task<IList<UsuarioSistemaFinanceiro>> ListaUsuariosSistemasFinanceiro(int idSistema)
    {
        throw new NotImplementedException();
    }

    public Task<UsuarioSistemaFinanceiro> ObterUsuarioSistemaFinanceiro(string emailUsuario)
    {
        throw new NotImplementedException();
    }

    public Task RemoverUsuarios(List<UsuarioSistemaFinanceiro> usuarios)
    {
        throw new NotImplementedException();
    }
}