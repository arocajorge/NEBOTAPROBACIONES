
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.App.Aprobacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GerenteMasterPage : MasterDetailPage
	{
		public GerenteMasterPage ()
		{
			InitializeComponent ();
            App.Navigator = Navigator;
            App.MasterGerente = this;
        }
	}
}