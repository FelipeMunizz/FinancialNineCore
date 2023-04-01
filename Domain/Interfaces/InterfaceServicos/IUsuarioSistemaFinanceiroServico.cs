using Entities.Entidades;

namespace Domain.Interfaces.InterfaceServicos;

public interface IUsuarioSistemaFinanceiroServico
{
    Task CadastraUsuarioSistema(UsuarioSistemaFinanceiro usuarioSistemaFinanceiro);
}
