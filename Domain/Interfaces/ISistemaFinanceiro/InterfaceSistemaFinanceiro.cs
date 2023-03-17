using Domain.Interfaces.Generics;
using Entities.Entidades;

namespace Domain.Interfaces.ISistemaFinanceiro;

public interface InterfaceSistemaFinanceiro : InterfaceGeneric<SistemaFinanceiro>
{
    Task<IList<SistemaFinanceiro>> ListaSistemasUsuario(string emailUsuario);
}