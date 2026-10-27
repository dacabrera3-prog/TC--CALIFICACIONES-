namespace TA_CALIFICACIONES_.Views
{
    [QueryProperty(nameof(UserName), "userName")]
    public partial class GradesPage : ContentPage
    {
        public string UserName { get; set; } = string.Empty;

        public GradesPage()
        {
            InitializeComponent();
            LoadStudents();
            FechaPicker.Date = DateTime.Today;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            UpdateWelcomeLabel();
        }

        private void LoadStudents()
        {
            StudentPicker.ItemsSource = new[]
            {
                "Carlos",
                "Ana",
                "José",
                "Pedro",
                "Laura"
            };

            StudentPicker.SelectedIndex = -1;
            UpdateWelcomeLabel();
        }

        private void OnStudentSelectionChanged(object sender, EventArgs e)
        {
            UpdateWelcomeLabel();
        }

        private void UpdateWelcomeLabel()
        {
            var selectedStudent = StudentPicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(selectedStudent))
            {
                WelcomeLabel.Text = string.Empty;
                return;
            }

            WelcomeLabel.Text = $"Bienvenido, {selectedStudent}";
        }

        private async void OnCalculateClicked(object sender, EventArgs e)
        {
            var selectedStudent = StudentPicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(selectedStudent))
            {
                await DisplayAlert("Datos incompletos", "Seleccione un estudiante.", "OK");
                return;
            }

            if (!TryParseGrade(Seguimiento1Entry.Text, "Nota Seguimiento 1", out var seguimiento1) ||
                !TryParseGrade(Examen1Entry.Text, "Examen 1", out var examen1) ||
                !TryParseGrade(Seguimiento2Entry.Text, "Nota Seguimiento 2", out var seguimiento2) ||
                !TryParseGrade(Examen2Entry.Text, "Examen 2", out var examen2))
            {
                return;
            }

            decimal porcentaje1 = 0.3m;
            decimal porcentaje2 = 0.2m;
            decimal porcentaje3 = 0.3m;
            decimal porcentaje4 = 0.2m;

            var notaParcial1 = (seguimiento1 * porcentaje1) + (examen1 * porcentaje2);
            var notaParcial2 = (seguimiento2 * porcentaje3) + (examen2 * porcentaje4);
            var notaFinal = notaParcial1 + notaParcial2;

            var estado = notaFinal >= 7
                ? "Aprobado 💯🎉"
                : notaFinal >= 5 && notaFinal <= 6.9m
                    ? "Complementario 🙂"
                    : "Reprobado 😫";

            var message = $"Nombre: {selectedStudent}\nFecha: {FechaPicker.Date:dd/MM/yyyy}\nNota Seguimiento 1: {seguimiento1:F2}\nExamen 1: {examen1:F2}\nNota Parcial 1: {notaParcial1:F2}\nNota Seguimiento 2: {seguimiento2:F2}\nExamen 2: {examen2:F2}\nNota Parcial 2: {notaParcial2:F2}\nNota Final: {notaFinal:F2}\nEstado: {estado}";

            ResultLabel.Text = $"Estado: {estado} | Nota final: {notaFinal:F2}";
            await AnimateResult();
            await DisplayAlert("Resultado del cálculo", message, "OK");
        }

        private async Task AnimateResult()
        {
            ResultLabel.Opacity = 0;
            ResultLabel.Scale = 0.8;

            await Task.WhenAll(
                ResultLabel.FadeTo(1, 250, Easing.CubicInOut),
                ResultLabel.ScaleTo(1.12, 250, Easing.CubicInOut));

            await ResultLabel.ScaleTo(1, 200, Easing.SpringOut);
        }

        private static bool TryParseGrade(string? value, string fieldName, out decimal result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value) || !decimal.TryParse(value, out result))
            {
                Application.Current?.MainPage?.DisplayAlert("Dato inválido", $"{fieldName} debe ser un número válido.", "OK");
                return false;
            }

            if (result < 0 || result > 10)
            {
                Application.Current?.MainPage?.DisplayAlert("Rango inválido", $"{fieldName} debe estar entre 0 y 10.", "OK");
                return false;
            }

            return true;
        }
    }
}
