using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Accounts")]
    public class AccountsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        public byte AccountType { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(19, 4)")]
        public decimal CurrentBalance { get; set; } = 0.00m;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
