
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.App.Aprobacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CumplimientoMasterPage : MasterDetailPage
	{
		public CumplimientoMasterPage ()
		{
			InitializeComponent ();
            App.Navigator = Navigator;
            App.MasterCumplimiento = this;
        }
	}
}