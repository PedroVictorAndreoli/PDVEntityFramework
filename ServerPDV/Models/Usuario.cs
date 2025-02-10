using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerPDV.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        [Column("uid")] 
        public int UId { get; set; }

        [Required] 
        [MaxLength(100)] 
        [Column("nome")] 
        public string Nome { get; set; }

        [Required] 
        [MaxLength(50)] 
        [Column("login")] 
        public string Login { get; set; }

        [Required] 
        [MaxLength(100)] 
        [Column("senha")] 
        public string Senha { get; set; }



    }
}
