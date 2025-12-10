using SistemaAlarmaMovil.Domain;
using SistemaAlarmaMovil.Helpers.Interfaces;
using SistemaAlarmaMovil.Models;
using SistemaAlarmaMovil.Repositories.Interfaces;
using SistemaAlarmaMovil.Services.Interfaces;

namespace SistemaAlarmaMovil.Views
{
    public partial class ActualizarPacientePage : ContentPage
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly IPacienteRepository _pacienteRepository; 
        private readonly IPacienteServiceWeb _pacienteServiceWeb;
        private Paciente _pacienteActual;

        public ActualizarPacientePage(ISessionHelper sessionHelper, IPacienteRepository pacienteRepository, IPacienteServiceWeb pacienteServiceWeb)
        {
            InitializeComponent();
            _sessionHelper = sessionHelper;
            _pacienteRepository = pacienteRepository;
            _pacienteServiceWeb = pacienteServiceWeb;
            CargarDatosDeUsuario();
        }

        private async void CargarDatosDeUsuario()
        {
            _pacienteActual = await _sessionHelper.ObtenerSessionUsuarioYRegistroBD();

            if (_pacienteActual != null)
            {
                txtApellido.Text = _pacienteActual.Apellido;
                txtNombre.Text = _pacienteActual.Nombre;
                txtDocumento.Text = _pacienteActual.Documento.ToString();
                txtEmail.Text = _pacienteActual.Email;
                dpFechaNacimiento.Date = _pacienteActual.FechaNacimiento;
                txtDocumento.IsReadOnly = true;
            }
            else
            {
                var _sessionActual = await _sessionHelper.ObtenerSessionUsuario();
                txtEmail.Text = _sessionActual.Email;
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                await DisplayAlert("Error", "Por favor complete todos los campos obligatorios", "OK");
                return;
            }

            if (!int.TryParse(txtDocumento.Text, out int documento))
            {
                await DisplayAlert("Error", "El documento debe ser un número válido", "OK");
                return;
            }

            if (_pacienteActual == null)
            {
                _pacienteActual = new Paciente
                {
                    Email = txtEmail.Text,
                    Apellido = txtApellido.Text,
                    Nombre = txtNombre.Text,
                    Documento = documento,
                    FechaNacimiento = dpFechaNacimiento.Date
                };
            }
            else
            {
                _pacienteActual.Apellido = txtApellido.Text;
                _pacienteActual.Nombre = txtNombre.Text;
                _pacienteActual.Documento = documento;
                _pacienteActual.FechaNacimiento = dpFechaNacimiento.Date;
            }

            try
            {
                await _pacienteRepository.SaveAsync(_pacienteActual);
                btnGuardar.IsEnabled = false;

                //enviar datos a la api
                var pacienteDto = new PacienteDto
                {
                    Apellido = _pacienteActual.Apellido,
                    Nombre = _pacienteActual.Nombre,
                    Documento = _pacienteActual.Documento,
                    FechaNacimiento = _pacienteActual.FechaNacimiento
                };
                var pacienteResponse = await _pacienteServiceWeb.AgregarOModificar(pacienteDto);

                await DisplayAlert("Éxito", "Datos guardados correctamente. Inicia nuevamente", "OK");

                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron guardar los datos: {ex.Message}", "OK");
            }
        }
    }
}