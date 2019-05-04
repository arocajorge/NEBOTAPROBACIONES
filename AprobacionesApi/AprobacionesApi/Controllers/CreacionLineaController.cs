using AprobacionesApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AprobacionesApi.Models;

namespace AprobacionesApi.Controllers
{
    public class CreacionLineaController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();

        public void POST([FromBody]BitacoraDetModel model)
        {
            try
            {
                var Entity = db.DET_BITACORA.Where(q => q.ID == model.ID && q.LINEA == model.LINEA).FirstOrDefault();
                if (Entity == null)
                {
                    model.LINEA = (short)(db.DET_BITACORA.Where(q => q.ID == model.ID).Count() == 0 ? 1 : db.DET_BITACORA.Where(q => q.ID == model.ID).Max(q => q.LINEA)+1);
                    db.DET_BITACORA.Add(new DET_BITACORA
                    {
                        ID = model.ID,
                        LINEA = model.LINEA,
                        DESCRIPCION = model.DETALLEOT,
                        CONTRATISTA = model.NOMPROVEEDOR,
                        EMPRESA = 1,
                        ST_EMPLEADO = model.ST_EMPLEADO
                    });
                }
                else
                {
                    Entity.DESCRIPCION = model.DETALLEOT;
                    Entity.CONTRATISTA = model.NOMPROVEEDOR;
                    Entity.ST_EMPLEADO = model.ST_EMPLEADO;
                }
                db.SaveChanges();
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
                        PROCESO = "CreacionLinea/POST",
                        SECUENCIA = IDERROR
                    });
                    Error.SaveChanges();
                }
            }
        }
    }
}
