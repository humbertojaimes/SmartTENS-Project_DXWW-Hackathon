using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using Xochipilli.Classes;

namespace Xochipilli.ViewModels
{
    public class CameraPageViewModel : ObservableObject
    {
        private INavigation _navigation;
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private IMediaPicker _Mediapicker;
        private ImageSource m_Picture;
        private string m_Status;

        public ICommand TakePhotoCommand { get; set; }

        public CameraPageViewModel(INavigation navigation)
        {
            _navigation = navigation;

            Setup();

            TakePhotoCommand = new Command(async () =>
            {
                await TakePictureAsync();
            });
        }

        private void Setup()
        {
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

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }

        public ImageSource Picture
        {
            get
            {
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
    }
}