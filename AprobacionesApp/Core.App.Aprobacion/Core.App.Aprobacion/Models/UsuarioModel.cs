namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class UsuarioModel
    {
        [JsonProperty("CODIGO")]
        public string Codigo { get; set; }
        [JsonProperty("CLAVE")]
        public string Clave { get; set; }
        [JsonProperty("USUARIO")]
        public string Usuario { get; set; }
        [JsonProperty("ROL_APRO")]
        public string RolApro { get; set; }
        [JsonProperty("MENUFILTRO")]
        public string MenuFiltro { get; set; }
        [JsonProperty("ListaMenu")] 
        public List<UsuarioMenuModel> LstMenu { get; set; }
    }
}
