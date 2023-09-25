namespace Entities.ModelsIntegration;

public class GetLoginV2
{
    public string I_AMBIENTE { get; set; } = "PROD";
    public string I_ANLAGE {get; set;} = string.Empty;
    public string I_BANDEIRA { get; set; } = "X";
    public string I_CANAL { get; set; } = "ZINT";
    public string I_CNPJ {get; set;} = string.Empty;
    public string I_COBRADORA {get; set;} = string.Empty;
    public string I_COD_SERV { get; set; } = "TC";
    public string I_CPF {get; set;} = string.Empty;
    public string I_EXEC_LOGIN { get; set; } = "X";
    public string I_FBIDTOKEN {get; set;}
    public string I_LISTA_INST { get; set; } = "X";
    public string I_PARTNER {get; set;} = string.Empty;
    public string I_RESPOSTA_01 {get; set;} = string.Empty;
    public string I_RESPOSTA_02 {get; set;} = string.Empty;
    public string I_VERTRAG { get; set; } = string.Empty;
}
