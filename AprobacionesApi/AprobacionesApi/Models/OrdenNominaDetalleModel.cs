﻿using System;

namespace AprobacionesApi.Models
{
    public class OrdenNominaDetalleModel
    {
        public int DINV_SEC { get; set; }
        public int DINV_CTINV { get; set; }
        public int DINV_ITEM { get; set; }
        public short DINV_LINEA { get; set; }
        public decimal DINV_CANT { get; set; }
        public decimal DINV_DSC { get; set; }
        public decimal DINV_VTA { get; set; }
        public decimal DINV_COS { get; set; }
        public decimal DINV_IVA { get; set; }
        public Nullable<decimal> DINV_ICE { get; set; }
        public decimal DINV_PRCT_DSC { get; set; }
        public Nullable<decimal> DINV_DSC_EX { get; set; }
        public string DINV_BOD { get; set; }
        public string DINV_TBOD { get; set; }
        public Nullable<System.DateTime> DINV_FECHA { get; set; }
        public Nullable<System.DateTime> DINV_FECANUL { get; set; }
        public Nullable<decimal> DINV_CANT2 { get; set; }
        public Nullable<decimal> DINV_PRECIO_REAL { get; set; }
        public Nullable<short> EMPRESA { get; set; }
        public Nullable<int> PROMOCION { get; set; }
        public Nullable<decimal> DINV_DCTOPRO { get; set; }
        public Nullable<decimal> DINV_VALPRO { get; set; }
        public string DINV_DETALLEDSCTO { get; set; }
        public string DINV_DETALLEADVAL1 { get; set; }
        public string DINV_DETALLEADVAL2 { get; set; }
        public string DINV_DETALLEADVAL3 { get; set; }
        public string DINV_DETALLEADVAL4 { get; set; }
        public Nullable<int> BODEGA1 { get; set; }
        public Nullable<int> BODEGA3 { get; set; }
        public string DINV_ST { get; set; }
        public string DINV_OC { get; set; }
    }
}