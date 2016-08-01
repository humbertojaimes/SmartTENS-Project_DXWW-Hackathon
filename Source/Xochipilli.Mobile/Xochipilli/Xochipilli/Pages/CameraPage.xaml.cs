using Xamarin.Forms;
using Xochipilli.ViewModels;

namespace Xochipilli.Pages
{
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();
            BindingContext = new CameraPageViewModel(this.Navigation);
        }
    }
}