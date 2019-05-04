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
    public class PresupuestoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();

        public PresupuestoModel GET(string BARCO = "", string VIAJE = "", string CINV_TDOC = "")
        {
            try
            {
                var Presupuesto = db.VW_PRESUPUESTO_APP.Where(q => q.BARCO == BARCO && q.VIAJE == VIAJE && q.CINV_TDOC == CINV_TDOC).Select(q => new PresupuestoModel
                {
                    BARCO = q.BARCO,
                    VIAJE = q.VIAJE,
                    SALDO = q.SALDO ?? 0,
                    PRESUPUESTO = CINV_TDOC == "OT" ? q.PRESUP_OT : q.PRESUP_OC,
                    APROBADO = q.APROBADO ?? 0,
                    PENDIENTE = q.PENDIENTE ?? 0,
                    CINV_TDOC = q.CINV_TDOC

                }).FirstOrDefault();
                return Presupuesto ?? new PresupuestoModel
                {
                    BARCO = BARCO,
                    VIAJE = VIAJE,
                    CINV_TDOC = CINV_TDOC
                };
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
                        PROCESO = "Presupuesto/GET",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                }
                return new PresupuestoModel
                {
                    BARCO = BARCO,
                    VIAJE = VIAJE,
                    CINV_TDOC = CINV_TDOC
                };
            }
        }
    }
}
