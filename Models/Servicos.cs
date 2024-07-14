using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CPTWorkouts.Models
{
    public class Servicos
    {
        // Vamos usar a Entity Framework para a construção do Model
        // https://learn.microsoft.com/en-us/ef/

        public Servicos()
        {
            ListaTreinadores = new HashSet<Treinadores>();
            ListaCompras = new HashSet<Compras>();
        }
        /// <summary>
        /// chave primária do serviço
        /// </summary>
        [Key] // PK
        public int Id { get; set; }
        /// <summary>
        /// Nome do Serviço a ser prestado pelo treinador
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Preco do serviço a ser prestado pelo treinador
        /// </summary>
        public decimal Preco { get; set; }

        /* ****************************************
         * Construção dos Relacionamentos
         * *************************************** */


        // relacionamento M-N, SEM atributos no relacionamento
        public ICollection<Treinadores> ListaTreinadores { get; set; }


        // relacionamento N-M, COM atributos no relacionamento
        public ICollection<Compras> ListaCompras { get; set; }
    }
}
