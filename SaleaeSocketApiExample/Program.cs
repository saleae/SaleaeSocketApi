using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saleae.SocketApi
{
	class SaleaeApiExample
	{
		static void Main( string[] args )
		{
			SaleaeApiExample app = new SaleaeApiExample();
			app.Run();
		}


		SaleaeClient Client;


		public void Run()
		{
			//Set this variable to have all text socket commands printed to the console.
			SaleaeClient.PrintCommandsToConsole = true;

			//Make sure to enable the socket server in the Logic software preferences, and make sure that it is running!

			//This demo is designed to show some common socket commands, and interacts best with either the simulation or real Logic 8, Logic Pro 8, or Logic Pro 16.

			//lets run a quick demo!
			Console.WriteLine( "Logic Socket API demonstation application.\n" );

			Console.WriteLine( "enter host IP address, or press enter for localhost" );
			String host = Console.ReadLine();
			if( host.Length == 0 )
				host = "127.0.0.1";
			Console.WriteLine( "enter host port, or press enter for default ( 10429 )" );
			String port_str = Console.ReadLine();
			if( port_str.Length == 0 )
				port_str = "10429";
			int port = int.Parse( port_str );

			Console.WriteLine( "Connecting..." );
			try
			{
				Client = new SaleaeClient( host, port );
			}
			catch( Exception ex )
			{
				Console.WriteLine( "Error while connecting: " + ex.Message );
				Console.ReadLine();
				return;
			}
			StringHelper.WriteLine( "Connected" );
			Console.WriteLine( "" );

			var devices = Client.GetConnectedDevices();
			var active_device = devices.Single( x => x.IsActive == true );

			Console.WriteLine( "currently availible devices:" );
			devices.ToList().ForEach( x => Console.WriteLine( x.Name ) );

			Console.WriteLine( "currently active device: " + active_device.Name );
			Console.WriteLine( "" );
			Console.WriteLine( "Press Enter to Continue" );
			Console.ReadLine();

			var analyzers = Client.GetAnalyzers();

			if( analyzers.Any() )
			{
				Console.WriteLine( "Current analyzers:" );
				analyzers.ToList().ForEach( x => Console.WriteLine( x.AnalyzerType ) );
				Console.WriteLine( "" );
				Console.WriteLine( "Press Enter to Continue" );
				Console.ReadLine();
			}

			if( active_device.DeviceType == DeviceType.Logic8 || active_device.DeviceType == DeviceType.LogicPro8 || active_device.DeviceType == DeviceType.LogicPro16 )
			{
				Console.WriteLine( "changing active channels" );
				Client.SetActiveChannels( new int[] { 2, 5, 6, 7 }, new int[] { 0, 1 } );
				Console.WriteLine( "" );
				Console.WriteLine( "Press Enter to Continue" );
				Console.ReadLine();

				var possible_sample_rates = Client.GetAvailableSampleRates();

				if( possible_sample_rates.Any( x => x.AnalogSampleRate == 125000 ) )
				{
					Console.WriteLine( "Changing sample rate" );
					Client.SetSampleRate( possible_sample_rates.First( x => x.AnalogSampleRate == 125000 ) );
					Console.WriteLine( "" );
					Console.WriteLine( "Press Enter to Continue" );
					Console.ReadLine();
				}

				//set trigger. There are 4 digital channels. all need to be specified.
				Console.WriteLine( "setting trigger" );
				Client.SetTrigger( new Trigger[] { Trigger.None, Trigger.PositivePulse, Trigger.Low, Trigger.High }, 1E-6, 5E-3 );
				Console.WriteLine( "" );
				Console.WriteLine( "Press Enter to Continue" );
				Console.ReadLine();


			}
			else
			{
				Console.WriteLine( "to see more cool features demoed by this example, please switch to a Logic 8, Logic Pro 8, or Logic Pro 16. Physical or simulation" );
			}

			Console.WriteLine( "setting capture time" );
			Client.SetCaptureSeconds( 0.25 );
			Console.WriteLine( "" );
			Console.WriteLine( "Press Enter to Continue" );
			Console.ReadLine();

			Console.WriteLine( "starting capture" );
			Client.Capture();
			Console.WriteLine( "" );
			Console.WriteLine( "Press Enter to Exit" );
			Console.ReadLine();





		}

	
	}
}
