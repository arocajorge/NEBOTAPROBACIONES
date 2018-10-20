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
    public class CatalogosController : ApiController
    {
        EntitiesGeneral db = new EntitiesGeneral();
        public List<CatalogoModel> GET()
        {
            List<CatalogoModel> Lista;

            Lista = (from q in db.SUCURSAL
                     where q.ESTADO == "Vigente"
                     select new CatalogoModel
                     {
                         Tipo = "Sucursal",
                         Codigo = q.CODIGO,
                         Descripcion = q.NOMBRE
                     }).ToList();

            Lista.AddRange((from q in db.ARINDEX
                            where q.DATA == "2"
                            && q.ESTADO == "Vigente"
                            select new CatalogoModel
                            {
                                Tipo = "Viaje",
                                Codigo = q.CODIGO,
                                Descripcion = q.NOMBRE
                            }).ToList());

            Lista.AddRange((from q in db.ARINDEX
                            where q.DATA == "O"
                            && q.ESTADO == "Vigente"
                            select new CatalogoModel
                            {
                                Tipo = "Bodega",
                                Codigo = q.CODIGO,
                                Descripcion = q.NOMBRE
                            }).ToList());

            return Lista;
        }
    }
}
