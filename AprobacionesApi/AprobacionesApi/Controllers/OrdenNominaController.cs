using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class OrdenNominaController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        
        public List<OrdenNominaModel> GET(string ESTADOGERENTE = "", string BARCO = "", int DIAINI = 0, int MESINI = 0, int ANIOINI = 0, int DIAFIN = 0, int MESFIN = 0, int ANIOFIN = 0)
        {
            try
            {
                DateTime FECHAINI = new DateTime(ANIOINI, MESINI, DIAINI);
                DateTime FECHAFIN = new DateTime(ANIOFIN, MESFIN, DIAFIN);

                List<OrdenNominaModel> Lista;

                Lista = get_list(ESTADOGERENTE, BARCO, FECHAINI, FECHAFIN);

                return Lista;
            }
            catch (Exception ex)
            {
                long ID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "OrdenNomina/GET",
                    SECUENCIA = ID
                });
                db.SaveChanges();
                return new List<OrdenNominaModel>();
            }
        }

        public List<OrdenNominaModel> get_list(string EstadoGerente, string Barco, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                List<OrdenNominaModel> Lista;

                string where_clause = "(CINV_TDOC = \"OT\" OR CINV_TDOC = \"OD\" OR CINV_TDOC = \"OC\") ";
                if (!string.IsNullOrEmpty(Barco))
                    where_clause += "AND CODIGOTR = \"" + Barco + "\" ";
                if (!string.IsNullOrEmpty(EstadoGerente))
                    where_clause += "AND CINV_ST = \"" + EstadoGerente + "\" ";

                where_clause += "AND DATETIME(" + fecha_ini.Date.Year + "," + fecha_ini.Date.Month + "," + fecha_ini.Date.Day + ") <= CINV_FECING ";
                where_clause += "AND CINV_FECING <= DATETIME(" + fecha_fin.Date.Year + "," + fecha_fin.Date.Month + "," + fecha_fin.Date.Day + ") ";

                Lista = (from q in db.VW_ORDENES_NOMINA_APP.Where(where_clause, null)
                         select new OrdenNominaModel
                         {
                             CINV_TDOC = q.CINV_TDOC,
                             CINV_NUM = q.CINV_NUM,
                             CINV_FECING = q.CINV_FECING,

                             CINV_STREFER = q.CINV_STREFER,//Estado referencia
                             CINV_COMREFER = q.CINV_COMREFER,

                             CINV_ST = q.CINV_ST,//Estado gerente,
                             CINV_MOTIVOANULA = q.CINV_MOTIVOANULA,

                             CINV_STPOLI = q.CINV_STPOLI,//Estado poligrafo
                             CINV_COMPOLI = q.CINV_COMPOLI,

                             CINV_STPSICO = q.CINV_STPSICO,//Estado psicologico
                             CINV_COMPSICO = q.CINV_COMPSICO,

                             CINV_STANTE = q.CINV_STANTE,//Estado antecedentes
                             CINV_COMANTE = q.CINV_COMANTE,
                             CINV_COMANTE2 = q.CINV_COMANTE2,

                             CINV_STPERFIL = q.CINV_STPERFIL,//Estado perfil
                             CINV_COMPERFIL = q.CINV_COMPERFIL,

                             NOM_SOLICITADO = q.NOM_SOLICITADO,
                             CED_SOLICITADO = q.CED_SOLICITADO,

                             NOM_CENTROCOSTO = q.NOM_CENTROCOSTO,
                             NOM_CARGO = q.NOM_CARGO
                         }).ToList();

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
                    PROCESO = "OrdenNomina/GET/GetList",
                    SECUENCIA = ID
                });
                db.SaveChanges();
                return new List<OrdenNominaModel>();
            }
        }

        public void Post([FromBody]OrdenNominaModel value)
        {
            try
            {
                var Orden = db.TBNOM_ORDEN_NOMINA_CAB.Where(q => q.CINV_NUM == value.CINV_NUM).FirstOrDefault();
                if (Orden == null)
                    return;
                
                Orden.CINV_STREFER = value.CINV_STREFER;                
                Orden.CINV_COMREFER = value.CINV_COMREFER == null ? "" : (value.CINV_COMREFER.Length > 200 ? value.CINV_COMREFER.Substring(0,200) : value.CINV_COMREFER);
                Orden.CINV_COMREFER2 = value.CINV_COMREFER == null ? "" : (value.CINV_COMREFER.Length > 200 ? value.CINV_COMREFER.Substring(199, 200) : string.Empty);

                if (value.ListaDetalle != null)
                    foreach (var item in value.ListaDetalle)
                    {
                        var linea = db.TBNOM_ORDEN_NOMINA_DET.Where(q => q.DINV_CTINV == value.CINV_NUM && q.DINV_LINEA == item.DINV_LINEA).FirstOrDefault();
                        if (linea != null)
                            linea.DINV_ST = item.DINV_ST;
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
                    PROCESO = "OrdenNomina/POST",
                    SECUENCIA = ID
                });
                db.SaveChanges();
            }
        }
    }
}
