using System.ComponentModel.DataAnnotations;

namespace ProjetoTarefas.Models
{

    public class Tarefa
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }


        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime Data { get; set; } = DateTime.Today;

  
        [Display(Name = "Concluída?")]
        public bool Concluida { get; set; }


        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
