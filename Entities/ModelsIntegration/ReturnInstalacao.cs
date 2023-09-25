namespace Entities.ModelsIntegration;

public class ReturnInstalacao
{
    public List<Instalacao> ET_INST { get; set; }
}

public class Instalacao
{
    public string ANLAGE {get; set;}
    public string ENDERECO {get; set;}
    public string NIVEL_TENSAO {get; set;}
    public string ICONE_NVL_TS {get; set;}
    public string APELIDO {get; set;}
    public string INST_CONTATO {get; set;}
    public string VERTRAG {get; set;}
    public string EINZDAT {get; set;}
    public string AUSZDAT {get; set;}
    public string TENSAO {get; set;}
    public string CIRCUITO {get; set;}
    public string EP {get; set;}
    public string DEMAND_CONT {get; set;}
    public string MODAL_TARIF {get; set;}
    public string PARTNER {get; set;}
    public string NOME_PARC {get; set;}
    public string FAVORITA {get; set;}
    public string TENSAO_MIN {get; set;}
    public string TENSAO_MAX {get; set;}
    public string SMARTMETER {get; set;}
    public string SERIE { get; set; }
}