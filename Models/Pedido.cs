using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_MAWS.Models
{

    public enum enEstado
    {
        En_Preparacion = 1,
        En_Camino = 2,
        Entregado = 3,
    }
    public class Pedido
    {

        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Fecha { get; set; }


        [Required, DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FechaLlegada { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal PrecioFinal { get; set; }


        //public List<PedidoProducto> ListaLineas{ get; set; }

        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }


        public String UsuarioACargo { get; set; }

        //0 disponible , 1 ocupada/a cargo
        public int Asignado { get; set; }


        public int Estado { get; set; }

        //public string EstadoNombre => Estado > 0 ? ((enEstado)Estado).ToString().Replace('_', ' ') : "";
        public string EstadoNombre { get; set; }
        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(enEstado);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return roles;
        }



    }
}
