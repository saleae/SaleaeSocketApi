using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saleae.SocketApi
{
	public class ExportUnitTests
	{
		SaleaeClient client = new SaleaeClient(); // auto opens ports.  throws if fails.
		string folder_path = Environment.GetFolderPath( Environment.SpecialFolder.Desktop ) + @"\ExportUnitTest\";


		public void RunAllUnitTests()
		{
			//CsvDitialTestSet();
			//CsvAnalogTestSet();
			//CsvMixedModeTest();
			BinaryTest();
			VcdTest();
			MatlabTest();
		}

		public void CsvDitialTestSet()
		{
			if( System.IO.Directory.Exists( folder_path ) == false )
				System.IO.Directory.CreateDirectory( folder_path );

			//digital only tests.
			ExportDataStruct export_settings = new ExportDataStruct
			{
				FileName = folder_path + "digital_tsv_allchannels_alltime",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				//StartingTime = 0.0,
				//EndingTime = 0.001,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvTab,
				CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] { 0, 1 }, new int[] { } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "digital_csv_somechannels_sometime",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.SpecificChannels,
				DigitalChannelsToExport = new int[] {2},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvNoHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvComma,
				CsvOutputMode = CsvOutputMode.CsvSingleNumber,
				CsvTimestampType = CsvTimestampType.CsvSample,
				CsvDisplayBase = CsvBase.CsvHexadecimal,
				CsvDensity = CsvDensity.CsvComplete,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};

			client.SetActiveChannels( new int[] { 0, 1, 2, 3 }, new int[] { } );
			client.SetCaptureSeconds( 0.003 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

		}

		public void CsvAnalogTestSet()
		{
			if( System.IO.Directory.Exists( folder_path ) == false )
				System.IO.Directory.CreateDirectory( folder_path );

			//digital only tests.
			ExportDataStruct export_settings = new ExportDataStruct
			{
				FileName = folder_path + "analog_tsv_allchannels_alltime_voltage",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				//StartingTime = 0.0,
				//EndingTime = 0.001,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.Voltage
			};


			client.SetActiveChannels( new int[] { }, new int[] { 0, 1 } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 15 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, false, true );

			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "analog_csv_somechannels_sometime_rawadc",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.SpecificChannels,
				DigitalChannelsToExport = new int[] { },
				AnalogChannelsToExport = new int[] { 1 },

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvNoHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvComma,
				//CsvOutputMode = CsvOutputMode.CsvSingleNumber,
				//CsvTimestampType = CsvTimestampType.CsvSample,
				CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvComplete,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.ADC
			};

			client.SetActiveChannels( new int[] {  }, new int[] { 0, 1, 2 } );
			client.SetCaptureSeconds( 0.003 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, false, true );

		}

		public void CsvMixedModeTest()
		{
			if( System.IO.Directory.Exists( folder_path ) == false )
				System.IO.Directory.CreateDirectory( folder_path );

			//mixed everything csv
			ExportDataStruct export_settings = new ExportDataStruct
			{
				FileName = folder_path + "mixed_tsv_everything",
				DataExportMixedExportMode = DataExportMixedModeExportType.AnalogAndDigital,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				//StartingTime = 0.0,
				//EndingTime = 0.001,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.Voltage
			};


			client.SetActiveChannels( new int[] { 0, 1 }, new int[] { 0, 1 } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 15 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );

			//mixed, selective channels and time
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "mixed_csv_both_somechannels_sometime",
				DataExportMixedExportMode = DataExportMixedModeExportType.AnalogAndDigital,
				ExportChannelSelection = DataExportChannelSelection.SpecificChannels,
				DigitalChannelsToExport = new int[] { 1 },
				AnalogChannelsToExport = new int[] { 1 },

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvComma,
				//CsvOutputMode = CsvOutputMode.CsvSingleNumber,
				//CsvTimestampType = CsvTimestampType.CsvSample,
				CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvComplete,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.ADC
			};

			client.SetActiveChannels( new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 } );
			client.SetCaptureSeconds( 0.003 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );

			//mixed as digital only:
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "mixed_csv_as_digital_all",
				DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.SpecificChannels,
				DigitalChannelsToExport = new int[] { 0, 1, 2 },
				//AnalogChannelsToExport = new int[] { 1 },

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				//StartingTime = 0.001,
				//EndingTime = 0.002,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvComma,
				CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				CsvTimestampType = CsvTimestampType.CsvSample,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.Voltage
			};

			client.SetActiveChannels( new int[] { 0, 1, 2 }, new int[] { 0, 1 } );
			client.SetCaptureSeconds( 0.003 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );


			//mixed as anaog only:
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "mixed_csv_as_analog_all",
				DataExportMixedExportMode = DataExportMixedModeExportType.AnalogOnly,
				ExportChannelSelection = DataExportChannelSelection.SpecificChannels,
				//DigitalChannelsToExport = new int[] { 1 },
				AnalogChannelsToExport = new int[] { 0 },

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				//StartingTime = 0.001,
				//EndingTime = 0.002,

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvComma,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvSample,
				CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.Voltage
			};

			client.SetActiveChannels( new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 } );
			client.SetCaptureSeconds( 0.003 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );

		}

		public void BinaryTest()
		{
			if( System.IO.Directory.Exists( folder_path ) == false )
				System.IO.Directory.CreateDirectory( folder_path );

			//digital binary yest
			ExportDataStruct export_settings = new ExportDataStruct
			{
				FileName = folder_path + "digital_binary_all_samples",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportBinary,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] { 0, 1 }, new int[] { } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital just transitions test
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "digital_binary_all_samples_just_trasitions",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportBinary,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				BinaryOutputMode = BinaryOutputMode.BinaryEveryChange,
				BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] { 0, 1 }, new int[] { } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//analog test
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "digital_binary_analog",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportBinary,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] {  }, new int[] { 3 } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 25 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, false, true );

		}

		public void VcdTest()
		{
			if( System.IO.Directory.Exists( folder_path ) == false )
				System.IO.Directory.CreateDirectory( folder_path );

			//digital VCD yest
			ExportDataStruct export_settings = new ExportDataStruct
			{
				FileName = folder_path + "digital_vcd_range",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportVcd,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] { 0, 1 }, new int[] { } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );
		}

		public void MatlabTest()
		{
			
			if( System.IO.Directory.Exists( folder_path ) == false )
				System.IO.Directory.CreateDirectory( folder_path );

			//digital only matlab
			ExportDataStruct export_settings = new ExportDataStruct
			{
				FileName = folder_path + "digital_matlab",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportMatlab,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] { 0, 1 }, new int[] { } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );



			//analog only matlab
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "analog_matlab",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportMatlab,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] {  }, new int[] { 5 } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, false, true );


			//matlab mixed mode:
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "mixed_matlab",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeTimes,
				StartingTime = 0.001,
				EndingTime = 0.002,

				DataExportType = DataExportType.ExportMatlab,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.ADC
			};


			client.SetActiveChannels( new int[] { 0, 1 }, new int[] { 5 } );
			client.SetCaptureSeconds( 0.001 );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );
		}

		public void TriggerOffsetTest()
		{
			//connect the signal generator to channel 0 of Logic8/Pro8/Pro16. (do not use Logic4, the original Logic, or Logic16)
			//set signal generator to 1 hz square wave and start.



			if( System.IO.Directory.Exists( folder_path ) == false )
				System.IO.Directory.CreateDirectory( folder_path );

			//digital only CSV all time
			ExportDataStruct export_settings = new ExportDataStruct
			{
				FileName = folder_path + "trigger_digital_csv_alltime",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				StartingTime = -0.005, //-5mS
				EndingTime = 0.005, //+5mS

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvComma,
				CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};

			
			client.SetActiveChannels( new int[] { 0 }, new int[] { } );
			client.SetCapturePretriggerBufferSize( 1000000 ); //1M pre-trigger buffer. that's 10ms at 100 MSPS.
			client.SetCaptureSeconds( 0.01 ); //10ms, prob go to 30 ms
			client.SetSampleRate( new SampleRate { DigitalSampleRate = 100000000, AnalogSampleRate = 0 } );
			
			
			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );


			//digital only CSV time range
			export_settings.FileName = folder_path + "trigger_digital_csv_timerange";
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;
			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only CSV time range with sample numbers

			export_settings.FileName = folder_path + "trigger_digital_csv_timerange_samples";
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;
			export_settings.CsvTimestampType = CsvTimestampType.CsvSample;
			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only binary all time
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "trigger_digital_binary_alltime",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				StartingTime = -0.005, //-5mS
				EndingTime = 0.005, //+5mS

				//DataExportType = DataExportType.ExportCsv,
				//CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				//CsvDelimiterType = CsvDelimiterType.CsvTab,
				//CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				//CsvTimestampType = CsvTimestampType.CsvTime,
				//CsvDisplayBase = CsvBase.CsvHexadecimal,
				//CsvDensity = CsvDensity.CsvTransition,

				BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				//AnalogFormat = AnalogOutputFormat.ADC
			};

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only binary time_rage
			export_settings.FileName = folder_path + "trigger_digital_binary_timerange";
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only binary all time with sample numbers
			export_settings.FileName = folder_path + "trigger_digital_binary_alltime_with_sample_number";
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeAll;
			export_settings.BinaryOutputMode = BinaryOutputMode.BinaryEveryChange;

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only binary time range with sample numbers
			export_settings.FileName = folder_path + "trigger_digital_binary_range_with_sample_number";
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only VCD all time
			export_settings.FileName = folder_path + "trigger_digital_vcd_alltime";
			export_settings.DataExportType = DataExportType.ExportVcd;
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeAll;

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only VCD time range
			export_settings.FileName = folder_path + "trigger_digital_vcd_range";
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only Matlab all time
			export_settings.FileName = folder_path + "trigger_digital_matlab_alltime";
			export_settings.DataExportType = DataExportType.ExportMatlab;
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeAll;

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );

			//digital only Matlab time range
			export_settings.FileName = folder_path + "trigger_digital_matlab_range";
			export_settings.DataExportType = DataExportType.ExportMatlab;
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 5 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, false );


			







			//Mix mode full mix CSV all time
			export_settings = new ExportDataStruct
			{
				FileName = folder_path + "trigger_mix_both_csv_alltime",
				//DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly,
				ExportChannelSelection = DataExportChannelSelection.AllChannels,
				//DigitalChannelsToExport = new int[] {},
				//AnalogChannelsToExport = new int[] {},

				SamplesRangeType = DataExportSampleRangeType.RangeAll,
				StartingTime = -0.005, //-5mS
				EndingTime = 0.005, //+5mS

				DataExportType = DataExportType.ExportCsv,
				CsvIncludeHeaders = CsvHeadersType.CsvIncludesHeaders,
				CsvDelimiterType = CsvDelimiterType.CsvComma,
				CsvOutputMode = CsvOutputMode.CsvOneColumnPerBit,
				CsvTimestampType = CsvTimestampType.CsvTime,
				CsvDisplayBase = CsvBase.CsvHexadecimal,
				CsvDensity = CsvDensity.CsvTransition,

				//BinaryOutputMode = BinaryOutputMode.BinaryEverySample,
				//BinaryBitShifting = BinaryBitShifting.BinaryOriginalBitPositions,
				//BinaryOutputWordSize = BinaryOutputWordSize.Binary16Bit,

				AnalogFormat = AnalogOutputFormat.Voltage
			};

			client.SetActiveChannels( new int[] { 0 }, new int[] { 0 } );
			client.SetCapturePretriggerBufferSize( 5000000 ); //5M pre-trigger buffer. that's 10ms at 500 MSPS.
			client.SetCaptureSeconds( 0.01 ); //10ms, prob go to 30 ms
			client.SetSampleRate( new SampleRate { DigitalSampleRate = 500000000, AnalogSampleRate = 125000 } );

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );



			//mix mode both matlab time range
			export_settings.FileName = folder_path + "trigger_mixed_both_matlab_range";
			export_settings.DataExportType = DataExportType.ExportMatlab;
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;
			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );

			//mix mode digital only csv range
			export_settings.FileName = folder_path + "trigger_mixed_digital_only_csv_range";
			export_settings.SamplesRangeType = DataExportSampleRangeType.RangeTimes;
			export_settings.DataExportType = DataExportType.ExportCsv;
			export_settings.ExportChannelSelection = DataExportChannelSelection.SpecificChannels;
			export_settings.DataExportMixedExportMode = DataExportMixedModeExportType.DigitalOnly;
			export_settings.DigitalChannelsToExport = new int[] { 0 };

			client.Capture(); //blocks until capture is complete, but processing is not complete.
			if( client.BlockUntillProcessingCompleteOrTimeout( new TimeSpan( 0, 0, 35 ) ) == false )
				throw new Exception( "processing took too long" );

			client.ExportData2( export_settings, true, true );


		}
	}
}
