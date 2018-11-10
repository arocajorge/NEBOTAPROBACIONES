using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class UsuarioController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public UsuarioModel Get(string USUARIO = "", string CLAVE = "")
        {
            try
            {
                UsuarioModel RESULTADO;

                if (string.IsNullOrEmpty(CLAVE))
                {
                    RESULTADO = db.USUARIOS.Where(q => q.USUARIO.ToLower() == USUARIO.ToLower()).Select(q => new UsuarioModel
                    {
                        USUARIO = q.USUARIO,
                        CLAVE = q.CLAVE,
                        CODIGO = q.CODIGO,
                        FECHA = q.FECHA,
                        GERCRE = q.GERCRE,
                        NOMBRE = q.NOMBRE,
                        OFICRECO = q.OFICRECO,
                        OFICREDA = q.OFICREDA,
                        P_VENTAS = q.P_VENTAS,
                        ROL_APRO = q.ROL_APRO,
                    }).FirstOrDefault();
                }
                else
                    RESULTADO = db.USUARIOS.Where(q => q.USUARIO.ToLower() == USUARIO.ToLower()
                                      && q.CLAVE.ToLower() == CLAVE.ToLower()).Select(q => new UsuarioModel
                                      {
                                          USUARIO = q.USUARIO,
                                          CLAVE = q.CLAVE,
                                          CODIGO = q.CODIGO,
                                          FECHA = q.FECHA,
                                          GERCRE = q.GERCRE,
                                          NOMBRE = q.NOMBRE,
                                          OFICRECO = q.OFICRECO,
                                          OFICREDA = q.OFICREDA,
                                          P_VENTAS = q.P_VENTAS,
                                          ROL_APRO = q.ROL_APRO,
                                      }).FirstOrDefault();

                if (RESULTADO != null)
                {
                    RESULTADO.ListaMenu = db.USUARIOS_MENU_APP.Where(q => q.USUARIO == RESULTADO.USUARIO).Select(q => new UsuarioMenuModel
                    {
                        MENU = q.MENU,
                        USUARIO = q.USUARIO,
                        MENUFILTRO = q.MENUFILTRO
                    }).ToList();
                    RESULTADO.MENUFILTRO = RESULTADO.ListaMenu.Count == 0 ? string.Empty : RESULTADO.ListaMenu.First().MENUFILTRO;
                }
                else
                    RESULTADO = new UsuarioModel();

                return RESULTADO;
            }
            catch (Exception ex)
            {
                long SECUENCIAID = db.APP_LOGERROR.Count() > 0 ? (db.APP_LOGERROR.Select(q => q.SECUENCIA).Max() + 1) : 1;
                db.APP_LOGERROR.Add(new APP_LOGERROR
                {
                    ERROR = ex == null ? string.Empty : (ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message),
                    INNER = ex.InnerException == null ? string.Empty : (ex.InnerException.Message.Length > 1000 ? ex.InnerException.Message.Substring(0, 1000) : ex.InnerException.Message),
                    FECHA = DateTime.Now,
                    PROCESO = "Usuario/GET",
                    SECUENCIA = SECUENCIAID
                });
                db.SaveChanges();
                return null;
            }            
        }
    }
}
