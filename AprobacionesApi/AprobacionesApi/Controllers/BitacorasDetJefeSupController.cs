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
            try
            {
                var lst = db.VW_BITACORAS2_APP.Where(q => q.ID == ID && q.LINEA == LINEA).Select(q => new BitacoraDetModel
                {
                    ID = q.ID,
                    LINEA = q.LINEA,
                    LINEA_DETALLE = q.LINEA_DETALLE,
                    NUMERO_ORDEN = q.NUMERO_ORDEN,
                    VALOR = q.VALOR,
                    CUMPLIMIENTO = q.CUMPLIMIENTO,
                    TIPO = q.TIPO,
                    CINV_NUM = (int)(q.CINV_NUM ?? 0),
                    NOMPROVEEDOR = q.NOMPROVEEDOR,
                    CINV_COM1 = q.CINV_COM1,
                    COLOR_ESTADO = q.COLOR_ESTADO,
                    CINV_ST = q.ESTADO
                }).ToList();

                return lst;
            }
            catch (Exception ex)
            {
                long SECUENCIAID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "BitacorasDetJefeSup/GET",
                    SECUENCIA = SECUENCIAID
                });
                db.SaveChanges();
                return new List<BitacoraDetModel>();
            }
        }

        public void Post([FromBody]BitacoraDetModel value)
        {
            try
            {
                if (!value.ELIMINAR)
                {
                    short LINEA_DETALLE = 1;
                    var lst = db.DET_BITACORA2.Where(q => q.ID == value.ID && q.LINEA == value.LINEA).Select(q => q.LINEA_DETALLE).ToList();
                    if (lst.Count > 0)
                        LINEA_DETALLE = (short)(lst.Max(q => q) + 1);

                    int NUMERO_ORDEN = 0;
                    decimal VALOR_OT = 0;

                    if (value.TIPO == "OT")
                    NUMERO_ORDEN = Convert.ToInt32(value.NUMERO_ORDEN);
                    var orden = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(q => q.CINV_TDOC == "OT" && q.CINV_NUM == NUMERO_ORDEN).FirstOrDefault();
                    if (orden != null)
                    {
                        NumberFormatInfo provider = new NumberFormatInfo();
                        provider.NumberDecimalSeparator = ".";
                        provider.NumberGroupSeparator = ",";
                        provider.NumberGroupSizes = new int[] { 3 };

                        VALOR_OT = (orden.CINV_TDOC == "OT") ? (Convert.ToDecimal(orden.CINV_COM3, provider) + Convert.ToDecimal(orden.CINV_COM4, provider)) : (orden.VALOR_OC == null ? 0 : Convert.ToDecimal(orden.VALOR_OC));
                    }
                    db.DET_BITACORA2.Add(new DET_BITACORA2
                    {
                        ID = value.ID,
                        LINEA = value.LINEA,
                        LINEA_DETALLE = LINEA_DETALLE,
                        VALOR = VALOR_OT,
                        NUMERO_ORDEN = value.NUMERO_ORDEN,
                        EMPRESA = 1,
                        CUMPLIMIENTO = 0,
                        TIPO = value.TIPO
                    });
                }
                else
                {
                    var obj = db.DET_BITACORA2.Where(q => q.ID == value.ID && q.LINEA == value.LINEA && q.NUMERO_ORDEN == value.NUMERO_ORDEN && q.TIPO == value.TIPO).FirstOrDefault();
                    if (obj != null)
                        db.DET_BITACORA2.Remove(obj);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                long SECUENCIAID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "BitacorasDetJefeSup/POST",
                    SECUENCIA = SECUENCIAID
                });
                db.SaveChanges();
            }            
        }
    }
}
