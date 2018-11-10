using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class CreacionOrdenTrabajoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();

        public List<OrdenModel> GET(string CINV_LOGIN = "")
        {
            try
            {
                List<OrdenModel> Lista;

                Lista = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(q => q.CINV_ST == "A" && q.CINV_LOGIN == CINV_LOGIN).Select(q => new OrdenModel
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

                    CINV_STCUMPLI1 = (q.CINV_STCUMPLI1 == null || q.CINV_STCUMPLI1 == "A") ? "Pendiente" : (q.CINV_STCUMPLI1 == "P" ? "Aprobado" : "Anulado"),
                    CINV_STCUMPLI2 = (q.CINV_STCUMPLI2 == null || q.CINV_STCUMPLI2 == "A") ? "Pendiente" : (q.CINV_STCUMPLI2 == "P" ? "Aprobado" : "Anulado"),

                    CINV_NOMID = q.CINV_NOMID,
                    CINV_TDOC = q.CINV_TDOC,
                    CINV_COM3 = q.CINV_COM3,
                    CINV_COM4 = q.CINV_COM4,
                    VALOR_OC = q.VALOR_OC,
                    NOM_CENTROCOSTO = q.NOM_CENTROCOSTO,
                }).ToList();

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                Lista.ForEach(q => { q.VALOR_OT = q.CINV_TDOC == "OT" ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : 0; });

                return Lista.OrderBy(q => q.CINV_NUM).ToList();
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "CreacionOrdenTrabajo/GET",
                    SECUENCIA = ID
                });
                db.SaveChanges();
                return new List<OrdenModel>();
            }
        }

        public void Post([FromBody]OrdenModel value)
        {
            try
            {
                if (value.CINV_NUM  == 0)
                {
                    int CINV_SEC = db.TBCINV.Count() > 0 ? (db.TBCINV.Select(q => q.CINV_SEC).Max() + 1) : 1;
                    
                    db.TBCINV.Add(new TBCINV
                    {
                        CINV_SEC = value.CINV_SEC = GETID("SEQCINV",1),
                        CINV_TDOC = "OT",
                        CINV_NUM = value.CINV_NUM = GETID("ORDTRA",1),
                        CINV_BOD = value.CINV_BOD,
                        CINV_TBOD = value.CINV_TBOD,
                        CINV_REF = value.CINV_REF,
                        CINV_FECING = value.CINV_FECING,
                        CINV_FPAGO = value.CINV_FPAGO,
                        CINV_ID = value.CINV_ID,
                        CINV_COM1 = value.CINV_COM1,
                        CINV_DSC = value.CINV_DSC,
                        CINV_COM2 = value.CINV_COM2,
                        CINV_COM3 = value.CINV_COM3,
                        CINV_COM4 = value.CINV_COM4,
                        CINV_TASA = value.CINV_TASA,
                        CINV_LOGIN = value.CINV_LOGIN,
                        CINV_ST = "A",
                        CINV_FECLLEGADA = value.CINV_FECLLEGADA,
                        CINV_TDIV = "D",
                        CODIGOTR = value.CODIGOTR,
                        CINV_TIPRECIO = value.CINV_TIPRECIO,
                        EMPRESA = 1,
                        CINV_STCUMPLI1 = "A",
                        CINV_STCUMPLI2 = "A",
                    });
                    int SecuenciaDet = GETID("SEQDINV", value.lst.Count);
                    short Linea = 1;
                    foreach (var item in value.lst)
                    {
                        db.TBDINV.Add(new TBDINV
                        {
                            DINV_SEC = SecuenciaDet++,
                            DINV_CTINV = value.CINV_SEC,
                            DINV_LINEA = Linea++,
                            DINV_ITEM = 0,
                            DINV_CANT = 0,
                            DINV_DSC = 0,
                            DINV_VTA = 0,
                            DINV_COS = 0,
                            DINV_IVA = 0,
                            DINV_PRCT_DSC = 0,
                            DINV_DETALLEDSCTO = item.DINV_DETALLEDSCTO,
                        });
                    }

                }else
                {
                    var orden = db.TBCINV.Where(q => q.CINV_TDOC == value.CINV_TDOC && q.CINV_NUM == value.CINV_NUM).FirstOrDefault();
                    if(orden != null)
                    {
                        orden.CINV_BOD = value.CINV_BOD;
                        orden.CINV_TBOD = value.CINV_TBOD;
                        orden.CINV_REF = value.CINV_REF;
                        orden.CINV_FECING = value.CINV_FECING;
                        orden.CINV_FPAGO = value.CINV_FPAGO;
                        orden.CINV_ID = value.CINV_ID;
                        orden.CINV_COM1 = value.CINV_COM1;
                        orden.CINV_DSC = value.CINV_DSC;
                        orden.CINV_COM2 = value.CINV_COM2;
                        orden.CINV_COM3 = value.CINV_COM3;
                        orden.CINV_COM4 = value.CINV_COM4;
                        orden.CINV_TASA = value.CINV_TASA;
                        orden.CINV_LOGIN = value.CINV_LOGIN;
                        orden.CINV_ST = "A";
                        orden.CINV_FECLLEGADA = value.CINV_FECLLEGADA;
                        orden.CINV_TDIV = "D";
                        orden.CODIGOTR = value.CODIGOTR;
                        orden.CINV_TIPRECIO = value.CINV_TIPRECIO;

                        var lista = db.TBDINV.Where(q => q.DINV_CTINV == orden.CINV_SEC).ToList();
                        foreach (var item in lista)
                        {
                            db.TBDINV.Remove(item);
                        }
                        int SecuenciaDet = GETID("SEQDINV", value.lst.Count);
                        short Linea = 1;
                        foreach (var item in value.lst)
                        {
                            db.TBDINV.Add(new TBDINV
                            {
                                DINV_SEC = SecuenciaDet++,
                                DINV_CTINV = orden.CINV_SEC,
                                DINV_LINEA = Linea++,
                                DINV_ITEM = 0,
                                DINV_CANT = 0,
                                DINV_DSC = 0,
                                DINV_VTA = 0,
                                DINV_COS = 0,
                                DINV_IVA = 0,
                                DINV_PRCT_DSC = 0,
                                DINV_DETALLEDSCTO = item.DINV_DETALLEDSCTO,
                            });
                        }
                    }
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "CreacionOrdenTrabajo/POST",
                    SECUENCIA = ID
                });
                db.SaveChanges();
            }
        }

        private int GETID(string DOC_SGL, int ITEMS)
        {
            try
            {
                int ID = 1;

                var secuencia = db.TBBI_SECDOC.Where(q => q.DOC_SGL == DOC_SGL).FirstOrDefault();
                if (secuencia != null)
                {
                    ID = secuencia.DOC_NUM;
                    secuencia.DOC_NUM = ID + ITEMS;
                    db.SaveChanges();
                }

                return ID;
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "CreacionOrdenTrabajo/POST/GETID",
                    SECUENCIA = ID
                });
                db.SaveChanges();
                return 0;
            }
        }
    }
}
