using System;
using System.Collections.Generic;
using System.Windows;

namespace SpeedyCoding
{
    /// <summary>
    /// Extension Method for Debugging
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Consoe.Writeline Extension Method
        /// "msg".Print()
        /// </summary>
        /// <param name="src">Source of Print</param>
        /// <returns>T</returns>
        public static T Print<T>(this T src)
        {
            Console.WriteLine(src?.ToString());
            return src;
        }

        /// <summary>
        /// Consoe.Writeline Extension Method
        /// Format : "msg : src "
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">Source of Print</param>
        /// <param name="msg">Print header string. "msg : src "</param>
        /// <returns>T</returns>
        public static T Print<T>(this T src, string msg)
        {
            Console.WriteLine(msg + " : " + src?.ToString());
            return src;
        }

        /// <summary>
        /// Loop of Consoe.Writeline Extension Method
        /// Print all elements of IEnuemrable<T>
        /// </summary>
        /// <param name="src">[T]</param>
        /// <returns><[T]</returns>
        public static IEnumerable<T> Print<T>(this IEnumerable<T> src)
        {
            if (src == null) return null;
            foreach (var item in src)
            {
                Console.Write(item.ToString() + " ");
            }
            Console.WriteLine();
            return src;
        }


        /// <summary>
        /// Loop of Consoe.Writeline Extension Method
        /// Print all elements of IEnuemrable<T>
        /// Format : "msg : src "
        /// </summary>
        /// <param name="src">[T]</param>
        /// <returns><[T]</returns>
        public static IEnumerable<T> Print<T>(this IEnumerable<T> src, string msg)
        {
            if (src == null) return null;
            Console.Write(msg + " : ");
            foreach (var item in src)
            {
                Console.Write(item.ToString() + " ");
            }
            Console.WriteLine();
            return src;
        }


        /// <summary>
        /// Show message based on 'True/False' condition
        /// </summary>
        /// <param name="src">condition</param>
        /// <param name="trueMsg">message in case of true</param>
        /// <param name="failMsg">message in case of false</param>
        public static void Show(
            this bool src,
            string trueMsg,
            string failMsg)
        {
            if (src) MessageBox.Show(trueMsg);
            else MessageBox.Show(failMsg);
        }


        /// <summary>
        /// Show message whem condition is false
        /// </summary>
        /// <param name="src">condition</param>
        /// <param name="failMsg">message in case of false</param>
        public static void FailShow(
                this bool src,
                string failMsg)
        {
            if (!src) MessageBox.Show(failMsg);
        }

    }
}
