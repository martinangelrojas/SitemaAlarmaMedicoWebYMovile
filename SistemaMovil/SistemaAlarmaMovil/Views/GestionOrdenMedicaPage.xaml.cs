using SistemaAlarmaMovil.ViewModels;

namespace SistemaAlarmaMovil.Views
{
    public partial class GestionOrdenMedicaPage : ContentPage
    {
        private int? _ordenId;

        public GestionOrdenMedicaPage(GestionOrdenMedicaViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        public void SetOrdenId(int ordenId)
        {
            _ordenId = ordenId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_ordenId.HasValue && BindingContext is GestionOrdenMedicaViewModel vm)
            {
                await vm.InicializarAsync(_ordenId.Value);
                _ordenId = null;
            }
        }
    }
}
