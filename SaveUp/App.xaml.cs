using SaveUp.Views;

namespace SaveUp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell()); // Verwende AppShell anstelle von HomePage
            return window;
        }
    }
}