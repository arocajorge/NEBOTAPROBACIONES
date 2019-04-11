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
    public class PedidosController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public List<PedidoModel> GET(decimal PED_ID = 0, string USUARIO = "", string BARCO = "", string VIAJE = "")
        {
            try
            {
                List<PedidoModel> Lista;
                if (PED_ID == 0)
                {
                    if (!string.IsNullOrEmpty(BARCO) && !string.IsNullOrEmpty(VIAJE))
                    {
                        Lista = db.VW_PEDIDOS_APP.Where(q => q.PED_LOGIN.ToLower() == USUARIO.ToLower() && q.PED_SUC == BARCO && q.PED_VIAJE == VIAJE).Select(q => new PedidoModel
                        {
                            PED_ID = q.PED_ID,
                            PED_FECHA = q.PED_FECHA,
                            PED_BOD = q.PED_BOD,
                            PED_VIAJE = q.PED_VIAJE,
                            PED_SUC = q.PED_SUC,
                            PED_SOL = q.PED_SOL,
                            PED_OBS = q.PED_OBS,
                            PED_ST = q.PED_ST,
                            OC_FECHA = q.OC_FECHA,
                            INV_FECHA = q.INV_FECHA,
                            NOM_BODEGA = q.NOM_BODEGA,
                            NOM_VIAJE = q.NOM_VIAJE,
                            NOM_SUCURSAL = q.NOM_SUCURSAL,
                            NOM_EMPLEADO = q.NOM_EMPLEADO,
                            COLOR = q.COLOR,
                            ESTADO = q.ESTADO,
                            PED_COM = q.PED_COM,
                            PED_FECAPRO = q.PED_FECAPRO
                        }).ToList();
                    }
                    else
                    Lista = db.VW_PEDIDOS_APP.Where(q => q.PED_LOGIN.ToLower() == USUARIO.ToLower()).Select(q => new PedidoModel
                    {
                        PED_ID = q.PED_ID,
                        PED_FECHA = q.PED_FECHA,
                        PED_BOD = q.PED_BOD,
                        PED_VIAJE = q.PED_VIAJE,
                        PED_SUC = q.PED_SUC,
                        PED_SOL = q.PED_SOL,
                        PED_OBS = q.PED_OBS,
                        PED_ST = q.PED_ST,
                        OC_FECHA = q.OC_FECHA,
                        INV_FECHA = q.INV_FECHA,
                        NOM_BODEGA = q.NOM_BODEGA,
                        NOM_VIAJE = q.NOM_VIAJE,
                        NOM_SUCURSAL = q.NOM_SUCURSAL,
                        NOM_EMPLEADO = q.NOM_EMPLEADO,
                        COLOR = q.COLOR,
                        ESTADO = q.ESTADO,
                        PED_COM = q.PED_COM,
                        PED_FECAPRO = q.PED_FECAPRO
                    }).ToList();
                }
                else
                    Lista = db.VW_PEDIDOS_APP.Where(q => q.PED_ID == PED_ID).Select(q => new PedidoModel
                    {
                        PED_ID = q.PED_ID,
                        PED_FECHA = q.PED_FECHA,
                        PED_BOD = q.PED_BOD,
                        PED_VIAJE = q.PED_VIAJE,
                        PED_SUC = q.PED_SUC,
                        PED_SOL = q.PED_SOL,
                        PED_OBS = q.PED_OBS,
                        PED_ST = q.PED_ST,
                        OC_FECHA = q.OC_FECHA,
                        INV_FECHA = q.INV_FECHA,
                        NOM_BODEGA = q.NOM_BODEGA,
                        NOM_VIAJE = q.NOM_VIAJE,
                        NOM_SUCURSAL = q.NOM_SUCURSAL,
                        NOM_EMPLEADO = q.NOM_EMPLEADO,
                        COLOR = q.COLOR,
                        ESTADO = q.ESTADO,
                        PED_COM = q.PED_COM,
                        PED_FECAPRO = q.PED_FECAPRO
                    }).ToList();

                return Lista;
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
                        PROCESO = "Pedidos/GET",
                        SECUENCIA = SECUENCIAID
                    });
                    error.SaveChanges();
                    return new List<PedidoModel>();
                }
            }
        }

        public void POST([FromBody]PedidoModel value)
        {
            try
            {
                #region Validaciones
                if (value == null)
                    return;

                if (string.IsNullOrEmpty(value.PED_OBS))
                    return;

                if (value.PED_OBS.Trim().Length == 0)
                    return;
                #endregion

                #region Obtener detalle
                decimal Cantidad = 0;
                List<PedidoDetModel> LstDet = new List<PedidoDetModel>();
                string[] Lineas = value.PED_OBS.Trim().Split('\n');
                foreach (var item in Lineas)
                {
                    string[] can = item.Trim().Split(' ');
                    if (can != null && can.Count() > 0 && !decimal.TryParse(can[0], out Cantidad))
                        Cantidad = 0;
                    LstDet.Add(new PedidoDetModel
                    {
                        PED_CANT = Cantidad,
                        PED_DETALLE = item
                    });
                }
                value.LstDet = LstDet;
                #endregion

                if (value.PED_ID == 0)
                {
                    var Model = new TBCPED
                    {
                        PED_ID = value.PED_ID = new CreacionOrdenTrabajoController().GETID("PED", 1),
                        PED_FECHA = value.PED_FECHA,
                        PED_BOD = value.PED_BOD,
                        PED_VIAJE = value.PED_VIAJE,
                        PED_SUC = value.PED_SUC,
                        PED_SOL = value.PED_SOL,
                        PED_FECTRAN = DateTime.Now,
                        PED_LOGIN = value.PED_LOGIN,
                        PED_OBS = value.PED_OBS,
                        PED_ST = "A"
                    };
                    int Secuencia = 1;
                    db.TBCPED.Add(Model);
                    foreach (var item in value.LstDet)
                    {
                        db.TBDPED.Add(new TBDPED
                        {
                            PED_ID = Model.PED_ID,
                            PED_SEC = Secuencia++,
                            PED_CANT = item.PED_CANT,
                            PED_DETALLE = item.PED_DETALLE
                        });
                    }
                }else
                {
                    var entity = db.TBCPED.Where(q => q.PED_ID == value.PED_ID).FirstOrDefault();
                    if (entity.PED_ST != "A")
                        return;

                    if (entity != null)
                    {
                        entity.PED_FECHA = value.PED_FECHA;
                        entity.PED_BOD = value.PED_BOD;
                        entity.PED_VIAJE = value.PED_VIAJE;
                        entity.PED_SUC = value.PED_SUC;
                        entity.PED_SOL = value.PED_SOL;
                        entity.PED_OBS = value.PED_OBS;
                    }
                    var lst = db.TBDPED.Where(q => q.PED_ID == value.PED_ID).ToList();
                    foreach (var item in lst)
                    {
                        db.TBDPED.Remove(item);
                    }
                    int Secuencia = 1;
                    
                    foreach (var item in value.LstDet)
                    {
                        db.TBDPED.Add(new TBDPED
                        {
                            PED_ID = value.PED_ID,
                            PED_SEC = Secuencia++,
                            PED_CANT = item.PED_CANT,
                            PED_DETALLE = item.PED_DETALLE
                        });
                    }
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
                        PROCESO = "Pedidos/POST",
                        SECUENCIA = SECUENCIAID
                    });
                    error.SaveChanges();
                }
            }
        }
    }
}
