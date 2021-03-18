using System;
using LocalConfigPOC.Managers;
using LocalConfigPOC.ViewModels;
using LocalConfigPOC.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace LocalConfigPOC
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                var nativeConfigManager = DependencyService.Get<INativeConfigManager>();
                var environmentManager = new ConfigManager(nativeConfigManager);
                var viewModel = new ConfigViewModel(environmentManager);
                var configPage = new ConfigPage(viewModel);

                MainPage = new NavigationPage(configPage);
            }
            catch (Exception ex)
            {
                //Show errror page if app fails to load
                var errorPage = CreateErrorPage(ex);
                
                MainPage = new NavigationPage(errorPage);
            }
        }

        private ContentPage CreateErrorPage(Exception ex)
        {
            var errorPage = new ContentPage()
            {
                Title = "Error!"
            };

            var exceptionLabel = new Label()
            {
                Text = ex.ToString(),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            errorPage.Content = exceptionLabel;

            return errorPage;
        }
    }
}