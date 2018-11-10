﻿using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class OrdenTrabajoController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public OrdenModel GET(string CINV_TDOC = "", int CINV_NUM = 0, string CINV_LOGIN = "")
        {
            DateTime Desde = DateTime.Now.Date.AddMonths(-10);
            OrdenModel orden = new OrdenModel();

            if (CINV_NUM != 0)
            {
                orden = db.VW_ORDENES_TRABAJO_TOTAL_APP.Where(q => q.CINV_TDOC == CINV_TDOC && q.CINV_NUM == CINV_NUM).Select(q => new OrdenModel
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
                    NOM_VIAJE = q.NOM_VIAJE
                }).FirstOrDefault();
            }
            else
            {
                orden = (from q in db.VW_ORDENES_TRABAJO_TOTAL_APS
                         where q.CINV_FECING >= Desde
                         && (q.CINV_ST == "A")
                         && (q.CINV_TDOC == "OT" || q.CINV_TDOC == "OC" || q.CINV_TDOC == "OK")
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
                             NOM_VIAJE = q.NOM_VIAJE
                         }).FirstOrDefault();

                if (orden == null)
                {
                    
                    db.Database.ExecuteSqlCommand("DELETE TBCINV_APP_ORDENES_SALTADAS WHERE USUARIO ='"+ CINV_LOGIN.ToUpper()+"'");

                    orden = (from q in db.VW_ORDENES_TRABAJO_TOTAL_APP
                             where q.CINV_FECING >= Desde
                             && (q.CINV_ST == "A")
                             && (q.CINV_TDOC == "OT" || q.CINV_TDOC == "OC" || q.CINV_TDOC == "OK")
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
                                 NOM_VIAJE = q.NOM_VIAJE
                             }).FirstOrDefault();
                }
            }

            if (orden == null)
                orden = new OrdenModel { lst = new List<OrdenDetalleModel>(), CINV_TDOC = "" };

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };
            if(orden.CINV_NUM != 0)
                orden.VALOR_OT = (orden.CINV_TDOC == "OT" || orden.CINV_TDOC == "OK") ? (Convert.ToDecimal(orden.CINV_COM3, provider) + Convert.ToDecimal(orden.CINV_COM4, provider)) : (orden.VALOR_OC == null ? 0 : Convert.ToDecimal(orden.VALOR_OC));

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
                             DINV_ICE = q.DINV_ICE
                         }).ToList();
            if (orden.lst != null)
                orden.lst.ForEach(q => { q.TOTAL = ((orden.CINV_TDOC == "OT" || orden.CINV_TDOC == "OK") ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : (q.DINV_COS +  (q.DINV_ICE == null ? 0 : Convert.ToDecimal(q.DINV_ICE,provider)))) + q.DINV_IVA; orden.NOM_VIAJE = q.NOM_VIAJE; });

            return orden;
        }

        public void Post([FromBody]OrdenModel value)
        {
            try
            {
                var usuario = db.USUARIOS.Where(q => q.USUARIO.ToLower() == value.CINV_LOGIN.ToLower()).FirstOrDefault();
                if (usuario == null)
                    return;

                var Entity = db.TBCINV.Where(q => q.CINV_NUM == value.CINV_NUM && q.CINV_TDOC == value.CINV_TDOC).FirstOrDefault();
                if (Entity != null)
                {
                    if (value.CINV_ST == "PASAR")
                    {
                        var query = db.TBCINV_APP_ORDENES_SALTADAS.ToList();
                        int ID = query.Count == 0 ? 1 : query.Max(q => q.ID) +1;
                        db.TBCINV_APP_ORDENES_SALTADAS.Add(new TBCINV_APP_ORDENES_SALTADAS
                        {
                            ID = ID,
                            CINV_NUM = value.CINV_NUM,
                            CINV_TDOC = value.CINV_TDOC,
                            USUARIO = usuario.USUARIO,
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        switch (usuario.ROL_APRO)
                        {
                            case "G":
                                Entity.CINV_ST = value.CINV_ST;
                                Entity.CINV_MOTIVOANULA = value.CINV_MOTIVOANULA;
                                Entity.CINV_FECAPRUEBA = DateTime.Now.Date;
                                break;
                            case "J":
                                Entity.CINV_STCUMPLI1 = value.CINV_ST;
                                Entity.CINV_COMENCUMPLI1 = value.CINV_MOTIVOANULA;
                                Entity.CINV_FECCUMPLI1 = DateTime.Now.Date;
                                Entity.CINV_LOGINCUMPLI1 = value.CINV_LOGIN;
                                break;
                            case "S":
                                Entity.CINV_STCUMPLI2 = value.CINV_ST;
                                Entity.CINV_COMENCUMPLI2 = value.CINV_MOTIVOANULA;
                                Entity.CINV_FECCUMPLI2 = DateTime.Now.Date;
                                Entity.CINV_LOGINCUMPLI2 = value.CINV_LOGIN;
                                break;
                        }

                        foreach (var item in value.lst)
                        {
                            var detalle = db.TBDINV.Where(q => q.DINV_CTINV == Entity.CINV_SEC && q.DINV_LINEA == item.DINV_LINEA).FirstOrDefault();
                            if (detalle != null)
                                detalle.DINV_ST = item.ESCHATARRA == true ? "S" : "N";
                        }
                        var query2 = db.TBCINV_APPCORREOS.ToList();
                        int ID2 = query2.Count == 0 ? 1 : query2.Max(q => q.SECUENCIAL)+1;
                        db.TBCINV_APPCORREOS.Add(new TBCINV_APPCORREOS
                        {
                            SECUENCIAL = ID2,
                            CINV_TDOC = value.CINV_TDOC,
                            CINV_NUM = value.CINV_NUM,
                            CINV_LOGIN = usuario.USUARIO,
                            CINV_ST = value.CINV_ST,
                            FECHA_APRO = DateTime.Now,
                            ROL_APRO = usuario.ROL_APRO
                        });
                        
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }   
                
         
    }
}
