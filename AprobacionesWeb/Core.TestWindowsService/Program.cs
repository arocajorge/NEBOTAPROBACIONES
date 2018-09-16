using Core.Data;
using Core.Data.general;
using Core.Info.general;
using Core.Web.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Core.TestWindowsService
{
    class Program
    {
        public static string correo_desde = "arocajorge92@gmail.com" /*ordenpesca@gmail.com"*/;
        public static string contrasenia = "Mimamamemima13*"/*"ordenpesca2016"*/;
        public static string host = "smtp.gmail.com";
        public static bool enableSSL = true;
        public static int port = 587;

        static void Main(string[] args)
        {
            OnTimedEvent();
            
        }
        private static void OnTimedEvent()
        {
            try
            {
                TBCINV_APPCORREOS_DATA Odata = new TBCINV_APPCORREOS_DATA();
                var Orden = Odata.GET_ORDEN();

                if (Orden == null)
                    return;

                int CINV_NUM = Orden.CINV_NUM;
                string CINV_TDOC = Orden.CINV_TDOC;

                if (Orden.CINV_ST == "P")
                {
                    enviar_correo_proveedor(Orden.CINV_NUM, Orden.CINV_TDOC);
                }

                enviar_correo(Orden.CINV_NUM, "Supervisor", Orden.CINV_TDOC);

                Odata.GUARDAR(Orden);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void enviar_correo(int IDs, string nivel_aprobacion, string tipo_doc)
        {
            #region Armar cuerpo del correo correo
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(correo_desde);//Correo de envio
            mail.Subject = "Orden de " + (tipo_doc == "OT" ? "Trabajo" : "Compra") + "  Aprobada " + IDs;

            ORDEN_TRABAJO_DATA data_ot = new ORDEN_TRABAJO_DATA();
            ORDEN_TRABAJO_INFO item = data_ot.get_info(IDs, tipo_doc);

            if (!string.IsNullOrEmpty(item.CORREO_SOLICITADO))
                mail.To.Add(item.CORREO_SOLICITADO);
            if (!string.IsNullOrEmpty(item.CORREO_CENTROCOSTO))
                mail.To.Add(item.CORREO_CENTROCOSTO);
            if (!string.IsNullOrEmpty(item.CORREO_CENTROCOSTO2))
                mail.To.Add(item.CORREO_CENTROCOSTO2);

            string Body = "";

            Body += "<p>Saludos</p>";
            Body += "<p>Estimado funcionario</p>";
            Body += "<p>Se ha realizado la aprobación a Nivel " + nivel_aprobacion + " de la órden de " + (tipo_doc == "OT" ? "Trabajo" : "Compra") + " " + IDs + "</p>";
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

        private static void enviar_correo_proveedor(int ID, string tipo_doc)
        {
            #region Armar cuerpo del correo correo
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(correo_desde);//Correo de envio
            ORDEN_TRABAJO_DATA data_ot = new ORDEN_TRABAJO_DATA();
            ORDEN_TRABAJO_INFO info = data_ot.get_info(Convert.ToInt32(ID), tipo_doc);


            if (string.IsNullOrEmpty(info.E_MAIL))
                return;
            mail.To.Add(info.E_MAIL);
            mail.Subject = "Ordenes de " + (tipo_doc == "OT" ? "trabajo" : "compra") + " Aprobada No." + (tipo_doc == "OT" ? (info.CODIGOTR + "-") : "") + ID.ToString();
            string Body = "";
            Body += "<p>Saludos, se detalla orden de " + (tipo_doc == "OT" ? "trabajo" : "compra") + " para: " + info.CINV_COM1 + "</p>";
            Body += "<p>Estimado Proveedor</p>";
            Body += "<p>En días próximos cambiaremos la forma de pago mediante transferencias bancarias por tal razón necesitamos que nos hagan llegar la siguiente información:*Formulario del Proveedor(entregado por Asistente administrativa) junto con los sigtes documentos:Persona Natural: Copia de cédula y certificado bancarioPersona Jurídica: Ruc, Copia y cédula del representante legal y certificado bancario</p>";
            Body += "<p>Gracias por su colaboración</p><br>";

            MemoryStream mem = new MemoryStream();
            if (tipo_doc == "OT")
            {
                rpt_OT rpt = new rpt_OT();
                rpt.p_tipo_doc.Value = info.CINV_TDOC;
                rpt.p_num.Value = Convert.ToInt32(ID);
                rpt.ExportToPdf(mem);
            }
            else
            {
                rpt_OC rpt_oc = new rpt_OC();
                rpt_oc.p_tipo_doc.Value = info.CINV_TDOC;
                rpt_oc.p_num.Value = Convert.ToInt32(ID);
                rpt_oc.ExportToPdf(mem);
            }

            // Create a new attachment and put the PDF report into it.
            mem.Seek(0, System.IO.SeekOrigin.Begin);
            Attachment att = new Attachment(mem, info.CINV_TDOC + " " + info.CODIGOTR + "-" + ID.ToString() + ".pdf", "application/pdf");
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
    }
}
