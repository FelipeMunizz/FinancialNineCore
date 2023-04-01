using Domain.Interfaces.ICategoria;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;

namespace Domain.Interfaces.Servicos;

public class CategoriaServico : ICategoriaServico
{
    private readonly InterfaceCategoria _categoria;

    public CategoriaServico(InterfaceCategoria categoria)
    {
        _categoria = categoria;
    }

    public async Task AdicionarCategoria(Categoria categoria)
    {
        var valido = categoria.ValidaString(categoria.Nome, "Nome");
        if (valido)
            await _categoria.Add(categoria);
    }

    public async Task AutalizarCategoria(Categoria categoria)
    {
        var valido = categoria.ValidaString(categoria.Nome, "Nome");
        if(valido)
            await _categoria.Update(categoria);
    }
}
