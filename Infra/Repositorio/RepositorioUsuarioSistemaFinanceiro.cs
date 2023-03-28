using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio;

public class RepositorioUsuarioSistemaFinanceiro : RepositorioGenerico<UsuarioSistemaFinanceiro>, InterfaceUsuarioSistemaFinanceiro
{
    private readonly DbContextOptions<AppDbContext> _context;

    public RepositorioUsuarioSistemaFinanceiro()
    {
        _context = new DbContextOptions<AppDbContext>();
    }

    public async Task<IList<UsuarioSistemaFinanceiro>> ListaUsuariosSistemasFinanceiro(int idSistema)
    {
        using (var banco = new AppDbContext(_context))
        {
            return await banco.UsuarioSistemaFinanceiro.Where(s => s.IdSistema == idSistema).AsNoTracking().ToListAsync();
        }
    }

    public async Task<UsuarioSistemaFinanceiro> ObterUsuarioSistemaFinanceiro(string emailUsuario)
    {
        using (var banco = new AppDbContext(_context))
        {
            return await banco.UsuarioSistemaFinanceiro.AsNoTracking().FirstOrDefaultAsync(x => x.EmailUsuario.Equals(emailUsuario));
        }
    }

    public async Task RemoverUsuarios(List<UsuarioSistemaFinanceiro> usuarios)
    {
        using (var banco = new AppDbContext(_context))
        {
            banco.UsuarioSistemaFinanceiro.RemoveRange(usuarios);
            await banco.SaveChangesAsync();
        }
    }
}