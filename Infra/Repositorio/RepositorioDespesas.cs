using Domain.Interfaces.IDespesa;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio;

public class RepositorioDespesas : RepositorioGenerico<Despesa>, InterfaceDespesa
{
    private readonly DbContextOptions<AppDbContext> _context;

    public RepositorioDespesas()
    {
        _context = new DbContextOptions<AppDbContext>();
    }
    public async Task<IList<Despesa>> ListaDespesasUsuario(string emailUsuario)
    {
        using( var banco = new AppDbContext(_context))
        {
            return await
                (
                    from s in banco.SistemaFinanceiro
                    join c in banco.Categoria on s.Id equals c.IdSistema
                    join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                    join d in banco.Despesa on c.Id equals d.IdCategoria
                    where us.EmailUsuario.Equals(emailUsuario) && s.Mes == d.Mes && s.Ano == d.Ano
                    select d
                ).AsNoTracking().ToListAsync();
        }
    }

    public async Task<IList<Despesa>> ListaDespesasUsuarioNaoPagasMesesAnteriores(string emailUsuario)
    {
        using (var banco = new AppDbContext(_context))
        {
            return await
                (
                    from s in banco.SistemaFinanceiro
                    join c in banco.Categoria on s.Id equals c.IdSistema
                    join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                    join d in banco.Despesa on c.Id equals d.IdCategoria
                    where us.EmailUsuario.Equals(emailUsuario) && d.Mes < DateTime.Now.Month && !d.Pago
                    select d
                ).AsNoTracking().ToListAsync();
        }
    }
}
