using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.Mvvm.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FourPlaces.ViewModels;

namespace FourPlaces.Views
{
    public partial class PlaceListPage : BaseContentPage
    {
        public PlaceListPage()
        {
            BindingContext = new PlaceListPageViewModel();
            InitializeComponent();
        }
    }
}