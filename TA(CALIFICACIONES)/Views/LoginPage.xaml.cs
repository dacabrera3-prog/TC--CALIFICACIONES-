using TA_CALIFICACIONES_.Services;

namespace TA_CALIFICACIONES_.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly StudentAuthService _authService = new();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = UserEntry.Text?.Trim();
            var password = PasswordEntry.Text?.Trim();

            if (!_authService.ValidateCredentials(username ?? string.Empty, password ?? string.Empty, out var message))
            {
                await DisplayAlert("Error de acceso", message, "OK");
                return;
            }

            var safeUsername = Uri.EscapeDataString(username ?? string.Empty);
            await Shell.Current.GoToAsync($"//GradesPage?userName={safeUsername}");
        }
    }
}
