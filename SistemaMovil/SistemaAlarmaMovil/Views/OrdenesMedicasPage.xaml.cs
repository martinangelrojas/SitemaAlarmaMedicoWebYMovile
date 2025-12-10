using SistemaAlarmaMovil.ViewModels;

namespace SistemaAlarmaMovil.Views
{
    public partial class OrdenesMedicasPage : ContentPage
    {
        public OrdenesMedicasPage(OrdenesMedicasViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
