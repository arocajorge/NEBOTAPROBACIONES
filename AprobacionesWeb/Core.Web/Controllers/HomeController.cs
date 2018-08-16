using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Info.general;
using Core.Data.general;
using System.Net.Mail;
using System.Net;
using Core.Web.Reportes;
using System.IO;

namespace Core.Web.Controllers
{
    public class HomeController : Controller
    {
        string correo_desde = "ordenpesca@gmail.com";
        //string correo_desde = "arocajorge92@gmail.com";
        string contrasenia = "ordenpesca2016";
        //string contrasenia = "Mimamamemima13*";
        string host = "smtp.gmail.com";
        bool enableSSL = true;
        int port = 587;

        public ActionResult Index()
        {
            return View();
        }

        private void cargar_combos()
        {
            ARINDEX_DATA data_arindex = new ARINDEX_DATA();
            var lst_bodega = data_arindex.get_list("O");
            ViewBag.lst_bodega = lst_bodega;

            var lst_viaje = data_arindex.get_list("2");
            ViewBag.lst_viaje = lst_viaje;

            SUCURSAL_DATA data_sucursal = new SUCURSAL_DATA();
            var lst_sucursal = data_sucursal.get_list();
            ViewBag.lst_sucursal = lst_sucursal;

            ARPROVEEDOR_DATA data_proveedor = new ARPROVEEDOR_DATA();
            var lst_proveedor = data_proveedor.get_list();
            ViewBag.lst_proveedor = lst_proveedor;

            Dictionary<string, string> lst_estado_jefe_bahia = new Dictionary<string, string>();
            lst_estado_jefe_bahia.Add("","Todas");
            lst_estado_jefe_bahia.Add("A", "Pendientes");
            lst_estado_jefe_bahia.Add("P", "Cumplidas");
            ViewBag.lst_estado_jefe_bahia = lst_estado_jefe_bahia;

            Dictionary<string, string> lst_estado_supervisor = new Dictionary<string, string>();
            lst_estado_supervisor.Add("", "Todas");
            lst_estado_supervisor.Add("A", "Pendientes");
            lst_estado_supervisor.Add("P", "Cumplidas");
            lst_estado_supervisor.Add("X", "Negadas");
            ViewBag.lst_estado_supervisor = lst_estado_supervisor;

            Dictionary<string, string> lst_tipo_doc = new Dictionary<string, string>();
            lst_tipo_doc.Add("OT", "Ordenes de trabajo");
            lst_tipo_doc.Add("OC", "Ordenes de compra");
            ViewBag.lst_tipo_doc = lst_tipo_doc;
        }

        public ActionResult AprobacionSupervisor()
        {
            cargar_combos();
            FILTRO_INFO model = new FILTRO_INFO
            {
                Fecha_inicio = new DateTime(DateTime.Now.Year, (DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1), 1).Date,
                Fecha_fin = DateTime.Now.Date,
                estado_supervisor = "A"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AprobacionSupervisor(FILTRO_INFO model)
        {
            cargar_combos();
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_aprobacion_supervisor(string bodega, string sucursal, string proveedor, string viaje, string num_ot, string solicitado, string estado_jefe_bahia, string estado_supervisor, DateTime fecha_inicio, DateTime fecha_fin)
        {
            ORDEN_TRABAJO_DATA odata = new ORDEN_TRABAJO_DATA();
            FILTRO_INFO model = new FILTRO_INFO();
            ViewBag.bodega = bodega;
            ViewBag.sucursal = sucursal;
            ViewBag.proveedor = proveedor;
            ViewBag.viaje = viaje;
            ViewBag.num_ot = num_ot;
            ViewBag.solicitado = solicitado;
            ViewBag.estado_jefe_bahia = estado_jefe_bahia;
            ViewBag.estado_supervisor = estado_supervisor;
            ViewBag.fecha_inicio = fecha_inicio;
            ViewBag.fecha_fin = fecha_fin;
            model.lst_ot = odata.get_list(bodega,sucursal,proveedor,viaje,num_ot,solicitado,estado_jefe_bahia,estado_supervisor,fecha_inicio,fecha_fin);
            return PartialView("_GridViewPartial_aprobacion_supervisor", model);
        }

        public JsonResult guardar_supervisor(string IDs, string comentario, string estado)
        {
            string nom_usuario = Session["nom_usuario"] == null ? "" : Session["nom_usuario"].ToString();
            ORDEN_TRABAJO_DATA odata = new ORDEN_TRABAJO_DATA();            
            bool respuesta = odata.guardar_supervisor(IDs, comentario, estado,nom_usuario);
            string resultado = respuesta == true ? "1" : "0";
            if (respuesta)
                enviar_correo(IDs, "Supervisor","OT");
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        private void enviar_correo(string IDs, string nivel_aprobacion,string tipo_doc)
        {
            #region Armar cuerpo del correo correo
            MailMessage mail = new MailMessage();


            //mail.From = new MailAddress("ordenpesca@gmail.com");//Correo de envio
            mail.From = new MailAddress(correo_desde);//Correo de envio
            mail.Subject = "Ordenes de "+(tipo_doc == "OT" ? "Trabajo" : "Compra")+"  Aprobadas " + IDs;

            ORDEN_TRABAJO_DATA data_ot = new ORDEN_TRABAJO_DATA();
            List<ORDEN_TRABAJO_INFO> lst_correos = data_ot.get_list(IDs,tipo_doc);

            if (lst_correos.Count == 0)
                return;

            foreach (var item in lst_correos)
            {
                if (!string.IsNullOrEmpty(item.CORREO_SOLICITADO))
                    mail.To.Add(item.CORREO_SOLICITADO);
                if (!string.IsNullOrEmpty(item.CORREO_CENTROCOSTO))
                    mail.To.Add(item.CORREO_CENTROCOSTO);
                if (!string.IsNullOrEmpty(item.CORREO_CENTROCOSTO2))
                    mail.To.Add(item.CORREO_CENTROCOSTO2);
            }

            string Body = "";

            Body += "<p>Saludos</p>";
            Body += "<p>Estimado funcionario</p>";
            Body += "<p>Se ha realizado la aprobación a Nivel "+nivel_aprobacion+ " de las Ordenes de " + (tipo_doc == "OT" ? "Trabajo" : "Compra" )+ " " + IDs + "</p>";
            Body += "<p>Gracias por su colaboración</p>";

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            mail.AlternateViews.Add(htmlView);
            #endregion

            try
            {
                #region smtp
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Host = host;
                smtp.EnableSsl = enableSSL;
                smtp.Port = port;
                smtp.Credentials = new NetworkCredential(correo_desde, contrasenia);
                smtp.Send(mail);
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        public ActionResult AprobacionGG()
        {
            cargar_combos();
            FILTRO_INFO model = new FILTRO_INFO
            {
                Fecha_inicio = new DateTime(DateTime.Now.Year, (DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1), 1).Date,
                Fecha_fin = DateTime.Now.Date,
                estado_supervisor = "A",
                tipo_doc = "OT"
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AprobacionGG(FILTRO_INFO model)
        {
            cargar_combos();
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_gg(string bodega, string sucursal, string proveedor, string viaje, string num_ot, string solicitado, string estado_supervisor, DateTime fecha_inicio, DateTime fecha_fin, string tipo_doc)
        {
            ORDEN_TRABAJO_DATA odata = new ORDEN_TRABAJO_DATA();
            FILTRO_INFO model = new FILTRO_INFO();
            ViewBag.bodega = bodega;
            ViewBag.sucursal = sucursal;
            ViewBag.proveedor = proveedor;
            ViewBag.viaje = viaje;
            ViewBag.num_ot = num_ot;
            ViewBag.solicitado = solicitado;
            ViewBag.estado_supervisor = estado_supervisor;
            ViewBag.fecha_inicio = fecha_inicio;
            ViewBag.fecha_fin = fecha_fin;
            ViewBag.tipo_doc = tipo_doc;
            model.lst_ot = odata.get_list(bodega, sucursal, proveedor, viaje, num_ot, solicitado, estado_supervisor, fecha_inicio, fecha_fin, tipo_doc);
            return PartialView("_GridViewPartial_gg", model);
        }

        public JsonResult guardar_gg(string IDs, string comentario, string estado, string tipo_doc)
        {
            string nom_usuario = Session["nom_usuario"] == null ? "" : Session["nom_usuario"].ToString();
            ORDEN_TRABAJO_DATA odata = new ORDEN_TRABAJO_DATA();
            bool respuesta = odata.guardar_gg(IDs, comentario, estado,tipo_doc,nom_usuario);
            string resultado = respuesta == true ? "1" : "0";
            if (respuesta)
            {
                if (estado == "P")
                {
                    string[] array = IDs.Split(',');
                    if (array.Count() > 0)
                    {
                        foreach (var item in array)
                        {
                            enviar_correo_proveedor(item,tipo_doc);
                        }
                    }
                }
                enviar_correo(IDs, "Supervisor",tipo_doc);
            }
                
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        private void enviar_correo_proveedor(string ID, string tipo_doc)
        {
            #region Armar cuerpo del correo correo
            MailMessage mail = new MailMessage();
            //mail.From = new MailAddress("ordenpesca@gmail.com");//Correo de envio
            mail.From = new MailAddress(correo_desde);//Correo de envio
            ORDEN_TRABAJO_DATA data_ot = new ORDEN_TRABAJO_DATA();
            ORDEN_TRABAJO_INFO info = data_ot.get_info(Convert.ToInt32(ID),tipo_doc);
            

            if (string.IsNullOrEmpty(info.E_MAIL))
                return;
            mail.To.Add(info.E_MAIL);
            mail.Subject = "Ordenes de " + (tipo_doc == "OT" ? "trabajo" : "compra") + " Aprobada No." + (tipo_doc == "OT" ? (info.CODIGOTR + "-") : "") + ID.ToString();
            string Body = "";
            Body += "<p>Saludos, se detalla orden de "+(tipo_doc == "OT" ? "trabajo" : "compra" )+" para: "+info.CINV_COM1+"</p>";
            Body += "<p>Estimado Proveedor</p>";
            Body += "<p>En días próximos cambiaremos la forma de pago mediante transferencias bancarias por tal razón necesitamos que nos hagan llegar la sigt información:*Formulario del Proveedor(entregado por Asistente administrativa) junto con los sigtes documentos:Persona Natural: Copia de cédula y certificado bancarioPersona Jurídica: Ruc, Copia y cédula del representante legal y certificado bancario</p>";
            Body += "<p>Gracias por su colaboración</p><br>";

            MemoryStream mem = new MemoryStream();
            if (tipo_doc == "OT")
            {
                rpt_OT rpt = new rpt_OT();
                rpt.p_tipo_doc.Value = info.CINV_TDOC;
                rpt.p_num.Value = Convert.ToInt32(ID);
                rpt.ExportToPdf(mem);
            }else
            {
                rpt_OC rpt_oc = new rpt_OC();
                rpt_oc.p_tipo_doc.Value = info.CINV_TDOC;
                rpt_oc.p_num.Value = Convert.ToInt32(ID);
                rpt_oc.ExportToPdf(mem);
            }           

            // Create a new attachment and put the PDF report into it.
            mem.Seek(0, System.IO.SeekOrigin.Begin);
            Attachment att = new Attachment(mem, info.CINV_TDOC+" "+info.CODIGOTR+"-"+ID.ToString()+".pdf", "application/pdf");
            mail.Attachments.Add(att);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            mail.AlternateViews.Add(htmlView);
            #endregion

            try
            {
                #region smtp
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Host = host;
                smtp.EnableSsl = enableSSL;
                smtp.Port = port;
                smtp.Credentials = new NetworkCredential(correo_desde, contrasenia);
                smtp.Send(mail);
                #endregion
            }
            catch (Exception ex)
            {

            }
            mem.Close();
            mem.Flush();
        }

        public ActionResult AprobacionJefe()
        {
            cargar_combos();
            FILTRO_INFO model = new FILTRO_INFO
            {
                Fecha_inicio = new DateTime(DateTime.Now.Year, (DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1), 1).Date,
                Fecha_fin = DateTime.Now.Date,
                estado_jefe_bahia = "A"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AprobacionJefe(FILTRO_INFO model)
        {
            cargar_combos();
            return View(model);
        }

        public JsonResult guardar_jefe(string IDs, string comentario, string estado)
        {
            string nom_usuario = Session["nom_usuario"] == null ? "" : Session["nom_usuario"].ToString();
            ORDEN_TRABAJO_DATA odata = new ORDEN_TRABAJO_DATA();
            bool respuesta = odata.guardar_jefe(IDs, comentario, estado,nom_usuario);
            string resultado = respuesta == true ? "1" : "0";
            if (respuesta)
            {
                enviar_correo(IDs,"Jefe","OT");
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_aprobacion_jefe(string bodega, string sucursal, string proveedor, string viaje, string num_ot, string solicitado, string estado_jefe_bahia, string estado_supervisor, DateTime fecha_inicio, DateTime fecha_fin)
        {
            ORDEN_TRABAJO_DATA odata = new ORDEN_TRABAJO_DATA();
            FILTRO_INFO model = new FILTRO_INFO();
            ViewBag.bodega = bodega;
            ViewBag.sucursal = sucursal;
            ViewBag.proveedor = proveedor;
            ViewBag.viaje = viaje;
            ViewBag.num_ot = num_ot;
            ViewBag.solicitado = solicitado;
            ViewBag.estado_jefe_bahia = estado_jefe_bahia;
            ViewBag.estado_supervisor = estado_supervisor;
            ViewBag.fecha_inicio = fecha_inicio;
            ViewBag.fecha_fin = fecha_fin;
            model.lst_ot = odata.get_list(bodega, sucursal, proveedor, viaje, num_ot, solicitado, estado_jefe_bahia, estado_supervisor, fecha_inicio, fecha_fin);
            return PartialView("_GridViewPartial_aprobacion_jefe", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_detalle_OC(string CINV_TDOC = "", int CINV_NUM = 0)
        {
            ORDEN_TRABAJO_DET_DATA data_det = new ORDEN_TRABAJO_DET_DATA();
            ViewBag.CINV_TDOC = CINV_TDOC;
            ViewBag.CINV_NUM = CINV_NUM;
            var model = data_det.get_list(CINV_TDOC, CINV_NUM);
            return PartialView("_GridViewPartial_detalle_OC", model);
        }
    }
}