﻿using AprobacionesApi.Data;
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
        public USUARIOS Get(string USUARIO = "", string CLAVE = "")
        {
            USUARIOS RESULTADO;

            if (string.IsNullOrEmpty(CLAVE))
            {
                RESULTADO = db.USUARIOS.Where(q => q.USUARIO.ToLower() == USUARIO.ToLower()).FirstOrDefault();
            }
            else
                RESULTADO = db.USUARIOS.Where(q => q.USUARIO.ToLower() == USUARIO.ToLower()
                                  && q.CLAVE.ToLower() == CLAVE.ToLower()).FirstOrDefault();

            return RESULTADO;
        }
    }
}
