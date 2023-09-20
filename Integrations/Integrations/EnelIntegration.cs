using Domain.Interfaces.InterfaceServicos;

namespace Integrations.Integrations;

public class EnelIntegration
{
    private readonly ILancamentoServico _despesaService;

    public EnelIntegration(ILancamentoServico despesaService)
    {
        _despesaService = despesaService;
    }

    public async Task ImportarContaLancamento()
    {
        return;
    }
}
