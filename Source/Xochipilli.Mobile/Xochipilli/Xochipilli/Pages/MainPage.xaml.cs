using Xamarin.Forms;
using Xochipilli.ViewModels;

namespace Xochipilli.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(this.Navigation);
        }
    }
}