using Newtonsoft.Json;
using System.Windows.Input;
using Xamarin.Forms;
using Xochipilli.Classes;
using Xochipilli.Entities;
using Xochipilli.Pages;

namespace Xochipilli.ViewModels
{
    public class MainPageViewModel
    {
        private INavigation _navigation;

        public ICommand SendTestDataCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand Step1Command { get; set; }

        public MainPageViewModel(INavigation navigation)
        {
            _navigation = navigation;

            SendTestDataCommand = new Command(() =>
            {
                SendTestAsync();
            });

            TakePhotoCommand = new Command(() =>
            {
                _navigation.PushAsync(new CameraPage());
            });

            Step1Command = new Command(() =>
            {
                _navigation.PushAsync(new Step1Page());
            });
        }

        private async void SendTestAsync()
        {
            Participant participant = new Participant();
            participant.FullName = "Fulanito Martínez";
            participant.Gender = "Male";
            participant.Age = "33";
            participant.FileName = "picture.jpg";
            participant.Intensity = "3";
            participant.TimeElapsed = "3000";

            var data = await HttpManager.PerformPostRequestAsync("saveparticipant", participant);
            if (data != null)
            {
                bool register_result = JsonConvert.DeserializeObject<bool>(data);

                if (register_result)
                {
                    await
                         App.Current.MainPage.DisplayAlert("Notification",
                             "Participant processed.", "Ok");
                }
                else
                {
                    await
                         App.Current.MainPage.DisplayAlert("Notification",
                             "There was a problem with the service.", "Ok");
                }
            }
            else
            {
                await
                         App.Current.MainPage.DisplayAlert("Notification",
                             "There was a problem with the service.", "Ok");
            }
        }
    }
}