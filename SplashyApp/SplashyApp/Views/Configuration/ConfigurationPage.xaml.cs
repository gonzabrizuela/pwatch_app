using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplashyApp.Views.Configuration
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigurationPage : ContentPage
	{
		IBluetoothLE ble;
		IAdapter adapter;
		ObservableCollection<IDevice> deviceList;
		IDevice device;
		public ConfigurationPage ()
		{


			InitializeComponent ();
			ble = CrossBluetoothLE.Current;
			adapter = CrossBluetoothLE.Current.Adapter;
			deviceList = new ObservableCollection<IDevice>();
			lv.ItemsSource = deviceList;

		}
		/// <summary>
		/// Scan the list of devices
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnStatus_Clicked(object sender, EventArgs e) {
			var state = ble.State;
			DisplayAlert("Notice", state.ToString(), "OK !");

			if (state == BluetoothState.Off) {
				txtErrorBle.BackgroundColor = Color.Red;
				txtErrorBle.TextColor = Color.White;
				txtErrorBle.Text = "Tu Bluetooth esta apagado, prendelo para poder conectarte";
			}
		}
		private async void btnScan_Clicked(object sender, EventArgs e)
		{
			try {
				deviceList.Clear();
				adapter.DeviceDiscovered += (s, a) =>
				{
					deviceList.Add(a.Device);
				};
				if (!ble.Adapter.IsScanning)
				{
					await adapter.StartScanningForDevicesAsync();
				}
			}
			catch(Exception ex)
			{
				DisplayAlert("Notice", ex.Message.ToString(), "ERROR !");
			}
			
		}
		private async void btnConnect_Clicked(object sender, EventArgs e)
		{
            try
            {
				if (device != null)
				{
					await adapter.ConnectToDeviceAsync(device);
				}
				else {
					DisplayAlert("Notice", "No hay ningún dispositivo seleccionado !", "OK");
				}
            }
            catch (DeviceConnectionException ex)
            {
				DisplayAlert("Notice", ex.Message.ToString(), "OK !");
				//throw;
			}
			
		}
	
		private async void btnKnowConnect_Clicked(object sender, EventArgs e)
		{
            try
            {
				await adapter.ConnectToKnownDeviceAsync(new Guid("guid"));
            }
            catch (DeviceConnectionException ex)
            {
				DisplayAlert("Notice", ex.Message.ToString(), "OK");
                //throw;
            }
		}
		IList<IService> Services;
		IService Service;
		/// <summary>
		/// Get list of services
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnGetServices_Clicked(object sender, EventArgs e)
		{
			Services = (IList<IService>)await device.GetServicesAsync();
			//Service = await device.GetServiceAsync(Guid.Parse("guid"));
			Service = await device.GetServiceAsync(device.Id);
		}
		IList<ICharacteristic> Characteristics;
		ICharacteristic Characteristic;
		/// <summary>
		/// Get Characteristics
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnGetcharacters_Clicked(object sender, EventArgs e)
		{
			 var Characteristics = await Service.GetCharacteristicsAsync();
			//Service = await device.GetServiceAsync(Guid.Parse("guid"));
			Guid idGuid = Guid.Parse("guid");
			Characteristic = await Service.GetCharacteristicAsync(idGuid);
		}
		IDescriptor descriptor;
		IList<IDescriptor> descriptors;
		private async void btnDescriptors_Clicked(object sender, EventArgs e)
		{
			descriptors = (IList<IDescriptor>)await Characteristic.GetDescriptorsAsync();
			descriptor = await Characteristic.GetDescriptorAsync(Guid.Parse("guid"));

		}
		private async void btnGetRW_Clicked(object sender, EventArgs e)
		{
			var bytes = await descriptor.ReadAsync();
			await descriptor.WriteAsync(bytes);

		}
		private async void btnUpdate_Clicked(object sender, EventArgs e)
		{
			Characteristic.ValueUpdated += (o, args) =>
			{
				var bytes = args.Characteristic.Value;
			};
			await Characteristic.StartUpdatesAsync();
		}

		/// <summary>
		/// Select items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
			if (lv.SelectedItem == null) {
				return;
			}
			device = lv.SelectedItem as IDevice;
		}
		private void btnDescRW_Clicked(object sender, EventArgs e)
		{

		}
	}
}