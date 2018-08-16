using AprobacionesApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class OrdenTrabajoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public IEnumerable<VW_ORDENES_TRABAJO> GET()
        {
            DateTime Desde = DateTime.Now.Date.AddMonths(-10);
            IEnumerable<VW_ORDENES_TRABAJO> Lista = db.VW_ORDENES_TRABAJO.Where(q => q.CINV_FECING >= Desde && (q.CINV_ST == "P" || q.CINV_ST == "G") && (q.CINV_TDOC == "OT" || q.CINV_TDOC == "OC")).ToList();
            return Lista;
        }
    }
}
