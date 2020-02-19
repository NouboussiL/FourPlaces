using System;
using System.Collections.Generic;
using Storm.Mvvm;
using FourPlaces.Models;
using FourPlaces.Views;
using Storm.Mvvm.Services;
using FourPlaces.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Net.Http;
using FourPlaces.Api;

namespace FourPlaces.ViewModels
{
    class PlaceListPageViewModel : ViewModelBase
    {
        private List<PlaceItemSummary> _places;
        private string _imageId;
        private readonly Lazy<INavigationService> _navigationService;
        private readonly Lazy<ISessionService> _sessionService;
        private readonly Lazy<IDialogService> _dialogService;

        public ICommand AddPlaceCommand { get; set; }
        public ICommand ViewProfileCommand { get; set; }

        private PlaceItemSummary _selectedPlace;

        public PlaceItemSummary SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                if (SetProperty(ref _selectedPlace, value) && value != null)
                {
                    LoadPlaceDetail();
                }
            }
        }

        public string ImageId
        {
            get => _imageId;
            set => SetProperty(ref _imageId, value);
        }

        public List<PlaceItemSummary> Places
        {
            get => _places;
            set => SetProperty(ref _places, value);
        }

        public PlaceListPageViewModel()
        {
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            _sessionService = new Lazy<ISessionService>(() => DependencyService.Resolve<ISessionService>());
            _dialogService = new Lazy<IDialogService>(() => DependencyService.Resolve<IDialogService>());
            ViewProfileCommand = new Command(ViewProfile);
            AddPlaceCommand = new Command(AddPlace);
            GetPlaces();
        }

        private async void ViewProfile()
        {
            if(_sessionService.Value.GetLogin().AccessToken == "")
            {
                var result = await _dialogService.Value.DisplayAlertAsync("Vous devez être connecté.", "Retourner à la page de connexion ?", "Oui", "Non");
                if (result)
                {
                    await _navigationService.Value.PopAsync();
                }
            }
            else
            {
                await _navigationService.Value.PushAsync<ProfilePage>();
            }
        }

        private async void LoadPlaceDetail()
        {
            await _navigationService.Value.PushAsync<PlaceDetailPage>(new Dictionary<string, object>
            {
                {"PlaceItemSummary",_selectedPlace }
            });
        }

        private async void AddPlace()
        {
            if (_sessionService.Value.GetLogin().AccessToken == "")
            {
                var result = await _dialogService.Value.DisplayAlertAsync("Vous devez être connecté.", "Retourner à la page de connexion ?", "Oui", "Non");
                if (result)
                {
                    await _navigationService.Value.PopAsync();
                }
            }
            else
            {
                await _navigationService.Value.PushAsync<AddPlacePage>();
            }
        }


        public async void GetPlaces()
        {
            ApiClient client = new ApiClient();
            HttpResponseMessage response = await client.Execute(HttpMethod.Get, "https://td-api.julienmialon.com/places");
            Response<List<PlaceItemSummary>> result = await client.ReadFromResponse<Response<List<PlaceItemSummary>>>(response);
            if (result.IsSuccess)
            {
                _places = result.Data;
            }
        }
    }
}
