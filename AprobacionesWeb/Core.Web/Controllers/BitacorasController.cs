using Core.Data.general;
using Core.Info.general;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Controllers
{
    public class BitacorasController : Controller
    {
        #region Variables
        BITACORAS_DATA odata_bit = new BITACORAS_DATA();
        ORDEN_TRABAJO_DET_DATA odata_ot = new ORDEN_TRABAJO_DET_DATA();
        SUCURSAL_DATA odata_su = new SUCURSAL_DATA();
        ARINDEX_DATA odata_ar = new ARINDEX_DATA();
        #endregion

        #region Index
        public ActionResult Index()
        {
            FILTRO_INFO model = new FILTRO_INFO();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(FILTRO_INFO model)
        {
            return View(model);
        }
        #endregion

        #region SucursalBajoDemanda
        public ActionResult CmbSucursal()
        {
            FILTRO_INFO model = new FILTRO_INFO();
            return PartialView("_CmbSucursal", model);
        }
        public List<SUCURSAL_INFO> get_list_bajo_demanda_sucursal(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return odata_su.get_list_bajo_demanda(args);
        }
        public SUCURSAL_INFO get_info_bajo_demanda_sucursal(ListEditItemRequestedByValueEventArgs args)
        {
            return odata_su.get_info_bajo_demanda(args);
        }
        #endregion

        #region ViajeBajoDemanda
        public ActionResult CmbViaje()
        {
            FILTRO_INFO model = new FILTRO_INFO();
            return PartialView("_CmbViaje", model);
        }
        public List<ARINDEX_INFO> get_list_bajo_demanda_viaje(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return odata_ar.get_list_bajo_demanda(args,"2");
        }
        public ARINDEX_INFO get_info_bajo_demanda_viaje(ListEditItemRequestedByValueEventArgs args)
        {
            return odata_ar.get_info_bajo_demanda(args, "2");
        }
        #endregion

        #region Bitacora
        public ActionResult GridViewPartialBitacora(string BARCO = "", string VIAJE = "")
        {
            ViewBag.BARCO = BARCO;
            ViewBag.VIAJE = VIAJE;
            FILTRO_INFO model = new FILTRO_INFO { lst_bit = odata_bit.get_list(VIAJE, BARCO) };
            return PartialView("_GridViewPartialBitacora",model);
        }

        #endregion
    }
}