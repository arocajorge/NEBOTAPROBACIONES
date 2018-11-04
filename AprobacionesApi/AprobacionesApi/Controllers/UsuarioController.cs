using AprobacionesApi.Data;
using AprobacionesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprobacionesApi.Controllers
{
    public class UsuarioController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public UsuarioModel Get(string USUARIO = "", string CLAVE = "")
        {
            UsuarioModel RESULTADO;

            if (string.IsNullOrEmpty(CLAVE))
            {
                RESULTADO = db.USUARIOS.Where(q => q.USUARIO.ToLower() == USUARIO.ToLower()).Select(q=> new UsuarioModel
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
    }
}
