using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPTWorkouts.Models
{
    public class Clientes:Utilizadores
    {
        // um Cliente é um objeto do tipo Utilizadores
        // um Cliente é um caso particular de Utlizadores

        public Clientes()
        {
            ListaCompras = new HashSet<Compras>();
        }


        public int NumCliente { get; set; }

        /// <summary>
        /// atributo auxiliar para recolher os dados do valor da compra
        /// </summary>
        [NotMapped] // informa a EF para ignorar este atributo
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
        [Display(Name = "Valor da Compra")]
        [StringLength(9)]
        [RegularExpression("[0-9]{1,6}([,.][0-9]{1,2})?", ErrorMessage = "Escreva um número com, no máximo 2 casas decimais, separadas por . ou ,")]
        public string ValorCompraAux { get; set; }

        /// <summary>
        /// Valor da compra pago pelo Cliente aquando da compra dos serviços
        /// </summary>
        public decimal ValorCompra { get; set; }

        [Display(Name = "Data Compra")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DataCompra { get; set; }


        /* ****************************************
        * Construção dos Relacionamentos
        * *************************************** */

        // relacionamento 1-N

        // esta anotação informa a EF
        // que o atributo 'EquipaFK' é uma FK em conjunto
        // com o atributo 'Equipa'
        [ForeignKey(nameof(Equipa))]
        [Display(Name = "Equipa")]
        public int EquipaFK { get; set; } // FK para o Curso
        public Equipas Equipa { get; set; } // FK para o Curso

        // relacionamento N-M, com atributos no relacionamento
        public ICollection<Compras> ListaCompras { get; set; }

    }
}
