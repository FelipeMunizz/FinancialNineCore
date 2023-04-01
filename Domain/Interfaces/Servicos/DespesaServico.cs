using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;

namespace Domain.Interfaces.Servicos;

public class DespesaServico : IDespesaServico
{
    private readonly InterfaceDespesa _despesa;

    public DespesaServico(InterfaceDespesa despesa)
    {
        _despesa = despesa;
    }

    public async Task AdicionarDespesa(Despesa despesa)
    {
        DateTime data = DateTime.UtcNow;
        despesa.DataCadastro = data;
        despesa.Ano = data.Year;
        despesa.Mes = data.Month;

        var valido = despesa.ValidaString(despesa.Nome, "Nome");
        if (valido)
            await _despesa.Add(despesa);
    }

    public async Task AutalizarDespesa(Despesa despesa)
    {
        DateTime data = DateTime.UtcNow;
        despesa.DataAlteração = data;

        if (despesa.Pago)
            despesa.DataPagamento = data;

        var valido = despesa.ValidaString(despesa.Nome, "Nome");
        if (valido)
            await _despesa.Update(despesa);
    }
}
