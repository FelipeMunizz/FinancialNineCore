using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entidades;

[Table("Categoria")]
public class Categoria : Base
{
    [ForeignKey("SistemaFinanceiro")]
    [Column(Order = 1)]
    public int IdSistema { get; set; }
    public virtual SistemaFinanceiro? SistemaFinanceiro { get; set; }
}
