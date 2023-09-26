using Entities.Entidades;
using Entities.ModelsIntegration;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Integration.Enel;

public class Enelintegracao
{

    private string _baseUrlEnel = "https://portalhome.eneldistribuicaosp.com.br";
    private string _baseUrlGoogleApi = "https://www.googleapis.com/identitytoolkit/v3/relyingparty";
    private string _token = string.Empty;

    public async Task ImportarContaLacamento(DadosEnel dadosEnel)
    {
        ReturnLoginEnel token = new ReturnLoginEnel();
        GetLoginV2 loginV2 = new GetLoginV2();
        ReturnInstalacao instalacao = new ReturnInstalacao();
        ReturnVerifyToken verificao = new ReturnVerifyToken();
        ReturnTokenValidate returnTokenValidate = new ReturnTokenValidate();
        ChangeInstallation changeInstallation = new ChangeInstallation();
        EnelLogin enelLogin = new EnelLogin
        {
            I_EMAIL = dadosEnel.Login,
            I_PASSWORD = dadosEnel.Senha
        };

        HttpClient cliente = new HttpClient();
        var getResponse = await cliente.GetAsync(_baseUrlEnel);
        string txtGetResponse = await getResponse.Content.ReadAsStringAsync();
        string key = Regex.Match(txtGetResponse, @"key=([^""']+)").Groups[1].Value;

        try
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = ~DecompressionMethods.None;
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{_baseUrlEnel}/api/firebase/login"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "portalhome.eneldistribuicaosp.com.br");
                    request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
                    request.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en;q=0.8");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "Windows");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");

                    request.Content = new StringContent(JsonSerializer.Serialize(enelLogin));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");

                    var response = await httpClient.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    token = JsonSerializer.Deserialize<ReturnLoginEnel>(content);
                }               

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{_baseUrlGoogleApi}/verifyCustomToken?key={key}"))
                {
                    var requestPayload = new
                    {
                        returnSecureToken = true,
                        token = token.token
                    };

                    request.Content = new StringContent(JsonSerializer.Serialize(requestPayload));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");

                    var response = await httpClient.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();

                    verificao = JsonSerializer.Deserialize<ReturnVerifyToken>(content);
                }

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{_baseUrlGoogleApi}/getAccountInfo?key={key}"))
                {
                    var requestPayload = new
                    {
                        idToken = verificao.idToken
                    };

                    request.Content = new StringContent(JsonSerializer.Serialize(requestPayload));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");

                    var response = await httpClient.SendAsync(request);
                }

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://securetoken.googleapis.com/v1/token?key={key}"))
                {
                    var formData = new Dictionary<string, string>
                    {
                        {"grant_type", "refresh_token" },
                        {"refresh_token", verificao.refreshToken }
                    };
                    var content = new FormUrlEncodedContent(formData);

                    request.Content = content;
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string textResponse = await response.Content.ReadAsStringAsync();

                    returnTokenValidate = JsonSerializer.Deserialize<ReturnTokenValidate>(textResponse);
                }

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{_baseUrlEnel}/api/sap/getloginv2"))
                {
                    loginV2 = new GetLoginV2
                    {
                        I_FBIDTOKEN = returnTokenValidate.access_token
                    };
                    request.Content = new StringContent(JsonSerializer.Serialize(loginV2));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");

                    var response = await httpClient.SendAsync(request);

                    if (response.Headers.TryGetValues("Authorization", out var authorizationValues))
                        _token =  authorizationValues.FirstOrDefault();

                    string content = await response.Content.ReadAsStringAsync();
                    instalacao = JsonSerializer.Deserialize<ReturnInstalacao>(content);
                }

                foreach(var item in instalacao.ET_INST)
                {
                    if(dadosEnel.NumeroInstalacao == Convert.ToInt32(item.ANLAGE))
                    {
                        using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{_baseUrlEnel}/api/sap/changeinstallation"))
                        {
                            changeInstallation = new ChangeInstallation
                            {
                                I_ANLAGE = item.ANLAGE,
                                I_VERTRAG = instalacao.E_VERTRAG
                            };
                            request.Headers.TryAddWithoutValidation("authority", "portalhome.eneldistribuicaosp.com.br");
                            request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
                            request.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en;q=0.8");
                            request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                            request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "Windows");
                            request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
                            request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                            request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                            request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");
                            request.Headers.TryAddWithoutValidation("authorization", _token);

                            request.Content = new StringContent(JsonSerializer.Serialize(changeInstallation));
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");

                            var response = await httpClient.SendAsync(request);
                            string content = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }            
        }
        catch
        {
            return;
        }
    }
}
