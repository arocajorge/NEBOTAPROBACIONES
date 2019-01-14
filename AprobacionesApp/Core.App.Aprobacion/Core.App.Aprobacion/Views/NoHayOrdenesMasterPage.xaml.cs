
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.App.Aprobacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoHayOrdenesMasterPage : MasterDetailPage
	{
		public NoHayOrdenesMasterPage ()
		{
			InitializeComponent ();
            App.Navigator = Navigator;
            App.NoHayOrdenes = this;
        }
	}
}