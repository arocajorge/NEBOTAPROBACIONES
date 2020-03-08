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
                List<ProveedorModel> Lista = new List<ProveedorModel>();

                var lst = (from q in db.ARPROVEEDOR
                          join c in db.ARPROVEEDOR_COLOR
                          on q.CODIGO equals c.CODIGO into col
                          from colo in col.DefaultIfEmpty()
                          where q.ESTADO == "Activo"
                          select new
                          {
                              Proveedor = q,
                              Color = colo == null ? "Black" : colo.COLOR
                          }).ToList();

                foreach (var q in lst)
                {
                    q.Proveedor.CEDULA = string.IsNullOrEmpty(q.Proveedor.CEDULA) ? string.Empty : q.Proveedor.CEDULA.Trim();
                    q.Proveedor.RUC = string.IsNullOrEmpty(q.Proveedor.RUC) ? string.Empty : q.Proveedor.RUC.Trim();
                    q.Proveedor.APELLIDO = string.IsNullOrEmpty(q.Proveedor.APELLIDO) && q.Proveedor.APELLIDO.Trim().Length > 2 ? string.Empty : q.Proveedor.APELLIDO.Trim();
                    q.Proveedor.NOMBRE = string.IsNullOrEmpty(q.Proveedor.NOMBRE) ? string.Empty : q.Proveedor.NOMBRE.Trim();
                    q.Proveedor.NOMBRE += " " + q.Proveedor.APELLIDO;
                    q.Proveedor.NOMBRE = q.Proveedor.NOMBRE.Trim();

                    Lista.Add(new ProveedorModel
                    {
                        CODIGO = q.Proveedor.CODIGO,
                        CEDULA = q.Proveedor.CEDULA,
                        RUC = q.Proveedor.RUC,
                        APELLIDO = q.Proveedor.APELLIDO,
                        NOMBRE = q.Proveedor.NOMBRE,
                        DURACION = q.Proveedor.DURACION,
                        E_MAIL = q.Proveedor.E_MAIL,
                        TELEFONOS = q.Proveedor.TELEFONOS,
                        IDENTIFICACION = q.Proveedor.CEDULA != "." && q.Proveedor.CEDULA.Trim().Length > 3 ? q.Proveedor.CEDULA : q.Proveedor.RUC,
                        Color = q.Color
                    });
                }

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
