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
    public class CatalogosController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public IEnumerable<CatalogoModel> GET()
        {
            try
            {
                var Lista = (from q in db.SUCURSAL
                             where q.ESTADO == "Vigente"
                             select new CatalogoModel
                             {
                                 Tipo = "Sucursal",
                                 Codigo = q.CODIGO,
                                 Descripcion = q.NOMBRE
                             }).ToList();

                Lista.AddRange((from q in db.ARINDEX
                                where q.DATA == "2"
                                && q.ESTADO == "Vigente"
                                select new CatalogoModel
                                {
                                    Tipo = "Viaje",
                                    Codigo = q.CODIGO,
                                    Descripcion = q.NOMBRE
                                }).ToList());

                Lista.AddRange((from q in db.ARINDEX
                                where q.DATA == "O"
                                && q.ESTADO == "Vigente"
                                select new CatalogoModel
                                {
                                    Tipo = "Bodega",
                                    Codigo = q.CODIGO,
                                    Descripcion = q.NOMBRE
                                }).ToList());
                Lista.AddRange((from q in db.TBNOM_EMPLEADO
                                where q.EMPL_ST == "A"
                                select new CatalogoModel
                                {
                                    Tipo = "Empleado",
                                    Codigo = q.EMPL_CO,
                                    Descripcion = q.EMPL_AP,
                                    Descripcion2 = q.EMPL_NO
                                }).ToList());

                Lista.Where(q => q.Tipo == "Empleado").ToList().ForEach(q => q.Descripcion = q.Descripcion2.Trim() + " " + q.Descripcion.Trim());
                return Lista;
            }
            catch (Exception ex)
            {
                long SECUENCIAID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "Catalogos/GET",
                    SECUENCIA = SECUENCIAID
                });
                db.SaveChanges();
                return new List<CatalogoModel>();
            }
        }
    }
}
