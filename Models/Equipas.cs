using System.ComponentModel.DataAnnotations;

namespace CPTWorkouts.Models
{
    /// <summary>
    /// Descrição da equipa a que um cliente se pode inscrevêr
    /// </summary>
    public class Equipas
    {
        public Equipas() {
            ListaClientes = new HashSet<Clientes>();
        }

        [Key] // PK
        public int Id { get; set; }
        /// <summary>
        /// Nome da Equipa
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório!")]
        [StringLength(100)]
        public string Nome { get; set; }

        /// <summary>
        /// Nome do ficheiro que contém o logótipo do Curso
        /// </summary>
        [Display(Name = "Logótipo")] // define o nome a aparecer no ecrã
        [StringLength(50)]
        public string? Logotipo { get; set; } // o '?' vai tornar o atributo em preenchimento facultativo

        /* ****************************************
         * Construção dos Relacionamentos
         * *************************************** */

        // relacionamento 1-N

        /// <summary>
        /// Lista dos Clientes 'inscritos' na Equipa
        /// </summary>
        public ICollection<Clientes> ListaClientes { get; set; }
    }
}
