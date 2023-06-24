using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;

namespace Domain.Servicos;

public class UsuarioSistemaFinanceiroServico : IUsuarioSistemaFinanceiroServico
{
    private readonly InterfaceUsuarioSistemaFinanceiro _uSistemaFinanceiro;

    public UsuarioSistemaFinanceiroServico(InterfaceUsuarioSistemaFinanceiro uSistemaFinanceiro)
    {
        _uSistemaFinanceiro = uSistemaFinanceiro;
    }

    public async Task CadastraUsuarioSistema(UsuarioSistemaFinanceiro usuarioSistemaFinanceiro)
    {
        await _uSistemaFinanceiro.Add(usuarioSistemaFinanceiro);
    }
}
