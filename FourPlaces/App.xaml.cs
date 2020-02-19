using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Storm.Mvvm;
using FourPlaces.Views;
using FourPlaces.Services;

namespace FourPlaces
{
    public partial class App : MvvmApplication
    {
        public App() :base (() => new MainPage())
        {
            InitializeComponent();
            DependencyService.Register<ISessionService, SessionService>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
