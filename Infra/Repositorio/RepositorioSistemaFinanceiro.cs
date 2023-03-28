using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio;

public class RepositorioSistemaFinanceiro : RepositorioGenerico<SistemaFinanceiro>, InterfaceSistemaFinanceiro
{
    private readonly DbContextOptions<AppDbContext> _context;

    public RepositorioSistemaFinanceiro()
    {
        _context = new DbContextOptions<AppDbContext>();
    }
    public async Task<IList<SistemaFinanceiro>> ListaSistemasUsuario(string emailUsuario)
    {
        using (var banco = new AppDbContext(_context))
        {
            return await
                (
                    from s in banco.SistemaFinanceiro
                    join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                    where us.EmailUsuario.Equals(emailUsuario)
                    select s
                ).AsNoTracking().ToListAsync();
        }
    }
}
