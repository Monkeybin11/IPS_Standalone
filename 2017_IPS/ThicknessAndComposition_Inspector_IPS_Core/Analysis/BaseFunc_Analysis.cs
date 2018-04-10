using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.AmplifiedType.Handler;
using static IPSDataHandler.Handler;
using SpeedyCoding;
using ModelLib.Data;
using ModelLib.Data.NewType;
using ModelLib.AmplifiedType;
using ThicknessAndComposition_Inspector_IPS_Data;
using ThicknessAndComposition_Inspector_IPS_Core;

namespace AnalysisBase
{
	using ModelLib.Data;
	using static System.IO.Path;
	using static IPSDataHandler.Handler;
	using static ModelLib.AmplifiedType.Handler;
	using static BaseFunc_Analysis;
	using IPSAnalysis;
	using static IPSAnalysis.Handler;
	using System.IO;
	using static System.Linq.Enumerable;
	using ThicknessAndComposition_Inspector_IPS_Data;

	public static class StateCreator
	{
		public static event Action<mCrtCrd ,Intensity[] > evtIntenList;
		public static event Action<mCrtCrd , Reflectivity[] > evtRflectList;
		public static event Action<mCrtCrd , Thickness> evtThickness;

		public static Maybe<List<IPSResultData>> ResultDataFrom( string path )
		{
			var headname = path.Split('_').First();
			var basepath =  GetDirectoryName(path);

			var resPath = headname + "_Result.csv";
			var rftPath = headname + "_Reflectivity.csv";
			var rawPath = headname + "_Raw.csv";

			string[] resStr = new string[] { };
			string[] rftStr = new string[] { };
			string[] rawStr = new string[] { };

			if ( File.Exists( resPath )
				&& File.Exists( rftPath )
				&& File.Exists( rawPath ) )
			{
				// missing Data => Exit Flow


				var posThickness = File.ReadAllLines( resPath )
										 .ResultRefine(0)
										 .ToPosThickness()
										 .ToArray();

				var wavLis = File.ReadAllLines( rftPath )
								.ColumnRead(0)
								.ToWaveLen()
								.ToArray();


				// missing Data => interpolation 

				var rftList  = File.ReadAllLines( rftPath )
								.ResultRefine(1)
								.ToReflectivity()
								.ToArray()
								.Transpose();

				var rawList  = File.ReadAllLines( rawPath )
								.ResultRefine(4)
								.ToIntensity()
								.ToArray()
								.Transpose();

				var scanResult = Range( 0 , posThickness.Count() )
								.Lift( i => new IPSResultData
								{
									Position = posThickness[i].Item1 ,
									WaveLegth = wavLis ,
									Thickness = posThickness[i].Item2,
									IntenList = rawList[i] ,
									Reflectivity = rftList[i]
								} ).ToList();

				return Just( scanResult );
			}
			return None;
		}

		static Action<IPSResultData> TransAllData
			=> x =>
			{
				evtThickness( x.Position , x.Thickness );
				evtRflectList( x.Position , x.Reflectivity );
				evtIntenList( x.Position , x.IntenList );
			};

		private static bool IsValid( mCrtCrd pos )
			=> pos.Match(
				() => false ,
				X => true );

		private static bool IsValid( WaveLength [ ] wlen )
			=> wlen.Where( x => x.Value.isJust ).Count() > 0 ? false : true;

		private static bool IsValid( Intensity [ ] wlen )
		=> wlen.Where( x => x.Value.isJust ).Count() > 0 ? false : true;
	}

	public struct IPSResultData
	{
		public Intensity[] IntenList;
		public Reflectivity[] Reflectivity;
		public Thickness Thickness;
		public mCrtCrd Position;

		private static WaveLength[] _WaveLegth;

		public IPSResultData( IEnumerable<double> inten , IEnumerable<double> refl , IEnumerable<double> wave , double thickness , mCrtCrd pos )
		{
			IntenList = new Intensity [ ] { };
			Reflectivity = new Reflectivity [ ] { };
			Thickness = thickness;
			Position = pos;

			DIntenList = inten;
			DReflectivity = refl;
			DWaveLength = wave;
		}


		public WaveLength [ ] WaveLegth
		{
			get
			{
				if ( _WaveLegth == null ) _WaveLegth = new WaveLength [ ] { };
				return _WaveLegth;
			}
			set
			{
				if ( _WaveLegth == null ) _WaveLegth = value;
			}
		}

		public IEnumerable<double> DIntenList
		{
			get
			{
				foreach ( var item in IntenList )
					yield return item;
			} 

			set
			{
				IntenList = value.Select( x => ( Intensity )Just( x ) ).ToArray();
			}
		}

		public IEnumerable<double> DReflectivity
		{
			get
			{
				foreach ( var item in Reflectivity )
					yield return item;
			}

			set
			{
				Reflectivity = value.Select( x => ( Reflectivity )Just( x ) ).ToArray();
			}
		}

		public IEnumerable<double> DWaveLength
		{
			get
			{
				foreach ( var item in _WaveLegth )
					yield return item;
			}

			set
			{
				WaveLegth = value.Select( x => ( WaveLength )Just( x ) ).ToArray();
			}

		}

		public double DThickness => Thickness;



	}
	public static class BaseFunc_Analysis
	{
		public static IEnumerable<string> ColumnRead(
			this IEnumerable<string> src ,
			int colNum ,
			int headerSkip = 0)
			=> src.Skip(headerSkip).Lift( x => x.Split( ',' ) [ colNum ] ); 

		public static IEnumerable<string[]> ResultRefine( 
			this IEnumerable<string> src ,
			int skipnum )
			=> src.Skip( 1 ).Lift( x => x.Split( ',' ).Skip( skipnum ).ToArray() );

		// NewType is Bad Idea on this situation. need to fix this. But explicitivity is good

		public static Tuple<mCrtCrd , Thickness> [ ] ToPosThickness(this IEnumerable<string [ ]> src )
			=> src.Lift( x => Tuple.Create(
								mCrtCrd( ParseToDouble( x [ 0 ] ) , ParseToDouble( x [ 1 ] ) ) ,
								Thickness( ParseToDouble( x [ 2 ] ) ) ) )
					.ToArray();

		public static Reflectivity [ ] [ ] ToReflectivity(
			this IEnumerable<string [ ]> self ) 
			=> self.Select( f =>
					f.Select( s => ( Reflectivity ) ParseToDouble( s ) ).ToArray() )
					.ToArray();

		public static Intensity [ ] [ ] ToIntensity(
			this IEnumerable<string [ ]> self )
			=> self.Select( f =>
					f.Select( s => ( Intensity )ParseToDouble( s ) ).ToArray() )
					.ToArray();

		public static WaveLength [ ] ToWaveLen(
			this IEnumerable<string> self )
			=> self.Select( f => ( WaveLength ) ParseToDouble( f ) ).ToArray();
	}

	public static class Adaptor
	{
		public static IPSResult ToIPSResult(
			this AnalysisState self )
		{
			List<SpotData> spotlist;
			var temp = self.State[0].WaveLegth.ToList();
			List<double> WaveLen = self.State[0].WaveLegth.Select( x => (double)x).ToList();

			spotlist = self.State.Select( x =>

				new SpotData
				( x.Value.Position.Pos.Value.ToPolar() as PlrCrd ,
					x.Value.DThickness ,
					x.Value.DIntenList.ToArray() ,
					x.Value.DReflectivity.ToArray() ) ).ToList();

			var res = new IPSResult(WaveLen) { SpotDataList = spotlist  };
			return res;
		}

		public static AnalysisState ToState(
			this IPSResult self )
		{
			var resState = new Dictionary<int, IPSResultData>();
			var wave = self.WaveLen;
			var count = self.SpotDataList.Count();
			int i = 0;

			foreach ( var spot in self.SpotDataList )
			{
				var dictdata = new IPSResultData(
					spot.IntenList,
					spot.Reflectivity,
					wave,
					spot.Thickness,
					new mCrtCrd( Just(spot.CrtPos.X) , Just(spot.CrtPos.Y) )); // 

				resState.Add( i++ , dictdata );
			}
			return CreateState( resState );
		}
	}
}
