using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class OrdenTrabajoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public OrdenModel GET()
        {
            DateTime Desde = DateTime.Now.Date.AddMonths(-10);
            var orden = (from q in db.VW_ORDENES_TRABAJO_TOTAL
                        where q.CINV_FECING >= Desde
                        && (q.CINV_ST == "P" || q.CINV_ST == "G")
                        && (q.CINV_TDOC == "OT" || q.CINV_TDOC == "OC")
                        select new OrdenModel
                        {
                            CINV_NUM = q.CINV_NUM,
                            NOM_SOLICITADO = q.NOM_SOLICITADO,
                            CINV_COM1 = q.CINV_COM1,
                            CODIGO1 = q.CODIGO1,
                            CINV_FPAGO = q.CINV_FPAGO,
                            CINV_FECING = q.CINV_FECING,
                            CODIGOTR = q.CODIGOTR,
                            CINV_BOD = q.CINV_BOD,
                            CINV_ST = q.CINV_ST,
                            CINV_STCUMPLI2 = q.CINV_STCUMPLI2,
                            CINV_NOMID = q.CINV_NOMID,
                            CINV_TDOC = q.CINV_TDOC,
                            CINV_COM3 = q.CINV_COM3,
                            CINV_COM4 = q.CINV_COM4,
                            VALOR_OC = q.VALOR_OC
                        }).FirstOrDefault();

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };
            orden.VALOR_OT = orden.CINV_TDOC == "OT" ? (Convert.ToDecimal(orden.CINV_COM3, provider) + Convert.ToDecimal(orden.CINV_COM4, provider)) : (orden.VALOR_OC == null ? 0 : Convert.ToDecimal(orden.VALOR_OC));

            orden.lst = (from q in db.VW_ORDENES_TRABAJO
                       where q.CINV_TDOC == orden.CINV_TDOC
                       && q.CINV_NUM == orden.CINV_NUM
                       orderby q.DINV_LINEA
                       select new OrdenDetalleModel
                       {
                           CINV_TDOC = q.CINV_TDOC,
                           UNIDAD = q.UNIDAD,
                           DINV_CANT = q.DINV_CANT,
                           DINV_DETALLEDSCTO = q.DINV_DETALLEDSCTO,
                           DINV_COS = q.DINV_COS,
                           DINV_PRCT_DSC = q.DINV_PRCT_DSC,
                           DINV_VTA = q.DINV_VTA,
                           DINV_LINEA = q.DINV_LINEA,
                           CINV_COM3 = q.CINV_COM3,
                           CINV_COM4 = q.CINV_COM4,
                           DINV_IVA = q.DINV_IVA,
                       }).ToList();

            orden.lst.ForEach(q => { q.TOTAL = (q.CINV_TDOC == "OT" ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : q.DINV_COS) + q.DINV_IVA; });

            return orden;
        }
    }
}
