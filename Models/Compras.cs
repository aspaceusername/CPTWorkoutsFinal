using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CPTWorkouts.Models
{
    [PrimaryKey(nameof(ClienteFK), nameof(ServicoFK))] // PK para EF >= 7.0
    public class Compras
    {
        /// <summary>
        /// Id que serve para distinguir compras realizadas por um mesmo cliente
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data em que se realizou a compra do serviço por parte do cliente
        /// </summary>
        [Display(Name = "Data Compra")]
        public DateTime DataCompra { get; set; }

        /// <summary>
        /// Atributo auxiliar para recolher os dados do valor da compra
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        [Display(Name = "Valor da Compra")]
        [StringLength(9)]
        [RegularExpression("[0-9]{1,6}([,.][0-9]{1,2})?", ErrorMessage = "Escreva um número com, no máximo 2 casas decimais, separadas por . ou ,")]
        public string ValorCompraAux { get; set; }

        /// <summary>
        /// Valor da compra pago pelo Cliente aquando da compra dos serviços
        /// </summary>
        public decimal ValorCompra { get; set; }

        // FK dos Servicos
        [ForeignKey(nameof(Servico))]
        public int ServicoFK { get; set; }
        public Servicos Servico { get; set; }

        // FK dos Clientes
        [ForeignKey(nameof(Cliente))]
        public int ClienteFK { get; set; }
        public Clientes Cliente { get; set; }
    }
}
