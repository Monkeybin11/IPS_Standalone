using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using ModelLib.AmplifiedType;
using static ModelLib.AmplifiedType.Handler;
using FittingDataStruct;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using static DataLoaderExt;
	using static FittingDataStruct.Handler;
	using FileNames = IEnumerable<string>;
	using DPosThckRflt = NumPosThckRflt<double>;
	using MssData = DatasAnMissing<List<double>>;

	using static System.IO.Directory;
	using static System.IO.Path;
	using System;
	public static partial class DataLoader
	{
		// Safe Version
		public static Maybe<List<IpsDataSet>> MGetEachDataSetWith( this Maybe<string> ipspath , Maybe<string> klapath )
		{
			var klaDataDict = klapath 
					   			.Lift( GetDirectoryName )
					   			.Lift( GetFiles )
								.Match(
									() => null ,
									x => GetKlaData(x) );

			var toAllPathList = ToAllPathListWith.Apply( klaDataDict );

			return ipspath 
					.Lift( GetDirectoryName )
					.Bind( MGetAllDirs )
					.Bind( MGetAllFileNames )
					.Bind( MGetIpsDataSetPaths )//[ SCalss , ResPath , RfltPath ]
					.Bind( x => x.Select (toAllPathList ))
					.Lift( ToIpsDataSet )
					.ToList();
		}

		// Unsafe Version
		public static List<IpsDataSet> GetEachDataSet( string ipspath , string klapath )
		{
			var klaDataDict = Just( klapath )
					   			.Lift( GetDirectoryName )
					   			.Lift( GetFiles )
								.Match(
									() => null ,
									x => GetKlaData(x) );
		
			return Just( ipspath )
								.Lift( GetDirectoryName )
								.Bind( GetAllDirs )
								.Map( GetAllFileNames )
								.Map( GetIpsDataSetPaths )//[ SCalss , ResPath , RfltPath ]
								.Lift( x => x.ToAllPathList( klaDataDict[x[0]] ) ).ToList()
								.Lift(ToIpsDataSet)
								.ToList();
		}

		public static IpsDataSet ToIpsDataSet(
			this NumThckResKlaPath src )
		{
			return new IpsDataSet()
			{
				SCLass = src.SClass ,
				Position = Just( src.ResultPath ).Bind( ReadPos ).Map( TryDoubleParse ).ToList() ,
				KlaThickness = src.Klathckness ,
				RfltList = Just( src.ReflectPath ).Bind( ReadReflectivity ).Map( TryFloatParse ).ToList()
			};
		}

	}

	public class NumThckResKlaPath
	{
		public string SClass;
		public string ResultPath;
		public string ReflectPath;
		public List<float> Klathckness;
	}

	public class IpsDataSet // 한 파일 기준. 한 파일에는 24포인트의 측정자료가 들어있다. 
	{
		private List<double> _Wave;

		public string		  SCLass;
		public List<double[]> Position;
		public List<float>   KlaThickness;
		public List<float[]> RfltList;
		public List<double> Wave
		{
			get{ return _Wave;}
			set{ if ( _Wave == null )
				{
					_Wave = new List<double>();
					_Wave = value;
				}  }
		}
	}

	public static class DataLoaderExt
	{
		public static NumThckResKlaPath ToAllPathList
			( this string [ ] src , List<float> kladata )
			=> new NumThckResKlaPath()
			{
				SClass		= src[0]	,
				ResultPath	= src[1]	,
				ReflectPath	= src[2]	,
				Klathckness		= kladata
			};

		public static Func<Dictionary<string,List<float>> , string[] ,NumThckResKlaPath> ToAllPathListWith
			=> (  kladata ,  src )
			=>  new NumThckResKlaPath()
			{
				SClass = src [ 0 ] ,
				ResultPath = src [ 1 ] ,
				ReflectPath = src [ 2 ] ,
				Klathckness = kladata[ src [ 0 ] ]
			} ;

	}

}
