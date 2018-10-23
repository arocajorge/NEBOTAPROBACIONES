using Core.Info.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Globalization;

namespace Core.Data.general
{
    public class ORDEN_TRABAJO_DATA
    {
        public List<ORDEN_TRABAJO_INFO> get_list(string Bodega, string Sucursal, string Proveedor, string Viaje, string num_ot, string solicitante, string estado_jefe_bodega, string estado_supervisor, DateTime fecha_ini, DateTime fecha_fin)
        {
            List<ORDEN_TRABAJO_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                //string where_clause = "CINV_ST IN ('P','G') AND CINV_TDOC = 'OT' ";
                string where_clause = "(CINV_ST = \"P\" OR CINV_ST = \"G\") AND CINV_TDOC = \"OT\" ";
                if (!string.IsNullOrEmpty(Bodega))
                    where_clause += "AND CINV_BOD = \""+Bodega+"\" ";
                if(!string.IsNullOrEmpty(Sucursal))
                    where_clause += "AND CODIGOTR = \"" + Sucursal + "\" ";
                if (!string.IsNullOrEmpty(Proveedor))
                    where_clause += "AND CINV_ID = \"" + Proveedor + "\" ";
                if (!string.IsNullOrEmpty(num_ot))
                    where_clause += "AND CINV_NUM = " + num_ot + " ";
                if (!string.IsNullOrEmpty(Viaje))
                    where_clause += "AND CINV_FPAGO = \"" + Viaje + "\" ";
                if (!string.IsNullOrEmpty(solicitante))
                    where_clause += "AND NOM_SOLICITADO.Contains(\"" + solicitante + "\") ";
                if (!string.IsNullOrEmpty(estado_jefe_bodega))
                    where_clause += "AND CINV_STCUMPLI1 = \"" + estado_jefe_bodega + "\" ";
                if (!string.IsNullOrEmpty(estado_supervisor))
                    where_clause += "AND CINV_STCUMPLI2 = \"" + estado_supervisor + "\" ";

                where_clause += "AND DATETIME(" + fecha_ini.Date.Year + "," + fecha_ini.Date.Month + "," + fecha_ini.Date.Day + ") <= CINV_FECING ";
                where_clause += "AND CINV_FECING <= DATETIME(" + fecha_fin.Date.Year + "," + fecha_fin.Date.Month + "," + fecha_fin.Date.Day + ")";

                Lista = (from q in Context.VW_ORDENES_TRABAJO_TOTAL.Where(where_clause,null)
                         select new ORDEN_TRABAJO_INFO
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
                             
                         }).ToList();
            }
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };
            Lista.ForEach(q => { q.VALOR_OT = q.CINV_TDOC == "OT" ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : 0; });

            return Lista.OrderBy(q=>q.CINV_NUM).ToList();
        }

        public List<ORDEN_TRABAJO_INFO> get_list(string Bodega, string Sucursal, string Proveedor, string Viaje, string num_ot, string solicitante, string estado_supervisor, DateTime fecha_ini, DateTime fecha_fin, string tipo_doc)
        {
            List<ORDEN_TRABAJO_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                string where_clause = "CINV_TDOC = \""+tipo_doc+"\" ";
                if (!string.IsNullOrEmpty(Bodega))
                    where_clause += "AND CINV_BOD = \"" + Bodega + "\" ";
                if (!string.IsNullOrEmpty(Sucursal))
                    where_clause += "AND CODIGOTR = \"" + Sucursal + "\" ";
                if (!string.IsNullOrEmpty(Proveedor))
                    where_clause += "AND CINV_ID = \"" + Proveedor + "\" ";
                if (!string.IsNullOrEmpty(num_ot))
                    where_clause += "AND CINV_NUM = " + num_ot + " ";
                if (!string.IsNullOrEmpty(Viaje))
                    where_clause += "AND CINV_FPAGO = \"" + Viaje + "\" ";
                if (!string.IsNullOrEmpty(solicitante))
                    where_clause += "AND NOM_SOLICITADO.Contains(\"" + solicitante + "\") ";
                if (!string.IsNullOrEmpty(estado_supervisor))
                    where_clause += "AND CINV_ST = \"" + estado_supervisor + "\" ";

                where_clause += "AND DATETIME(" + fecha_ini.Date.Year + "," + fecha_ini.Date.Month + "," + fecha_ini.Date.Day + ") <= CINV_FECING ";
                where_clause += "AND CINV_FECING <= DATETIME(" + fecha_fin.Date.Year + "," + fecha_fin.Date.Month + "," + fecha_fin.Date.Day + ")";

                Lista = (from q in Context.VW_ORDENES_TRABAJO_TOTAL.Where(where_clause, null)
                         select new ORDEN_TRABAJO_INFO
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
                             CINV_NOMID = q.CINV_NOMID,
                             CINV_TDOC = q.CINV_TDOC,
                             CINV_COM3 = q.CINV_COM3,
                             CINV_COM4 = q.CINV_COM4, 
                             VALOR_OC = q.VALOR_OC                            
                         }).ToList();
            }
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };
            Lista.ForEach(q => { q.VALOR_OT = q.CINV_TDOC == "OT" ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : q.VALOR_OC; });

            return Lista.OrderBy(q => q.CINV_NUM).ToList();
        }

        public List<ORDEN_TRABAJO_INFO> get_list(string IDs, string tipo_doc)
        {
            List<ORDEN_TRABAJO_INFO> Lista = new List<ORDEN_TRABAJO_INFO>();

            using (Entities_general Context = new Entities_general())
            {
                string[] array = IDs.Split(',');
                if (array.Count() > 0)
                {
                    foreach (var item in array)
                    {
                        int ID = Convert.ToInt32(item);
                        VW_ORDENES_TRABAJO_TOTAL Entity = Context.VW_ORDENES_TRABAJO_TOTAL.FirstOrDefault(q => q.CINV_NUM == ID && q.CINV_TDOC == tipo_doc);
                        if (Entity != null)
                        {
                            ORDEN_TRABAJO_INFO info = new ORDEN_TRABAJO_INFO
                            {
                                CORREO_SOLICITADO = Entity.CORREO_SOLICITADO,
                                CORREO_CENTROCOSTO = Entity.CORREO_CENTROCOSTO,
                                CORREO_CENTROCOSTO2 = Entity.CORREO_CENTROCOSTO2,
                            };
                            Lista.Add(info);
                        }
                    }
                }
            }
            return Lista;
        }

        public ORDEN_TRABAJO_INFO get_info(int ID, string tipo_doc)
        {
            ORDEN_TRABAJO_INFO info = new ORDEN_TRABAJO_INFO();

            using (Entities_general Context = new Entities_general())
            {
                VW_ORDENES_TRABAJO_TOTAL Entity = Context.VW_ORDENES_TRABAJO_TOTAL.FirstOrDefault(q => q.CINV_NUM == ID && q.CINV_TDOC == tipo_doc);
                if (Entity != null)
                {
                    info = new ORDEN_TRABAJO_INFO
                    {
                        CINV_COM1 = Entity.CINV_COM1,
                        E_MAIL = Entity.E_MAIL,
                        CODIGOTR = Entity.CODIGOTR,
                        CINV_TDOC = Entity.CINV_TDOC,
                        CORREO_CENTROCOSTO = Entity.CORREO_CENTROCOSTO,
                        CORREO_CENTROCOSTO2 = Entity.CORREO_CENTROCOSTO2,
                        CORREO_SOLICITADO = Entity.CORREO_SOLICITADO
                    };
                }
            }

            return info;
        }

        public bool guardar_supervisor(string IDs, string comentario, string estado,string elaborado_por)
        {
            using (Entities_general Context = new Entities_general())
            {
                elaborado_por = elaborado_por.Length >= 10 ? elaborado_por.Substring(0, 10) : elaborado_por;
                string[] array = IDs.Split(',');
                if (array.Count() > 0)
                {
                    foreach (var item in array)
                    {
                        int ID = Convert.ToInt32(item);
                        TBCINV Entity = Context.TBCINV.FirstOrDefault(q => q.CINV_NUM == ID && q.CINV_TDOC == "OT");
                        if(Entity != null)
                        {
                            Entity.CINV_STCUMPLI2 = estado;
                            Entity.CINV_COMENCUMPLI2 = comentario;
                            Entity.CINV_FECCUMPLI2 = DateTime.Now.Date;
                        }
                        Context.SaveChanges();
                    }                    
                }
            }

            return true;
        }

        public bool guardar_jefe(string IDs, string comentario, string estado,string elaborado_por)
        {
            using (Entities_general Context = new Entities_general())
            {
                elaborado_por = elaborado_por.Length >= 10 ? elaborado_por.Substring(0, 10) : elaborado_por;
                string[] array = IDs.Split(',');
                if (array.Count() > 0)
                {
                    foreach (var item in array)
                    {
                        int ID = Convert.ToInt32(item);
                        TBCINV Entity = Context.TBCINV.FirstOrDefault(q => q.CINV_NUM == ID && q.CINV_TDOC == "OT");
                        if (Entity != null)
                        {
                            Entity.CINV_STCUMPLI1 = estado;
                            Entity.CINV_COMENCUMPLI1 = comentario;                            
                            Entity.CINV_FECCUMPLI1 = DateTime.Now.Date;
                        }
                        Context.SaveChanges();
                    }
                }
            }

            return true;
        }

        public bool guardar_gg(string IDs, string comentario, string estado, string tipo_doc, string elaborado_por)
        {
            elaborado_por = elaborado_por.Length >= 10 ? elaborado_por.Substring(0, 10) : elaborado_por;
            using (Entities_general Context = new Entities_general())
            {
                string[] array = IDs.Split(',');
                if (array.Count() > 0)
                {
                    foreach (var item in array)
                    {
                        int ID = Convert.ToInt32(item);
                        TBCINV Entity = Context.TBCINV.FirstOrDefault(q => q.CINV_NUM == ID && q.CINV_TDOC == tipo_doc);
                        if (Entity != null)
                        {
                            Entity.CINV_ST = estado;
                            Entity.CINV_MOTIVOANULA = comentario;
                            Entity.CINV_FECAPRUEBA = DateTime.Now.Date;
                            
                        }
                        Context.SaveChanges();
                    }
                }
            }

            return true;
        }
    }
}
