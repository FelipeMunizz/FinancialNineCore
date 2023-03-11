using Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entidades;

[Table("Despesa")]
public class Despesa : Base
{
    public decimal Valor { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public EnumTipoDespesa TipoDespesa { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAlteração { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime DataVencimento { get; set; }
    public bool Pago { get; set; }
    public bool DespesaAtrasada { get; set; }

    [ForeignKey("Categoria")]
    [Column(Order = 1)]
    public int IdCategoria { get; set; }
    public virtual Categoria Categoria { get; set; }
}
