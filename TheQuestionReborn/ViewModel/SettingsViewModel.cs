using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.Settings;

namespace TheQuestionReborn.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private Visibility isEnableTileUpdateTiming;
        public List<TimeTileUpdate> TimeUpdateTiles { get; set; }

        public SettingsViewModel()
        {
            InitializeMenu();
        }
        public string Version => GetAppVersion();

        private void InitializeMenu()
        {
            TimeUpdateTiles = new List<TimeTileUpdate>
            {
                new TimeTileUpdate {Title = "15 минут", Value = 15},
                new TimeTileUpdate {Title = "30 минут", Value = 30},
                new TimeTileUpdate {Title = "1 час", Value = 60},
                new TimeTileUpdate {Title = "2 часа", Value = 120},
                new TimeTileUpdate {Title = "3 часа", Value = 180},
                new TimeTileUpdate {Title = "6 часа", Value = 360},
                new TimeTileUpdate {Title = "12 часа", Value = 720},
                new TimeTileUpdate {Title = "24 часа", Value = 720}
            };

        }
        public bool IsTileShow
        {
            get
            {
                var isTileShow = ProgramState.CurrentState.Settings.IsTileShow;
                VisibilityTileUpdateTiming = isTileShow ? Visibility.Visible : Visibility.Collapsed;
                return isTileShow;
            }
            set
            {
                ProgramState.CurrentState.Settings.IsTileShow = value;

                Task.Run(() =>
                {
                    ProgramState.CurrentState.SaveSettings();
                    App.RegisterBackgroundTask();
                });


                VisibilityTileUpdateTiming = value ? Visibility.Visible : Visibility.Collapsed;

                RaisePropertyChanged("IsTileShow");
            }
        }

        public Visibility VisibilityTileUpdateTiming
        {
            get
            {
                return isEnableTileUpdateTiming;
            }
            set
            {
                isEnableTileUpdateTiming = value;
                RaisePropertyChanged("VisibilityTileUpdateTiming");
            }
        }

        public int TimeUpdateTileIndex
        {
            get
            {
                return TimeUpdateTiles.FindIndex(x => x.Value == ProgramState.CurrentState.Settings.TimeTileUpdate);
            }
            set
            {
                ProgramState.CurrentState.Settings.TimeTileUpdate = TimeUpdateTiles[value].Value;

                Task.Run(() =>
                {
                    ProgramState.CurrentState.SaveSettings();
                    App.RegisterBackgroundTask();
                });

                RaisePropertyChanged("TimeUpdateTileIndex");
            }
        }

        public static string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"v. {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }

    public class TimeTileUpdate
    {
        public uint Value { get; set; }

        public string Title { get; set; }
    }
}
