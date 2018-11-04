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
                              PENTIENTEJEFE = grouping.Where(q=>string.IsNullOrEmpty(q.STCUMPLI1)).Count(),
                              PENTIENTESUPERVISOR = grouping.Where(q => string.IsNullOrEmpty(q.STCUMPLI2)).Count(),                              
                              LINEA = grouping.Key,
                              LINEA_FECCUMPLI1 = grouping.Max(q => q.LINEA_FECCUMPLI1),
                              LINEA_FECCUMPLI2 = grouping.Max(q => q.LINEA_FECCUMPLI2),
                              LINEA_STCUMPLI1 = grouping.Max(q => q.LINEA_STCUMPLI1),
                              LINEA_STCUMPLI2 = grouping.Max(q => q.LINEA_STCUMPLI2),
                          }).ToList();
            lst.ForEach(q =>
            {
                q.CUMPLIJEFE = (q.CANTIDADLINEAS - q.PENTIENTEJEFE) == 0 ? true : false;
                q.CUMPLISUP = (q.CANTIDADLINEAS - q.PENTIENTESUPERVISOR) == 0 ? true : false;
            });
            return lst;
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
            catch (Exception)
            {
                throw;
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
                        int ContAprobadas = ROL_APRO == "J" ? bitacoras.Where(q => q.LINEA_STCUMPLI1 == "P").Count() : bitacoras.Where(q => q.LINEA_STCUMPLI2 == "P").Count();
                        int ContPendientes = ROL_APRO == "J" ? bitacoras.Where(q => q.LINEA_STCUMPLI1 == "A" || string.IsNullOrEmpty(q.LINEA_STCUMPLI1)).Count() : bitacoras.Where(q => q.LINEA_STCUMPLI2 == "A" || string.IsNullOrEmpty(q.LINEA_STCUMPLI2)).Count();
                        int ContTotal = bitacoras.Count();

                        var OrdenEntity = db.TBCINV.Where(q => q.CINV_TDOC == "OT" && q.CINV_NUM.ToString() == orden.NUMERO_ORDEN).FirstOrDefault();
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
                                OrdenEntity.CINV_STCUMPLI2 = ((ContTotal == ContAprobadas) ? "Cumplido " : "Pendiente ") + "por bitácora";
                            }

                            db.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }
        
    }
}
