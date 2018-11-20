using Core.App.Aprobacion.Helpers;
using Newtonsoft.Json;

namespace Core.App.Aprobacion.Models
{
    public class CatalogoModel
    {
        [JsonProperty("Tipo")]
        public string Tipo { get; set; }
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }
        [JsonProperty("Descripcion")]
        public string Descripcion { get; set; }
        public Enumeradores.eCombo Combo { get; set; }
    }
}
