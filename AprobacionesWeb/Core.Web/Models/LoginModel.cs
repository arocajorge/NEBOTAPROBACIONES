using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Core.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="El campo usuario es obligatorio")]
        public string usuario { get; set; }
        [Required(ErrorMessage = "El campo contraseña es obligatorio")]
        public string contrasenia { get; set; }
    }
}