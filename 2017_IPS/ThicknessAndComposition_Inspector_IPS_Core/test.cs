using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;
using System.IO;
using SpeedyCoding;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public static class Mapping
	{
		static string path = @"E:\temp\pos.csv";
		static string path2 = @"E:\temp\thckness.csv";
		public static void Map1()
		{
			// get pos , thcknes
			
			//  


		}

		public static void WritePos(List<PlrCrd> src , List<double> thckness)
		{
			StringBuilder sb = new StringBuilder();

			foreach ( var item in src )
			{
				var t = Math.Round(item.Theta).ToString();
				var rho = Math.Round(item.Rho).ToString();

				sb.Append( t );
				sb.Append( "," );
				sb.Append( rho );
				sb.Append( Environment.NewLine );
			}
			File.WriteAllText( path , sb.ToString() );

			StringBuilder sb2 = new StringBuilder();

			foreach ( var item in thckness )
			{
				sb2.Append( item.ToString() );
				sb2.Append( "," );
			}
			sb2.Remove( sb2.Length-1 , 1 );
			File.WriteAllText( path2 , sb2.ToString() );
		}

		public static void LoadMapdata()
		{
			var pos = File.ReadAllLines(path).Select(x => 
				x.Split(',').Select(s => Math.Round(Convert.ToDouble(s))).ToArray()).ToList();
			var thckness = File.ReadAllLines(path2)[0].Split(',').Select( x =>
				Convert.ToDouble(x)  ).ToList();

			Console.WriteLine();

			var total = pos.Zip(thckness, (f,s) => new double[] { f[0] , f[1] , s} ).ToList();


			var rhosplited =  InterpolationRecur( total , 2 );
			var thetasplited =  InterpolationRecurTheta( total ,2 );


			ContourPolar( thetasplited );


			Console.WriteLine();
		}



		public static List<PosData [ ]> ListedbyTheta( List<double [ ]> src )
		{
			var thetalist = src.Select( x => x[0]).Distinct().ToList();
			var orders = src.OrderBy(x=> x[0] ) // theta
							.ThenBy(x => x[1]) // rho
							.Select( x => new PosData() { T = x[0] , R = x[1] , Th = x[2]} )
							.ToArray();

			var height = thetalist.Count();
			var width = src.Count / height;

			double[][] total = new double[height][];
			List<PosData[]> rows = new List<PosData[]>();
			for ( int j = 0 ; j < height ; j++ )
			{
				rows.Add( orders.Skip( j * width ).Take( width ).ToArray() ); // pick same rho datas
			}
			return rows;
		}


		#region only color map


		#endregion	


		#region old method 
		// 컨투어로 찾는 방법은 안된다. 
		public static List<double [ ]> ContourPolar(List<double[]> src)
		{
			var spaceDatas = ListedbyTheta(src).ToArray(); // Full data
			var lastdata = spaceDatas[0].Select( x => new PosData() { T = x.T + 360 , R = x.R , Th = x.Th} ).ToArray();

			var lastddatalist = new List<PosData[]>();
			lastddatalist.Add( lastdata );

			var extended = spaceDatas.Concat(lastddatalist).ToArray();

			// -- 클래스 분류 데이터 
			double[] claslist = new double[]
			{
				300,340,360,400
			};
			// --

			var contourList = new List<PosData[]>();


			for ( int i = 0 ; i < spaceDatas[0].Length ; i++ )
			{
				i.Print( "Current Iter " );
				

				var res = spaceDatas [ 0 ] [ i ]; // 스타팅 포인트
				var startTh = res.Th;
				var rlimit = spaceDatas[0].Last().R; // 로우 최대
				int tIdx = 0; //현 세타
				int rIdx = i; // 현 로우

				var CheckEndCritic = FnCheckEnd( lastdata[0].T , rlimit );

				var contours = new List<PosData>();
				bool contourExist = false;

				//var updw = GetClass(claslist , res.Th); // 범주 확인 및 체크
				//if ( updw == null ) break;
				//var up = updw[1].Print("UP H");
				//var dw = updw[0].Print("DW H");

				Console.WriteLine( $"Start Position [ T : {res.T}   R : {res.R}  ]   Thickness : {res.Th}" );

				bool running = true;

				int pastmovement = -1;

				while ( running )
				{
					var currentPos = spaceDatas[tIdx][rIdx]; //현재의 포지션 데이터
					contours.Add( currentPos );
					Console.WriteLine( $"Current Position [  T : {currentPos.T}   R : {currentPos.R} ] Thickness : {currentPos.Th}" );
					//  체크를 위한 변환 
					//var selfpos = new double[] { start.T , start.R};
					//var limitpos = new double[] { start.T , start.R};

					// class 분류
					Dictionary<Dirt , int[]> nb = new Dictionary<Dirt, int[]>();
					nb.Add( Dirt.o1 , new int [ ] { tIdx , rIdx + 1 } );
					nb.Add( Dirt.o2 , new int [ ] { tIdx + 1 , rIdx - 1 } );
					nb.Add( Dirt.o3 , new int [ ] { tIdx + 1 , rIdx } );
					nb.Add( Dirt.o4 , new int [ ] { tIdx + 1 , rIdx + 1 } );
					nb.Add( Dirt.o5 , new int [ ] { tIdx , rIdx - 1 } );

					var dirtlist = Enum.GetValues( typeof( Dirt ) ).Cast<Dirt>().ToList();
					var thlist = dirtlist.Select( x => spaceDatas.Pick( nb [ x ] )?.Th ).ToList(); // 다음 목표 장소의 두께들
					

					// 현재 값과 가장 가까운 값의 인덱스를 구해서 딕셔너리에서 그 인덱스 좌표를 뽑아낸다. 

					List<int[]> singlecntr = new List<int[]>();

					// get nearest value

					//var l1 = thlist.Select( x => x != null ? Math.Abs(currentPos.Th - (double)x) : double.MaxValue).ToList();
					var l1 = thlist.Select( x => x != null ? Math.Abs(startTh - (double)x) : double.MaxValue).ToList();
					if ( pastmovement > -1 )
						l1 [ pastmovement ] = double.MaxValue; //되돌아가는 무브먼트 안하게 
					var minIdx = l1.IndexOf( l1.Min() );

					var nextnear = thlist[minIdx];
					var nextposIdx = nb[(Dirt)minIdx];
					pastmovement = 4 - minIdx;
					var nextpos = extended[nextposIdx[0]][nextposIdx[1]];

					var chkres = CheckEndCritic( nextpos.ToDoublePos());


					if ( chkres != EndCon.None )
					{
						" **** Contour Regist".Print();
						nextpos.T.Print( " @@@ Last T :" );
						nextpos.R.Print( " @@@ Last R :" );
						nextnear.Print( " @@@ Last H :" );
						contourExist = true;
						running = false;
					}

					//if ( nextnear < dw || nextnear > up )
					//{
					//	" **** Contour Denied".Print();
					//	nextpos.T.Print( " @@@ Last H :" );
					//	nextpos.R.Print( " @@@ Last H :" );
					//	nextnear.Print( " @@@ Last H :" );
					//	running = false; // 엔드도 아니고 다음 갈곳이 없다. 기각 
					//}
					tIdx = nextposIdx [ 0 ];
					rIdx = nextposIdx [ 1 ];
				}
				if( contourExist )
					contourList.Add( contours.ToArray() );
			}

			Console.WriteLine();

			// spacedatas[0][0:-1]은 모든 각도 성분 이다. 
			


			//var chkres = 
			// check this is end crit

			return null;
		}


		public static double [ ] GetClass( double [ ] classes , double input )
		{
			var uplist = classes.Where( x => x >= input);
			var dwlist = classes.Where( x => x < input);
			if ( uplist.Count() == 0 || dwlist.Count() == 0 )
				return null;
			else
				return new double [ ] { dwlist.Max() , uplist.Min() };
		}

		public static Func<double [ ], EndCon> FnCheckEnd(double lastT , double lastR )
		{
			return new Func<double [ ] , EndCon>( // doublep[] = { theta , Rho }
				x =>
				{
					if ( x [ 0 ] == lastT )
						return EndCon.Self;

					else if ( x [ 1 ] >= lastR
					|| x[ 0 ] < 0
					|| x [ 1 ] < 0 )
						return EndCon.Limit;

					else
						return EndCon.None;
				} );
		}

		public static Func<double , bool> CheckSeg( double start , double last )
		{
			return new Func<double , bool>( x => x >= start && x < last ? true : false );

		}

		public enum Dirt { o1 = 0, o2, o3, o4, o5 }
		public enum EndCon { None, Self, Limit}
		#endregion	

		#region done
		public static List<double [ ]> Interpolation( List<double [ ]> src )
		{
			List<double[]> total = new List<double[]>(src);
			var rholsit = src.Select( x => x[1]).Distinct().ToList();
			foreach ( var rho in rholsit )
			{
				var res = src.Where( x => x[1] == rho).Select( x => x).OrderBy( x => x[0] ).ToList();

				List<double[]> added = new List<double[]>();
				for ( int i = 0 ; i < res.Count - 1 ; i++ )
				{
					var temp1 = new double[3];
					temp1 [ 0 ] = ( res [ i ] [ 0 ] + res [ i + 1 ] [ 0 ] ) / 2.0;
					temp1 [ 1 ] = ( res [ i ] [ 1 ] + res [ i + 1 ] [ 1 ] ) / 2.0;
					temp1 [ 2 ] = ( res [ i ] [ 2 ] + res [ i + 1 ] [ 2 ] ) / 2.0;
					added.Add( temp1 );
				}
				total = total.Concat( added ).ToList();
			}
			return total;
		}

		public static List<double [ ]> InterpolationRecur( List<double [ ]> src , int count )
		{
			if ( count == 0 ) return src;
			List<double[]> total = new List<double[]>(src);
			var rholsit = src.Select( x => x[1]).Distinct().ToList();
			foreach ( var rho in rholsit )
			{
				var res = src.Where( x => x[1] == rho).Select( x => x).OrderBy( x => x[0] ).ToList();

				List<double[]> added = new List<double[]>();
				for ( int i = 0 ; i < res.Count - 1 ; i++ )
				{
					var temp1 = new double[3];
					temp1 [ 0 ] = ( res [ i ] [ 0 ] + res [ i + 1 ] [ 0 ] ) / 2.0;
					temp1 [ 1 ] = ( res [ i ] [ 1 ] + res [ i + 1 ] [ 1 ] ) / 2.0;
					temp1 [ 2 ] = ( res [ i ] [ 2 ] + res [ i + 1 ] [ 2 ] ) / 2.0;
					added.Add( temp1 );
				}
				total = total.Concat( added ).ToList();
			}
			return InterpolationRecur( total , count - 1 );
		}

		public static List<double [ ]> InterpolationRecurTheta( List<double [ ]> src , int count )
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
					var temp1 = new double[3];
					temp1 [ 0 ] = ( res [ i ] [ 0 ] + res [ i + 1 ] [ 0 ] ) / 2.0;
					temp1 [ 1 ] = ( res [ i ] [ 1 ] + res [ i + 1 ] [ 1 ] ) / 2.0;
					temp1 [ 2 ] = ( res [ i ] [ 2 ] + res [ i + 1 ] [ 2 ] ) / 2.0;
					added.Add( temp1 );
				}
				total = total.Concat( added ).ToList();
			}
			return InterpolationRecurTheta( total , count - 1 );
		}

		#endregion	
	}


	public class PosData
	{
		public double T { get; set; }
		public double R { get; set; }
		public double Th { get; set; }
	}

	public static class ext
	{
		public static double [ ] ToDoublePos(
			this PosData self )
	=> new double [ ] { self.T , self.R };


	}
}
