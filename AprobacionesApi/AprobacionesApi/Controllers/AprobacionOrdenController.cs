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
    public class AprobacionOrdenController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public OrdenModel GET(string CINV_LOGIN = "")
        {
            try
            {
                var orden = (from q in db.VW_ORDENES_TRABAJO_TOTAL_APS
                         where (q.CINV_ST == "D")
                         && (q.CINV_TDOC == "OT" || q.CINV_TDOC == "OC" || q.CINV_TDOC == "OK" || q.CINV_TDOC == "OR")
                         orderby q.CINV_FECING
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
                             CINV_STCUMPLI1 = q.CINV_STCUMPLI1,
                             CINV_NOMID = q.CINV_NOMID,
                             CINV_TDOC = q.CINV_TDOC,
                             CINV_COM3 = q.CINV_COM3,
                             CINV_COM4 = q.CINV_COM4,
                             VALOR_OC = q.VALOR_OC,
                             NOM_CENTROCOSTO = q.NOM_CENTROCOSTO,
                             NOM_VIAJE = q.NOM_VIAJE,
                             DURACION = q.DURACION,
                             CINV_PEDIDO = q.CINV_PEDIDO
                         }).FirstOrDefault();

                if (orden == null)
                {

                    db.Database.ExecuteSqlCommand("DELETE TBCINV_APP_ORDENES_SALTADAS WHERE USUARIO ='" + CINV_LOGIN.ToUpper() + "'");

                    orden = (from q in db.VW_ORDENES_TRABAJO_TOTAL_APP
                             where (q.CINV_ST == "D")
                             && (q.CINV_TDOC == "OT" || q.CINV_TDOC == "OC" || q.CINV_TDOC == "OK" || q.CINV_TDOC == "OR")
                             orderby q.CINV_FECING
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
                                 CINV_STCUMPLI1 = q.CINV_STCUMPLI1,
                                 CINV_NOMID = q.CINV_NOMID,
                                 CINV_TDOC = q.CINV_TDOC,
                                 CINV_COM3 = q.CINV_COM3,
                                 CINV_COM4 = q.CINV_COM4,
                                 VALOR_OC = q.VALOR_OC,
                                 NOM_CENTROCOSTO = q.NOM_CENTROCOSTO,
                                 NOM_VIAJE = q.NOM_VIAJE,
                                 DURACION = q.DURACION,
                                 CINV_PEDIDO = q.CINV_PEDIDO
                             }).FirstOrDefault();
                }

                if (orden == null)
                    orden = new OrdenModel { lst = new List<OrdenDetalleModel>(), CINV_TDOC = "", ListaBitacoras = new List<BitacoraModel>() };

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                if (orden.CINV_NUM != 0)
                    orden.VALOR_OT = (orden.CINV_TDOC == "OT" || orden.CINV_TDOC == "OK" || orden.CINV_TDOC == "OR") ? (Convert.ToDecimal(orden.CINV_COM3, provider) + Convert.ToDecimal(orden.CINV_COM4, provider)) : (orden.VALOR_OC == null ? 0 : Convert.ToDecimal(orden.VALOR_OC));

                orden.lst = (from q in db.VW_ORDENES_TRABAJO_APP
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
                                 NOM_VIAJE = q.NOM_VIAJE,
                                 DINV_DSC = q.DINV_DSC,
                                 DINV_ICE = q.DINV_ICE,
                                 ESCHATARRA = q.DINV_ST == "S" ? true : false,
                             }).ToList();
                if (orden.lst != null)
                    orden.lst.ForEach(q => { q.TOTAL = ((orden.CINV_TDOC == "OT" || orden.CINV_TDOC == "OK" || orden.CINV_TDOC == "OR") ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : (q.DINV_COS + (q.DINV_ICE == null ? 0 : Convert.ToDecimal(q.DINV_ICE, provider)))) + q.DINV_IVA; orden.NOM_VIAJE = q.NOM_VIAJE; });

                return orden;
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
                        PROCESO = "AprobacionOrde /GET",
                        SECUENCIA = SECUENCIAID
                    });
                    error.SaveChanges();
                    return new OrdenModel();
                }
            }
        }

        public void POST([FromBody]OrdenModel value)
        {
            try
            {
                var usuario = db.USUARIOS.Where(q => q.USUARIO.ToLower() == value.CINV_LOGIN.ToLower()).FirstOrDefault();
                if (usuario == null)
                    return;

                var Entity = db.TBCINV.Where(q => q.CINV_NUM == value.CINV_NUM && q.CINV_TDOC == value.CINV_TDOC).FirstOrDefault();
                if (Entity != null)
                {
                    Entity.CINV_FECCUMPLIJP = DateTime.Now.Date;
                    Entity.CINV_COMENCUMPLIJP = value.CINV_MOTIVOANULA;
                    Entity.CINV_ST = value.CINV_ST;
                    Entity.CINV_LOGINCUMPLIJP = usuario.USUARIO;
                }

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
                        PROCESO = "AprobacionOrden/POST",
                        SECUENCIA = SECUENCIAID
                    });
                    error.SaveChanges();
                }
            }
        }
    }
}
