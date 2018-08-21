using Core.Data.general;
using Core.Info.general;
using Core.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            model.ID = odata_bit.GetID(model.IdViaje, model.IdSucursal);
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

        public ActionResult GridViewPartialBitacoraDet()
        {
            SessionFixed.ID = Request.Params["PKID"] != null ? Request.Params["PKID"] :"0";
            SessionFixed.LINEA = Request.Params["PKLINEA"] != null ? Request.Params["PKLINEA"]: "0";
            FILTRO_INFO model = new FILTRO_INFO { lst_bit = odata_bit.get_list_det(Convert.ToInt32(SessionFixed.ID), Convert.ToInt16(SessionFixed.LINEA)) };
            return PartialView("_GridViewPartialBitacoraDet", model);
        }
        public ActionResult EditingDelete(short LINEA_DETALLE)
        {
            odata_bit.DELETELINEA(Convert.ToInt32(SessionFixed.ID), Convert.ToInt16(SessionFixed.LINEA), LINEA_DETALLE);
            FILTRO_INFO model = new FILTRO_INFO { lst_bit = odata_bit.get_list_det(Convert.ToInt32(SessionFixed.ID), Convert.ToInt16(SessionFixed.LINEA)) };
            return PartialView("_GridViewPartialBitacoraDet", model);
        }
        #endregion

        #region Json
        public JsonResult GetTotalOrden(int OT = 0)
        {
            decimal valor = 0;

            valor = odata_ot.GetTotalOrden("OT", OT);

            return Json(valor, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ADDLINEA(int ID = 0, short LINEA = 0, string ORDEN = "", string VALOROT = "")
        {
            decimal resultado = 0;
            VALOROT = VALOROT == "" ? "0" : VALOROT;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };
            if (odata_bit.ADDLINEA(new BITACORAS_INFO
            {
                ID = ID,
                LINEA = LINEA,
                NUMERO_ORDEN = ORDEN,
                VALOR = Convert.ToDecimal(VALOROT,provider)
            })) resultado = 1;

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}