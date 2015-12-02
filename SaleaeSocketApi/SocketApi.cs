using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Reflection;
using System.ComponentModel;

namespace Saleae.SocketApi
{
	public class SaleaeClient
	{

		public static bool PrintCommandsToConsole = false;

		TcpClient Socket;
		NetworkStream Stream;
		int port;
		String host;

		//Command strings
		const String set_trigger_cmd = "SET_TRIGGER";
		const String set_num_samples_cmd = "SET_NUM_SAMPLES";
		const String set_sample_rate_cmd = "SET_SAMPLE_RATE";
		const String set_capture_seconds_cmd = "SET_CAPTURE_SECONDS";
		const String capture_to_file_cmd = "CAPTURE_TO_FILE";
		const String save_to_file_cmd = "SAVE_TO_FILE";
		const String load_from_file_cmd = "LOAD_FROM_FILE";
		const String export_data_cmd = "EXPORT_DATA";
		const String export_data2_cmd = "EXPORT_DATA2";

		const String get_all_sample_rates_cmd = "GET_ALL_SAMPLE_RATES";
		const String get_analyzers_cmd = "GET_ANALYZERS";
		const String export_analyzer_cmd = "EXPORT_ANALYZER";
		const String get_inputs_cmd = "GET_INPUTS";
		const String capture_cmd = "CAPTURE";
		const String stop_capture_cmd = "STOP_CAPTURE";
		const String get_capture_pretrigger_buffer_size_cmd = "GET_CAPTURE_PRETRIGGER_BUFFER_SIZE";
		const String set_capture_pretrigger_buffer_size_cmd = "SET_CAPTURE_PRETRIGGER_BUFFER_SIZE";
		const String get_connected_devices_cmd = "GET_CONNECTED_DEVICES";
		const String select_active_device_cmd = "SELECT_ACTIVE_DEVICE";

		const String get_active_channels_cmd = "GET_ACTIVE_CHANNELS";
		const String set_active_channels_cmd = "SET_ACTIVE_CHANNELS";
		const String reset_active_channels_cmd = "RESET_ACTIVE_CHANNELS";

		const String set_performance_cmd = "SET_PERFORMANCE";
		const String get_performance_cmd = "GET_PERFORMANCE";
		const String is_processing_complete_cmd = "IS_PROCESSING_COMPLETE";
		const String is_analyzer_complete_cmd = "IS_ANALYZER_COMPLETE";

		const String close_all_tabs_cmd = "CLOSE_ALL_TABS";

		public SaleaeClient( String host_str = "127.0.0.1", int port_input = 10429 )
		{
			this.port = port_input;
			this.host = host_str;

			Socket = new TcpClient( host, port );
			Stream = Socket.GetStream();
		}

		private void WriteString( String str )
		{
			byte[] data = str.toByteArray().Concat( "\0".toByteArray() ).ToArray();

			Stream.Write( data, 0, data.Length );

			StringHelper.WriteLine( "Wrote data: " + str );
		}

		private void GetResponse( ref String response )
		{
			while( ( String.IsNullOrEmpty( response ) ) )
			{
				response += Stream.ReadString();
			}
			StringHelper.WriteLine( "Response: " + response );

			if( !( response.Substring( response.LastIndexOf( 'A' ) ) == "ACK" ) ) //note: this does not properly handle NAK replies.
				throw new SaleaeSocketApiException();
		}

		/// <summary>
		/// Give the Socket API a custom command
		/// </summary>
		/// <param name="export_command">Ex: "set_sample_rate, 10000000"</param>
		/// <returns>Response String</returns>
		public String CustomCommand( String export_command )
		{
			WriteString( export_command );

			String response = "";
			while( ( String.IsNullOrEmpty( response ) ) )
			{
				response += Stream.ReadString();
			}

			return response;
		}

		/// <summary>
		/// Set the capture trigger. Every active digital channel must be set, in order.
		/// </summary>
		/// <param name="triggers">List of triggers for active channels. Ex"High, Low, Posedge, Negedge, Low, High, ..."</param>
		public void SetTrigger( Trigger[] triggers, double minimum_pulse_width_s = 0.0, double maximum_pulse_width_s = 1.0 )
		{

			List<string> command = new List<string>();

			command.Add( set_trigger_cmd );

			if( triggers.Count( x => x == Trigger.PositivePulse || x == Trigger.NegativePulse || x == Trigger.RisingEdge || x == Trigger.FallingEdge ) > 1 )
				throw new SaleaeSocketApiException( "invalid trigger specifications" );

			foreach( Trigger channel in triggers )
			{
				command.Add( channel.GetDescription() );
				if( channel == Trigger.PositivePulse || channel == Trigger.NegativePulse )
				{
					command.Add( minimum_pulse_width_s.ToString() );
					command.Add( maximum_pulse_width_s.ToString() );
				}

			}

			string tx_command = String.Join(", ", command );
			WriteString( tx_command );
				
			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Set number of samples for capture
		/// </summary>
		/// <param name="num_samples">Number of samples to set</param>
		public void SetNumSamples( int num_samples )
		{
			String export_command = set_num_samples_cmd + ", ";
			export_command += num_samples.ToString();
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Set number of seconds to capture for
		/// </summary>
		/// <param name="capture_seconds">Number of seconds to capture</param>
		public void SetCaptureSeconds( double seconds )
		{
			String export_command = set_capture_seconds_cmd + ", ";
			export_command += seconds.ToString();
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Closes all currently open tabs.
		/// </summary>
		public void CloseAllTabs()
		{
			String export_command = close_all_tabs_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Set the sample rate for capture
		/// </summary>
		/// <param name="sample_rate">Sample rate to set</param>
		public void SetSampleRate( SampleRate sample_rate )
		{
			String export_command = set_sample_rate_cmd + ", ";
			export_command += sample_rate.DigitalSampleRate.ToString();
			export_command += ", " + sample_rate.AnalogSampleRate.ToString();

			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Start capture and save when capture finishes
		/// </summary>
		/// <param name="file">File to save capture to</param>
		public void CaptureToFile( String file )
		{
			String export_command = capture_to_file_cmd + ", ";
			export_command += file;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Save active tab capture to file
		/// </summary>
		/// <param name="file">File to save capture to</param>
		public void SaveToFile( String file )
		{
			String export_command = save_to_file_cmd + ", ";
			export_command += file;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Load a saved capture from fil
		/// </summary>
		/// <param name="file">File to load</param>
		public void LoadFromFile( String file )
		{
			String export_command = load_from_file_cmd + ", ";
			export_command += file;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		//create input struct
		public void ExportData( ExportDataStruct export_data_struct )
		{
			//channels
			const String all_channels_option = ", ALL_CHANNELS";
			const String digital_channels_option = ", DIGITAL_CHANNELS";
			const String analog_channels_option = ", ANALOG_CHANNELS";

			//time span
			const String all_time_option = ", ALL_TIME";
			const String time_span_option = ", TIME_SPAN";

			const String csv_option = ", CSV";
			const String headers_option = ", HEADERS";
			const String no_headers_option = ", NO_HEADERS";
			const String tab_option = ", TAB";
			const String comma_option = ", COMMA";
			const String sample_number_option = ", SAMPLE_NUMBER";
			const String time_stamp_option = ", TIME_STAMP";
			const String combined_option = ", COMBINED";
			const String separate_option = ", SEPARATE";
			const String row_per_change_option = ", ROW_PER_CHANGE";
			const String row_per_sample_option = ", ROW_PER_SAMPLE";
			const String dec_option = ", DEC";
			const String hex_option = ", HEX";
			const String bin_option = ", BIN";
			const String ascii_option = ", ASCII";

			const String binary_option = ", BINARY";
			const String each_sample_option = ", EACH_SAMPLE";
			const String on_change_option = ", ON_CHANGE";

			const String voltage_option = ", VOLTAGE";
			const String raw_adc_option = ", ADC";
			const String vcd_option = ", VCD";
			const String matlab_option = ", MATLAB";


			String export_command = export_data_cmd;
			export_command += ", " + export_data_struct.FileName;

			if( export_data_struct.ExportChannelSelection == DataExportChannelSelection.AllChannels )
				export_command += all_channels_option;
			else
			{
				if( export_data_struct.DigitalChannelsToExport.Length > 0 )
				{
					export_command += digital_channels_option;
					foreach( int channel in export_data_struct.DigitalChannelsToExport )
						export_command += ", " + channel.ToString();
				}

				if( export_data_struct.AnalogChannelsToExport.Length > 0 )
				{
					export_command += analog_channels_option;
					foreach( int channel in export_data_struct.AnalogChannelsToExport )
						export_command += ", " + channel.ToString();
				}
			}

			if( ( export_data_struct.ExportChannelSelection == DataExportChannelSelection.AllChannels ) || ( export_data_struct.AnalogChannelsToExport != null && export_data_struct.AnalogChannelsToExport.Length > 0 ) )
			{
				if( export_data_struct.AnalogFormat == AnalogOutputFormat.Voltage )
					export_command += voltage_option;
				else if( export_data_struct.AnalogFormat == AnalogOutputFormat.ADC )
					export_command += raw_adc_option;
			}

			if( export_data_struct.SamplesRangeType == DataExportSampleRangeType.RangeAll )
				export_command += all_time_option;
			else if( export_data_struct.SamplesRangeType == DataExportSampleRangeType.RangeTimes )
			{
				export_command += time_span_option;
				export_command += ", " + export_data_struct.StartingTime;
				export_command += ", " + export_data_struct.EndingTime;
			}

			if( export_data_struct.DataExportType == DataExportType.ExportCsv )
			{
				export_command += csv_option;

				if( export_data_struct.CsvIncludeHeaders == CsvHeadersType.CsvIncludesHeaders )
					export_command += headers_option;
				else if( export_data_struct.CsvIncludeHeaders == CsvHeadersType.CsvNoHeaders )
					export_command += no_headers_option;

				if( export_data_struct.CsvDelimiterType == CsvDelimiterType.CsvTab )
					export_command += tab_option;
				else if( export_data_struct.CsvDelimiterType == CsvDelimiterType.CsvComma )
					export_command += comma_option;

				if( export_data_struct.CsvTimestampType == CsvTimestampType.CsvSample )
					export_command += sample_number_option;
				else if( export_data_struct.CsvTimestampType == CsvTimestampType.CsvTime )
					export_command += time_stamp_option;

				if( export_data_struct.CsvOutputMode == CsvOutputMode.CsvSingleNumber )
					export_command += combined_option;
				else if( export_data_struct.CsvOutputMode == CsvOutputMode.CsvOneColumnPerBit )
					export_command += separate_option;

				if( export_data_struct.CsvDensity == CsvDensity.CsvTransition )
					export_command += row_per_change_option;
				else if( export_data_struct.CsvDensity == CsvDensity.CsvComplete )
					export_command += row_per_sample_option;

				if( export_data_struct.CsvDisplayBase == CsvBase.CsvDecimal )
					export_command += dec_option;
				else if( export_data_struct.CsvDisplayBase == CsvBase.CsvHexadecimal )
					export_command += hex_option;
				else if( export_data_struct.CsvDisplayBase == CsvBase.CsvBinary )
					export_command += bin_option;
				else if( export_data_struct.CsvDisplayBase == CsvBase.CsvAscii )
					export_command += ascii_option;
			}
			else if( export_data_struct.DataExportType == DataExportType.ExportBinary )
			{
				export_command += binary_option;

				if( export_data_struct.BinaryOutputMode == BinaryOutputMode.BinaryEverySample )
					export_command += each_sample_option;
				else if( export_data_struct.BinaryOutputMode == BinaryOutputMode.BinaryEveryChange )
					export_command += on_change_option;

				if( export_data_struct.BinaryOutputWordSize == BinaryOutputWordSize.Binary8Bit )
					export_command += ", 8";
				else if( export_data_struct.BinaryOutputWordSize == BinaryOutputWordSize.Binary16Bit )
					export_command += ", 16";
				else if( export_data_struct.BinaryOutputWordSize == BinaryOutputWordSize.Binary32Bit )
					export_command += ", 32";
				else if( export_data_struct.BinaryOutputWordSize == BinaryOutputWordSize.Binary64Bit )
					export_command += ", 64";

			}
			else if( export_data_struct.DataExportType == DataExportType.ExportVcd )
			{
				export_command += vcd_option;
			}
			else if( export_data_struct.DataExportType == DataExportType.ExportMatlab )
			{
				export_command += matlab_option;
			}


			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// This replaced the hard to use and buggy EXPORT_DATA command.
		/// </summary>
		/// <param name="export_settings"></param>
		/// <param name="capture_contains_digital_channels"></param>
		/// <param name="capture_contains_analog_channels"></param>
		/// <returns></returns>
		public bool ExportData2( ExportDataStruct export_settings, bool capture_contains_digital_channels, bool capture_contains_analog_channels )
		{
			bool is_mixed_mode_capture = capture_contains_digital_channels && capture_contains_analog_channels; //different export options happen in this case.
			if( is_mixed_mode_capture && export_settings.ExportChannelSelection == DataExportChannelSelection.AllChannels )
				export_settings.DataExportMixedExportMode = DataExportMixedModeExportType.AnalogAndDigital; //this is not required to be explicitly set by the user.

			List<string> command_parts = new List<string>();
			command_parts.Add( export_data2_cmd );

			command_parts.Add( export_settings.FileName );

			command_parts.Add( export_settings.ExportChannelSelection.GetDescription() );

			if( export_settings.ExportChannelSelection == DataExportChannelSelection.SpecificChannels )
			{
				if( is_mixed_mode_capture )
					command_parts.Add( export_settings.DataExportMixedExportMode.GetDescription() );

				if( export_settings.DigitalChannelsToExport != null && export_settings.DigitalChannelsToExport.Any() )
					command_parts.AddRange( export_settings.DigitalChannelsToExport.Select( x => new Channel { Index = x, DataType = Channel.ChannelDataType.DigitalChannel }.GetExportString() ) );
				if( export_settings.AnalogChannelsToExport != null && export_settings.AnalogChannelsToExport.Any() )
				command_parts.AddRange( export_settings.AnalogChannelsToExport.Select( x => new Channel { Index = x, DataType = Channel.ChannelDataType.AnalogChannel }.GetExportString() ) );
			}

			//time options.
			command_parts.Add( export_settings.SamplesRangeType.GetDescription() );

			if( export_settings.SamplesRangeType == DataExportSampleRangeType.RangeTimes )
			{
				command_parts.Add( export_settings.StartingTime.ToString() );
				command_parts.Add( export_settings.EndingTime.ToString() );
			}


			command_parts.Add( export_settings.DataExportType.GetDescription() );
			//digital only CSV
			if( capture_contains_digital_channels && export_settings.DataExportType == DataExportType.ExportCsv && ( !is_mixed_mode_capture || export_settings.DataExportMixedExportMode == DataExportMixedModeExportType.DigitalOnly ) )
			{
				command_parts.Add( export_settings.CsvIncludeHeaders.GetDescription() );
				command_parts.Add( export_settings.CsvDelimiterType.GetDescription() );
				command_parts.Add( export_settings.CsvTimestampType.GetDescription() );
				command_parts.Add( export_settings.CsvOutputMode.GetDescription() );
				if( export_settings.CsvOutputMode == CsvOutputMode.CsvSingleNumber )
					command_parts.Add( export_settings.CsvDisplayBase.GetDescription() );
				command_parts.Add( export_settings.CsvDensity.GetDescription() );
			}

			//analog only CSV
			if( capture_contains_analog_channels && export_settings.DataExportType == DataExportType.ExportCsv && ( !is_mixed_mode_capture || export_settings.DataExportMixedExportMode == DataExportMixedModeExportType.AnalogOnly ) )
			{
				command_parts.Add( export_settings.CsvIncludeHeaders.GetDescription() );
				command_parts.Add( export_settings.CsvDelimiterType.GetDescription() );
				command_parts.Add( export_settings.CsvDisplayBase.GetDescription() );
				command_parts.Add( export_settings.AnalogFormat.GetDescription() );

			}

			//mixed mode CSV
			if( export_settings.DataExportType == DataExportType.ExportCsv && is_mixed_mode_capture && export_settings.DataExportMixedExportMode == DataExportMixedModeExportType.AnalogAndDigital )
			{
				command_parts.Add( export_settings.CsvIncludeHeaders.GetDescription() );
				command_parts.Add( export_settings.CsvDelimiterType.GetDescription() );
				command_parts.Add( export_settings.CsvDisplayBase.GetDescription() );
				command_parts.Add( export_settings.AnalogFormat.GetDescription() );
			}

			//digital binary
			if( capture_contains_digital_channels &&  export_settings.DataExportType == DataExportType.ExportBinary && ( !is_mixed_mode_capture || export_settings.DataExportMixedExportMode == DataExportMixedModeExportType.DigitalOnly ) )
			{
				command_parts.Add( export_settings.BinaryOutputMode.GetDescription() );
				command_parts.Add( export_settings.BinaryBitShifting.GetDescription() );
				command_parts.Add( export_settings.BinaryOutputWordSize.GetDescription() );
			}

			//analog only binary
			if( capture_contains_analog_channels && export_settings.DataExportType == DataExportType.ExportBinary && ( !is_mixed_mode_capture || export_settings.DataExportMixedExportMode == DataExportMixedModeExportType.AnalogOnly ) )
			{
				command_parts.Add( export_settings.AnalogFormat.GetDescription() );
			}

			//VCD (always digital only)
			if( export_settings.DataExportType == DataExportType.ExportVcd )
			{
				//no settings
			}

			//Matlab digital:
			if( capture_contains_digital_channels && export_settings.DataExportType == DataExportType.ExportMatlab && ( !is_mixed_mode_capture || export_settings.DataExportMixedExportMode == DataExportMixedModeExportType.DigitalOnly ) )
			{

				//no settings
			}

			//Matlab analog or mixed:
			if( capture_contains_analog_channels && export_settings.DataExportType == DataExportType.ExportMatlab && ( !is_mixed_mode_capture || export_settings.DataExportMixedExportMode != DataExportMixedModeExportType.DigitalOnly ) )
			{
				command_parts.Add( export_settings.AnalogFormat.GetDescription() );
			}


			string socket_command = String.Join( ", ", command_parts );
			WriteString( socket_command );

			String response = "";
			GetResponse( ref response );

			return true;
		}

		/// <summary>
		/// Get the active analyzers on the current tab
		/// </summary>
		/// <returns>A string of the names of the analyzers</returns>
		public List<Analyzer> GetAnalyzers()
		{
			string export_command = get_analyzers_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );

			var lines = response.Split( '\n' ).Select( x => x.Trim() ).Where( x => !String.IsNullOrWhiteSpace( x ) && !x.Contains( "ACK" ) ).ToList();

			List<Analyzer> analyzers = new List<Analyzer>();

			foreach( string line in lines )
			{
				var elements = line.Split( ',' ).Select( x => x.Trim() ).ToList();
				analyzers.Add( new Analyzer
				{
					AnalyzerType = elements[0],
					Index = int.Parse( elements[1] )
				} );
			}

			return analyzers;
		}

		/// <summary>
		/// Export a selected analyzer to a file
		/// </summary>
		/// <param name="selected">index of the selected analyzer(GetAnalyzer return string index + 1)</param>
		/// <param name="filename">file to save analyzer to</param>
		/// <param name="mXmitFile">mXmitFile</param>
		public void ExportAnalyzers( int selected, String filename, bool mXmitFile )
		{
			String export_command = export_analyzer_cmd + ", ";
			export_command += selected.ToString() + ", " + filename;
			if( mXmitFile == true )
				export_command += ", mXmitFile";
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
			if( mXmitFile == true )
				Console.WriteLine( response );
		}

		/// <summary>
		/// Start device capture
		/// </summary>
		public void Capture()
		{
			String export_command = capture_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Stop the current capture
		/// </summary>

		public void StopCapture()
		{
			String export_command = stop_capture_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}


		/// <summary>
		/// Get size of pre-trigger buffer
		/// </summary>
		/// <returns>buffer size in # of samples</returns>
		public int GetCapturePretriggerBufferSize()
		{
			String export_command = get_capture_pretrigger_buffer_size_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
			String[] input_string = response.Split( '\n' );
			int buffer_size = int.Parse( input_string[ 0 ] );
			return buffer_size;
		}

		/// <summary>
		/// set pre-trigger buffer size
		/// </summary>
		/// <param name="buffer_size">buffer size in # of samples</param>
		public void SetCapturePretriggerBufferSize( int buffer_size )
		{
			String export_command = set_capture_pretrigger_buffer_size_cmd + ", ";
			export_command += buffer_size.ToString();
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Return the devices connected to the software
		/// </summary>
		/// <returns>Array of ConnectedDevices structs which contain device information</returns>
		public List<ConnectedDevice> GetConnectedDevices()
		{
			String command = get_connected_devices_cmd;
			WriteString( command );

			String response = "";
			GetResponse( ref response );
			var response_strings = response.Split( '\n' ).ToList();
			response_strings.RemoveAll( x => x.Contains( "ACK" ) );
			

			List<ConnectedDevice> devices = new List<ConnectedDevice>();

			foreach( string line in response_strings )
			{
				var elements = line.Split( ',' ).Select( x => x.Trim() ).ToList();

				DeviceType device_type;

				if( TryParseDeviceType( elements[ 2 ], out device_type ) == false )
					throw new SaleaeSocketApiException( "unexpected value" );

				ConnectedDevice device = new ConnectedDevice
				{
					Index = int.Parse( elements[ 0 ] ),
					Name = elements[ 1 ],
					DeviceType = device_type,
					DeviceId = Convert.ToUInt64( elements[ 3 ].Substring( 2 ), 16 ),
					IsActive = (elements.Count() == 5 && elements[4] == "ACTIVE") ? true : false
				};
				devices.Add( device );
			}

			return devices;
		}

		/// <summary>
		/// Select the active capture device
		/// </summary>
		/// <param name="device_number">Index of device (as returned from ConnectedDevices struct)</param>
		public void SelectActiveDevice( int device_number )
		{
			String export_command = select_active_device_cmd + ", ";
			export_command += device_number.ToString();
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Set the performance option
		/// </summary>
		public void SetPerformanceOption( PerformanceOption performance )
		{
			String export_command = set_performance_cmd + ", ";
			export_command += performance.ToString( "D" );
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Get the performance option currently selected.
		/// </summary>
		/// <returns>A PerformanceOption enum</returns>
		public PerformanceOption GetPerformanceOption()
		{
			String export_command = get_performance_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );

			PerformanceOption selected_option = ( PerformanceOption )Convert.ToInt32( response.Split( ',' )[ 0 ] );
			return selected_option;
		}


		/// <summary>
		/// Get whether or not the software is done processing data. You must wait for data to be finished processing before you can export/save. 
		/// </summary>
		/// <returns>A boolean indicating if processing is complete</returns>

		public bool IsProcessingComplete()
		{
			String export_command = is_processing_complete_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );

			bool complete_processing = Convert.ToBoolean( response.Split( '\n' )[ 0 ] );
			return complete_processing;
		}

		/// <summary>
		/// Get whether or not the software is done processing data. You must wait for data to be finished processing before you can export/save. 
		/// </summary>
		/// <returns>A boolean indicating if processing is complete</returns>

		public bool IsAnalyzerProcessingComplete( int index )
		{
			String export_command = is_analyzer_complete_cmd;
			export_command += ", " + Convert.ToString( index );
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );

			bool complete_processing = Convert.ToBoolean( response.Split( '\n' )[ 0 ] );
			return complete_processing;
		}

		/// <summary>
		/// Calls IsProcessingComplete every 250 ms.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public bool BlockUntillProcessingCompleteOrTimeout( TimeSpan timeout )
		{
			DateTime processing_timeout = DateTime.Now.Add( timeout );
			bool processing_finished = false;
			do
			{
				
				processing_finished = IsProcessingComplete();

				if( !processing_finished )
					System.Threading.Thread.Sleep( 250 );
			}
			while( !processing_finished && DateTime.Now < processing_timeout );

			return processing_finished;
		}


		/// <summary>
		/// Get the currently available sample rates for the selected performance options
		/// </summary>
		/// <returns>Array of sample rate combinations available</returns>
		public List<SampleRate> GetAvailableSampleRates()
		{
			WriteString( get_all_sample_rates_cmd );
			String response = "";
			GetResponse( ref response );

			List<SampleRate> sample_rates = new List<SampleRate>();
			String[] new_line = { "\n" };
			String[] responses = response.Split( new_line, StringSplitOptions.RemoveEmptyEntries );

			for( int i = 0; i < responses.Length - 1; i++ )
			{
				String[] split_sample_rate = responses[ i ].Split( ',' );
				if( split_sample_rate.Length != 2 )
				{
					sample_rates.Clear();
					return sample_rates;
				}

				SampleRate new_sample_rate = new SampleRate();
				new_sample_rate.DigitalSampleRate = Convert.ToInt32( split_sample_rate[ 0 ].Trim() );
				new_sample_rate.AnalogSampleRate = Convert.ToInt32( split_sample_rate[ 1 ].Trim() );
				sample_rates.Add( new_sample_rate );
			}

			return sample_rates;
		}

		/// <summary>
		/// Get active channels for devices Logic16, Logic 8(second gen), Logic 8 pro, Logic 16 pro
		/// </summary>
		/// <returns>array of active channel numbers</returns>
		public void GetActiveChannels( List<int> digital_channels, List<int> analog_channels )
		{
			String export_command = get_active_channels_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );

			digital_channels.Clear();
			analog_channels.Clear();

			String[] input_string = response.Split( '\n' );
			String[] channels_string = input_string[ 0 ].Split( ',' );

			bool add_to_digital_channel_list = true;
			for( int i = 0; i < channels_string.Length; ++i )
			{
				if( channels_string[ i ] == "digital_channels" )
				{
					add_to_digital_channel_list = true;
					continue;
				}
				else if( channels_string[ i ] == "analog_channels" )
				{
					add_to_digital_channel_list = false;
					continue;
				}

				if( add_to_digital_channel_list )
					digital_channels.Add( int.Parse( channels_string[ i ] ) );
				else
					analog_channels.Add( int.Parse( channels_string[ i ] ) );
			}

		}

		/// <summary>
		/// Set the active channels for devices Logic16, Logic 8(second gen), Logic 8 pro, Logic 16 pro
		/// </summary>
		/// <param name="channels">array of channels to be active: 0-15</param>
		public void SetActiveChannels( int[] digital_channels = null, int[] analog_channels = null )
		{

			String export_command = set_active_channels_cmd;
			if( digital_channels != null )
			{
				export_command += ", " + "digital_channels";
				for( int i = 0; i < digital_channels.Length; ++i )
					export_command += ", " + digital_channels[ i ].ToString();
			}
			if( analog_channels != null )
			{
				export_command += ", " + "analog_channels";
				for( int i = 0; i < analog_channels.Length; ++i )
					export_command += ", " + analog_channels[ i ].ToString();
			}
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		/// <summary>
		/// Reset to default active logic 16 channels (0-15)
		/// </summary>
		public void ResetActiveChannels()
		{
			String export_command = reset_active_channels_cmd;
			WriteString( export_command );

			String response = "";
			GetResponse( ref response );
		}

		private bool TryParseDeviceType( string input, out DeviceType device_type )
		{
			device_type = DeviceType.Logic; // defualt.

			var all_options = Enum.GetValues( typeof( DeviceType ) ).Cast<DeviceType>();

			if( all_options.Any( x => x.GetDescription() == input.Trim() ) )
			{
				device_type = all_options.Single( x => x.GetDescription() == input.Trim() );
				return true;
			}
			else
			{
				return false;
			}
		}
	}



	public static class StringHelper
	{


		public static byte[] toByteArray( this String str )
		{
			int count = str.Length;
			char[] char_array = str.ToCharArray();
			byte[] array = new byte[ count ];
			for( int i = 0; i < count; ++i )
			{
				array[ i ] = ( byte )char_array[ i ];

			}
			return array;
		}

		public static String ReadString( this NetworkStream stream )
		{

			int max_length = 128;
			byte[] buffer = new byte[ max_length ];
			String str = "";
			int bytes_read = 0;
			while( true )
			{
				bytes_read = stream.Read( buffer, 0, max_length );

				for( int i = 0; i < bytes_read; ++i )
				{
					str += ( char )buffer[ i ];
				}

				if( bytes_read < max_length )
					break;
			}
			return str;

		}

		public static void WriteLine( String str )
		{
			if( SaleaeClient.PrintCommandsToConsole )
				Console.WriteLine( str );
		}

		public static void Write( String str )
		{
			if( SaleaeClient.PrintCommandsToConsole )
				Console.Write( str );
		}

		public static string GetDescription<T>( this T enumerationValue )
			where T : struct
		{
			Type type = enumerationValue.GetType();
			if( !type.IsEnum )
			{
				throw new ArgumentException( "EnumerationValue must be of Enum type", "enumerationValue" );
			}

			//Tries to find a DescriptionAttribute for a potential friendly name
			//for the enum
			MemberInfo[] memberInfo = type.GetMember( enumerationValue.ToString() );
			if( memberInfo != null && memberInfo.Length > 0 )
			{
				object[] attrs = memberInfo[ 0 ].GetCustomAttributes( typeof( DescriptionAttribute ), false );

				if( attrs != null && attrs.Length > 0 )
				{
					//Pull out the description value
					return ( ( DescriptionAttribute )attrs[ 0 ] ).Description;
				}
			}
			//If we have no description attribute, just return the ToString of the enum
			return enumerationValue.ToString();

		}
	}


}
