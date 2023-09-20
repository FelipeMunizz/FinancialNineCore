using Domain.Interfaces.ILancamento;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio;

public class RepositorioLancamento : RepositorioGenerico<Lancamento>, InterfaceLancamento
{
    private readonly DbContextOptions<AppDbContext> _context;

    public RepositorioLancamento()
    {
        _context = new DbContextOptions<AppDbContext>();
    }

    public async Task AdicionarListaLancamentos(List<Lancamento> lancamentos)
    {
        using (var banco = new AppDbContext(_context))
        {
            foreach(Lancamento lancamento in lancamentos)
            {
                banco.Lancamentos.Add(lancamento);
                await banco.SaveChangesAsync();
            }                
        }
    }

    public async Task<IList<Lancamento>> ListaLancamentosUsuario(string emailUsuario)
    {
        using( var banco = new AppDbContext(_context))
        {
            return await
                (
                    from s in banco.SistemaFinanceiro
                    join c in banco.Categoria on s.Id equals c.IdSistema
                    join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                    join d in banco.Lancamentos on c.Id equals d.IdCategoria
                    where us.EmailUsuario.Equals(emailUsuario) && s.Mes == d.Mes && s.Ano == d.Ano
                    select d
                ).AsNoTracking().ToListAsync();
        }
    }

    public async Task<IList<Lancamento>> ListaLancamentosUsuarioNaoPagasMesesAnteriores(string emailUsuario)
    {
        using (var banco = new AppDbContext(_context))
        {
            return await
                (
                    from s in banco.SistemaFinanceiro
                    join c in banco.Categoria on s.Id equals c.IdSistema
                    join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                    join d in banco.Lancamentos on c.Id equals d.IdCategoria
                    where us.EmailUsuario.Equals(emailUsuario) && d.Mes < DateTime.Now.Month && !d.Pago
                    select d
                ).AsNoTracking().ToListAsync();
        }
    }
}
