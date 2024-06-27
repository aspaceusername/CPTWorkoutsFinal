using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CPTWorkouts.Models
{
    [PrimaryKey(nameof(ClienteFK), nameof(ServicoFK))] // PK para EF >= 7.0
    public class Compras
    {
        /// <summary>
        /// Data em que se realizou a compra do servico por parte do cliente
        /// </summary>
        [Display(Name = "Data Compra")]
        public DateTime DataCompra { get; set; }


        // relacionamento N-M, COM atributos no relacionamento

        //  [Key, Column(Order = 1)] // PK para EF <= 6.0
        [ForeignKey(nameof(Servico))]
        public int ServicoFK { get; set; }
        public Servicos Servico { get; set; }

        // tabela do relacionamento N-M, COM atributos do relacionamento

        //  [Key, Column(Order = 2)] // PK para EF <= 6.0
        [ForeignKey(nameof(Cliente))]
        public int ClienteFK { get; set; }
        public Clientes Cliente { get; set; }
    }
}
