﻿using Entities.Entidades;
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

    public async Task ImportarContaLacamento(DadosEnel dadosEnel)
    {
        ReturnLoginEnel token = new ReturnLoginEnel();
        GetLoginV2 loginV2 = new GetLoginV2();
        ReturnInstalacao instalacao = new ReturnInstalacao();
        ReturnVerifyToken verificao = new ReturnVerifyToken();
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

                loginV2 = new GetLoginV2
                {
                    I_FBIDTOKEN = token.token
                };

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

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{_baseUrlEnel}/api/sap/getloginv2"))
                {
                    request.Content = new StringContent(JsonSerializer.Serialize(loginV2));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");

                    var response = await httpClient.SendAsync(request);
                    string content = await response.Content.ReadAsStringAsync();
                    instalacao = JsonSerializer.Deserialize<ReturnInstalacao>(content);
                }
            }            
        }
        catch
        {
            return;
        }
    }
}