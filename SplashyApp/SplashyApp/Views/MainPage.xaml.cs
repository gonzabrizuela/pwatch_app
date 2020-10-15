using SplashyApp.Models;
using SplashyApp.Views.AboutUS;

using SplashyApp.Views.Configuration;

using SplashyApp.Views.Home;
using SplashyApp.Views.Profile_Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplashyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }

        public MainPage()
        {

            InitializeComponent();

           
            menuList = new List<MasterPageItem>();


            //Fot Android / IOS icons
            var page1 = new MasterPageItem() {id = 1, Title = "Inicio", Icon = "Home.png" };
            var page2 = new MasterPageItem() {id = 2,  Title = "Perfil", Icon = "UserProfile.png" };
            var page3 = new MasterPageItem() {id = 3 , Title = "Configuracion", Icon = "Configuration.png" };
         
          

            menuList.Add(page1);
            menuList.Add(page2);
            menuList.Add(page3);
          
           

           
            navigationDrawerList.ItemsSource = menuList;
           
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));
          
        }


        async private void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {


            var myselecteditem = e.Item as MasterPageItem;

            switch (myselecteditem.id)
            {

                case 1:
                    await Navigation.PushAsync(new HomePage());
                   
                   
                    break;
                case 2:
                    await Navigation.PushAsync(new ProfileSettingsPage());
                 
                    break;
                case 3:
                    await Navigation.PushAsync(new ConfigurationPage());
                  
                    break;
           
             


            }
            ((ListView)sender).SelectedItem = null;
            IsPresented = false;


        }
    }

 
 

  
    }
