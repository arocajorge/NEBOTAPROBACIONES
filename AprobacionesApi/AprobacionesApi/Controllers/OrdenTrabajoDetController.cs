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
    public class OrdenTrabajoDetController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public void Post([FromBody]OrdenModel value)
        {
            try
            {
                var usuario = db.USUARIOS.Where(q => q.USUARIO.ToLower() == value.CINV_LOGIN.ToLower()).FirstOrDefault();
                if (usuario == null)
                    return;

                var Entity = db.TBCINV.Where(q => q.CINV_NUM == value.CINV_NUM && q.CINV_TDOC == value.CINV_TDOC).FirstOrDefault();
                if (Entity != null)
                {
                    foreach (var item in value.lst)
                    {
                        var detalle = db.TBDINV.Where(q => q.DINV_CTINV == Entity.CINV_SEC && q.DINV_LINEA == item.DINV_LINEA).FirstOrDefault();
                        if (detalle != null)
                            detalle.DINV_ST = item.ESCHATARRA == true ? "S" : "N";
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                long SECUENCIAID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "OrdenTrabajoDet/POST",
                    SECUENCIA = SECUENCIAID
                });
                db.SaveChanges();

            }
        }
    }
}
