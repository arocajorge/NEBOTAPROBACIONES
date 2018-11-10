using AprobacionesApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class ProveedorController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public List<ARPROVEEDOR> GET()
        {
            try
            {
                List<ARPROVEEDOR> Lista;

                Lista = db.ARPROVEEDOR.Where(q => q.ESTADO == "Activo").ToList();

                return Lista;
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = string.IsNullOrEmpty(ex.Message) ? string.Empty : ex.Message.Substring(0, 1000),
                    INNER = string.IsNullOrEmpty(ex.InnerException.Message) ? string.Empty : ex.InnerException.Message.Substring(0, 1000),
                    FECHA = DateTime.Now,
                    PROCESO = "Proveedor/GET",
                    SECUENCIA = ID
                });
                db.SaveChanges();
                return new List<ARPROVEEDOR>();
            }
        }
    }
}
