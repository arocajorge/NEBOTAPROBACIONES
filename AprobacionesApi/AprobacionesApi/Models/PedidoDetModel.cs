using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class PedidoDetModel
    {
        public decimal PED_ID { get; set; }
        public decimal PED_SEC { get; set; }
        public Nullable<decimal> PED_CANT { get; set; }
        public string PED_DETALLE { get; set; }
    }
}