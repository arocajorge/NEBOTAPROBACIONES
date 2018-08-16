using Core.Info.general;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.general
{
    public class ORDEN_TRABAJO_DET_DATA
    {
        public List<ORDEN_TRABAJO_DET_INFO> get_list(string TipoDoc, int cinv_num)
        {
            List<ORDEN_TRABAJO_DET_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.VW_ORDENES_TRABAJO
                         where q.CINV_TDOC == TipoDoc
                         && q.CINV_NUM == cinv_num
                         select new ORDEN_TRABAJO_DET_INFO
                         {
                             CINV_SEC = q.CINV_SEC,
                             CINV_TDOC = q.CINV_TDOC,
                             CINV_NUM = q.CINV_NUM,
                             CINV_BOD = q.CINV_BOD,
                             CINV_TBOD = q.CINV_TBOD,
                             CINV_REF = q.CINV_REF,
                             CINV_FECING = q.CINV_FECING,
                             CINV_ID = q.CINV_ID,
                             CINV_NOMID = q.CINV_NOMID,
                             CINV_DSC = q.CINV_DSC,
                             CINV_COM1 = q.CINV_COM1,
                             CINV_COM2 = q.CINV_COM2,
                             CINV_COM3 = q.CINV_COM3,
                             CINV_COM4 = q.CINV_COM4,
                             CINV_TASA = q.CINV_TASA,
                             CINV_LOGIN = q.CINV_LOGIN,
                             CINV_TDIV = q.CINV_TDIV,
                             CINV_ST = q.CINV_ST,
                             EMPRESA = q.EMPRESA,
                             DATATR = q.DATATR,
                             CODIGOTR = q.CODIGOTR,
                             CINV_NOTA = q.CINV_NOTA,
                             CINV_FPAGO = q.CINV_FPAGO,
                             CINV_HORA = q.CINV_HORA,
                             CINV_TIPRECIO = q.CINV_TIPRECIO,
                             CINV_FECAPRUEBA = q.CINV_FECAPRUEBA,
                             CINV_FECLLEGADA = q.CINV_FECLLEGADA,
                             CINV_FECEMBARQUE = q.CINV_FECEMBARQUE,
                             CINV_FECDESADUA = q.CINV_FECDESADUA,
                             CINV_MOTIVOANULA = q.CINV_MOTIVOANULA,
                             DINV_LINEA = q.DINV_LINEA,
                             DINV_DETALLEDSCTO = q.DINV_DETALLEDSCTO,
                             DINV_DETALLEADVAL1 = q.DINV_DETALLEADVAL1,
                             DINV_DETALLEADVAL2 = q.DINV_DETALLEADVAL2,
                             DINV_DETALLEADVAL3 = q.DINV_DETALLEADVAL3,
                             DINV_DETALLEADVAL4 = q.DINV_DETALLEADVAL4,
                             CODIGO1 = q.CODIGO1,
                             NOMBRE = q.NOMBRE,
                             APELLIDO = q.APELLIDO,
                             RUC = q.RUC,
                             CEDULA = q.CEDULA,
                             DOMICILIO = q.DOMICILIO,
                             NOM_EMPLEADO = q.NOM_EMPLEADO,
                             CENTRO_COSTO = q.CENTRO_COSTO,
                             NOM_BODEGA = q.NOM_BODEGA,
                             NOM_VIAJE = q.NOM_VIAJE,
                             CINV_COM5 = q.CINV_COM5,
                             CINV_COM6 = q.CINV_COM6,
                             CINV_COM7 = q.CINV_COM7,
                             CINV_DETAPAGO = q.CINV_DETAPAGO,
                             CINV_STCUMPLI1 = q.CINV_STCUMPLI1,
                             CINV_STCUMPLI2 = q.CINV_STCUMPLI2,
                             UNIDAD = q.UNIDAD,
                             DINV_CANT = q.DINV_CANT,
                             DINV_COS = q.DINV_COS,
                             DINV_PRECIO_REAL = q.DINV_PRECIO_REAL,
                             DINV_IVA = q.DINV_IVA,
                             DINV_DSC = q.DINV_DSC,
                             DINV_PRCT_DSC = q.DINV_PRCT_DSC,
                             DINV_VTA = q.DINV_VTA,
                             TELEFONOS = q.TELEFONOS
                             
                         }).ToList();

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };
                Lista.ForEach(q => { q.TOTAL = q.CINV_TDOC == "OT" ? Convert.ToDecimal(q.CINV_COM3, provider) + Convert.ToDecimal(q.CINV_COM4, provider) : q.DINV_COS + q.DINV_IVA; });
            }

            return Lista.OrderBy(q=>q.DINV_LINEA).ToList();
        }
    }
}
