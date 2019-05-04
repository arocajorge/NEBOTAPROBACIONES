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
    public class CreacionBitacoraController : ApiController
    {
        CreacionOrdenTrabajoController Ot = new CreacionOrdenTrabajoController();
        EntitiesGeneral db = new EntitiesGeneral();
        public void POST([FromBody]BitacoraModel value)
        {
            try
            {
                var Entity = db.CAB_BITACORA.Where(q => q.BARCO == value.BARCO && q.VIAJE == value.VIAJE).FirstOrDefault();
                if (Entity == null)
                {
                    db.CAB_BITACORA.Add(new CAB_BITACORA
                    {
                        ID = Ot.GETID("BITACO", 1),
                        VIAJE = value.VIAJE,
                        DATA_VIAJE = "2",
                        FECINGRESO = DateTime.Now.Date,
                        BARCO = value.BARCO,
                        EMPRESA = 1,
                        ESTADO = "A",
                        LOGIN = value.LOGIN,
                        FECHA_ARRIBO = value.FECHA_ARRIBO,
                        FECHA_ZARPE = value.FECHA_ZARPE,
                        FECHA_ZARPE_REAL = value.FECHA_ZARPE_REAL
                    });
                }
                else
                {
                    Entity.FECHA_ARRIBO = value.FECHA_ARRIBO;
                    Entity.FECHA_ZARPE = value.FECHA_ZARPE;
                    Entity.FECHA_ZARPE_REAL = value.FECHA_ZARPE_REAL;
                }
                db.SaveChanges();
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
                        PROCESO = "CreacionBitacora/GET",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                }
            }
        }
    }
}
