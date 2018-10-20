﻿using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class AprobacionJefeSupController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();

        public List<OrdenModel> GET(DateTime? FECHAINI, DateTime? FECHAFIN, string USUARIO = "", string BODEGA = "", string VIAJE = "", string CINV_NUM = "", string ESTADOJEFE = "", string ESTADOSUPERVISOR = "")
        {
            try
            {
                FECHAINI = FECHAINI == null ? DateTime.Now.Date.AddMonths(-2) : FECHAINI;
                FECHAFIN = FECHAFIN == null ? DateTime.Now.Date : FECHAFIN;
                List<OrdenModel> Lista;
                if (string.IsNullOrEmpty(USUARIO))
                    return new List<OrdenModel>();

                var UsuarioInfo = db.USUARIOS.Where(q => q.USUARIO.ToLower() == USUARIO.ToLower()).FirstOrDefault();
                if (UsuarioInfo == null)
                    return new List<OrdenModel>();

                Lista = get_list(BODEGA, VIAJE, CINV_NUM, ESTADOJEFE, ESTADOSUPERVISOR, Convert.ToDateTime(FECHAINI), Convert.ToDateTime(FECHAFIN));

                return Lista;
            }
            catch (Exception)
            {
                return new List<OrdenModel>();
            }
        }


        public List<OrdenModel> get_list(string Bodega, string Viaje, string num_ot, string estado_jefe_bodega, string estado_supervisor, DateTime fecha_ini, DateTime fecha_fin)
        {
            List<OrdenModel> Lista;
            
                string where_clause = "(CINV_ST = \"P\" OR CINV_ST = \"G\") AND CINV_TDOC = \"OT\" ";
                if (!string.IsNullOrEmpty(Bodega))
                    where_clause += "AND CINV_BOD = \"" + Bodega + "\" ";
                if (!string.IsNullOrEmpty(Viaje))
                    where_clause += "AND CINV_FPAGO = \"" + Viaje + "\" ";
                if (!string.IsNullOrEmpty(estado_jefe_bodega))
                    where_clause += "AND CINV_STCUMPLI1 = \"" + estado_jefe_bodega + "\" ";
                if (!string.IsNullOrEmpty(estado_supervisor))
                    where_clause += "AND CINV_STCUMPLI2 = \"" + estado_supervisor + "\" ";

                where_clause += "AND DATETIME(" + fecha_ini.Date.Year + "," + fecha_ini.Date.Month + "," + fecha_ini.Date.Day + ") <= CINV_FECING ";
                where_clause += "AND CINV_FECING <= DATETIME(" + fecha_fin.Date.Year + "," + fecha_fin.Date.Month + "," + fecha_fin.Date.Day + ")";

                Lista = (from q in db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(where_clause, null)
                         select new OrdenModel
                         {
                             CINV_NUM = q.CINV_NUM,
                             NOM_SOLICITADO = q.NOM_SOLICITADO,
                             CINV_COM1 = q.CINV_COM1,
                             CODIGO1 = q.CODIGO1,
                             CINV_FPAGO = q.CINV_FPAGO,
                             CINV_FECING = q.CINV_FECING,
                             CODIGOTR = q.CODIGOTR,
                             CINV_BOD = q.CINV_BOD,
                             CINV_ST = q.CINV_ST,
                             CINV_STCUMPLI2 = q.CINV_STCUMPLI2,
                             CINV_NOMID = q.CINV_NOMID,
                             CINV_TDOC = q.CINV_TDOC,
                             CINV_COM3 = q.CINV_COM3,
                             CINV_COM4 = q.CINV_COM4,
                             VALOR_OC = q.VALOR_OC,
                             NOM_CENTROCOSTO = q.NOM_CENTROCOSTO,
                         }).ToList();
            
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };
            Lista.ForEach(q => { q.VALOR_OT = q.CINV_TDOC == "OT" ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : 0; });

            return Lista.OrderBy(q => q.CINV_NUM).ToList();
        }
    }
}