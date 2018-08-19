using Core.Info.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.general
{
    public class BITACORAS_DATA
    {
        public List<BITACORAS_INFO> get_list(string VIAJE, string BARCO)
        {
            List<BITACORAS_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.VW_BITACORAS
                         where q.VIAJE == VIAJE
                         && q.BARCO == BARCO
                         && q.ESTADO == "A"
                         group q by q.LINEA into grouping                         
                         select new BITACORAS_INFO
                         {
                             ID = grouping.Max(c=> c.ID),
                             DESCRIPCION = grouping.Max(c => c.DESCRIPCION),
                             CONTRATISTA = grouping.Max(c => c.CONTRATISTA),
                             LINEA = grouping.Key,
                             CANTLINEAS = grouping.Where(c => c.LINEA_DETALLE != null).Count()
                         }).ToList();
            }

            return Lista.OrderBy(q=>q.LINEA).ToList();
        }

        public List<BITACORAS_INFO> get_list_det(int ID, short LINEA)
        {
            List<BITACORAS_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.DET_BITACORA2
                         where q.ID == ID
                         && q.LINEA == LINEA
                         select new BITACORAS_INFO
                         {
                             ID = q.ID,
                             LINEA = q.LINEA,
                             LINEA_DETALLE = q.LINEA_DETALLE,
                             VALOR = q.VALOR,
                             NUMERO_ORDEN = q.NUMERO_ORDEN
                         }).ToList();

            }

            return Lista;
        }

        public bool ADDLINEA(BITACORAS_INFO info)
        {
            using (Entities_general Context = new Entities_general())
            {
                short LINEA_DETALLE = 1;
                var lst = Context.DET_BITACORA2.Where(q => q.ID == info.ID && q.LINEA == info.LINEA).Select(q => q.LINEA_DETALLE).ToList();
                if (lst.Count > 0)
                    LINEA_DETALLE = (short)(lst.Max(q => q) + 1);
                Context.DET_BITACORA2.Add(new DET_BITACORA2
                {
                    ID = info.ID,
                    LINEA = info.LINEA,
                    LINEA_DETALLE = LINEA_DETALLE,
                    VALOR = info.VALOR,
                    NUMERO_ORDEN = info.NUMERO_ORDEN,
                    EMPRESA = 1,
                    CUMPLIMIENTO = 0
                });
                Context.SaveChanges();
            }
            return true;
        }
    }
}
