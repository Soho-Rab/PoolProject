using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Pool.Common
{
    public class Strings
    {

        #region 常规字符操作
        public static bool IsInArray(string s, string[] arr)
        {
            var returnBool = false;
            foreach (var ss in arr)
            {
                if (ss.Trim().Equals(s.Trim()))
                {
                    returnBool = true;
                    break;
                }
            }
            return returnBool;
        }

        public static string BoolOrNullToString(bool? isex)
        {
            if (isex == null)
            {
                return "false";
            }
            else
            {
                return isex.Value.ToString().ToLower();
            }
        }

        public static string AddLeftZero(string s, int len)
        {
            var rs = s.Trim().Length;
            if (rs < len)
            {
                for (int i = 0; i < len - rs; i++)
                {
                    s = "0" + s;
                }
            }
            return s;
        }

        public static string Left(string s, int leg)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            else
            {
                if (s.Length > leg)
                {
                    return s.Substring(0, leg);
                }
                else
                {
                    return s;
                }
            }
        }

        public static string TrimZero(decimal? dec)
        {
            if (dec == null)
            {
                return "";
            }
            else
            {
                string tz = dec.Value.ToString();
                if (tz.IndexOf(".") >= 0)
                {
                    tz = tz.TrimEnd(new char[] { '0' });
                    tz = tz.TrimEnd('.');
                }
                return tz;
            }
        }


        public static string TrimLeftZero(string s)
        {
            if (s == null)
            {
                return "";
            }
            else
            {
                s = s.TrimStart(new char[] { '0' });
                return s;
            }
        }

        #endregion

        public static decimal ChangeDataToD(string strData, int round)
        {
            decimal dData = 0.000M;
            if (strData.Contains("E"))
            {
                dData = Convert.ToDecimal(decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            else
            {
                var isdec = decimal.TryParse(strData, out dData);
            }
            dData = Math.Round(dData, round);
            return dData;
        }

        /// <summary>
        /// 本地时间与标准时间时差（小时）
        /// </summary>
        public static int ServerTimeZone { get { return TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Hours; } }


        public static bool IsEmail(string email)
        {
            //如果为空，认为验证不合格
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            else
            {
                //清除要验证字符串中的空格
                email = email.Trim();
                //模式字符串
                string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
                //验证
                return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
            }
        }


        /// <summary>
        /// 以周为单位列出某一年季度
        /// </summary>
        /// <returns>The time.</returns>
        /// <param name="year">Year.</param>
        /// <param name="quarter">Quarter.</param>
        public static List<ChartTimeSpan> ChartTime(int year, int quarter)
        {
            if (year > 999 && year < 10000)
            {
                var begin = DateTime.Now;
                var end = DateTime.Now;
                if (quarter == 1)
                {
                    begin = DateTime.ParseExact(year.ToString() + "-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    end = DateTime.ParseExact(year.ToString() + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                }
                else if (quarter == 2)
                {
                    begin = DateTime.ParseExact(year.ToString() + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    end = DateTime.ParseExact(year.ToString() + "-07-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                }
                else if (quarter == 3)
                {
                    begin = DateTime.ParseExact(year.ToString() + "-07-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    end = DateTime.ParseExact(year.ToString() + "-10-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                }
                else if (quarter == 4)
                {
                    begin = DateTime.ParseExact(year.ToString() + "-10-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    end = DateTime.ParseExact((year + 1).ToString() + "-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                }
                if (begin == end)
                {
                    return null;
                }
                else
                {
                    List<ChartTimeSpan> ls = new List<ChartTimeSpan>();
                    var ts = end - begin;
                    var days = ts.Days;
                    var begindays = 1;
                    var beginweek = 1;
                    var enddays = 0;
                    var endweek = 0;
                    switch (begin.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            begindays = 7;
                            break;
                        case DayOfWeek.Tuesday:
                            begindays = 6;
                            break;
                        case DayOfWeek.Wednesday:
                            begindays = 5;
                            break;
                        case DayOfWeek.Thursday:
                            begindays = 4;
                            break;
                        case DayOfWeek.Friday:
                            begindays = 3;
                            break;
                        case DayOfWeek.Saturday:
                            begindays = 2;
                            break;
                        case DayOfWeek.Sunday:
                            begindays = 1;
                            break;
                    }
                    switch (end.AddDays(-1).DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            enddays = 1;
                            break;
                        case DayOfWeek.Tuesday:
                            enddays = 2;
                            break;
                        case DayOfWeek.Wednesday:
                            enddays = 3;
                            break;
                        case DayOfWeek.Thursday:
                            enddays = 4;
                            break;
                        case DayOfWeek.Friday:
                            enddays = 5;
                            break;
                        case DayOfWeek.Saturday:
                            enddays = 6;
                            break;
                        case DayOfWeek.Sunday:
                            enddays = 7;
                            break;
                    }
                    var weeklen = (days - begindays - enddays) / 7 + endweek + beginweek;
                    for (int i = 0; i < weeklen; i++)
                    {
                        if (i == 0)
                        {
                            ls.Add(new ChartTimeSpan { Begin = begin, End = begin.AddDays(begindays) });
                        }
                        else if (i == weeklen - 1)
                        {
                            ls.Add(new ChartTimeSpan { Begin = end.AddDays(-enddays), End = end });
                        }
                        else
                        {
                            ls.Add(new ChartTimeSpan { Begin = begin.AddDays(begindays + i * 7 - 7), End = begin.AddDays(begindays + i * 7) });
                        }
                    }
                    return ls;
                }
            }
            else
            {
                return null;
            }

        }



        /// <summary>
        /// 以二进制为单位取前几位
        /// </summary>
        public static string LeftBytes(string s, int len)
        {

            if (!string.IsNullOrWhiteSpace(s))
            {
                s = s.Trim();
                string rs = "";
                int codeLen = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    codeLen += System.Text.Encoding.Default.GetBytes(s.Substring(i, 1)).Length;
                    if (codeLen <= len)
                    {
                        rs += s.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return rs;

            }
            else
            {
                return "";
            }

        }


        /// <summary>
        /// 组合成SQL IN语句使用字符串
        /// </summary>
        /// <returns>The to sql string.</returns>
        /// <param name="arr">Arr.</param>
        public static string ArrayToSqlString(string[] arr)
        {
            var s = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    s += "'" + arr[i] + "'";
                }
                else
                {
                    s += ",'" + arr[i] + "'";
                }
            }
            return s;
        }

        /// <summary>
        /// 报表需要，根据数组长度生成0.0,0.0.......
        /// </summary>
        /// <returns>The array.</returns>
        /// <param name="arrlong">Arrlong.</param>
        public static string ZeroArray(int arrlong)
        {
            var s = "";
            for (int i = 0; i < arrlong; i++)
            {
                if (i == 0)
                {
                    s = "0.0";
                }
                else
                {
                    s += ",0.0";
                }
            }
            return s;
        }



        #region  Excel计算公式实现
        /// <summary>
        /// 计算预估不良率
        /// </summary>
        public static double PPMValue(double minValue, double maxValue, double avgValue, double sigmaValue)
        {
            var one = Math.Round((maxValue - avgValue) / sigmaValue, 12);
            var two = Math.Round((minValue - avgValue) / sigmaValue, 12);
            var oneNorm = GetNormSDistValue(one);
            var twoNorm = GetNormSDistValue(two);
            return Math.Round(1 - oneNorm + twoNorm, 12) * 1000000;
        }

        /// <summary>
        /// 计算标准正态数
        /// </summary>
        /// <param name="NormSDistValue">需要计算标准正态数值</param>
        /// <returns>返回计算结果</returns>
        public static double GetNormSDistValue(double NormSDistValue)
        {
            int S = 2;
            double Q = 0;
            double b = NormSDistValue;
            if (b > 6) return 1;
            if (b < -6) return 0;
            while (true)
            {
                double a = b - S;
                int M = 1, N = 1, k = 1, m = 1;
                double ep, I, h;
                ep = 0.000000000001;
                h = b - a;
                I = h * (f(a) + f(b)) / 2;
                var T = new double[5000, 5000];
                T[1, 1] = I;
                while (1 > 0)
                {
                    N = (int)Math.Pow(2, m - 1);
                    if (N > 5000)
                    {
                        return 0;
                        // break;
                    }
                    else
                    {
                        h = h / 2;
                        I = I / 2;
                        for (int i = 1; i <= N; i++)
                            I = I + h * f(a + (2 * i - 1) * h);
                        T[m + 1, 1] = I;
                        M = 2 * N;
                        k = 1;
                        while (M > 1)
                        {
                            T[m + 1, k + 1] = (Math.Pow(4, k) * T[m + 1, k] - T[m, k]) / (Math.Pow(4, k) - 1);
                            M = M / 2;
                            k = k + 1;
                        }
                        if (Math.Abs(T[k, k] - T[k - 1, k - 1]) < ep) break;
                        m = m + 1;
                    }
                }
                I = T[k, k];
                Q = Q + I;
                if (Math.Abs(I) < ep) break;
                b = a;
                S = 2 * S;
            }
            return Q;
        }

        /// <summary>
        /// 计算X的次方(jisuanXdecifang)
        /// </summary>
        /// <param name="x">需要算次方的值(xuyaosuancifangde值)</param>
        /// <returns>返回计算结果(fanhuijisuanjieguo)</returns>
        public static double f(double x)
        {
            double f = Math.Exp(-x * x / 2) / Math.Sqrt(2 * Math.PI);
            return f;
        }
        #endregion


        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstend">当枚举没有定义DescriptionAttribute,是否用枚举名代替，默认使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(Enum value, bool nameInstend = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }



        #region Base64位加解密方法集合
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            string rs = "";
            try
            {
                rs = Convert.ToBase64String(bytes);
            }
            catch
            {
                rs = source;
            }
            return rs;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
        #endregion


        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertIntDateTime(double d)
        {
            return new DateTime(1970, 1, 1).AddSeconds(d);
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(DateTime time)
        {
            return (time - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 省略小数点后数字
        /// </summary>
        public static decimal CutDecimalWithN(decimal d, int n)
        {
            string strDecimal = d.ToString();
            int index = strDecimal.IndexOf(".");
            if (index == -1 || strDecimal.Length < index + n + 1)
            {
                strDecimal = string.Format("{0:F" + n + "}", d);
            }
            else
            {
                int length = index;
                if (n != 0)
                {
                    length = index + n + 1;
                }
                strDecimal = strDecimal.Substring(0, length);
            }
            return Decimal.Parse(strDecimal);
        }

        /// <summary>
        /// 验证bitcoin-cli代码是否正确
        /// </summary>
        public static bool ScriptTrue(string script)
        {
            if (string.IsNullOrWhiteSpace(script)) return false;
            if (Left(script, 5).ToLower() == "error") return false;
            return true;
        }

        /// <summary>
        /// 1个币换成聪比率
        /// </summary>
        public static decimal CoinToCong { get { return 100000000.00m; } }


        public static int WeekDayToInt(DayOfWeek wd)
        {
            switch (wd)
            {
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                default:
                    return -1;
            }
        }
    }

    public class ChartTimeSpan
    {
        public DateTime Begin { set; get; }

        public DateTime End { set; get; }
    }
}
