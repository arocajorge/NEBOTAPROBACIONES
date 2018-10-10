namespace Core.App.Aprobacion.Infraestructure
{
    using Core.App.Aprobacion.ViewModels;
    public class InstanceLocator
    {
        #region Propiedades
        public MainViewModel Main { get; set; }
        #endregion

        #region Constructor
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
        #endregion
    }
}
