using Core.Info.general;
using DevExpress.Web;
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

        public List<ARINDEX_INFO> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args,string DATA)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<ARINDEX_INFO> Lista;
            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.ARINDEX
                         where q.ESTADO == "Vigente"
                         && q.DATA == DATA
                         && (q.CODIGO + " " + q.NOMBRE).Contains(args.Filter)
                         orderby q.CODIGO
                         select new ARINDEX_INFO
                         {
                             CODIGO = q.CODIGO,
                             NOMBRE = q.NOMBRE
                         }).Skip(skip).Take(take).ToList();
            }
            return Lista;
        }

        public ARINDEX_INFO get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, string DATA)
        {
            decimal id;
            if (args.Value == null || !decimal.TryParse(args.Value.ToString(), out id))
                return null;
            using (Entities_general Context = new Entities_general())
            {
                var info = Context.ARINDEX.Where(q => q.CODIGO == args.Value.ToString() && q.DATA == DATA).Select(q => new ARINDEX_INFO
                {
                    CODIGO = q.CODIGO,
                    NOMBRE = q.NOMBRE
                }).FirstOrDefault();

                return info;
            }

        }
    }
}
