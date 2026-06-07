using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Transactions")]
    public class TransactionsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        [Column(TypeName = "decimal(19, 4)")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(128)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime TransactionDate { get; set; }

        [MaxLength(128)]
        public string? Reference { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
