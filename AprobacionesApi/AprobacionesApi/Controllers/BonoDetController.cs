﻿using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class BonoDetController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public List<BonoDetModel> GET(int ID = 0, string OPCION = "")
        {
            try
            {
                List<BonoDetModel> Lista = new List<BonoDetModel>();

                var bitacora = db.CAB_BITACORA.Where(q => q.ID == ID).FirstOrDefault();
                if (bitacora == null)
                    return new List<BonoDetModel>();

                var param = db.CAB_BITACORA_PARAM.Where(q => q.ID == ID).FirstOrDefault();
                if (param == null)
                    return new List<BonoDetModel>();

                switch (OPCION)
                {
                    case "OPCION1":
                        var op1 = db.VW_BITACORAS_APP.Where(q => q.ID == ID && q.FECHA_OT != null && !q.DESCRIPCION.Contains("REASIGNADA")).GroupBy(q => new { q.TIPO, q.NUMERO_ORDEN, q.FECHA_ARRIBO, q.FECHA_OT }).ToList();

                        Lista = op1.Select(q => new BonoDetModel
                        {
                            TITULO = q.Key.TIPO + ". " + q.Key.NUMERO_ORDEN,
                            FECHA = q.Key.FECHA_OT ?? DateTime.Now.Date,
                            DIAS = (int)(q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays,
                            COLOR = (double)param.OP1_INICIO <= (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays && (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays <= (double)param.OP1_FIN ? "LightGreen" : "Red",
                            DESCRIPCION = (double)param.OP1_INICIO <= (q.Key.FECHA_ARRIBO.Value.Date - q.Key.FECHA_OT.Value.Date).TotalDays && (q.Key.FECHA_ARRIBO.Value.Date - q.Key.FECHA_OT.Value.Date).TotalDays <= (double)param.OP1_FIN ? "+"+param.OP1_PREMIO.ToString() : "-"+ Math.Round(Math.Abs((q.Key.FECHA_ARRIBO.Value.Date - q.Key.FECHA_OT.Value.Date).TotalDays * (double)param.OP1_MULTA),2,MidpointRounding.AwayFromZero).ToString(),
                            NUMERO_ORDEN = Convert.ToInt32(q.Key.NUMERO_ORDEN),
                            TIPO = q.Key.TIPO
                        }).ToList();

                        foreach (var item in Lista)
                        {
                            item.DESCRIPCION = db.TBCINV.Where(q => q.CINV_TDOC == item.TIPO && q.CINV_NUM == item.NUMERO_ORDEN).FirstOrDefault().CINV_COM1+" \n"+item.DESCRIPCION;
                        }

                        break;
                    case "OPCION2":
                        param.OP3_ROJO = param.OP3_ROJO ?? 0;

                        Lista = db.DET_BITACORA.AsEnumerable().Where(q => q.ID == bitacora.ID && q.LINEA_FECCUMPLI1 != null && param.OP2_INICIO <= (bitacora.FECHA_ZARPE - q.LINEA_FECCUMPLI1).Value.Days && (bitacora.FECHA_ZARPE - q.LINEA_FECCUMPLI1).Value.Days <= param.OP2_FIN).Select(q => new BonoDetModel
                        {
                            TITULO = "No. "+q.LINEA.ToString(),
                            FECHA = q.LINEA_FECCUMPLI1 ?? DateTime.Now,
                            DIAS = (bitacora.FECHA_ZARPE.Value.Date - q.LINEA_FECCUMPLI1.Value.Date).Days,
                            COLOR = param.OP2_INICIO <= (bitacora.FECHA_ZARPE.Value.Date - q.LINEA_FECCUMPLI1.Value.Date).Days && (bitacora.FECHA_ZARPE.Value.Date - q.LINEA_FECCUMPLI1.Value.Date).Days <= param.OP2_FIN ? "LightGreen" : "White",
                            DESCRIPCION = q.DESCRIPCION
                        }).ToList();
                        Lista.ForEach(q => q.DESCRIPCION += " \n" +(q.COLOR == "LightGreen" ? "+"+param.OP2_PREMIO.ToString() : "0"));
                        break;
                    case "OPCION3":
                        decimal TotalObras = db.DET_BITACORA.Where(q => q.ID == bitacora.ID && q.LINEA_STCUMPLI2 != null && !q.DESCRIPCION.Contains("REASIGNADA")).Count();
                        if (TotalObras != 0)
                        {
                            decimal ValorPorObra = Math.Round(param.OP3_TOTAL / TotalObras, 2, MidpointRounding.AwayFromZero);
                            Lista = db.DET_BITACORA.AsEnumerable().Where(q => q.ID == bitacora.ID && q.LINEA_STCUMPLI2 != null && !q.DESCRIPCION.Contains("REASIGNADA")).Select(q => new BonoDetModel
                            {
                                TITULO = "No. " + q.LINEA.ToString(),
                                FECHA = q.LINEA_FECCUMPLI2,
                                DIAS = null,
                                COLOR = (q.LINEA_STCUMPLI2 == "P" ? "LightGreen" :
                            (q.LINEA_STCUMPLI2 == "T" ? "LightBlue" :
                            (q.LINEA_STCUMPLI2 == "X" ? "Red" : "White"))),
                                DESCRIPCION = q.DESCRIPCION + " \n" + (q.LINEA_STCUMPLI2 == "P" ? "+" + ValorPorObra.ToString() :
                            (q.LINEA_STCUMPLI2 == "T" ? "-" + Math.Round(Convert.ToDecimal(((double)param.OP3_TOTAL * ((double)param.OP3_AZUL / 100.00))), 2, MidpointRounding.AwayFromZero).ToString() :
                            (q.LINEA_STCUMPLI2 == "X" ? "-" + Math.Round(Convert.ToDecimal(((double)param.OP3_TOTAL * ((double)param.OP3_ROJO / 100.00))), 2, MidpointRounding.AwayFromZero).ToString() : "White")))
                            }).ToList();
                        }else
                        {
                            Lista.Add(new BonoDetModel
                            {
                                TITULO = "No existen obras revisadas por un supervisor",
                                FECHA = DateTime.Now.Date,
                                DIAS = null,
                                COLOR = "White",
                                DESCRIPCION = null
                            });
                        }
                        
                        break;
                }

                return Lista;
            }
            catch (Exception ex)
            {
                using (EntitiesGeneral Error = new EntitiesGeneral())
                {
                    long IDERROR = Error.APP_LOGERROR.Count() > 0 ? (Error.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                    Error.APP_LOGERROR.Add(new APP_LOGERROR
                    {
                        ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                        INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                        FECHA = DateTime.Now,
                        PROCESO = "BonoDet/GET",
                        SECUENCIA = IDERROR
                    });
                    Error.SaveChanges();
                }
                return new List<BonoDetModel>();
            }
        }
    }
}
