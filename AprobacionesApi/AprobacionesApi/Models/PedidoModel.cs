﻿using System;
using System.Collections.Generic;

namespace AprobacionesApi.Models
{
    public class PedidoModel
    {
        public decimal PED_ID { get; set; }
        public Nullable<System.DateTime> PED_FECHA { get; set; }
        public string PED_BOD { get; set; }
        public string PED_VIAJE { get; set; }
        public string PED_SUC { get; set; }
        public string PED_SOL { get; set; }
        public string PED_OBS { get; set; }
        public string PED_ST { get; set; }
        public Nullable<System.DateTime> OC_FECHA { get; set; }
        public Nullable<System.DateTime> INV_FECHA { get; set; }
        public string NOM_BODEGA { get; set; }
        public string NOM_VIAJE { get; set; }
        public string PED_LOGIN { get; set; }
        public Nullable<System.DateTime> PED_FECTRAN { get; set; }
        public string NOM_SUCURSAL { get; set; }
        public string NOM_EMPLEADO { get; set; }
        public string PED_COM { get; set; }
        public Nullable<System.DateTime> PED_FECAPRO { get; set; }

        #region Campos que no existen en la tabla
        public string ESTADO { get; set; }
        public string COLOR { get; set; }
        #endregion


        public List<PedidoDetModel> LstDet { get; set; }
    }
}