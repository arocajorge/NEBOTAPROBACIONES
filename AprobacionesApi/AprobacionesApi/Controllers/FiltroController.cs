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
    public class FiltroController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public FiltroModel GET(string USUARIO = "")
        {
            try
            {
                FiltroModel model = db.VW_FILTROS_APP.Where(q => q.USUARIO.ToLower() == USUARIO.ToLower()).Select(q=> new FiltroModel
                {
                    USUARIO = q.USUARIO,
                    BARCO = q.BARCO,
                    VIAJE = q.VIAJE,
                    SOLICITANTE = q.SOLICITANTE,
                    BODEGA = q.BODEGA,

                    NOMBREBARCO = q.NOMBREBARCO,
                    NOMBREBODEGA = q.NOMBREBODEGA,
                    NOMBRESOLICITANTE = q.NOMBRESOLICITANTE,
                    NOMBREVIAJE = q.NOMBREVIAJE
                }).FirstOrDefault();
                return model ?? new FiltroModel();
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
                        PROCESO = "Filtro/GET",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                    return new FiltroModel();
                }
            }
        }

        public void POST([FromBody]FiltroModel value)
        {
            try
            {
                var Entity = db.TBCINV_FILTROS_APP.Where(q => q.USUARIO == value.USUARIO).FirstOrDefault();
                if (Entity == null)
                    db.TBCINV_FILTROS_APP.Add(new TBCINV_FILTROS_APP
                    {
                        USUARIO = value.USUARIO,
                        BARCO = value.BARCO,
                        BODEGA = value.BODEGA,
                        SOLICITANTE = value.SOLICITANTE,
                        VIAJE = value.VIAJE
                    });
                else
                {
                    Entity.BARCO = value.BARCO;
                    Entity.BODEGA = value.BODEGA;
                    Entity.VIAJE = value.VIAJE;
                    Entity.SOLICITANTE = value.SOLICITANTE;
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
                        PROCESO = "Filtro/POST",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();                    
                }
            }
        }
    }
}
