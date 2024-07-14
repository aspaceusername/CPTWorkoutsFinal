using System.ComponentModel.DataAnnotations;

namespace CPTWorkouts.Models
{
    public class Utilizadores
    {
        /// <summary>
        /// chave primaria do modelo dos utilizadores
        /// </summary>
        [Key] // PK
        public int Id { get; set; }

        /// <summary>
        /// nome do utilizador
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Nome { get; set; }
        /// <summary>
        /// data de nascimento correspondente ao utilizador
        /// </summary>
        [Display(Name = "Data Nascimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateOnly DataNascimento { get; set; }
        /// <summary>
        /// numero de telemovel correspondente ao utilizador
        /// </summary>
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
