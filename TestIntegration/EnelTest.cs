using Entities.Entidades;
using Integration.Enel;

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

        Enelintegracao enel = new Enelintegracao();
        await enel.ImportarContaLacamento(dados);
    }
}
