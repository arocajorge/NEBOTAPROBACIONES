using Core.Info.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.general
{
    public class SUCURSAL_DATA
    {
        public List<SUCURSAL_INFO> get_list()
        {
            List<SUCURSAL_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.SUCURSAL
                         where q.ESTADO == "Vigente"
                         select new SUCURSAL_INFO
                         {
                             CODIGO = q.CODIGO,
                             NOMBRE = q.NOMBRE
                         }).ToList();
            }

            return Lista;
        }
    }
}
