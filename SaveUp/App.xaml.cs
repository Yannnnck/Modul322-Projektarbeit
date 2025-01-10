namespace SaveUp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Setze die Shell als MainPage
            MainPage = new AppShell();
        }
    }
}