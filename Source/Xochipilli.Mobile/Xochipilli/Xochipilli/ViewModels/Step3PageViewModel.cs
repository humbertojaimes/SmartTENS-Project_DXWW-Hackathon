using System.Windows.Input;
using Xamarin.Forms;
using Xochipilli.Classes;
using Xochipilli.Entities;
using Xochipilli.Interfaces;

namespace Xochipilli.ViewModels
{
    public class Step3PageViewModel : ObservableObject
    {
        private INavigation _navigation;
        private IProcessProvider processProvider;
        private Participant participant;

        public ICommand NextPageCommand { get; set; }

        public Step3PageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Setup();        

            NextPageCommand = new Command(() =>
            {
                DependencyService.Get<ISpecificPlatform>().CloseApplication();
            });
        }

        private void Setup()
        {
            this.processProvider = DependencyService.Get<IProcessProvider>();
            participant = processProvider.ReadLocalData();
        }
    }
}