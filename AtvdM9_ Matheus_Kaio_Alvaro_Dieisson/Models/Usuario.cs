using System.ComponentModel.DataAnnotations;

namespace ProjetoTarefas.Models
{

    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

 
        [Required]
        public string SenhaHash { get; set; } = string.Empty;
    }
}
