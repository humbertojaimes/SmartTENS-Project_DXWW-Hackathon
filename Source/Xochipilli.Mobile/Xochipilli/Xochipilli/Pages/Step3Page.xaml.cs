using Xamarin.Forms;
using Xochipilli.ViewModels;

namespace Xochipilli.Pages
{
    public partial class Step3Page : ContentPage
    {
        public Step3Page()
        {
            InitializeComponent();
            BindingContext = new Step3PageViewModel(this.Navigation);
        }
    }
}