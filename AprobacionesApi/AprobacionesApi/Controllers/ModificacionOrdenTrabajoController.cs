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
    public class ModificacionOrdenTrabajoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();        
        public OrdenModel GET(int CINV_SEC = 0)
        {
            try
            {
                OrdenModel Orden = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(q => q.CINV_SEC == CINV_SEC).Select(q => new OrdenModel
                {
                    CINV_SEC = q.CINV_SEC,

                    CINV_NUM = q.CINV_NUM,
                    CINV_TDOC = q.CINV_TDOC,

                    CINV_COM2 = q.CINV_COM2,
                    NOM_SOLICITADO = q.NOM_SOLICITADO,

                    CINV_FPAGO = q.CINV_FPAGO,
                    NOM_VIAJE = q.NOM_VIAJE,

                    CODIGOTR = q.CODIGOTR,
                    NOM_CENTROCOSTO = q.NOM_CENTROCOSTO,

                    CINV_BOD = q.CINV_BOD,
                    NOM_BODEGA = q.NOM_BODEGA,

                    CINV_COM1 = q.CINV_COM1,
                    CODIGO1 = q.CODIGO1,
                    CINV_FECING = q.CINV_FECING,

                    CINV_ID = q.CINV_ID,
                    CINV_NOMID = q.CINV_NOMID,

                    CINV_ST = q.CINV_ST,
                    CINV_STCUMPLI1 = (q.CINV_STCUMPLI1 == null || q.CINV_STCUMPLI1 == "A") ? "Pendiente" : (q.CINV_STCUMPLI1 == "P" ? "Aprobado" : "Anulado"),
                    CINV_STCUMPLI2 = (q.CINV_STCUMPLI2 == null || q.CINV_STCUMPLI2 == "A") ? "Pendiente" : (q.CINV_STCUMPLI2 == "P" ? "Aprobado" : "Anulado"),
                    CINV_COM3 = q.CINV_COM3,
                    CINV_COM4 = q.CINV_COM4,
                    VALOR_OC = q.VALOR_OC,

                }).FirstOrDefault();

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                if (Orden != null)
                {
                    Orden.VALOR_OT = (Orden.CINV_TDOC == "OT" || Orden.CINV_TDOC == "OK") ? Convert.ToDecimal(Orden.CINV_COM3, provider) + Convert.ToDecimal(Orden.CINV_COM4, provider) : 0;
                    var lst = db.TBDINV.Where(q => q.DINV_CTINV == CINV_SEC).ToList();
                    Orden.CINV_COM1 = Orden.CINV_COM1;
                    int secuencia = 1;
                    foreach (var item in lst)
                    {
                        if (secuencia == 1) Orden.CINV_COM1 = item.DINV_DETALLEDSCTO;
                        else
                            Orden.CINV_COM1 += ("\n" + item.DINV_DETALLEDSCTO);
                        secuencia++;
                    }
                }

                return Orden;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
