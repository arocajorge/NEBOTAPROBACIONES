using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class ControlBitacoraController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public List<BitacoraModel> GET(string BARCO = "", string VIAJE = "" )
        {
            try
            {
                List<BitacoraModel> Lista = new List<BitacoraModel>();

                string where_clause = string.Empty;
                if (!string.IsNullOrEmpty(BARCO))
                    where_clause += "BARCO = \"" + BARCO + "\" ";
                if (!string.IsNullOrEmpty(VIAJE))
                    where_clause += (where_clause.Length > 0 ? "AND " : "") + ("VIAJE = \"" + VIAJE + "\" ");


                if (string.IsNullOrEmpty(BARCO) && string.IsNullOrEmpty(VIAJE))
                    Lista = db.VW_BITACORAS_CUMPLIMIENTO_APP.Select(q => new BitacoraModel
                    {
                        VIAJE = q.VIAJE,
                        BARCO = q.BARCO,
                        NOM_BARCO = q.NOM_BARCO,
                        NOM_VIAJE = q.NOM_VIAJE,
                        ID = q.ID,
                        STCUMPLI_TOTAL = q.STCUMPLI_TOTAL,
                        STCUMPLI_APRO = q.STCUMPLI_APRO,
                        STCUMPLI_JEFE = q.STCUMPLI_JEFE,
                        STCUMPLI_SUP = q.STCUMPLI_SUP,
                        FECHA_ARRIBO = q.FECHA_ARRIBO,
                        FECHA_ZARPE = q.FECHA_ZARPE,
                        FECHA_ZARPE_REAL = q.FECHA_ZARPE_REAL,
                        
                    }).ToList();
                else
                    Lista = db.VW_BITACORAS_CUMPLIMIENTO_APP.Where(where_clause, null).Select(q => new BitacoraModel
                    {
                        VIAJE = q.VIAJE,
                        BARCO = q.BARCO,
                        NOM_BARCO = q.NOM_BARCO,
                        NOM_VIAJE = q.NOM_VIAJE,
                        ID = q.ID,
                        STCUMPLI_TOTAL = q.STCUMPLI_TOTAL,
                        STCUMPLI_APRO = q.STCUMPLI_APRO,
                        STCUMPLI_JEFE = q.STCUMPLI_JEFE,
                        STCUMPLI_SUP = q.STCUMPLI_SUP,
                        FECHA_ARRIBO = q.FECHA_ARRIBO,
                        FECHA_ZARPE = q.FECHA_ZARPE,
                        FECHA_ZARPE_REAL = q.FECHA_ZARPE_REAL
                    }).ToList();

                return Lista;
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
                        PROCESO = "ControlBitacoraController/POST",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                }
                return new List<BitacoraModel>();
            }
        }
    }
}
