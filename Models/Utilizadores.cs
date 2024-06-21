using System.ComponentModel.DataAnnotations;

namespace CPTWorkouts.Models
{
    public class Utilizadores
    {
        [Key] // PK
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Data Nascimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateOnly DataNascimento { get; set; }

        [Display(Name = "Telemóvel")]
        [StringLength(19)]
        [RegularExpression("([+]|00)?[0-9]{6,17}", ErrorMessage = "o {0} só pode conter digitos. No mínimo 6.")]
        public string Telemovel { get; set; }

        /// <summary>
        /// Atributo para funcionar como FK entre a tabela dos Utilizadores e a tabela da Autenticação
        /// </summary>
        public string UserID { get; set; }
    }
}
