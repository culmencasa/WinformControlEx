using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows
{
    public class Conv
    {
        #region Objectת��������

        /// <summary>
        /// ת�����ַ���
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NS(object value)
        {
            return NS(value, string.Empty);
        }
        /// <summary>
        /// ת�����ַ���
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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
        /// ת��������Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int NI(object value)
        {
            return NI(value, default(int));
        }
        /// <summary>
        /// ת��������Int64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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
        /// ת��������Int64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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
        /// ת����Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal NDec(object value)
        {
            return NDec(value, default(decimal));
        }
        /// <summary>
        /// ת����Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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
        /// ת����Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float NF(object value)
        {
            return NF(value, default(float));
        }
        /// <summary>
        /// ת����Float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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
        /// ת����Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ND(object value)
        {
            return ND(value, default(double));
        }
        /// <summary>
        /// ת����Double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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
        /// ת���ɲ���
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool NBool(object value)
        {
            return NBool(value, false);
        }
        /// <summary>
        /// ת���ɲ���
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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
        /// ת��������
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime NDT(object value)
        {
            return NDT(value, DateTime.MinValue);
        }
        /// <summary>
        /// ת��������
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">Ĭ��ֵ</param>
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

        #region Listת��

        /// <summary>
        /// ����תList
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
        /// ת��ö������Ϊ
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<TEnum> ToList<TEnum>()
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(typeof(TEnum).ToString() + "����ö�����͡�");
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

        #region С���ض�

        /// <summary>
        /// ��С��ֵ��ָ����С��λ���ض�
        /// </summary>
        /// <param name="d">Ҫ�ضϵ�С��</param>
        /// <param name="s">С��λ����s���ڵ���0��С�ڵ���28</param>
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
        /// ��˫���ȸ���ֵ��ָ����С��λ���ض�
        /// </summary>
        /// <param name="d">Ҫ�ضϵ�˫���ȸ�����</param>
        /// <param name="s">С��λ����s���ڵ���0��С�ڵ���15</param>
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

        #region ȫ�ǰ��ת��

        /// <summary>
        /// תȫ�ǵĺ���(SBC case)
        /// </summary>
        /// <param name="input">�����ַ���</param>
        /// <returns>ȫ���ַ���</returns>
        ///<remarks>
        ///ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
        ///�����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248
        ///</remarks>
        public static string ToSBC(string input)
        {
            //���תȫ�ǣ�
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


        /// <summary> ת��ǵĺ���(DBC case) </summary>
        /// <param name="input">�����ַ���</param>
        /// <returns>����ַ���</returns>
        ///<remarks>
        ///ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
        ///�����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248
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

        #region ʮ����ʮ�����ƻ�ת

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
