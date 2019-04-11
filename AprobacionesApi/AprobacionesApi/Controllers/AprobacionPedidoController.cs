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
    public class AprobacionPedidoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public PedidoModel GET()
        {
            try
            {
                PedidoModel model = db.VW_PEDIDOS_APP.Where(q => q.PED_ST == "A").Select(q => new PedidoModel
                {
                    PED_ID = q.PED_ID,
                    PED_FECHA = q.PED_FECHA,
                    PED_BOD = q.PED_BOD,
                    PED_VIAJE = q.PED_VIAJE,
                    PED_SUC = q.PED_SUC,
                    PED_SOL = q.PED_SOL,
                    PED_OBS = q.PED_OBS,
                    PED_ST = q.PED_ST,
                    OC_FECHA = q.OC_FECHA,
                    INV_FECHA = q.INV_FECHA,
                    NOM_BODEGA = q.NOM_BODEGA,
                    NOM_VIAJE = q.NOM_VIAJE,
                    NOM_SUCURSAL = q.NOM_SUCURSAL,
                    NOM_EMPLEADO = q.NOM_EMPLEADO,
                    COLOR = q.COLOR,
                    ESTADO = q.ESTADO,
                    PED_COM = q.PED_COM,
                    PED_FECAPRO = q.PED_FECAPRO
                }).FirstOrDefault();

                model = model ?? new PedidoModel { PED_FECHA = DateTime.Now };
                return model;
            }
            catch (Exception ex)
            {
                using (EntitiesGeneral error = new EntitiesGeneral())
                {
                    long SECUENCIAID = error.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                    error.APP_LOGERROR.Add(new APP_LOGERROR
                    {
                        ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                        INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                        FECHA = DateTime.Now,
                        PROCESO = "AprobacionPedido/GET",
                        SECUENCIA = SECUENCIAID
                    });
                    error.SaveChanges();
                    return new PedidoModel();
                }
            }
        }

        public void POST([FromBody]PedidoModel value)
        {
            try
            {
                var model = db.TBCPED.Where(q => q.PED_ID == value.PED_ID).FirstOrDefault();
                if (model == null) return;

                model.PED_ST = value.PED_ST;
                model.PED_FECAPRO = DateTime.Now;
                model.PED_COM = value.PED_COM;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                using (EntitiesGeneral error = new EntitiesGeneral())
                {
                    long SECUENCIAID = error.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                    error.APP_LOGERROR.Add(new APP_LOGERROR
                    {
                        ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                        INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                        FECHA = DateTime.Now,
                        PROCESO = "AprobacionPedido/POST",
                        SECUENCIA = SECUENCIAID
                    });
                    error.SaveChanges();                    
                }
            }
        }
    }
}
