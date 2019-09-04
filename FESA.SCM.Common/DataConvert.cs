using System;

namespace FESA.SCM.Common
{
    public class DataConvert
    {
        public static Int64 ToInt64(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToInt64(obj);
        }

        public static Int16 ToInt16(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? Int16.MinValue : Convert.ToInt16(obj);
        }

        public static int ToInt32(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToInt32(obj);
        }

        public static int ToInt32Menos(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? -1 : Convert.ToInt32(obj);
        }

        public static int ToInt32MenosVacio(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || ((string)obj == "")) ? -1 : Convert.ToInt32(obj);
        }

        public static decimal ToDecimal(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0.00M : Convert.ToDecimal(obj);
        }

        public static decimal StrToDecimal(string obj)
        {
            return string.IsNullOrEmpty(obj) ? 0.00M : Convert.ToDecimal(obj);
        }

        public static int ToInt(object obj)
        {
            return ToInt32(obj);
        }

        public static double ToDouble(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToDouble(obj);
        }

        public static string ToString(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj).Trim();
        }

        public static DateTime ToDateTime(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? DateTime.MinValue : Convert.ToDateTime(obj);
        }

        public static TimeSpan ToTimeSpan(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? TimeSpan.MinValue : TimeSpan.Parse(obj.ToString());
        }

        public static DateTime? ToDateTimeNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? (DateTime?)null : Convert.ToDateTime(obj);
        }

        public static DateTime StringToDateTime(string str)
        {
            return ((str == "")) ? DateTime.MinValue : Convert.ToDateTime(str);
        }

        public static byte ToByte(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? byte.MinValue : Convert.ToByte(obj);
        }

        public static byte[] ToByteArrayNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (byte[])obj;
        }

        public static int StringToInt(string str)
        {
            return ((str == null) || (str == "")) ? 0 : Convert.ToInt32(str);
        }

        public static Int64 StringToInt64(string str)
        {
            return ((str == null) || (str == "")) ? 0 : Convert.ToInt64(str);
        }

        public static string IntToString(int int1)
        {
            return ((int1 == 0)) ? "" : Convert.ToString(int1);
        }

        public static string ObjectDecimalToStringFormatMiles(object obj)
        {
            return ToDecimal(obj).ToString("#,#0.00");
        }

        public static string BoolToString(bool flag)
        {
            return flag ? "1" : "0";
        }

        public static bool StringToBool(string flag)
        {
            return flag == "1";
        }

        public static Guid ToGui(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? Guid.Empty : (Guid)(obj);
        }

        public static bool ToBool(object obj)
        {
            return ((obj != null) && (obj != DBNull.Value)) && Convert.ToBoolean(obj);
        }

        public static bool? ToBoolNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? (bool?)null : Convert.ToBoolean(obj);
        }

        public static DateTime? DbNullToDatetimeNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (DateTime?)obj;
        }

        public static Int32? ToInt32Null(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (Int32?)(obj);
        }

        public static Int64? ToInt64Null(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (Int64?)(obj);
        }

        public static Decimal? ToDecimalNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (Decimal?)(obj);
        }

        public static string ToStringNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (string)obj;
        }

        public static int ToInt32Custom(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? -100 : Convert.ToInt32(obj);
        }
    }
}