using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Saleae.SocketApi
{

	public enum DeviceType
	{
		[Description( "LOGIC_DEVICE" )]
		Logic,
		[Description( "LOGIC_16_DEVICE" )]
		Logic16,
		[Description( "LOGIC_4_DEVICE" )]
		Logic4,
		[Description( "LOGIC_8_DEVICE" )]
		Logic8,
		[Description( "LOGIC_PRO_8_DEVICE" )]
		LogicPro8,
		[Description( "LOGIC_PRO_16_DEVICE" )]
		LogicPro16

	}

	public struct ConnectedDevice
	{
		public DeviceType DeviceType { get; set; }
		public String Name { get; set; }
		public UInt64 DeviceId { get; set; }
		public int Index { get; set; }
		public bool IsActive { get; set; }
	}

	//SetTrigger
	public enum Trigger {
		[Description( "" )]
		None,
		[Description( "HIGH" )]
		High,
		[Description( "LOW" )]
		Low,
		[Description( "NEGEDGE" )]
		FallingEdge,
		[Description( "POSEDGE" )]
		RisingEdge,
		[Description( "POSPULSE" )]
		NegativePulse,
		[Description( "NEGPULSE" )]
		PositivePulse 
	};

	public enum PerformanceOption { Full = 100, Half = 50, Third = 33, Quarter = 25, Low = 20 };

	//Export Data
	public enum DataExportChannelSelection
	{
		[Description( "ALL_CHANNELS" )]
		AllChannels,
		[Description( "SPECIFIC_CHANNELS" )]
		SpecificChannels 
	}

	public enum DataExportMixedModeExportType
	{
		[Description( "DIGITAL_ONLY" )]
		DigitalOnly,
		[Description( "ANALOG_ONLY" )]
		AnalogOnly,
		[Description( "ANALOG_AND_DIGITAL" )]
		AnalogAndDigital
	};

	public enum DataExportSampleRangeType {
		[Description( "ALL_TIME" )]
		RangeAll,
		[Description( "TIME_SPAN" )]
		RangeTimes 
	};
	public enum DataExportType {
		[Description( "BINARY" )]
		ExportBinary,
		[Description( "CSV" )]
		ExportCsv,
		[Description( "VCD" )]
		ExportVcd,
		[Description( "MATLAB" )]
		ExportMatlab
	};

	public enum CsvHeadersType {
		[Description( "HEADERS" )]
		CsvIncludesHeaders,
		[Description( "NO_HEADERS" )]
		CsvNoHeaders
	};
	public enum CsvDelimiterType {
		[Description( "COMMA" )]
		CsvComma,
		[Description( "TAB" )]
		CsvTab 
	};
	public enum CsvOutputMode {
		[Description( "COMBINED" )]
		CsvSingleNumber,
		[Description( "SEPARATE" )]
		CsvOneColumnPerBit
	};
	public enum CsvTimestampType {
		[Description( "TIME_STAMP" )]
		CsvTime,
		[Description( "SAMPLE_NUMBER" )]
		CsvSample
	};
	public enum CsvBase {
		[Description( "BIN" )]
		CsvBinary,
		[Description( "DEC" )]
		CsvDecimal,
		[Description( "HEX" )]
		CsvHexadecimal,
		[Description( "ASCII" )]
		CsvAscii 
	};
	public enum CsvDensity {
		[Description( "ROW_PER_CHANGE" )]
		CsvTransition,
		[Description( "ROW_PER_SAMPLE" )]
		CsvComplete 
	};

	public enum BinaryOutputMode {
		[Description( "EACH_SAMPLE" )]
		BinaryEverySample,
		[Description( "ON_CHANGE" )]
		BinaryEveryChange
	};
	public enum BinaryBitShifting {
		[Description( "NO_SHIFT" )]
		BinaryOriginalBitPositions,
		[Description( "RIGHT_SHIFT" )]
		BinaryShiftRight 
	};
	public enum BinaryOutputWordSize {
		[Description( "8" )]
		Binary8Bit,
		[Description( "16" )]
		Binary16Bit,
		[Description( "32" )]
		Binary32Bit,
		[Description( "64" )]
		Binary64Bit 
	};

	public enum AnalogOutputFormat {
		[Description( "VOLTAGE" )]
		Voltage,
		[Description( "ADC" )]
		ADC 
	};

	public struct Channel
	{
		public enum ChannelDataType
		{
			[Description( "ANALOG" )]
			AnalogChannel,
			[Description( "DIGITAL" )]
			DigitalChannel
		};

		public int Index { get; set; }
		public ChannelDataType DataType { get; set; }

		public string GetExportString()
		{
			return Index.ToString() + " " + DataType.GetDescription();
		}

	}

	public struct ExportDataStruct
	{
		public String FileName;

		//mixed mode export type. Only applies when analog and digital channels were present in the original capture.
		public DataExportMixedModeExportType DataExportMixedExportMode;

		//Channels
		public DataExportChannelSelection ExportChannelSelection;
		public int[] DigitalChannelsToExport;
		public int[] AnalogChannelsToExport;

		//Time Range
		public DataExportSampleRangeType SamplesRangeType; //{ RangeAll, RangeTimes }
		public double StartingTime;
		public double EndingTime;

		//Export Type
		public DataExportType DataExportType; //{ ExportBinary, ExportCsv, ExportVcd }

		//Type: CSV (only set if Export type is CSV)
		public CsvHeadersType CsvIncludeHeaders; //{ CsvIncludesHeaders, CsvNoHeaders }
		public CsvDelimiterType CsvDelimiterType;//{ CsvComma, CsvTab }
		public CsvOutputMode CsvOutputMode;//{ CsvSingleNumber, CsvOneColumnPerBit }
		public CsvTimestampType CsvTimestampType;//{ CsvTime, CsvSample }
		public CsvBase CsvDisplayBase;//{ CsvBinary, CsvDecimal, CsvHexadecimal, CsvAscii }
		public CsvDensity CsvDensity;//{ CsvTransition, CsvComplete }

		//Type: Binary
		public BinaryOutputMode BinaryOutputMode;//{ BinaryEverySample, BinaryEveryChange }
		public BinaryBitShifting BinaryBitShifting;//{ BinaryOriginalBitPositions, BinaryShiftRight }
		public BinaryOutputWordSize BinaryOutputWordSize;//{ Binary8Bit, Binary16Bit, Binary32Bit, Binary64Bit }

		//Type: Analog Value
		public AnalogOutputFormat AnalogFormat; //This feature needs v1.1.32+ 
	}

	public struct SampleRate
	{
		public int AnalogSampleRate { get; set; }
		public int DigitalSampleRate { get; set; }
	}

	public struct Analyzer
	{
		public String AnalyzerType { get; set; }
		public int Index { get; set; }
	}

	class SaleaeSocketApiException : Exception
	{
		public SaleaeSocketApiException()
			: base()
		{
		}

		public SaleaeSocketApiException( string message )
			: base( message )
		{
		}

		public SaleaeSocketApiException( string message, Exception inner_exception )
			: base( message, inner_exception )
		{
		}

	}
}
