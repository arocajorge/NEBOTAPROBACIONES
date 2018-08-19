using Core.Info.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.general
{
    public class ARINDEX_DATA
    {
        public List<ARINDEX_INFO> get_list(string DATA)
        {
            List<ARINDEX_INFO> Lista;

            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.ARINDEX
                         where q.DATA == DATA
                         && q.ESTADO == "Vigente"
                         select new ARINDEX_INFO
                         {
                             DATA = q.DATA,
                             CODIGO = q.CODIGO,
                             NOMBRE = q.NOMBRE,                            
                         }).ToList();
            }

            return Lista;
        }
    }
}
