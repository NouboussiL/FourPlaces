using System;
using Storm.Mvvm;
using Storm.Mvvm.Services;
using FourPlaces.Services;
using Xamarin.Forms;
using System.Windows.Input;
using FourPlaces.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Net.Http;
using FourPlaces.Models;
using FourPlaces.Api;

namespace FourPlaces.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private string _identifiant = "mail@mail.com";
        private string _mdp = "mdp";
        private string _erreur;
        private readonly Lazy<ISessionService> _sessionService;
        private readonly Lazy<INavigationService> _navigationService;
        private readonly Lazy<IDialogService> _dialogService;

        public string Identifiant
        {
            get => _identifiant;
            set => SetProperty(ref _identifiant, value);
        }

        public string Mdp
        {
            get => _mdp;
            set => SetProperty(ref _mdp, value);
        }

        public string Erreur
        {
            get => _erreur;
            set => SetProperty(ref _erreur, value);
        }

        public ICommand GoToPlaceList { get; set; }
        public ICommand GoToPlaceListLogged { get; set; }
        public ICommand Register { get; set; }

        public MainPageViewModel()
        {
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            _sessionService = new Lazy<ISessionService>(() => DependencyService.Resolve<ISessionService>());
            _dialogService = new Lazy<IDialogService>(() => DependencyService.Resolve<IDialogService>());

            GoToPlaceList = new Command(LoadPlaceList);
            GoToPlaceListLogged = new Command(LoadPlaceListLogged);
            Register = new Command(ToRegister);
        }

        public void AskPermission()
        {
            CrossPermissions.Current.RequestPermissionsAsync(Permission.Location, Permission.Camera, Permission.Storage);
        }

        public async void ToRegister()
        {
            await _navigationService.Value.PushAsync<RegisteringPage>();
        }

        private async void LoadPlaceList()
        {
            bool result = await _dialogService.Value.DisplayAlertAsync("Si vous continuez sans vous connectez, vous n'aurez pas accès à certaines fonctionnalités."," Voulez-vous continuer ?", "Oui", "Non");
            if (result)
            {
                await _navigationService.Value.PushAsync<PlaceListPage>();
            }
        }



        public async void LoadPlaceListLogged()
        {
            AskPermission();
            ApiClient client = new ApiClient();
            HttpResponseMessage response = await client.Execute(HttpMethod.Post, "https://td-api.julienmialon.com/auth/login", new LoginRequest { Email=_identifiant, Password=_mdp});
            Response<LoginResult> result = await client.ReadFromResponse<Response<LoginResult>>(response);
            if (result.IsSuccess)
            {
                _sessionService.Value.Connect(result.Data);
                await _navigationService.Value.PushAsync<PlaceListPage>();
            }
            else
            {
                Erreur = "Identifiant ou mot de passe incorrect.";
            }
        }
    }
}
