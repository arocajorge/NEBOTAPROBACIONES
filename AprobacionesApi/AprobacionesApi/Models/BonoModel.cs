using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class BonoModel
    {
        public int ID { get; set; }
        public string BARCO { get; set; }
        public string VIAJE { get; set; }
        public string OPCION1 { get; set; }
        public decimal OP1_PREMIO { get; set; }
        public decimal OP1_MULTA { get; set; }
        public string OPCION2 { get; set; }
        public decimal OP2_PREMIO { get; set; }
        public string OPCION3 { get; set; }
        public decimal OP3_VERDE { get; set; }
        public decimal OP3_AZUL { get; set; }
        public decimal OP3_ROJO { get; set; }
        public string OPCION4 { get; set; }
        public decimal OP4_PREMIO { get; set; }
        public Nullable<System.DateTime> FECHA_ARRIBO { get; set; }
        public Nullable<System.DateTime> FECHA_ZARPE { get; set; }
        public Nullable<System.DateTime> FECHA_ZARPE_REAL { get; set; }
        public decimal TOTAL { get; set; }
    }
}