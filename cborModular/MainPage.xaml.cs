using cborModular.Application;
using System.Reflection.PortableExecutable;

namespace cborModular
{
    public partial class MainPage : ContentPage
    {

        private readonly IMotorcycleService _motorcycleService;

        public MainPage(IMotorcycleService motorcycleService)
        {
            InitializeComponent();
            _motorcycleService = motorcycleService;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Volání služby pro načtení dat z motorky (předání deviceId, pokud je potřeba)
                var data = await _motorcycleService.LoadDataAsync(Guid.NewGuid()); // Například deviceId pro připojení k zařízení

                // Aktualizace UI prvků na základě načtených dat
                if (data != null)
                {
                    SpeedLabel.Text = $"Speed: {data.Speed.Value} km/h";
                }
            }
            catch (Exception ex)
            {          
                    Console.WriteLine($"Error loading motorcycle data: {ex.Message}");            
            }
        }

    }

}
