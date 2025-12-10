namespace SistemaAlarmaMovil.GoogleServices
{
    public interface IGoogleAuthService
    {
        public Task<GoogleUserDTO> AuthenticateAsync();
        public Task<GoogleUserDTO> GetCurrentUserAsync();
        public Task LogoutAsync();

    }
}
