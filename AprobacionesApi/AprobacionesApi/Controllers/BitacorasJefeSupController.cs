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
    public class BitacorasJefeSupController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public IEnumerable<BitacoraModel> GET(string BARCO = "", string VIAJE = "")
        {
            var lst =
                 (from q in db.VW_BITACORAS_APP
                          where q.VIAJE == VIAJE
                          && q.BARCO == BARCO
                          && q.ESTADO == "A"
                          group q by q.LINEA into grouping
                          select new BitacoraModel
                          {
                              ID = grouping.Max(c => c.ID),
                              DESCRIPCION = grouping.Max(c => c.DESCRIPCION),
                              CONTRATISTA = grouping.Max(c => c.CONTRATISTA),
                              NOM_BARCO = grouping.Max(c => c.NOM_BARCO),
                              NOM_VIAJE = grouping.Max(c => c.NOM_VIAJE),
                              CANTIDADLINEAS = grouping.Where(c => c.LINEA_DETALLE != null).Count(),
                              PENTIENTEJEFE = grouping.Where(q=>string.IsNullOrEmpty(q.STCUMPLI1)).Count(),
                              PENTIENTESUPERVISOR = grouping.Where(q => string.IsNullOrEmpty(q.STCUMPLI2)).Count(),                              
                              LINEA = grouping.Key,
                              LINEA_FECCUMPLI1 = grouping.Max(q => q.LINEA_FECCUMPLI1),
                              LINEA_FECCUMPLI2 = grouping.Max(q => q.LINEA_FECCUMPLI2),
                              LINEA_STCUMPLI1 = grouping.Max(q => q.LINEA_STCUMPLI1),
                              LINEA_STCUMPLI2 = grouping.Max(q => q.LINEA_STCUMPLI2),
                          }).ToList();
            lst.ForEach(q =>
            {
                q.CUMPLIJEFE = (q.CANTIDADLINEAS - q.PENTIENTEJEFE) == 0 ? true : false;
                q.CUMPLISUP = (q.CANTIDADLINEAS - q.PENTIENTESUPERVISOR) == 0 ? true : false;
            });
            return lst;
        }

        public void Post([FromBody]BitacoraModel value)
        {
            try
            {
                bool ActualizarBitacora = false;

                var linea = db.DET_BITACORA.Where(q => q.ID == value.ID && q.LINEA == value.LINEA).FirstOrDefault();
                if (linea == null)
                    return;

                var usuario = db.USUARIOS.Where(q => q.USUARIO.ToLower() == value.LOGIN.ToLower()).FirstOrDefault();
                if (usuario == null)
                    return;                

                if (usuario.ROL_APRO == "J")
                {

                    var ordenes = db.VW_BITACORAS_APP.Where(q => q.ID == value.ID && q.LINEA == value.LINEA).ToList();
                    foreach (var item in ordenes)
                    {
                        //var bitacoras =         
                    }

                    linea.LINEA_FECCUMPLI1 = DateTime.Now;
                    linea.LINEA_STCUMPLI1 = value.ESTADOAPRO;
                    linea.LINEA_LOGINCUMPLI1 = usuario.USUARIO;                    
                }
                else
                {
                    linea.LINEA_FECCUMPLI2 = DateTime.Now;
                    linea.LINEA_STCUMPLI2 = value.ESTADOAPRO;
                    linea.LINEA_LOGINCUMPLI2 = usuario.USUARIO;
                }

                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
