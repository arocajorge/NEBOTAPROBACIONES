using System.Collections.Generic;

namespace AprobacionesApi.Models
{
    public class UsuarioModel
    {
        public int CODIGO { get; set; }
        public string CLAVE { get; set; }
        public string USUARIO { get; set; }
        public string NOMBRE { get; set; }
        public decimal P_VENTAS { get; set; }
        public System.DateTime FECHA { get; set; }
        public string OFICREDA { get; set; }
        public string OFICRECO { get; set; }
        public string GERCRE { get; set; }
        public string ROL_APRO { get; set; }
        public string MENUFILTRO { get; set; }
        public List<UsuarioMenuModel> ListaMenu { get; set; }
    }
}