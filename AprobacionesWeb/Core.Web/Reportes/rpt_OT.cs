using Core.Data.general;
using Core.Info.general;
using System;
using System.Collections.Generic;

namespace Core.Web.Reportes
{
    public partial class rpt_OT : DevExpress.XtraReports.UI.XtraReport
    {
        public rpt_OT()
        {
            InitializeComponent();
        }

        private void rpt_OT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string TipoDoc = p_tipo_doc.Value.ToString();
            int cinv_num = Convert.ToInt32(p_num.Value);
            ORDEN_TRABAJO_DET_DATA odata = new ORDEN_TRABAJO_DET_DATA();
            List<ORDEN_TRABAJO_DET_INFO> lst_rpt = odata.get_list(TipoDoc,cinv_num);

            lblEstado.Text = lst_rpt.Count > 0 ? (lst_rpt[0].CINV_ST == "X" ? "ANULADA" : "APROBADA") : "";
            this.DataSource = lst_rpt;
        }
    }
}
