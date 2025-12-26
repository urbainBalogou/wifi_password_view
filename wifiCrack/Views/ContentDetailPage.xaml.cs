using Microsoft.Maui.Controls;
using wifiCrack.Models;

namespace wifiCrack.Views
{
    public partial class ContentDetailPage : ContentPage
    {
        public ContentDetailPage(EducationalContent content)
        {
            InitializeComponent();
            BindingContext = content;
        }
    }
}
