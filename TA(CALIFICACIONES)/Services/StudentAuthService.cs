using TA_CALIFICACIONES_.Models;

namespace TA_CALIFICACIONES_.Services
{
    public class StudentAuthService
    {
        private readonly Dictionary<string, string> _accounts = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Diego"] = "diego123",
            ["Carlos"] = "carlos123",
            ["Ana"] = "ana123",
            ["Jose"] = "jose123"
        };

        public IReadOnlyList<UserAccount> GetAccounts()
        {
            return _accounts
                .Select(account => new UserAccount
                {
                    UserName = account.Key,
                    Password = account.Value
                })
                .ToList();
        }

        public bool ValidateCredentials(string username, string password, out string message)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                message = "Debe ingresar usuario y contraseña.";
                return false;
            }

            if (!_accounts.TryGetValue(username.Trim(), out var storedPassword))
            {
                message = "Usuario no registrado.";
                return false;
            }

            if (!string.Equals(password.Trim(), storedPassword, StringComparison.Ordinal))
            {
                message = "Contraseña incorrecta.";
                return false;
            }

            message = "Acceso correcto.";
            return true;
        }
    }
}
