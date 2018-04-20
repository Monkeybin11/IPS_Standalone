using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
	public static class Statistic
	{
        #region Distance

        /// <summary>
        /// L1 int distance 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double L1(
             this Tuple<int, int> src,
            Tuple<int, int> target
            )
        {
            return System.Math.Abs((src.Item1 - target.Item1) + (src.Item2 - target.Item2));
        }

        /// <summary>
        /// L1 double distance
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double L1(
            this Tuple<double, double> src,
           Tuple<double, double> target
           )
        {
            return System.Math.Abs(src.Item1 - target.Item2);
        }


        /// <summary>
        /// L2 int distance
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double L2(
            this Tuple<int, int> src,
            Tuple<int, int> target
            )
        {
            return System.Math.Sqrt(System.Math.Pow((src.Item1 - target.Item1), 2) + System.Math.Pow((src.Item2 - target.Item2), 2));
        }

        /// <summary>
        /// L2 double distance
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <returns></returns>
		public static double L2(
         this Tuple<double, double> src,
         Tuple<double, double> target
         )
        {
            return System.Math.Sqrt(System.Math.Pow((src.Item1 - target.Item1), 2) + System.Math.Pow((src.Item2 - target.Item2), 2));
        }



        #endregion

        #region statistical parameter

        /// <summary>
        /// Convert IEnumerable<double> to Z-score 
        /// </summary>
        /// <param name="self">Source</param>
        /// <returns>Collection of Z-score based on source</returns>
        public static IEnumerable<double> ToZScore(
			this IEnumerable<double> self )
		{
			double avg = self.Average();
			double sd;
			if ( self.Count() > 1000000 )
			{
				sd = self.AsParallel().Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average();
				return self.AsParallel().Select( x => ( x - avg ) / sd );
			}
			else
			{
				sd = self.Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average();
				return self.Select( x => ( x - avg ) / sd );
			}
		}

        /// <summary>
        /// Calculate standard deviation
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
		public static double SD(
			this IEnumerable<double> self )
		{
			var avg = self.Average();
			if ( self.Count() > 1000000 )
				return self.AsParallel().Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average(); 
			else
				return self.Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average();
		}

        /// <summary>
        /// Calculate covariance
        /// </summary>
        /// <param name="self"></param>
        /// <param name="trg"></param>
        /// <returns></returns>
		public static double Cov(
			this IEnumerable<double> self ,
			IEnumerable<double> trg )
		{
			double ux =  self.Average(); 
			double uy =  trg.Average();
			var ydatas = trg.ToArray();
			if ( self.Count() > 1000000 )
				return self.AsParallel().Select( ( x , i ) => ( x - ux ) * ( ydatas [ i ] - uy ) ).Average();
			else
				return self.Select( ( x , i ) => ( x - ux ) * ( ydatas [ i ] - uy ) ).Average();
		}

		public static double PearsonCorrelation(
			this IEnumerable<double> self ,
			IEnumerable<double> trg )
		=> self.Cov( trg ) / ( self.SD() * trg.SD() );
		#endregion
	}
}
