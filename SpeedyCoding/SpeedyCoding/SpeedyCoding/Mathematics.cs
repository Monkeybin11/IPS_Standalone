using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
	public static class Mathematics
	{
        /// <summary>
        /// Integral collection. range is startiddx ~ endidx . 
        /// Range of index is index of collection 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="startidx"></param>
        /// <param name="endidx"></param>
        /// <returns></returns>
		public static double Integral(
			this IEnumerable<int> self ,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx 
				|| startidx > self.Count()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0 
						? 0 
						: startidx;

			var end  = endidx ;

			return self.Where( ( _ , i ) => i >= start && i <= end ).Sum();
		}



        /// <summary>
        /// Integral collection. range is startiddx ~ endidx . 
        /// Intregral is followed by collection of index. 
        /// Data structure is "Data = [c ,a ,b ...]" , Index = [3 , 1 ,2 ....]
        /// Index must be int type
        /// </summary>
        /// <param name="self"></param>
        /// <param name="indices"></param>
        /// <param name="startidx"></param>
        /// <param name="endidx"></param>
        /// <returns>Type of double</returns>
		public static double Integral(
			this IEnumerable<double> self ,
			IEnumerable<int> indices ,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx
				|| startidx > indices.Max()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			var res = self.Where( ( _ , i ) => indices.ElementAt(i) >= start && indices.ElementAt( i ) <= end ).Sum();
			return res;
		}



        /// <summary>
        /// Integral collection. range is startiddx ~ endidx . 
        /// Intregral is followed by collection of index. 
        /// Data structure is "Data = [c ,a ,b ...]" , Index = [0.3 , 0.1 ,0.2 ....]
        /// collection of index must be double type
        /// </summary>
        /// <param name="self"></param>
        /// <param name="indices"></param>
        /// <param name="startidx"></param>
        /// <param name="endidx"></param>
        /// <returns>Type of double</returns>
        public static double Integral(
			this IEnumerable<double> self ,
			IEnumerable<double> indices ,
			double startidx ,
			double endidx )
		{
			if ( startidx >= endidx
				|| startidx > indices.Max()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			var res = self.Where( ( _ , i ) => indices.ElementAt(i) >= start && indices.ElementAt( i ) <= end ).Sum();
			return res;
		}


        /// <summary>
        /// Integral collection. range is startiddx ~ endidx . 
        /// Intregral is followed by collection of index. 
        /// Data structure is "Data = [c ,a ,b ...]" , Index = [0.3 , 0.1 ,0.2 ....]
        /// Collection of index must be double type 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="indices"></param>
        /// <param name="startidx"></param>
        /// <param name="endidx"></param>
        /// <returns>Type of double</returns>
        public static double Integral(
			this IEnumerable<double> self ,
			IEnumerable<double> indices ,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx
				|| startidx > indices.Max()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			var res = self.Where( ( _ , i ) => indices.ElementAt(i) >= start && indices.ElementAt( i ) <= end ).Sum();
			return res;
		}


	}
}
