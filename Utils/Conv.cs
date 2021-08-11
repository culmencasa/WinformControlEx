using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public class Conv
    {
        #region Object转基本类型

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NS(object value)
        {
            return NS(value, string.Empty);
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static string NS(object value, string def)
        {
            string s = def;
            if (value == null)
            {
                return s;
            }
            else
            {
                return value.ToString();
            }
        }
        
        /// <summary>
        /// 转换成整型Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int NI(object value)
        {
            return NI(value, default(int));
        }
        /// <summary>
        /// 转换成整型Int64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static int NI(object value, int def)
        {
            int i = def;
            if (value == null)
            {
                return i;
            }
            else
            {
                if (value.GetType().IsEnum)
                {
                    i = (int)value;
                }
                else if (!int.TryParse(value.ToString(), out i))
                {
                    decimal d = 0;
                    if (decimal.TryParse(value.ToString(), out d))
                    {
                        i = (int)d;
                    }
                    else
                    {
                        i = def;
                    }
                }

                return i;
            }

        }

        public static long NL(object value)
        {
            return NL(value, default(long));
        }
        /// <summary>
        /// 转换成整型Int64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static long NL(object value, long def)
        {
            long i = def;
            if (value == null)
            {
                return i;
            }
            else
            {
                if (value.GetType().IsEnum)
                {
                    i = (long)value;
                }
                else if (!long.TryParse(value.ToString(), out i))
                {
                    decimal d = 0;
                    if (decimal.TryParse(value.ToString(), out d))
                    {
                        i = (long)d;
                    }
                }

                return i;
            }

        }

        /// <summary>
        /// 转换成Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal NDec(object value)
        {
            return NDec(value, default(decimal));
        }
        /// <summary>
        /// 转换成Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static decimal NDec(object value, decimal def)
        {
            decimal d = def;
            if (value == null)
            {
                return d;
            }
            else
            {
                if (!decimal.TryParse(value.ToString(), out d))
                {
                    d = def;
                }
                return d;
            }
        }

        /// <summary>
        /// 转换成Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float NF(object value)
        {
            return NF(value, default(float));
        }
        /// <summary>
        /// 转换成Float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static float NF(object value, float def)
        {
            float d = def;
            if (value == null)
            {
                return d;
            }
            else
            {
                if (!float.TryParse(value.ToString(), out d))
                {
                    d = def;
                }
                return d;
            }
        }

        /// <summary>
        /// 转换成Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ND(object value)
        {
            return ND(value, default(double));
        }
        /// <summary>
        /// 转换成Double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static double ND(object value, double def)
        {
            double d = def;
            if (value == null)
            {
                return d;
            }
            else
            {
                if (!double.TryParse(value.ToString(), out d))
                {
                    d = def;
                }
                return d;
            }
        }
        /// <summary>
        /// 转换成布尔
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool NBool(object value)
        {
            return NBool(value, false);
        }
        /// <summary>
        /// 转换成布尔
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static bool NBool(object value, bool def)
        {
            bool returnValue = def;
            if (value == null)
            {
                return returnValue;
            }

            if (!bool.TryParse(value.ToString(), out returnValue))
            {
                returnValue = def;
            }

            return returnValue;
        }

        /// <summary>
        /// 转换成日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime NDT(object value)
        {
            return NDT(value, DateTime.MinValue);
        }
        /// <summary>
        /// 转换成日期
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static DateTime NDT(object value, DateTime def)
        {
            DateTime dt = def;
            if (value == null)
            {
                return dt;
            }

            if (!DateTime.TryParse(value.ToString(), out dt))
            {
                dt = def;
            }
            return dt;
        }

        #endregion

        #region List转换

        /// <summary>
        /// 数组转List
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(IEnumerable<TSource> source)
        {
            if (source == null)
                return null;
            return new List<TSource>(source);
        }

        /// <summary>
        /// 转换枚举类型为
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<TEnum> ToList<TEnum>()
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(typeof(TEnum).ToString() + "不是枚举类型。");
            }

            List<TEnum> result = new List<TEnum>();
            Array array = Enum.GetValues(enumType);
            foreach (object item in array)
            {
                TEnum member = (TEnum)Enum.ToObject(enumType, item);
                result.Add(member);
            }

            return result;
        }
        
        public static TSource[] ToArray<TSource>(IEnumerable<TSource> source)
        {
            List<TSource> list = new List<TSource>(source);
            return list.ToArray();
        }

        #endregion

        #region 小数截断

        /// <summary>
        /// 将小数值按指定的小数位数截断
        /// </summary>
        /// <param name="d">要截断的小数</param>
        /// <param name="s">小数位数，s大于等于0，小于等于28</param>
        /// <returns></returns>
        public static decimal ToFixed(decimal d, int s)
        {
            decimal sp = Convert.ToDecimal(Math.Pow(10, s));

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }

        /// <summary>
        /// 将双精度浮点值按指定的小数位数截断
        /// </summary>
        /// <param name="d">要截断的双精度浮点数</param>
        /// <param name="s">小数位数，s大于等于0，小于等于15</param>
        /// <returns></returns>
        public static double ToFixed(double d, int s)
        {
            double sp = Math.Pow(10, s);

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }
        
        #endregion

        #region 全角半角转换

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        #endregion

        #region 十进制十六进制互转

        public static uint ToUInt(string hexString)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(uint.Parse(hexString, System.Globalization.NumberStyles.AllowHexSpecifier)), 0);
        }


        #endregion

        public static string IfStringEmpty(string value, string def)
        {
            string s = def;
            if (string.IsNullOrEmpty(value))
            {
                return s;
            }
            else
            {
                return value;
            }
        }
    }
}
