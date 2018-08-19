using Core.Info.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.general
{
    public class USUARIO_DATA
    {
        public USUARIO_INFO get_info(string NOM_USUARIO, string CONTRASENIA)
        {
            USUARIO_INFO info = new USUARIO_INFO();
            using (Entities_general Context = new Entities_general())
            {
                USUARIOS Entity = Context.USUARIOS.FirstOrDefault(q => q.USUARIO == NOM_USUARIO && q.CLAVE == CONTRASENIA);
                if (Entity == null)
                    return null;
                info = new USUARIO_INFO
                {
                    USUARIO1 = Entity.USUARIO,
                    CLAVE = Entity.CLAVE,
                    CODIGO = Entity.CODIGO,
                    ROL_APRO = Entity.ROL_APRO,
                    NOMBRE = Entity.NOMBRE
                };
            }
            return info;
        }
    }
}
