using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda_MAWS.Models
{
    public class PedidoProducto
    {
        [Display(Name = "Código")]
        public int Id { get; set; }



        [Required]
        [Display(Name = "PrecioUnidad")]
        public decimal PrecioUnidad { get; set; }


        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }


        [Required]
        [Display(Name = "MontoTotal")]
        public decimal MontoTotal { get; set; }



        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }


    }
}
