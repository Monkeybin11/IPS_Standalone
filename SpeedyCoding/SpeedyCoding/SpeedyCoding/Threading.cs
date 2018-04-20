using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    /// <summary>
    /// Extension Method for Threading. Current verrsion support only' Thread.Sleep'
    /// </summary>
    public static class Threading
    {

        /// <summary>
        /// Insert Thread.Sleep into any type of something
        /// time ( ms )
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="time">Sleep time</param>
        /// <returns></returns>
        public static T Delay<T>(
           this T src,
           int time)
        {
            Thread.Sleep(time);
            return src;
        }

        /// <summary>
        /// Insert Thread.Sleep into any type of something
        /// 50ms
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Delay50<T>(
            this T src )
        {
            Thread.Sleep( 50 );
            return src;
        }

        /// <summary>
        /// Insert Thread.Sleep into any type of something
        /// 100ms
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Delay100<T>(
            this T src )
        {
            Thread.Sleep( 100 );
            return src;
        }

        /// <summary>
        /// Insert Thread.Sleep into any type of something
        /// 300ms
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Delay300<T>(
            this T src )
        {
            Thread.Sleep( 300 );
            return src;
        }

        /// <summary>
        /// Insert Thread.Sleep into any type of something
        /// 500ms
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Delay500<T>(
            this T src )
        {
            Thread.Sleep( 500 );
            return src;
        }

        /// <summary>
        /// Insert Thread.Sleep into any type of something
        /// 1000ms
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Delay1000<T>(
            this T src )
        {
            Thread.Sleep( 1000 );
            return src;
        }



    }
}
