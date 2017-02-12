using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using TheQuestionReborn.Model;
using TheQuestionReborn.View;
using TheQuestionReborn.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using TheQuestionReborn.Settings;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TheQuestionReborn
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationData.AppFrame = SplitViewFrame;
            ApplicationData.MainVm = new MainViewModel();
            DataContext = ApplicationData.MainVm;
        }

        private void SplitViewOpener_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X > 5)
            {
                ApplicationData.MainVm.IsMenuOpen = true;
            }
        }

        private void SplitViewPane_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X < -40)
            {
                ApplicationData.MainVm.IsMenuOpen = false;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.RegisterBackgroundTask();
        }
    }
}
    

