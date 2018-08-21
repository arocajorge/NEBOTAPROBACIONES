using Core.Info.general;
using DevExpress.Web;
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

        public List<SUCURSAL_INFO> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<SUCURSAL_INFO> Lista;
            using (Entities_general Context = new Entities_general())
            {
                Lista = (from q in Context.SUCURSAL
                         where q.ESTADO == "Vigente"
                         && (q.CODIGO + " " + q.NOMBRE).Contains(args.Filter)
                         orderby q.CODIGO                        
                         select new SUCURSAL_INFO
                         {
                             CODIGO = q.CODIGO,
                             NOMBRE = q.NOMBRE
                         }).Skip(skip).Take(take).ToList();
            }
            return Lista;
        }

        public SUCURSAL_INFO get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value == null)
                return null;
            using (Entities_general Context = new Entities_general())
            {
                var info = Context.SUCURSAL.Where(q => q.CODIGO == args.Value.ToString()).Select(q => new SUCURSAL_INFO
                {
                    CODIGO = q.CODIGO,
                    NOMBRE = q.NOMBRE
                }).FirstOrDefault();

                return info;
            }
            
        }
    }
}
