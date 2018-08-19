using Core.Info.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.general
{
    public class ARPROVEEDOR_DATA
    {
        public List<ARPROVEEDOR_INFO> get_list()
        {
            List<ARPROVEEDOR_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.ARPROVEEDOR
                         where q.ESTADO == "Activo"
                         select new ARPROVEEDOR_INFO
                         {
                             CODIGO = q.CODIGO,
                             CODIGO1 = q.CODIGO1,
                             NOMBRE = q.NOMBRE,
                         }).ToList();
            }

            return Lista;
        }
    }
}
