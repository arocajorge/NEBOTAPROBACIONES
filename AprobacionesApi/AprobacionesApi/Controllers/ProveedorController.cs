using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class ProveedorController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public List<ProveedorModel> GET()
        {
            try
            {
                List<ProveedorModel> Lista;

                Lista = db.ARPROVEEDOR.Where(q => q.ESTADO == "Activo").Select(q=> new ProveedorModel
                {
                    CODIGO = q.CODIGO,
                    CEDULA = q.CEDULA,
                    RUC = q.RUC,
                    APELLIDO = q.APELLIDO,
                    NOMBRE = q.NOMBRE
                }).ToList();
                Lista.ForEach(q =>
                {
                    q.CEDULA = string.IsNullOrEmpty(q.CEDULA) ? string.Empty : q.CEDULA.Trim();
                    q.RUC = string.IsNullOrEmpty(q.RUC) ? string.Empty : q.RUC.Trim();
                    q.IDENTIFICACION = q.CEDULA != "." && q.CEDULA.Trim().Length > 3 ? q.CEDULA : q.RUC;
                    q.APELLIDO = string.IsNullOrEmpty(q.APELLIDO) && q.APELLIDO.Trim().Length > 2 ? string.Empty : q.APELLIDO.Trim();
                    q.NOMBRE = string.IsNullOrEmpty(q.NOMBRE) ? string.Empty : q.NOMBRE.Trim();
                    q.NOMBRE += " " + q.APELLIDO;
                    q.NOMBRE = q.NOMBRE.Trim();
                });
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
                return new List<ProveedorModel>();
            }
        }
    }
}
