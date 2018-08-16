using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Core.Web.Models;
using Core.Data.general;
using Core.Info.general;

namespace Core.Web.Controllers
{
    public class AccountController : Controller
    {
        USUARIO_DATA odata = new USUARIO_DATA();
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session["IdUsuario"] = null;
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            USUARIO_INFO info = odata.get_info(model.usuario, model.contrasenia);
            if (info == null)
            {
                ViewBag.mensaje = "Credenciales incorrectas";
                return View(model);
            }
            Session["IdUsuario"] = info.USUARIO1;
            Session["ROL_APRO"] = info.ROL_APRO;
            Session["nom_usuario"] = info.NOMBRE;
            if (info.ROL_APRO == "G")
            {
                return RedirectToAction("AprobacionGG", "Home");
            }
            if (info.ROL_APRO == "J")
            {
                return RedirectToAction("AprobacionJefe", "Home");
            }
            if (info.ROL_APRO == "S")
            {
                return RedirectToAction("AprobacionSupervisor", "Home");
            }
            if (info.ROL_APRO == "A")
            {
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty(info.ROL_APRO))
            {
                ViewBag.mensaje = "Estimado usuario, al momento no tiene asignado un perfil para el uso de esta aplicación";
                return View(model);
            }

            return RedirectToAction("Index","Home");
        }
    }
}