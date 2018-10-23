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
                 (from q in db.VW_BITACORAS
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
                          }).ToList();
            lst.ForEach(q =>
            {
                q.CUMPLIJEFE = (q.CANTIDADLINEAS - q.PENTIENTEJEFE) == 0 ? true : false;
                q.CUMPLISUP = (q.CANTIDADLINEAS - q.PENTIENTESUPERVISOR) == 0 ? true : false;
            });
            return lst;
        }        
    }
}
