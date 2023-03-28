using Domain.Interfaces.ICategoria;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio;

public class RepositorioCategoria : RepositorioGenerico<Categoria>, InterfaceCategoria
{
    private readonly DbContextOptions<AppDbContext> _context;

    public RepositorioCategoria()
    {
        _context = new DbContextOptions<AppDbContext>();
    }

    public async Task<IList<Categoria>> ListarCategoriasUsuario(string emailUsuario)
    {
        using (var banco = new AppDbContext(_context))
        {
            return await
                (
                    from s in banco.SistemaFinanceiro
                    join c in banco.Categoria on s.Id equals c.IdSistema
                    join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                    where us.EmailUsuario.Equals(emailUsuario) && us.SistemaAtual
                    select c
                ).AsNoTracking().ToListAsync();
        }
    }
}
