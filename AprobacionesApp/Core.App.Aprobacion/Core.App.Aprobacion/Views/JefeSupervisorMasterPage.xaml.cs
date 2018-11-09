
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.App.Aprobacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JefeSupervisorMasterPage : MasterDetailPage
    {
		public JefeSupervisorMasterPage ()
		{
			InitializeComponent ();
            App.Navigator = Navigator;
            App.MasterJefeSupervisor = this;
        }
	}
}