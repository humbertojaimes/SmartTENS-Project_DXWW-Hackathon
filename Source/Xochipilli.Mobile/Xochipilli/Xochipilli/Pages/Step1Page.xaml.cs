using Xamarin.Forms;
using Xochipilli.ViewModels;

namespace Xochipilli.Pages
{
    public partial class Step1Page : ContentPage
    {
        public Step1Page()
        {
            InitializeComponent();
            BindingContext = new Step1PageViewModel(this.Navigation);
        }
    }
}