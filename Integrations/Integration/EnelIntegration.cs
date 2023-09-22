using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Integrations.Model.Enel;
using System.Text.Json;

namespace Integrations.Integration;

public class EnelIntegration
{
    private readonly ILancamentoServico _despesaService;
    private string _baseUrl = "https://portalhome.eneldistribuicaosp.com.br";

    public EnelIntegration(ILancamentoServico despesaService)
    {
        _despesaService = despesaService;
    }

    public async Task ImportarContaLancamento(DadosEnel dadosEnel)
    {
        _baseUrl = $"{_baseUrl}/api/firebase/login";
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
        client.DefaultRequestHeaders.Add("authority", _baseUrl);
        client.DefaultRequestHeaders.Add("origin", _baseUrl);
        client.DefaultRequestHeaders.Add("referer", _baseUrl);
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");

        EnelLogin enelLogin = new EnelLogin
        {
            I_EMAIL = dadosEnel.Login,
            I_PASSWORD = dadosEnel.Senha
        };

        var request = await client.PostAsync(_baseUrl, new StringContent(JsonSerializer.Serialize(enelLogin)));

        return;
    }
}
