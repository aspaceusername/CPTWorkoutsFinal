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
        /// <summary>
        /// numcliente serve para atribuir uma distinção clara entre os clientes num contexto de frontend, substituindo nesta função o UserID
        /// </summary>
        public int? NumCliente { get; set; }


        /* ****************************************
        * Construção dos Relacionamentos
        * *************************************** */

        // relacionamento 1-N

        // esta anotação informa a EF
        // que o atributo 'EquipaFK' é uma FK em conjunto
        // com o atributo 'Equipa'
        [ForeignKey(nameof(Equipa))]
        [Display(Name = "Equipa")]
        public int? EquipaFK { get; set; } // FK para o Curso
        public Equipas Equipa { get; set; } // FK para o Curso

        // relacionamento N-M, com atributos no relacionamento
        public ICollection<Compras> ListaCompras { get; set; }

    }
}
