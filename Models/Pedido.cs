using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_MAWS.Models
{
    public class Pedido
    {

        [Display(Name = "Código")]
        public int Id { get; set; }


        [Required, DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal PrecioFinal { get; set; }



        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        public int EstadoPedidoId { get; set; }
        [ForeignKey("EstadoPedidoId")]
        public EstadoPedido EstadoPedido { get; set; }


        public String UsuarioACargo { get; set; }

        //0 disponible , 1 ocupada/a cargo
        public int Asingnado { get; set; }

    }
}
