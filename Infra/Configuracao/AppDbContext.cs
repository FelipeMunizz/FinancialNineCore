using Entities.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Configuracao;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public AppDbContext() { }

    #region Entidades
    public DbSet<SistemaFinanceiro> SistemaFinanceiro { get; set; }
    public DbSet<UsuarioSistemaFinanceiro> UsuarioSistemaFinanceiro { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Lancamento> Lancamentos { get; set; }
    #endregion

    #region Metodos Override
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

        base.OnModelCreating(builder);
    }
    #endregion

    #region Metodos Nativos
    public string GetConnectionString()
    {
        string pcFelipe = "Data Source=DESKTOP-HER7P6R;Initial Catalog=FinancialNineCore;Integrated Security=True;Pooling=False;Encrypt=False;TrustServerCertificate=False;";
        string not = "Data Source=DESKTOP-HH8094V;Initial Catalog=FinancialNineCore;Integrated Security=True;Pooling=False;Encrypt=False;TrustServerCertificate=False;";
        string producao = "workstation id=financialninecore.mssql.somee.com;packet size=4096;user id=FinancialCore_SQLLogin_1;pwd=5alc1cha;data source=financialninecore.mssql.somee.com;persist security info=False;initial catalog=financialninecore; Pooling=False;Encrypt=true;TrustServerCertificate=true;";
        return producao;
    }
    #endregion    
}
