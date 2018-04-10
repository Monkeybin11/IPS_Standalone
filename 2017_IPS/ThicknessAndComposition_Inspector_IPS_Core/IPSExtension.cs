using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThicknessAndComposition_Inspector_IPS_Data;
using SpeedyCoding;
using ModelLib.Data;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;
using Emgu.CV;
using System.IO;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public static class IPSExtension
	{
		public static IEnumerable<Tuple<PlrCrd , T>> Duplicate0ToAllTheta<T>(
			this IEnumerable<Tuple<PlrCrd , T>> self )
		{
			var targetT = self.Where( x => x.Item1.Rho == 0 ).Select( x => x.Item2).First(); // extract plrcrd (0 , ? )
			var allthetalist = self.Select( x => x.Item1.Theta).Distinct(); // extract unique rho
			var duplicated = allthetalist.Select( x=> Tuple.Create( new PlrCrd(x , 0) , targetT ) ); // combine theta = 0 with unique rho
			return self.Concat( duplicated );
		}


		public static List<double [ ]> Result2TRThArr(
			this IPSResult self )
		=> self.SpotDataList.Select( x =>
							   new double [ ] { Math.Round( x.PlrPos.Theta)
												, Math.Round(x.PlrPos.Rho/10.0)
												, x.Thickness } )
							 .ToList();

		public static List<double [ ]> Interpol_Theta(
			this List<double [ ]> src ,
			int count )
		{
			var srcorders = src.OrderBy( x => x[0]).ThenBy( x => x[1]).ToList();
			if ( count == 0 ) return src;
			var firsttheta = srcorders.First()[0]; // first theta for last add ( f + 360)
			var fs = src.Where( x => x[0] == firsttheta ).ToList(); // all first theta datas
			var lastdatas = fs.Select( f =>  new double[] { f[0] + 360 , f[1] , f[2] } ).ToList();
			var processData = src.Concat( lastdatas ).OrderBy( x=> x[0]).ThenBy(x =>x[1]).ToList(); // Use this data for interpolate
			List<double[]> total = new List<double[]>( processData ); // insert 360 degree
			var rholsit = total.Select( x => x[1]).Distinct().ToList(); // Extract Unique Element

			foreach ( var rho in rholsit )
			{
				var res = total.Where( x => x[1] == rho).Select( x => x).OrderBy( x => x[0] ).ToList(); // Extract All degree had same Rho set( t , r) -> set(t | r = k)

				List<double[]> added = new List<double[]>();
				for ( int i = 0 ; i < res.Count - 1 ; i++ )
				{
					var temp = new double[3];
					temp [ 0 ] = ( res [ i ] [ 0 ] + res [ i + 1 ] [ 0 ] ) / 2.0; // theta
					temp [ 1 ] = ( res [ i ] [ 1 ] + res [ i + 1 ] [ 1 ] ) / 2.0; // rho
					temp [ 2 ] = ( res [ i ] [ 2 ] + res [ i + 1 ] [ 2 ] ) / 2.0; // thickness
					added.Add( temp );
				}
				total = total.Concat( added ).ToList();
			}
			return Interpol_Theta( total , count - 1 );
		}
		public static List<double [ ]> Interpol_Rho(
			this List<double [ ]> src ,
			int count )
		{
			if ( count == 0 ) return src;
			List<double[]> total = new List<double[]>(src);
			var thetalist = src.Select( x => x[0]).Distinct().ToList();
			foreach ( var theta in thetalist )
			{
				var res = src.Where( x => x[0] == theta).Select( x => x).OrderBy( x => x[1] ).ToList();

				List<double[]> added = new List<double[]>();
				for ( int i = 0 ; i < res.Count - 1 ; i++ )
				{
					var temp = new double[3];
					temp [ 0 ] = ( res [ i ] [ 0 ] + res [ i + 1 ] [ 0 ] ) / 2.0;
					temp [ 1 ] = ( res [ i ] [ 1 ] + res [ i + 1 ] [ 1 ] ) / 2.0;
					temp [ 2 ] = ( res [ i ] [ 2 ] + res [ i + 1 ] [ 2 ] ) / 2.0;
					added.Add( temp );
				}
				total = total.Concat( added ).ToList();
			}
			return Interpol_Rho( total , count - 1 );
		}

		public static List<double [ ]> ToCartesianReslt(
			this List<double [ ]> polarRes )
			=> polarRes.Select( x =>
			   {
				   var crt = new PlrCrd(x[0] , x[1]).ToCartesian() as CrtnCrd;
				   return crt != null ? new double [ ] { crt.X , crt.Y , x [ 2 ] }
										 : new double [ 3 ];
			   } ).ToList();

		public static string ToTempDataFormat(
			this IEnumerable<double> wave ,
			IEnumerable<double> inten ,
			IEnumerable<double> reflect )
		{
			StringBuilder stb = new StringBuilder();
			wave.ActLoop( ( x , i ) => stb.Append( x.ToString() + ',' + inten.ElementAt(i).ToString() + ',' + reflect.ElementAt(i).ToString() + Environment.NewLine ) );
			return stb.ToString();
		}



		[DllImport( "gdi32" )]
		private static extern int DeleteObject( IntPtr o );

		public static BitmapSource ToBitmapSource( this IImage image )
		{
			try
			{
				using ( System.Drawing.Bitmap source = image.Bitmap )
				{
					IntPtr ptr = source.GetHbitmap();

					BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
						ptr,
						IntPtr.Zero,
						Int32Rect.Empty,
						System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

					DeleteObject( ptr );
					return bs;
				}
			}
			catch ( Exception )
			{
				return null;
			}
		}

		public static BitmapSource BitmapToBitmapSource( this System.Drawing.Bitmap source )
		{
			try
			{

				IntPtr ptr = source.GetHbitmap();
				BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					ptr,
					IntPtr.Zero,
					Int32Rect.Empty,
					System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
				DeleteObject( ptr );
				return bs;
			
			}
			catch ( Exception )
			{
				return null;
			}
		}
	}
}
