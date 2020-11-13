using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda_MAWS.Models
{
    public class EstadoPedido
    {

		[Key]
		[Display(Name = "Código")]
		public int Id { get; set; }

		[Required]
		public string NombreEstado { get; set; }

		[Required, DataType(DataType.Date)]
		public DateTime FechaSalida { get; set; }

		[Required, DataType(DataType.Date)]
		public DateTime FechaLlegada { get; set; }

	}
}
