namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
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
    }
}
