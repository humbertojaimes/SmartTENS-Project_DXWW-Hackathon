using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using Xochipilli.Classes;
using Xochipilli.Entities;
using Xochipilli.Interfaces;
using Xochipilli.Pages;

namespace Xochipilli.ViewModels
{
    public class Step1PageViewModel : ObservableObject
    {
        private INavigation _navigation;
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private IProcessProvider processProvider;
        private IMediaPicker _Mediapicker;

        private ImageSource unknown = Device.OnPlatform(
                        iOS: ImageSource.FromFile("unknown.jpg"),
                        Android: ImageSource.FromFile("unknown.jpg"),
                        WinPhone: ImageSource.FromFile("unknown.jpg"));

        private ImageSource m_Picture;
        private Stream m_PictureStream;
        private string m_Status = string.Empty;
        private string m_Fullname = string.Empty;
        private string m_Age = string.Empty;
        private string m_Weight = string.Empty;
        private List<string> m_Gender = new List<string>();
        private string m_SelectedGender = string.Empty;
        private bool m_IsRunning = false;
        private bool m_IsNextEnabled = true;
        private string m_EstimatedIntensity = string.Empty;

        public ICommand TakePhotoCommand { get; set; }
        public ICommand NextPageCommand { get; set; }

        public Step1PageViewModel(INavigation navigation)
        {            
            _navigation = navigation;

            Setup();        

            TakePhotoCommand = new Command(async () =>
            {
                await TakePictureAsync();
            });     

            NextPageCommand = new Command(async () =>
            {
                var isSimulator = DependencyService.Get<ISpecificPlatform>().CheckIfSimulator();
                if (isSimulator)
                    PictureStream = processProvider.GetDummyStream();

                if (string.IsNullOrEmpty(Fullname) || string.IsNullOrEmpty(Age) || string.IsNullOrEmpty(Weight) || SelectedGender == "Select a gender" || PictureStream == null)
                {
                    await App.Current.MainPage.DisplayAlert("Notification", "Please ensure all required fields are filled.", "Ok");
                }
                else
                {
                    IsRunning = true;
                    IsNextEnabled = false;

                    Participant participant = new Participant();
                    participant.FullName = Fullname;
                    participant.Age = Age;
                    participant.Weight = Weight;
                    participant.Gender = SelectedGender;

                    //save participant in cloud.
                    await processProvider.ProcessParticipantAsync(PictureStream)
                    .ContinueWith((b) =>
                    {
                        participant.FileName = b.Result;
                        //save participant in file.
                        processProvider.SaveLocalData(participant);

                        Device.BeginInvokeOnMainThread(() => {

                            IsRunning = false;
                            IsNextEnabled = true;

                            //navigate to next page.
                            _navigation.PushAsync(new Step2Page());
                        });
                    });
                }     
          
            });
        }

        private void Setup()
        {
            m_Gender.Add("Select a gender");
            m_Gender.Add("Male");
            m_Gender.Add("Female");
            SelectedGender = Gender[0];

            this.processProvider = DependencyService.Get<IProcessProvider>();

            if (_Mediapicker == null)
            {
                try
                {
                    var device = Resolver.Resolve<IDevice>();
                    _Mediapicker = DependencyService.Get<IMediaPicker>() ?? device.MediaPicker;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: {0}", ex.Message);
                }
            }
        }

        public async Task<MediaFile> TakePictureAsync()
        {
            Setup();

            Picture = null;

            return await _Mediapicker.TakePhotoAsync(new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Front,
                MaxPixelDimension = 400
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Status = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    Status = "Canceled";
                }
                else
                {
                    var mediaFile = t.Result;
                    Picture = ImageSource.FromStream(() => mediaFile.Source);
                    PictureStream = mediaFile.Source;

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }

        private async Task<bool> GetEstimatedIntensityAsync()
        {
            bool result = false;
            if ((!string.IsNullOrEmpty(Age)) && (!string.IsNullOrEmpty(Weight)) && SelectedGender != "Select a gender")
            {
                int maxRetry = 3;
                int i = 1;
                while (i <= maxRetry)
                {
                    try
                    {
                        string GenderAcro = (SelectedGender == "Male") ? "M" : "F";
                        var data = await HttpManager.PerformGetPredictorRequestAsync(string.Format("{0}-{1}-{2}", Age, GenderAcro, Weight));
                        EstimatedIntensity = JsonConvert.DeserializeObject<string>(data);
                        result = true;
                        break;
                    }
                    catch
                    {
                        i++;
                    }
                }
            }
            
            return result;
        }

        public Stream PictureStream
        {
            get
            {             
                return m_PictureStream;
            }
            set
            {
                m_PictureStream = value;
                OnPropertyChanged();
            }
        }

        public ImageSource Picture
        {
            get
            {
                if (m_Picture == null)
                {
                    m_Picture = unknown;
                }
                return m_Picture;
            }
            set
            {
                m_Picture = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
                OnPropertyChanged();
            }
        }

        public string Fullname
        {
            get
            {
                return m_Fullname;
            }
            set
            {
                m_Fullname = value;
                OnPropertyChanged();
            }
        }

        public string Age
        {
            get
            {
                return m_Age;
            }
            set
            {
                m_Age = value;
                GetEstimatedIntensityAsync();
                OnPropertyChanged();
            }
        }

        public string Weight
        {
            get
            {
                return m_Weight;
            }
            set
            {
                m_Weight = value;
                GetEstimatedIntensityAsync();
                OnPropertyChanged();
            }
        }

        public List<string> Gender
        {
            get
            {
                return m_Gender;
            }          
        }        

        public string SelectedGender
        {
            get
            {
                return m_SelectedGender;
            }
            set
            {
                m_SelectedGender = value;
                GetEstimatedIntensityAsync();
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

        public string EstimatedIntensity
        {
            get
            {
                return m_EstimatedIntensity;
            }
            set
            {
                m_EstimatedIntensity = value;
                OnPropertyChanged();
            }
        }
        
    }
}