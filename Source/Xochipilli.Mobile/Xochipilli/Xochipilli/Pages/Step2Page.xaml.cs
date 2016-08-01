using Xamarin.Forms;
using Xochipilli.ViewModels;

namespace Xochipilli.Pages
{
    public partial class Step2Page : ContentPage
    {
        public Step2Page()
        {
            InitializeComponent();
            BindingContext = new Step2PageViewModel(this.Navigation);
        }
    }
}