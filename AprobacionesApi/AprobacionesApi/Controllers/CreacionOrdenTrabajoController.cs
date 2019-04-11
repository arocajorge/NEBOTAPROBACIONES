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

                Lista = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(q => q.CINV_ST == "A" && q.CINV_LOGIN == CINV_LOGIN.ToUpper() && (q.CINV_TDOC == "OT" || q.CINV_TDOC == "OK")).Select(q => new OrdenModel
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
                    DURACION = q.DURACION
                }).ToList();

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                Lista.ForEach(q => { q.VALOR_OT = q.CINV_TDOC == "OT" || q.CINV_TDOC == "OK" ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : 0; });

                return Lista.OrderBy(q => q.CINV_NUM).ToList();
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
                        PROCESO = "CreacionOrdenTrabajo/GET",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                    return new List<OrdenModel>();
                }
            }
        }
        
        public void Post([FromBody]OrdenModel value)
        {
            try
            {
                value.lst = new List<OrdenDetalleModel>();
                value.CINV_COM1.Trim();
                string[] arregloString = value.CINV_COM1.Split('\n');
                foreach (var item in arregloString)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        for (int i = 0; i < item.Length; i = i + 254)
                        {
                            value.lst.Add(new OrdenDetalleModel
                            {
                                DINV_DETALLEDSCTO = item.Substring(i, (item.Length - i > 254 ? 254 : (item.Length - i)))
                            });
                        }
                    }                    
                }
                value.CINV_COM1 = arregloString.Count() > 0 ? (arregloString[0].Length > 190 ? arregloString[0].Substring(0,190) : arregloString[0]) : "";

                if (value.CINV_NUM  == 0)
                {                    
                    db.TBCINV.Add(new TBCINV
                    {
                        CINV_SEC = value.CINV_SEC = GETID("SEQCINV",1),
                        CINV_TDOC = value.CINV_TDOC,
                        CINV_NUM = value.CINV_NUM = GETID(value.CINV_TDOC == "OT" ? ("ORDTRA") : "ORDCOCC",1),
                        CINV_BOD = value.CINV_BOD,
                        CINV_TBOD = "O",
                        CINV_REF = "000",
                        CINV_FECING = value.CINV_FECING,
                        CINV_FPAGO = value.CINV_FPAGO,
                        CINV_ID = value.CINV_ID,
                        CINV_NOMID = value.CINV_NOMID,
                        CINV_COM1 = value.CINV_COM1,
                        CINV_DSC = 0,
                        CINV_COM2 = value.CINV_COM2,
                        CINV_COM3 = value.CINV_COM3,
                        CINV_COM4 = value.CINV_COM4,
                        CINV_TASA = 1,
                        CINV_LOGIN = value.CINV_LOGIN.ToUpper(),
                        CINV_ST = "A",
                        CINV_FECLLEGADA = value.CINV_FECING,
                        CINV_TDIV = "D",
                        CODIGOTR = value.CODIGOTR,
                        CINV_TIPRECIO = 0,
                        EMPRESA = 1,
                        CINV_STCUMPLI1 = "A",
                        CINV_STCUMPLI2 = "A",
                        DURACION = value.DURACION
                    });
                    int SecuenciaDet = GETID("SEQDINV", value.lst.Count + 1) - value.lst.Count;
                    short Linea = 1;
                    foreach (var item in value.lst)
                    {
                        if (!string.IsNullOrEmpty(item.DINV_DETALLEDSCTO))
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
                    }

                }else
                {
                    var orden = db.TBCINV.Where(q => q.CINV_TDOC == value.CINV_TDOC && q.CINV_NUM == value.CINV_NUM).FirstOrDefault();
                    if(orden != null)
                    {
                        if (orden.CINV_ST != "A") return;

                        orden.CINV_BOD = value.CINV_BOD;
                        orden.CINV_FECING = value.CINV_FECING;
                        orden.CINV_FPAGO = value.CINV_FPAGO;
                        orden.CINV_ID = value.CINV_ID;
                        orden.CINV_COM1 = value.CINV_COM1;
                        orden.CINV_COM2 = value.CINV_COM2;
                        orden.CINV_COM3 = value.CINV_COM3;
                        orden.CINV_COM4 = value.CINV_COM4;
                        orden.CINV_NOMID = value.CINV_NOMID;
                        orden.CINV_FECLLEGADA = value.CINV_FECING;
                        orden.CODIGOTR = value.CODIGOTR;
                        orden.CINV_TIPRECIO = 0;
                        orden.DURACION = value.DURACION;

                        var lista = db.TBDINV.Where(q => q.DINV_CTINV == orden.CINV_SEC).ToList();
                        foreach (var item in lista)
                        {
                            db.TBDINV.Remove(item);
                        }
                        int SecuenciaDet = GETID("SEQDINV", value.lst.Count +1) - value.lst.Count;
                        short Linea = 1;
                        foreach (var item in value.lst)
                        {
                            if (!string.IsNullOrEmpty(item.DINV_DETALLEDSCTO))
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
                        PROCESO = "CreacionOrdenTrabajo/POST",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                }
            }
        }

        public int GETID(string DOC_SGL, int ITEMS)
        {
            try
            {
                int ID = 1;
                using (EntitiesGeneral Secuenciales = new EntitiesGeneral())
                {
                    var secuencia = Secuenciales.TBBI_SECDOC.Where(q => q.DOC_SGL == DOC_SGL).FirstOrDefault();
                    if (secuencia != null)
                    {
                        ID = secuencia.DOC_NUM + ITEMS;
                        secuencia.DOC_NUM = ID;
                        Secuenciales.SaveChanges();
                    }
                }
                return ID;
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
                        PROCESO = "CreacionOrdenTrabajo/POST/GETID",
                        SECUENCIA = ID
                    });

                    Error.SaveChanges();
                }
                return 0;
            }
        }        
    }
}
