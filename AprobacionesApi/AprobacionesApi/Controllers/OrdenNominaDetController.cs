using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class OrdenNominaDetController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public List<OrdenNominaDetalleModel> GET(int DINV_CTINV = 0)
        {
            try
            {
                List<OrdenNominaDetalleModel> Lista;

                Lista = db.TBNOM_ORDEN_NOMINA_DET.Where(q => q.DINV_CTINV == DINV_CTINV).Select(q => new OrdenNominaDetalleModel
                {
                    DINV_CTINV = q.DINV_CTINV,
                    DINV_LINEA = q.DINV_LINEA,
                    DINV_ST = q.DINV_ST,
                    DINV_DETALLEDSCTO = q.DINV_DETALLEDSCTO,
                }).ToList();

                return Lista;
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "OrdenNominaDet/GET",
                    SECUENCIA = ID
                });
                db.SaveChanges();
                return new List<OrdenNominaDetalleModel>();
            }
        }
    }
}
