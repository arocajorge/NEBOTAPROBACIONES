using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class BonoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public BonoModel GET(string BARCO = "", string VIAJE = "")
        {
            try
            {
                BonoModel Bono = new BonoModel
                {
                    OPCION1 = "ASIGNACION DE ORDENES",
                    OPCION2 = "CUMPLIMIENTO RAPIDO",
                    OPCION3 = "CALIFICACION DE OBRAS CUMPLIDAS",
                    OPCION4 = "COMISION GLOBAL",

                    OPCION3_VERDE = "(+) Bien hechas",
                    OPCION3_AZUL = "(-) Hechas a medias",
                    OPCION3_ROJO = "(-) No realizada"
                };

                var bitacora = db.VW_BITACORAS_APP.Where(q => q.BARCO == BARCO && q.VIAJE == VIAJE).FirstOrDefault();
                if (bitacora == null)
                    return Bono;

                Bono.ID = bitacora.ID;
                Bono.BARCO = bitacora.NOM_BARCO;
                Bono.VIAJE = bitacora.NOM_VIAJE;
                Bono.FECHA_ARRIBO = bitacora.FECHA_ARRIBO ?? DateTime.Now.Date;
                Bono.FECHA_ZARPE = bitacora.FECHA_ZARPE ?? DateTime.Now.Date;
                Bono.FECHA_ZARPE_REAL = bitacora.FECHA_ZARPE_REAL ?? DateTime.Now.Date;

                var param = db.CAB_BITACORA_PARAM.Where(q => q.ID == bitacora.ID).FirstOrDefault();
                if(param == null)
                    return Bono;

                #region Opción 1
                var dias = (Bono.FECHA_ARRIBO - Bono.FECHA_ZARPE).Value.TotalDays;
                var lst = db.VW_BITACORAS_APP.Where(q => q.ID == bitacora.ID && q.FECHA_OT != null && !q.DESCRIPCION.Contains("REASIGNADA")).GroupBy(q => new { q.NUMERO_ORDEN, q.FECHA_ARRIBO, q.FECHA_OT }).ToList();
                var premio_op1 = lst.Where(q => (double)param.OP1_INICIO <= (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays && (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays <= (double)param.OP1_FIN).Select(q=> new { Dias = (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays, Orden = q.Key.NUMERO_ORDEN }).ToList();
                var multa_op1 = lst.Where(q => (double)param.OP1_INICIO > (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays || (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays > (double)param.OP1_FIN).Select(q => new { Dias = (q.Key.FECHA_ARRIBO - q.Key.FECHA_OT).Value.TotalDays, Orden = q.Key.NUMERO_ORDEN }).ToList();

                Bono.OP2_CAN_PREMIO = premio_op1.Count;
                Bono.OP2_CAN_MULTA = (int)multa_op1.Sum(q => q.Dias);

                Bono.OP1_PREMIO = premio_op1.Count * param.OP1_PREMIO;
                Bono.OP1_MULTA = (decimal)multa_op1.Sum(q=> q.Dias) * param.OP1_MULTA;
                
                #endregion

                #region Opción 2
                decimal premio_op2 = db.DET_BITACORA.AsEnumerable().Where(q => q.ID == bitacora.ID && q.LINEA_FECCUMPLI1 != null && param.OP2_INICIO <= (bitacora.FECHA_ZARPE.Value.Date - q.LINEA_FECCUMPLI1.Value.Date).Days && (bitacora.FECHA_ZARPE.Value.Date - q.LINEA_FECCUMPLI1.Value.Date).Days <= param.OP2_FIN).Count();

                Bono.OP2_PREMIO = premio_op2 * param.OP2_PREMIO;
                #endregion

                #region Opcion 3
                decimal TotalObras = db.DET_BITACORA.Where(q => q.ID == bitacora.ID && q.LINEA_STCUMPLI2 != null && !q.DESCRIPCION.Contains("REASIGNADA")).Count();
                decimal op3_verde = db.DET_BITACORA.Where(q => q.ID == bitacora.ID && q.LINEA_STCUMPLI2 == "P").Count();
                Bono.OPCION3_VERDE += "("+op3_verde.ToString()+"):";
                decimal op3_azul = db.DET_BITACORA.Where(q => q.ID == bitacora.ID && q.LINEA_STCUMPLI2 == "T").Count();
                Bono.OPCION3_AZUL += "(" + op3_azul.ToString() + "):";
                decimal op3_rojo = db.DET_BITACORA.Where(q => q.ID == bitacora.ID && q.LINEA_STCUMPLI2 == "X").Count();
                Bono.OPCION3_ROJO += "(" + op3_rojo.ToString() + "):";

                decimal ValorPorObra = Math.Round(param.OP3_TOTAL / TotalObras,2,MidpointRounding.AwayFromZero);

                param.OP3_ROJO = param.OP3_ROJO ?? 0;

                Bono.OP3_VERDE = Math.Round(ValorPorObra * op3_verde,2,MidpointRounding.AwayFromZero);
                Bono.OP3_AZUL = Math.Round(Convert.ToDecimal(((double)param.OP3_TOTAL * ((double)param.OP3_AZUL/100.00))) * op3_azul,2,MidpointRounding.AwayFromZero);
                Bono.OP3_ROJO = Math.Round(Convert.ToDecimal(((double)param.OP3_TOTAL * ((double)param.OP3_ROJO / 100.00))) * op3_rojo, 2, MidpointRounding.AwayFromZero);
                #endregion

                #region Opcion 4
                var HorasOP4 = (Bono.FECHA_ZARPE - Bono.FECHA_ZARPE_REAL).Value.TotalHours;
                //Valido que sean 24 horas
                var DiasOP4 = Convert.ToDecimal(Math.Truncate(HorasOP4 / 24));
                var OP4 = db.CAB_BITACORA_PARAM_OP4.Where(q => q.ID == bitacora.ID && q.OP4_INI <= DiasOP4 && DiasOP4 <= q.OP4_FIN).FirstOrDefault();
                if(OP4 != null)
                {
                    //Resto el porcentaje que salga
                    if (OP4.OP4_POR != null)
                    {
                        Bono.OP4_PREMIO = param.OP4_COMISION - Convert.ToDecimal((Convert.ToDouble(OP4.OP4_POR) / 100.00) * Convert.ToDouble(param.OP4_COMISION));
                        Bono.OP2_PREMIO = Bono.OP2_PREMIO - Convert.ToDecimal((Convert.ToDouble(OP4.OP4_POR) / 100.00) * Convert.ToDouble(Bono.OP2_PREMIO));
                    }
                    //Sumo el valor que tenga parametrizado
                    else
                        if (OP4.OP4_VALOR != null)
                    {
                        Bono.OP4_PREMIO = Convert.ToDecimal(param.OP4_COMISION + (OP4.OP4_VALOR ?? 0));
                        Bono.OP2_PREMIO = Bono.OP2_PREMIO + (OP4.OP4_VALOR ?? 0);
                    }
                    else
                        Bono.OP4_PREMIO = param.OP4_COMISION;
                }
                #endregion
                Bono.OP1_MULTA = Math.Abs(Bono.OP1_MULTA);
                Bono.TOTAL = Bono.OP1_PREMIO - Bono.OP1_MULTA + Bono.OP2_PREMIO + Bono.OP3_VERDE - Bono.OP3_AZUL - Bono.OP3_ROJO + Bono.OP4_PREMIO;

                Bono.FECHA_ARRIBO = bitacora.FECHA_ARRIBO;
                Bono.FECHA_ZARPE = bitacora.FECHA_ZARPE;
                Bono.FECHA_ZARPE_REAL = bitacora.FECHA_ZARPE_REAL;

                Bono.COLOR = Bono.TOTAL > 0 ? "Green" : (Bono.TOTAL == 0 ? "Black" : "Red");

                return Bono;
            }
            catch (Exception ex)
            {
                using (EntitiesGeneral Error = new EntitiesGeneral())
                {
                    long ID = Error.APP_LOGERROR.Count() > 0 ? (Error.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                    Error.APP_LOGERROR.Add(new APP_LOGERROR
                    {
                        ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                        INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                        FECHA = DateTime.Now,
                        PROCESO = "Bono/GET",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                }
                return new BonoModel();
            }
        }
    }
}
