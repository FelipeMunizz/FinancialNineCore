using Domain.Interfaces.Generics;
using Entities.Entidades;
using System.Globalization;

namespace Domain.Interfaces.IUsuarioSistemaFinanceiro;

public interface InterfaceUsuarioSistemaFinanceiro : InterfaceGeneric<UsuarioSistemaFinanceiro>
{
    Task<IList<UsuarioSistemaFinanceiro>> ListaUsuariosSistemasFinanceiro(int idSistema);
    Task RemoverUsuarios(List<UsuarioSistemaFinanceiro> usuarios);
    Task<UsuarioSistemaFinanceiro> ObterUsuarioSistemaFinanceiro(string emailUsuario);
}