using System;

namespace NBF.Qubica.Common
{
    public static class Conversion
    {
        #region C# type to C# type
        public static bool? ValueToBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
                return value != "0";
        }

        public static string ValueToString(object value)
        {
            if (value == null)
                return null;
            else
                return value.ToString();
        }

        public static short BoolToShort(bool b)
        {
            return b ? (short)1 : (short)0;
        }

        public static int? BoolToShort(bool? b)
        {
            return b.HasValue ? (b.Value ? (short?)1 : (short?)0) : null;
        }

        public static string BoolToValue(bool? b)
        {
            return b.HasValue ? (b.Value ? "1" : "0") : "";
        }

        public static int? StringToInt(string value)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;
            else
                return null;
        }

        public static DateTime? StringToDate(string value)
        {
            DateTime? result = null;
            try
            {
                result = Convert.ToDateTime(value);
            }
            catch
            {
            }
            return result;
        }

        public static String StringToString(string value)
        {
            String result = null;

            if (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
                result = value;

            return result;
        }

        public static bool? StringToBool(string value)
        {
            bool? result = null;

            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
                result = null;
            else
                result = value == "True" ? true : false;

            return result;
        }        
        
        #endregion

        #region SQL to C# Type
        public static bool? SqlToBoolOrNull(object field)
        {
            if (field == DBNull.Value)
                return null;
            else
                return Convert.ToInt16(field) > 0;
        }

        public static bool SqlToBool(object field)
        {
            if (field == DBNull.Value)
                return false;
            else
                return Convert.ToInt16(field) > 0;
        }

        public static short? SqlToShortOrNull(object field)
        {
            if (field == DBNull.Value)
                return null;
            else
                return Convert.ToInt16(field);
        }

        public static int? SqlToIntOrNull(object field)
        {
            if (field == DBNull.Value)
                return null;
            else
                return Convert.ToInt32(field);
        }

        public static long? SqlToLongOrNull(object field)
        {
            if (field == DBNull.Value)
                return null;
            else
                return Convert.ToInt64(field);
        }

        public static double? SqlToDoubleOrNull(object field)
        {
            if (field == DBNull.Value)
                return null;
            else
                return Convert.ToDouble(field);
        }

        public static string SqlToString(object field)
        {
            if (field == DBNull.Value)
                return "";
            else
                return Convert.ToString(field);
        }

        public static DateTime? SqlToDateTimeOrNull(object field)
        {
            if (field == DBNull.Value)
                return null;
            else
                return Convert.ToDateTime(field);
        }
        #endregion

        #region C# type to SQL
        public static object BoolToSql(bool? obj)
        {
            if (obj == null || !obj.HasValue)
                return DBNull.Value;
            else
                return obj.Value ? 1 : 0;
        }

        public static object DateTimeToSql(DateTime? obj)
        {
            if (obj == null || !obj.HasValue)
                return DBNull.Value;
            else
                return obj.Value;
        }

        public static object DateTimeToSql(DateTime obj)
        {
            if (obj == null)
                return DBNull.Value;
            else
                return obj;
        }

        public static object ShortToSql(short s)
        {
            return s;
        }

        public static object ShortToSql(short? s)
        {
            if (!s.HasValue)
                return DBNull.Value;
            else
                return s;
        }

        public static object IntToSql(int i)
        {
            return i;
        }

        public static object IntToSql(int? i)
        {
            if (!i.HasValue)
                return DBNull.Value;
            else
                return i;
        }

        public static object Int32ToSql(Int32 l)
        {
            return l;
        }

        public static object Int32ToSql(Int32? i)
        {
            if (!i.HasValue)
                return DBNull.Value;
            else
                return i;
        }

        public static object LongToSql(long l)
        {
            return l;
        }

        public static object LongToSql(long? l)
        {
            if (!l.HasValue)
                return DBNull.Value;
            else
                return l;
        }

        public static object DoubleToSql(double d)
        {
            return d;
        }

        public static object DoubleToSql(double? d)
        {
            if (!d.HasValue)
                return DBNull.Value;
            else
                return d;
        }

        public static object StringToSql(string s)
        {
            if (s == null)
                return DBNull.Value;
            else
                return s;
        }

        public static object StringToSqlL(string s, int maxLength)
        {
            if (s == null)
                return DBNull.Value;
            else if (s.Length <= maxLength)
                return s;
            else
                return s.Substring(0, maxLength);
        }
        #endregion

        #region QubicaXML To C#
        public static DateTime? StringToDateTimeQubica(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            
            try
            {
                int year = Int16.Parse(value.Substring(0, 4));
                int month = Int16.Parse(value.Substring(4, 2));
                int day = Int16.Parse(value.Substring(6, 2));
                int hour = Int16.Parse(value.Substring(8, 2));
                int min = Int16.Parse(value.Substring(10, 2));

                return new DateTime(year, month, day, hour, min, 0);
            }
            catch
            {
                return null;
            }
        }

        public static bool? StringToBoolQubica(string value)
        {
            bool? result = null;

            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
                result = null;
            else
                result = value == "Yes" ? true : false;

            return result;
        } 
        #endregion

        public static string UriToEscapedUri(string uri)
        {
            return uri.Replace(" ", "%20");
        }

        public static string DateToTitle(DateTime from, DateTime to)
        {
            string title = "";

            int month = from.Month;
            switch (month)
            {
                case 1: title = "Januari"; break;
                case 2: title = "Februari"; break;
                case 3: title = "Maart"; break;
                case 4: title = "April"; break;
                case 5: title = "Mei"; break;
                case 6: title = "Juni"; break;
                case 7: title = "Juli"; break;
                case 8: title = "Augustus"; break;
                case 9: title = "September"; break;
                case 10: title = "Oktober"; break;
                case 11: title = "November"; break;
                case 12: title = "December"; break;
            }

            title += " " + from.Year;

            if (to.Month == from.Month && to.Year == from.Year)
                return title;
            else
            {
                month = to.Month;
                switch (month)
                {
                    case 1: title += " - Januari"; break;
                    case 2: title += " - Februari"; break;
                    case 3: title += " - Maart"; break;
                    case 4: title += " - April"; break;
                    case 5: title += " - Mei"; break;
                    case 6: title += " - Juni"; break;
                    case 7: title += " - Juli"; break;
                    case 8: title += " - Augustus"; break;
                    case 9: title += " - September"; break;
                    case 10: title += " - Oktober"; break;
                    case 11: title += " - November"; break;
                    case 12: title += " - December"; break;
                }

                title += " " + to.Year;
                return title;
            }
        }
    }
}
