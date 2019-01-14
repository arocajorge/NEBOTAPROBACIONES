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
        public IEnumerable<BitacoraModel> GET(string BARCO = "", string VIAJE = "", string MOSTRARTODO = "")
        {
            try
            {
                List<BitacoraModel> lst = new List<BitacoraModel>();
                if (string.IsNullOrEmpty(MOSTRARTODO))
                {
                    lst =
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
                      PENTIENTEJEFE = grouping.Where(q => string.IsNullOrEmpty(q.STCUMPLI1)).Count(),
                      PENTIENTESUPERVISOR = grouping.Where(q => string.IsNullOrEmpty(q.STCUMPLI2)).Count(),
                      LINEA = grouping.Key,
                      LINEA_FECCUMPLI1 = grouping.Max(q => q.LINEA_FECCUMPLI1),
                      LINEA_FECCUMPLI2 = grouping.Max(q => q.LINEA_FECCUMPLI2),
                      LINEA_STCUMPLI1 = grouping.Max(q => q.LINEA_STCUMPLI1),
                      LINEA_STCUMPLI2 = grouping.Max(q => q.LINEA_STCUMPLI2),
                      NUMERO_ORDEN = grouping.Max(q => q.NUMERO_ORDEN),
                      FECHA_ARRIBO = grouping.Max(q=> q.FECHA_ARRIBO),
                      FECHA_ZARPE = grouping.Max(q => q.FECHA_ZARPE),
                      FECHA_ZARPE_REAL = grouping.Max(q => q.FECHA_ZARPE_REAL),
                  }).ToList();
                    lst.ForEach(q =>
                    {
                        q.CUMPLIJEFE = (q.CANTIDADLINEAS - q.PENTIENTEJEFE) == 0 ? true : false;
                        q.CUMPLISUP = (q.CANTIDADLINEAS - q.PENTIENTESUPERVISOR) == 0 ? true : false;
                    });
                }else
                {
                    lst = db.VW_BITACORAS_APP.Where(q => q.BARCO == BARCO && q.VIAJE == VIAJE && q.ESTADO == "A").Select(q =>
                      new BitacoraModel
                      {
                          ID = q.ID,
                          DESCRIPCION = q.DESCRIPCION,
                          CONTRATISTA = q.CONTRATISTA,
                          NOM_BARCO = q.NOM_BARCO,
                          NOM_VIAJE = q.NOM_VIAJE,
                          LINEA = q.LINEA,
                          LINEA_FECCUMPLI1 = q.LINEA_FECCUMPLI1,
                          LINEA_FECCUMPLI2 = q.LINEA_FECCUMPLI2,
                          LINEA_STCUMPLI1 = q.LINEA_STCUMPLI1,
                          LINEA_STCUMPLI2 = q.LINEA_STCUMPLI2,
                          NUMERO_ORDEN = q.NUMERO_ORDEN,
                          VALOR = q.VALOR,
                          ST_EMPLEADO = q.ST_EMPLEADO,
                          FECHA_ARRIBO = q.FECHA_ARRIBO,
                          FECHA_ZARPE = q.FECHA_ZARPE,
                          FECHA_ZARPE_REAL = q.FECHA_ZARPE_REAL
                      }).ToList();

                    lst.ForEach(q => {
                        q.CONTRATISTA = q.ST_EMPLEADO == 1 ? ("EMPLEADO: " + q.CONTRATISTA) : ("CONTRATISTA: " + q.CONTRATISTA);
                        q.NUMERO_ORDEN = string.IsNullOrEmpty(q.NUMERO_ORDEN) ? "" : ("OT." + q.NUMERO_ORDEN); });
                }

                return lst;
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
                        PROCESO = "OrdenTrabajo/POST",
                        SECUENCIA = ID
                    });
                    Error.SaveChanges();
                }
                return new List<BitacoraModel>();
            }            
        }

        public void Post([FromBody]BitacoraModel value)
        {
            try
            {                
                var linea = db.DET_BITACORA.Where(q => q.ID == value.ID && q.LINEA == value.LINEA).FirstOrDefault();
                if (linea == null)
                    return;

                var usuario = db.USUARIOS.Where(q => q.USUARIO.ToLower() == value.LOGIN.ToLower()).FirstOrDefault();
                if (usuario == null)
                    return;                

                if (usuario.ROL_APRO == "J")
                {
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

                ActualizarOrdenes(value.ID,value.LINEA,usuario.ROL_APRO, usuario.USUARIO);
            }
            catch (Exception ex)
            {
                long SECUENCIAID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "BitacorasJefeSup/POST",
                    SECUENCIA = SECUENCIAID
                });
                db.SaveChanges();
            }
        }

        private bool ActualizarOrdenes(int ID, int LINEA, string ROL_APRO, string CINV_LOGIN)
        {
            try
            {
                var ordenes = db.VW_BITACORAS_APP.Where(q => q.ID == ID && q.LINEA == LINEA).ToList();
                foreach (var orden in ordenes)
                {
                    var bitacoras = db.VW_BITACORAS_APP.Where(q => q.NUMERO_ORDEN == orden.NUMERO_ORDEN).ToList();
                    int ContAnuladas = ROL_APRO == "J" ? bitacoras.Where(q => q.LINEA_STCUMPLI1 == "X").Count() : bitacoras.Where(q => q.LINEA_STCUMPLI2 == "X").Count();

                    if (ContAnuladas > 0)
                    {
                        var OrdenAnular = db.TBCINV.Where(q => q.CINV_TDOC == "OT" && q.CINV_NUM.ToString() == orden.NUMERO_ORDEN).FirstOrDefault();
                        if (OrdenAnular != null)
                        {
                            if (ROL_APRO == "J")
                            {
                                OrdenAnular.CINV_FECCUMPLI1 = DateTime.Now;
                                OrdenAnular.CINV_STCUMPLI1 = "X";
                                OrdenAnular.CINV_LOGINCUMPLI1 = CINV_LOGIN;
                                OrdenAnular.CINV_COMENCUMPLI1 = "Incumplido por bitácora";
                            }
                            else
                            {
                                OrdenAnular.CINV_FECCUMPLI2 = DateTime.Now;
                                OrdenAnular.CINV_STCUMPLI2 = "X";
                                OrdenAnular.CINV_LOGINCUMPLI2 = CINV_LOGIN;
                                OrdenAnular.CINV_COMENCUMPLI2 = "Incumplido por bitácora";
                            }
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        int ContAprobadas = ROL_APRO == "J" ? bitacoras.Where(q => q.LINEA_STCUMPLI1 == "P").Count() : bitacoras.Where(q => q.LINEA_STCUMPLI2 == "P" || q.LINEA_STCUMPLI2 == "T").Count();
                        int ContPendientes = ROL_APRO == "J" ? bitacoras.Where(q => q.LINEA_STCUMPLI1 == "A" || string.IsNullOrEmpty(q.LINEA_STCUMPLI1)).Count() : bitacoras.Where(q => q.LINEA_STCUMPLI2 == "A" || string.IsNullOrEmpty(q.LINEA_STCUMPLI2)).Count();
                        int ContTotal = bitacoras.Count();

                        int NUMERO_ORDEN = string.IsNullOrEmpty(orden.NUMERO_ORDEN) ? 0 : Convert.ToInt32(orden.NUMERO_ORDEN);

                        var OrdenEntity = db.TBCINV.Where(q => q.CINV_TDOC == "OT" && q.CINV_NUM == NUMERO_ORDEN).FirstOrDefault();
                        if (OrdenEntity != null)
                        {
                            if (ROL_APRO == "J")
                            {
                                OrdenEntity.CINV_FECCUMPLI1 = DateTime.Now;
                                OrdenEntity.CINV_LOGINCUMPLI1 = CINV_LOGIN;
                                OrdenEntity.CINV_STCUMPLI1 = (ContTotal == ContAprobadas) ? "P" : "A";
                                OrdenEntity.CINV_COMENCUMPLI1 = ((ContTotal == ContAprobadas) ? "Cumplido " : "Pendiente ") +"por bitácora";
                            }
                            else
                            {
                                OrdenEntity.CINV_FECCUMPLI2 = DateTime.Now;
                                OrdenEntity.CINV_LOGINCUMPLI2 = CINV_LOGIN;
                                OrdenEntity.CINV_STCUMPLI2 = (ContTotal == ContAprobadas) ? "P" : "A";
                                OrdenEntity.CINV_COMENCUMPLI2= ((ContTotal == ContAprobadas) ? "Cumplido " : "Pendiente ") + "por bitácora";
                            }

                            db.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                using (EntitiesGeneral Error = new EntitiesGeneral())
                {
                    long SECUENCIAID = Error.APP_LOGERROR.Count() > 0 ? (Error.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                    Error.APP_LOGERROR.Add(new APP_LOGERROR
                    {
                        ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                        INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                        FECHA = DateTime.Now,
                        PROCESO = "BitacorasJefeSup/POST/ActualizarOrdenes",
                        SECUENCIA = SECUENCIAID
                    });

                    Error.SaveChanges();
                }
                db.SaveChanges();
                return false;
            }
        }
        
    }
}
