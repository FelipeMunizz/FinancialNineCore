using Domain.Interfaces.InterfaceServicos;
using Domain.Servicos;
using Entities.Entidades;

namespace TestIntegration;

public class EnelTest
{

    [Fact]
    public async Task TestImportarContaLancamento()
    {
        DadosEnel dados = new DadosEnel
        {
            NumeroInstalacao = 1,
            Login = "barreto-veronica@bol.com.br",
            Senha = "Veve2023@sofia"
        };

        await new LancamentoServico().ImportarContaLancamento(dados);
    }
}
