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
                    NOMBRE = q.NOMBRE,
                    DURACION = q.DURACION,
                    E_MAIL = q.E_MAIL,
                    TELEFONOS = q.TELEFONOS
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

        public ProveedorModel GET(string CODIGO)
        {
            try
            {
                ProveedorModel model;

                model = db.ARPROVEEDOR.Where(q => q.CODIGO == CODIGO).Select(q =>
                new ProveedorModel
                {
                    CODIGO = q.CODIGO,
                    NOMBRE = q.NOMBRE,
                    APELLIDO = q.APELLIDO,
                    CEDULA = q.CEDULA,
                    RUC = q.RUC,
                    DURACION = q.DURACION
                }).FirstOrDefault();

                if (model != null)
                {
                    model.CEDULA = string.IsNullOrEmpty(model.CEDULA) ? string.Empty : model.CEDULA.Trim();
                    model.RUC = string.IsNullOrEmpty(model.RUC) ? string.Empty : model.RUC.Trim();
                    model.IDENTIFICACION = model.CEDULA != "." && model.CEDULA.Trim().Length > 3 ? model.CEDULA : model.RUC;
                    model.APELLIDO = string.IsNullOrEmpty(model.APELLIDO) && model.APELLIDO.Trim().Length > 2 ? string.Empty : model.APELLIDO.Trim();
                    model.NOMBRE = string.IsNullOrEmpty(model.NOMBRE) ? string.Empty : model.NOMBRE.Trim();
                    model.NOMBRE += " " + model.APELLIDO;
                    model.NOMBRE = model.NOMBRE.Trim();

                    decimal? DURACION = 0;
                    var lst = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(q => q.CINV_ID == CODIGO && q.CINV_STCUMPLI1 == "A" && q.CINV_ST == "P" && q.DURACION > 0).ToList();
                    if (lst.Count > 0)
                        lst.ForEach(q => DURACION += q.DURACION);

                    model.DURACIONACUMULADA = DURACION;
                }                

                return model;
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = string.IsNullOrEmpty(ex.Message) ? string.Empty : ex.Message.Substring(0, 1000),
                    INNER = string.IsNullOrEmpty(ex.InnerException.Message) ? string.Empty : ex.InnerException.Message.Substring(0, 1000),
                    FECHA = DateTime.Now,
                    PROCESO = "Proveedor/GET?CODIGO=" + CODIGO,
                    SECUENCIA = ID
                });
                db.SaveChanges();
                return new ProveedorModel();
            }
        }

        public void POST([FromBody]ProveedorModel value)
        {
            try
            {
                var proveedor = db.ARPROVEEDOR.Where(q => q.CODIGO == value.CODIGO).FirstOrDefault();
                if (proveedor != null)
                {
                    proveedor.DURACION = value.DURACION;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = string.IsNullOrEmpty(ex.Message) ? string.Empty : ex.Message.Substring(0, 1000),
                    INNER = string.IsNullOrEmpty(ex.InnerException.Message) ? string.Empty : ex.InnerException.Message.Substring(0, 1000),
                    FECHA = DateTime.Now,
                    PROCESO = "Proveedor/POST",
                    SECUENCIA = ID
                });
                db.SaveChanges();
            }
        }
    }
}
