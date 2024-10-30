namespace cborModular
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
           // MainPage = new AppShell();          
        }
     
    }
}
