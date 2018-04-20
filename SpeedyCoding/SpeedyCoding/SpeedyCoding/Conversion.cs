using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
	public static class Conversion
	{
		/// <summary>
		/// Nullable to non-nullable 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <returns></returns>
		public static T ToNonNullable<T>( this Nullable<T> src ) where T : struct
		{
			return src == null ? default( T ) : (T)src;
		}

        /// <summary>
        /// Convert radian to degree
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double ToDegree(this double radian)
        {
            return radian * 180 / System.Math.PI;
        }

        /// <summary>
        /// Convert degree to radian
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double ToRadian(this double degree)
        {
            return degree / 180.0 * System.Math.PI;
        }

    }
}
