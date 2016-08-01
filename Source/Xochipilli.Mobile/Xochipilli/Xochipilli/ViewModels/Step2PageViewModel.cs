using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Xochipilli.Classes;
using Xochipilli.Entities;
using Xochipilli.Interfaces;
using Xochipilli.Pages;

namespace Xochipilli.ViewModels
{
    public class Step2PageViewModel : ObservableObject
    {
        private Stopwatch timeMeter = new Stopwatch();
        private INavigation _navigation;
        private IProcessProvider processProvider;
        private Participant participant;
        private string m_Bluetooth;
        private string m_Intensity;
        private string m_elapsed;
        private string m_maxIntensity;
        private bool m_IsRunning = false;
        private bool m_IsNextEnabled = true;
        int levelNumber = 0, maxLevelNumber = 0;

        public ICommand ConnectCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand NextPageCommand { get; set; }

        public Step2PageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Setup();

            ConnectCommand = new Command(() =>
            {
                DependencyService.Get<IBluetoothService>().Connect();
                Bluetooth = "Connected";
            });

            UpCommand = new Command(() =>
            {
                DependencyService.Get<IBluetoothService>().WriteData("R");
                Intensity = (levelNumber++).ToString();
                if (levelNumber >= maxLevelNumber)
                {
                    maxLevelNumber = levelNumber;
                }
                MaxIntensity = (maxLevelNumber).ToString();
                timeMeter.Start();
            });

            DownCommand = new Command(() =>
            {
                DependencyService.Get<IBluetoothService>().WriteData("L");
                Intensity = (levelNumber--).ToString();
                if (levelNumber >= maxLevelNumber)
                {
                    maxLevelNumber = levelNumber;
                }
                MaxIntensity = (maxLevelNumber).ToString();
            });

            StopCommand = new Command(() =>
            {
                DependencyService.Get<IBluetoothService>().WriteData("B");
                timeMeter.Stop();
                ElapsedTime = Math.Round(timeMeter.Elapsed.TotalSeconds, 1).ToString();
            });

            NextPageCommand = new Command(async () =>
            {
                IsRunning = true;
                IsNextEnabled = false;
                participant.Intensity = MaxIntensity;
                participant.TimeElapsed = ElapsedTime;

                await Sender.SaveDataInCloudAsync(participant)
                       .ContinueWith((c) =>
                       {
                           if (c.Result)
                           {
                               Device.BeginInvokeOnMainThread(() =>
                               {
                                   IsRunning = false;
                                   IsNextEnabled = true;

                                   //navigate to next page.
                                   _navigation.PushAsync(new Step3Page());
                               });
                           }
                           else
                           {
                               Device.BeginInvokeOnMainThread(async () =>
                               {
                                   await App.Current.MainPage.DisplayAlert("Notification", "There was a problem with the service.", "Ok");
                               });
                           }

                       });
            });
        }

        private void Setup()
        {
            this.processProvider = DependencyService.Get<IProcessProvider>();
            participant = processProvider.ReadLocalData();
        }

        public string Bluetooth
        {
            get
            {
                return m_Bluetooth;
            }
            set
            {
                m_Bluetooth = value;
                OnPropertyChanged();
            }
        }

        public string Intensity
        {
            get
            {
                return m_Intensity;
            }
            set
            {
                m_Intensity = value;
                OnPropertyChanged();
            }
        }

        public string MaxIntensity
        {
            get
            {
                return m_maxIntensity;
            }
            set
            {
                m_maxIntensity = value;
                OnPropertyChanged();
            }
        }

        public string ElapsedTime
        {
            get
            {
                return m_elapsed;
            }
            set
            {
                m_elapsed = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get
            {
                return m_IsRunning;
            }
            set
            {
                m_IsRunning = value;
                OnPropertyChanged();
            }
        }

        public bool IsNextEnabled
        {
            get
            {
                return m_IsNextEnabled;
            }
            set
            {
                m_IsNextEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}