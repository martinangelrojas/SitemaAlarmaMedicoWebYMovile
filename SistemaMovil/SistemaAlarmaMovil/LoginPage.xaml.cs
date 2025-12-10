using SistemaAlarmaMovil.GoogleServices;
using SistemaAlarmaMovil.Helpers.Interfaces;

namespace SistemaAlarmaMovil
{
    public partial class LoginPage : ContentPage
    {
        private readonly IGoogleAuthService _googleAuthService;

        public LoginPage(IGoogleAuthService googleAuthService)
        {
            InitializeComponent();
            _googleAuthService = googleAuthService;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var loggedUser = await _googleAuthService.GetCurrentUserAsync();
            //var loggedUser = new GoogleUserDTO()
            //{
            //    Email = "juan.foxer@gmail.com",
            //    FullName = "Juan Arce",
            //    TokenId = "33764132",
            //    UserName = "juan.arce"
            //};

            if (loggedUser == null)
            {
                loggedUser = await _googleAuthService.AuthenticateAsync();
            }

            if (loggedUser != null)
            {
                var toastHelper = Application.Current.Handler.MauiContext.Services.GetService<IToastHelper>();
                toastHelper.ShowToast("Hola " + loggedUser.FullName);

                var mainPage = Application.Current.Handler.MauiContext.Services.GetService<MainPage>();
                await Navigation.PushAsync(mainPage);
            }
            else
            {
                await DisplayAlert("Error", "No se pudo completar el inicio de sesión", "OK");
            }
        }
    }
}