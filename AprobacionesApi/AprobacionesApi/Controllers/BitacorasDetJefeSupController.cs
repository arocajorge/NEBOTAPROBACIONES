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
    public class BitacorasDetJefeSupController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public IEnumerable<BitacoraDetModel> GET(int ID = 0, int LINEA = 0)
        {
            var lst = db.DET_BITACORA2.Where(q => q.ID == ID && q.LINEA == LINEA).Select(q=> new BitacoraDetModel
            {
                ID = q.ID,
                LINEA = q.LINEA,
                LINEA_DETALLE = q.LINEA_DETALLE,
                NUMERO_ORDEN = q.NUMERO_ORDEN,
                VALOR = q.VALOR,
                CUMPLIMIENTO = q.CUMPLIMIENTO,
            }).ToList();
            lst.ForEach(q => {
                q.CINV_NUM = string.IsNullOrEmpty(q.NUMERO_ORDEN) ? 0 : Convert.ToInt32(q.NUMERO_ORDEN);
                q.NOMPROVEEDOR = q.CINV_NUM > 0 ? "" : db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(l => l.CINV_TDOC == "OT" && l.CINV_NUM == q.CINV_NUM).FirstOrDefault().CINV_NOMID;
                q.CINV_COM1 = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(l => l.CINV_TDOC == "OT" && l.CINV_NUM == q.CINV_NUM).FirstOrDefault().CINV_COM1;
            });
            
            return lst;
        }

        public void Post([FromBody]BitacoraDetModel value)
        {
            if (!value.ELIMINAR)
            {
                short LINEA_DETALLE = 1;
                var lst = db.DET_BITACORA2.Where(q => q.ID == value.ID && q.LINEA == value.LINEA).Select(q => q.LINEA_DETALLE).ToList();
                if (lst.Count > 0)
                    LINEA_DETALLE = (short)(lst.Max(q => q) + 1);
                int NUMERO_ORDEN = Convert.ToInt32(value.NUMERO_ORDEN);
                var orden = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(q => q.CINV_TDOC == "OT" && q.CINV_NUM == NUMERO_ORDEN).FirstOrDefault();
                if (orden == null)
                    return;

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                
                decimal VALOR_OT = (orden.CINV_TDOC == "OT") ? (Convert.ToDecimal(orden.CINV_COM3, provider) + Convert.ToDecimal(orden.CINV_COM4, provider)) : (orden.VALOR_OC == null ? 0 : Convert.ToDecimal(orden.VALOR_OC));

                db.DET_BITACORA2.Add(new DET_BITACORA2
                {
                    ID = value.ID,
                    LINEA = value.LINEA,
                    LINEA_DETALLE = LINEA_DETALLE,
                    VALOR = VALOR_OT,
                    NUMERO_ORDEN = value.NUMERO_ORDEN,
                    EMPRESA = 1,
                    CUMPLIMIENTO = 0
                });
            }
            else
            {
                var obj = db.DET_BITACORA2.Where(q => q.ID == value.ID && q.LINEA == value.LINEA && q.NUMERO_ORDEN == value.NUMERO_ORDEN).FirstOrDefault();
                if (obj != null)
                    db.DET_BITACORA2.Remove(obj);
            }

            db.SaveChanges();
        }
    }
}
