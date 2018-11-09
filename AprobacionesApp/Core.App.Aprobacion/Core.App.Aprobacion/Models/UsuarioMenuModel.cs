namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
    public class UsuarioMenuModel
    {
        [JsonProperty("USUARIO")]
        public string IdUsuario { get; set; }
        [JsonProperty("MENU")]
        public string Menu { get; set; }
        [JsonProperty("MENUFILTRO")]
        public string MenuFiltro { get; set; }
    }
}
