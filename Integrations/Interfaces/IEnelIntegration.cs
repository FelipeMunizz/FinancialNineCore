using Entities.Entidades;

namespace Integrations.Interfaces;

public interface IEnelIntegration
{
    Task ImportarContaLancamento(DadosEnel dadosEnel)
}
