using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Media;
using PM2E139.Services;
using PM2E139.Views;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using Plugin.Media.Abstractions;
using Plugin.Geolocator.Abstractions;
using static Xamarin.Essentials.Permissions;

namespace PM2E139
{
    public partial class MainPage : ContentPage
    {
        String pathImagen;

        public MainPage()
        {
            InitializeComponent();
            ubicacion();

        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    var conexion = Connectivity.NetworkAccess;
        //    var local = CrossGeolocator.Current;

        //    if (conexion == NetworkAccess.Internet)
        //    {
        //        if (local != null)
        //        {
        //            local.PositionChanged += Local_PositionChanged; ;
        //            if (!local.IsListening)
        //            {
        //                await local.StartListeningAsync(TimeSpan.FromSeconds(10), 100);
        //            }

        //            var posicion = await local.GetPositionAsync();
        //            txtLat.Text = posicion.Latitude.ToString();
        //            txtLon.Text = posicion.Longitude.ToString();
        //        }
        //        else
        //        {
        //            var posicion = await local.GetLastKnownLocationAsync();
        //            txtLat.Text = posicion.Latitude.ToString();
        //            txtLon.Text = posicion.Longitude.ToString();
        //        }
        //    }
        //}

        private void salir_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private async void Agregar_Clicked(object sender, EventArgs e)
        {
            if (pathImagen == "" || String.IsNullOrEmpty(txtDes.Text) || String.IsNullOrEmpty(txtLat.Text) || String.IsNullOrEmpty(txtLon.Text))
            {
                await DisplayAlert("Aviso", "Datos obligatorios: imagen, latitud, longitud, descripcion", "Ok");

            }
            else
            {
                var emple = new Lugar
                {
                    latitud = txtLat.Text,
                    longitud = txtLon.Text,
                    descripcion = txtDes.Text,
                    image = pathImagen
                };

                var resultado = await App.BaseDatos.EmpleadoGuardar(emple);
                if (resultado != 0)
                {
                    await DisplayAlert("Aviso", "Datos guardados con exito", "OK");
                    pinkFoto.Source = ("");
                    pathImagen = "";
                    txtDes.Text = "";
                    txtLat.Text = "";
                    txtLon.Text = "";
                    ubicacion();

                }
                else
                {
                    await DisplayAlert("Error", "Debe habilitar la ubicacion", "OK");
                }
            }
             ubicacion();
        }

        private async void Lista_Clicked(object sender, EventArgs e)
        {
            var newpage = new ListEmple();
            await Navigation.PushAsync(newpage);

        }

        private async void pinkFoto_Clicked(object sender, EventArgs e)
        {
            var takepic = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "PhotoApp",
                Name = "photo.jpg"
            });
            pathImagen = takepic.Path;


            if (takepic != null)
            {
                pinkFoto.Source = ImageSource.FromStream(() => {
                    return takepic.GetStream();
                });
            }
        }

        async private void ubicacion()
        {
            try
            {
                var local = CrossGeolocator.Current;
                local.DesiredAccuracy = 50;
                if (local.IsGeolocationAvailable)
                {
                    if (local.IsGeolocationEnabled)
                    {
                        if (!local.IsListening)
                        {
                            await local.StartListeningAsync(TimeSpan.FromSeconds(1),5);
                        }
                        local.PositionChanged += Local_PositionChanged1;
                    }
                }

                //var posicion = await local.GetPositionAsync();

                //if (local != null)
                //{
                //    txtLat.Text = posicion.Latitude.ToString();
                //    txtLon.Text = posicion.Longitude.ToString();
                //}
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error", "Error: " + fnsEx, "OK");
            }

        }

        private void Local_PositionChanged1(object sender, PositionEventArgs e)
        {
            
            txtLat.Text = e.Position.Latitude.ToString();
            txtLon.Text = e.Position.Longitude.ToString();

        }

        public bool EmptyField(Entry campo)
        {
            return String.IsNullOrEmpty(campo.Text);
        }
    }
}
