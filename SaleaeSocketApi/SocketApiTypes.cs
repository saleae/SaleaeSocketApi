using System;
using System.ComponentModel;

namespace Saleae.SocketApi
{
    public enum DeviceType
    {
        [Description("LOGIC_DEVICE")]
        Logic,
        [Description("LOGIC_16_DEVICE")]
        Logic16,
        [Description("LOGIC_4_DEVICE")]
        Logic4,
        [Description("LOGIC_8_DEVICE")]
        Logic8,
        [Description("LOGIC_PRO_8_DEVICE")]
        LogicPro8,
        [Description("LOGIC_PRO_16_DEVICE")]
        LogicPro16
    }

    public struct ConnectedDevice
    {
        public DeviceType DeviceType { get; set; }
        public string Name { get; set; }
        public ulong DeviceId { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
    }

    // SetTrigger
    public enum Trigger
    {
        [Description("")]
        None,
        [Description("HIGH")]
        High,
        [Description("LOW")]
        Low,
        [Description("NEGEDGE")]
        FallingEdge,
        [Description("POSEDGE")]
        RisingEdge,
        [Description("NEGPULSE")]
        NegativePulse,
        [Description("POSPULSE")]
        PositivePulse
    }

    public enum PerformanceOption
    {
        OneHundredPercent = 100,
        EightyPercent = 80,
        SixtyPercent = 60,
        FortyPercent = 40,
        TwentyPercent = 20
    }

    // Export Data
    public enum DataExportChannelSelection
    {
        [Description("ALL_CHANNELS")]
        AllChannels,
        [Description("SPECIFIC_CHANNELS")]
        SpecificChannels
    }

    public enum DataExportMixedModeExportType
    {
        [Description("DIGITAL_ONLY")]
        DigitalOnly,
        [Description("ANALOG_ONLY")]
        AnalogOnly,
        [Description("ANALOG_AND_DIGITAL")]
        AnalogAndDigital
    }

    public enum DataExportSampleRangeType
    {
        [Description("ALL_TIME")]
        RangeAll,
        [Description("TIME_SPAN")]
        RangeTimes
    }

    public enum DataExportType
    {
        [Description("BINARY")]
        ExportBinary,
        [Description("CSV")]
        ExportCsv,
        [Description("VCD")]
        ExportVcd,
        [Description("MATLAB")]
        ExportMatlab
    }

    public enum CsvHeadersType
    {
        [Description("HEADERS")]
        CsvIncludesHeaders,
        [Description("NO_HEADERS")]
        CsvNoHeaders
    }

    public enum CsvDelimiterType
    {
        [Description("COMMA")]
        CsvComma,
        [Description("TAB")]
        CsvTab
    }

    public enum CsvOutputMode
    {
        [Description("COMBINED")]
        CsvSingleNumber,
        [Description("SEPARATE")]
        CsvOneColumnPerBit
    }

    public enum CsvTimestampType
    {
        [Description("TIME_STAMP")]
        CsvTime,
        [Description("SAMPLE_NUMBER")]
        CsvSample
    }

    public enum CsvBase
    {
        [Description("BIN")]
        CsvBinary,
        [Description("DEC")]
        CsvDecimal,
        [Description("HEX")]
        CsvHexadecimal,
        [Description("ASCII")]
        CsvAscii
    }

    public enum CsvDensity
    {
        [Description("ROW_PER_CHANGE")]
        CsvTransition,
        [Description("ROW_PER_SAMPLE")]
        CsvComplete
    }

    public enum BinaryOutputMode
    {
        [Description("EACH_SAMPLE")]
        BinaryEverySample,
        [Description("ON_CHANGE")]
        BinaryEveryChange
    }

    public enum BinaryBitShifting
    {
        [Description("NO_SHIFT")]
        BinaryOriginalBitPositions,
        [Description("RIGHT_SHIFT")]
        BinaryShiftRight
    }

    public enum BinaryOutputWordSize
    {
        [Description("8")]
        Binary8Bit,
        [Description("16")]
        Binary16Bit,
        [Description("32")]
        Binary32Bit,
        [Description("64")]
        Binary64Bit
    }

    public enum AnalogOutputFormat
    {
        [Description("VOLTAGE")]
        Voltage,
        [Description("ADC")]
        ADC
    }

    public struct Channel
    {
        public enum ChannelDataType
        {
            [Description("ANALOG")]
            AnalogChannel,
            [Description("DIGITAL")]
            DigitalChannel
        }

        public int Index { get; set; }
        public ChannelDataType DataType { get; set; }

        public string GetExportString()
        {
            return Index.ToString() + " " + DataType.GetDescription();
        }
    }

    public class DigitalVoltageOption
    {
        public int Index;
        public string Description;
        public bool IsSelected;
    }

    public struct ExportDataStruct
    {
        /// <summary>
        /// The fully qualified path to the target file. Folder must exist. Path must be absolute. Always required.
        /// Features like ~/ and %appdata% are not supported. You can expand those first before passing the path.
        /// </summary>
        public string FileName;

        /// <summary>
        /// This option is only required & applied IF your capture contains digital and analog channels, AND you select "Specific Channels" for the channel selection option.
        /// I. e. if you then select digital only, you will get the native, digital only interface for an export mode.
        /// </summary>
        public DataExportMixedModeExportType DataExportMixedExportMode;

        /// <summary>
        /// This option allows you to export all channels in the capture, or only specific channels. Always required.
        /// </summary>
        public DataExportChannelSelection ExportChannelSelection;

        /// <summary>
        /// List channel indexes of digital channels to export. Only required if you select "SpecificChannels".
        /// </summary>
        public int[] DigitalChannelsToExport;

        /// <summary>
        /// List channel indexes of analog channels to export. Only required if you select "SpecificChannels".
        /// </summary>
        public int[] AnalogChannelsToExport;

        /// <summary>
        /// Export all time, or just export a specific range of time. Always required.
        /// Command will NAK if a custom time range extends past captured data.
        /// </summary>
        public DataExportSampleRangeType SamplesRangeType; // { RangeAll, RangeTimes }

        /// <summary>
        /// Start time of export. Only applies if "RangeTimes" is set.
        /// Relative to trigger sample, can be negative.
        /// </summary>
        public double StartingTime;

        /// <summary>
        /// End time of export. Only applies if "RangeTimes" is set.
        /// Relative to trigger sample, can be negative.
        /// </summary>
        public double EndingTime;

        /// <summary>
        /// Primary export type. Always required.
        /// </summary>
        public DataExportType DataExportType; // { ExportBinary, ExportCsv, ExportVcd }

        /// <summary>
        /// Required for all CSV exports.
        /// </summary>
        public CsvHeadersType CsvIncludeHeaders; // { CsvIncludesHeaders, CsvNoHeaders }

        /// <summary>
        /// Required for all CSV exports.
        /// </summary>
        public CsvDelimiterType CsvDelimiterType; // { CsvComma, CsvTab }

        /// <summary>
        /// Required only when exporting digital only CSV.
        /// Does not apply when exporting analog and digital or analog only.
        /// </summary>
        public CsvOutputMode CsvOutputMode; // { CsvSingleNumber, CsvOneColumnPerBit }

        /// <summary>
        /// Only applies when exporting digital only CSV.
        /// Does not apply when exporting analog and digital or analog only. (time values used)
        /// </summary>
        public CsvTimestampType CsvTimestampType; // { CsvTime, CsvSample }

        /// <summary>
        /// Required when exporting analog samples as raw ADC value.
        /// Also required when exporting digital only data in "CsvSingleNumber" format.
        /// </summary>
        public CsvBase CsvDisplayBase; // { CsvBinary, CsvDecimal, CsvHexadecimal, CsvAscii }

        /// <summary>
        /// Only required when exporting digital only CSV data.
        /// CsvTransition produces a smaller file where only transition timestamps are exported.
        /// CsvComplete includes every single sample.
        /// </summary>
        public CsvDensity CsvDensity; // { CsvTransition, CsvComplete }

        // Type: Binary
        /// <summary>
        /// Only required for digital only Binary mode.
        /// </summary>
        public BinaryOutputMode BinaryOutputMode; // { BinaryEverySample, BinaryEveryChange }

        /// <summary>
        /// Only required for digital only Binary mode.
        /// </summary>
        public BinaryBitShifting BinaryBitShifting; // { BinaryOriginalBitPositions, BinaryShiftRight }

        /// <summary>
        /// Only required for digital only Binary mode.
        /// </summary>
        public BinaryOutputWordSize BinaryOutputWordSize; // { Binary8Bit, Binary16Bit, Binary32Bit, Binary64Bit }

        /// <summary>
        /// ADC values or floating point voltages. Required for all export types that include analog channels.
        /// </summary>
        public AnalogOutputFormat AnalogFormat; // This feature needs v1.1.32+ 
    }

    public struct SampleRate
    {
        public int AnalogSampleRate { get; set; }
        public int DigitalSampleRate { get; set; }

        public static bool operator ==(SampleRate a, SampleRate b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.AnalogSampleRate == b.AnalogSampleRate && a.DigitalSampleRate == b.DigitalSampleRate;
        }

        public static bool operator !=(SampleRate a, SampleRate b)
        {
            return !(a == b);
        }
    }

    public struct Analyzer
    {
        public string AnalyzerType { get; set; }
        public int Index { get; set; }
    }

    public struct CaptureRange
    {
        /// <summary>
        /// The first valid sample of data. Usually 0, unless a trigger was used, and the entire pre-trigger buffer has been filled, so older pre-trigger data has been discarded.
        /// </summary>
        public ulong StartingSample { get; set; }
        /// <summary>
        /// The time 0 trigger sample. When no trigger is used, this is 0.
        /// </summary>
        public ulong TriggerSample { get; set; }
        /// <summary>
        /// This is the last sample in the capture.
        /// </summary>
        public ulong EndingSample { get; set; }
        /// <summary>
        /// The LCM sample rate of the capture, which is least common multiple of the digital and analog sample rates. All sample indexes are relative to the LCM sample rate.
        /// </summary>
        public uint LcmSampleRate { get; set; }
    }

    public struct ViewState
    {
        /// <summary>
        /// The display zoom level, given in samples (floating point) per pixel. Larger is more zoomed out. 1 indicates 1 sample per pixel.
        /// </summary>
        public double ZoomSamplesPerPixel { get; set; }
        /// <summary>
        /// The sample index of the left edge of the screen. 
        /// </summary>
        public double PanStartingSample { get; set; }
        /// <summary>
        /// The LCM sample rate of the capture, which is least common multiple of the digital and analog sample rates. All sample indexes are relative to the LCM sample rate.
        /// </summary>
        public uint LcmSampleRate { get; set; }
    }

    public class SaleaeSocketApiException : Exception
    {
        public SaleaeSocketApiException()
            : base()
        {
        }

        public SaleaeSocketApiException(string message)
            : base(message)
        {
        }

        public SaleaeSocketApiException(string message, Exception inner_exception)
            : base(message, inner_exception)
        {
        }
    }
}
