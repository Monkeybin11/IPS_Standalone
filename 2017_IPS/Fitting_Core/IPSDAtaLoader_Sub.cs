using System;
using System.Collections.Generic;
using System.Linq;
using SpeedyCoding;
using ModelLib.AmplifiedType;
using static ModelLib.AmplifiedType.Handler;
using FittingDataStruct;

using static ApplicationUtilTool.FileIO.CsvTool;
using static System.IO.Directory;
using static System.IO.Path;
using static FittingDataStruct.Handler;


using System.IO;

namespace Fitting_Core
{
	using Paths = IEnumerable<string>;
	using FileNames = IEnumerable<string>;
	using RowDatas = List<double>;
	using DataVector = List<double>;

	using MList = Maybe<List<double>>;
	using MssData = DatasAnMissing<List<double>>;

	// Base Funcs
	public static partial class DataLoader
	{

		static bool CheckFiles( FileNames files )
		{
			var headList = files.Select( x => GetFileName(x).Split( '_' ).First() ).Distinct().ToList();

			foreach ( var head in headList )
			{
				var test = GetFileName(files.First()).Split( '_' ).Last();
				var namelist = files.Where( x=>  head == GetFileName(x).Split( '_' ).First() ).ToList();

				// case : only single files exist
				if ( namelist.Count < 2 ) return false;

				var thcks = ThckFilter(namelist);
				var rflts = RfltFilter(namelist);

				// case : only single rflt and result exist.
				// prevent 1-1_1_Result.csv and  1-1_2_Result.csv 
				if ( thcks.Count() != 1 || rflts.Count() != 1 ) return false;
			}
			return true;
		}

		#region Applied Func


		static Dictionary<string , List<float>> GetKlaData
			( FileNames paths )
		{
			var output = new Dictionary<string , List<float>>();
			foreach ( var item in paths )
			{
				output.Add( GetNumOfClass( item ) , ReadKlaData( item ).TryFloatParse().ToList() );
			}
			return output;
		}


		static List<MssData> GetDataWith
			( Func<string , string [ ] [ ]> type ,
			  Func<Paths , Paths> filter ,
			  FileNames basepaths )
			=> Just( basepaths )
				.Bind( filter )
				.Lift( type )
				.Lift( ToMListTable )
				.Lift( ToWithMissing )
				.ToList();
		//static List<double [ ]> GetNonMissingDataWith
		//	( Func<string , string [ ] [ ]> type ,
		//	  Func<Paths , Paths> filter ,
		//	  FileNames basepaths )
		//	=> Just( basepaths )
		//		.Bind( filter )
		//		.Lift( type )
		//		.Lift( x => x.Select( k => TryDoubleParse( k ).ToArray() )
		//		.ToList()).ToList(); 



		static Func<Paths , Paths> ThckFilter
			=> FileFilter.Apply( ResFileName ); //  Filtered Name -> BasPath -> Filtered Names Path
		static Func<Paths , Paths> RfltFilter
			=> FileFilter.Apply( RefFileName );
		static Func<Paths , Paths> KlaThckFilter
			=> FileFilter.Apply( KlaFileName ); //  Filtered Name -> BasPath -> Filtered Names Path

		#endregion

		#region BaseFunc

		static Func<string , string> GetNumOfClass
			=> x
			=> x.Split( '-' ).First().Split( '\\' ).Last();

		static Func<string , string [ ] [ ]> ReadPos
		=> file => ReadCsv2String( file , rowend: 25 , colend: 2 , rowskip: 1 , order0Dirction: false );

		static Func<string , string [ ] [ ]> ReadReflectivity
			=> file => ReadCsv2String( file , rowend: 850 , colend: 25 , rowskip: 451 , colskip: 1 , order0Dirction: true );

		static Func<string , string [ ] [ ]> ReadWaveLen
		=> file => ReadCsv2String( file , rowend: 850 , colend: 1 , rowskip: 451 , colskip: 0 , order0Dirction: true );

		static Func<string , string [ ]> ReadKlaData
			=> path => ReadCsv2String( path , rowend: 25 , colend: 3 , rowskip: 1 , colskip: 2 , order0Dirction: false )
						.Select( x => x [ 0 ] )
						.ToArray();

		/// <summary>
		/// ( Filtered Name , BasPath ) -> Filtered Names Path  
		/// </summary>
		static Func<string , FileNames , FileNames> FileFilter
			=> ( filter , filenames )
			=> filenames.Where( x => x.Split( new char [ ] { '_' } ).Last() == filter );

		static FileNames GetAllFileNames( Paths dirpaths )
			=> dirpaths.Lift( f => GetFiles( f ) ).Flatten();

		static Paths GetAllDirs( string topdir )
		{
			var dir =  GetDirectories( topdir , "*" , System.IO.SearchOption.AllDirectories );
			return dir.Count() == 0
					? Just( topdir ).AsEnumerable()
					: dir;
		}

		#endregion


		// For Maybe Class
		#region 


		static Maybe<FileNames> MGetAllFileNames( Paths dirpaths )
			=> Just( dirpaths.Lift( f => GetFiles( f ) ).Flatten() );

		static Maybe<string [ ]> MGetAllDirs( string topdir )
		{
			var dir =  GetDirectories( topdir , "*" , System.IO.SearchOption.AllDirectories );
			return dir.Count() == 0
					? Just( Just( topdir ).AsEnumerable().ToArray() )
					: Just( dir );
		}
		#endregion


		#region Helper
		static IEnumerable<double> TryDoubleParse
			( this IEnumerable<string> strs )
		{
			List<double> result = new List<double>();
			var strlist = strs.ToList();
			for ( int i = 0 ; i < strlist.Count ; i++ )
			{
				double output;
				if ( double.TryParse( strlist [ i ] , out output ) )
					result.Add( output );
			}
			return result;
		}

		static IEnumerable<double [ ]> TryDoubleParse
		( this IEnumerable<IEnumerable<string>> strs )
		{
			List<double[]> result = new List<double[]>();
			var strlist = strs.ToArray();
			var rowcount = strlist.Count();

			for ( int i = 0 ; i < strlist.Length ; i++ )
			{

				var rowstrdata = strlist[i].ToArray();
				var rows = new double[ rowstrdata.Length ];

				for ( int j = 0 ; j < rowstrdata.Length ; j++ )
				{
					double output;
					if ( double.TryParse( rowstrdata [ j ] , out output ) )
						rows [ j ] = output;
				}

				result.Add( rows );
			}
			return result;
		}


		static double TryDoubleParse
			( this string str )
		{
			double output;
			if ( double.TryParse( str , out output ) )
			{ return output; }
			return -999999;

		}



		static IEnumerable<float> TryFloatParse
			( this IEnumerable<string> strs )
		{
			List<float> result = new List<float>();
			var strlist = strs.ToList();
			for ( int i = 0 ; i < strlist.Count ; i++ )
			{
				float output;
				if ( float.TryParse( strlist [ i ] , out output ) )
					result.Add( output );
			}
			return result;
		}


		static IEnumerable<float [ ]> TryFloatParse
		( this IEnumerable<IEnumerable<string>> strs )
		{
			List<float[]> result = new List<float[]>();
			var strlist = strs.ToArray();
			var rowcount = strlist.Count();

			for ( int i = 0 ; i < strlist.Length ; i++ )
			{

				var rowstrdata = strlist[i].ToArray();
				var rows = new float[ rowstrdata.Length ];

				for ( int j = 0 ; j < rowstrdata.Length ; j++ )
				{
					float output;
					if ( float.TryParse( rowstrdata [ j ] , out output ) )
						rows [ j ] = output;
				}

				result.Add( rows );
			}
			return result;
		}



		static IEnumerable<MList> ToMListTable // IEnumerable< Maybe<IEnumerable<double>>>;
		( this IEnumerable<IEnumerable<string>> src )
		=> src.Select( x => x.ToDoubleList() );

		static MList ToDoubleList    // Maybe<IEnumerable<double>>;
			( this IEnumerable<string> src )
		{
			var output = new RowDatas();
			foreach ( var strdata in src )
			{
				double res;
				if ( !double.TryParse( strdata , out res ) ) return None;
				output.Add( res );
			}
			return output;
		}

		private static Func<MList,bool> IsNothing
			= datas => !datas.isJust;

		// 한 파일에 대해서 
		public static Maybe<DataVector> OnlyNonMissingDatas(
			this DatasAnMissing<RowDatas> src )
		=> src.MissingIndex.Count > 0
					? None
					: Just( src.Datas.Select( x => x.Value.First() ).ToList() );

		//public static List<Tuple<List<string> , MssData , MssData>> ToClsNumWihtThckRflt
		//	( List<string> classnum ,List<MssData> thck ,List<MssData> rflt )
		//{
		//	var output = new List<Tuple<List<string> , MssData , MssData>>();
		//	for ( int i = 0 ; i < classnum.Count ; i++ )
		//	{
		//		output.Add( Tuple.Create( classnum [ i ] , thck [ i ] , rflt [ i ] ) );
		//	}
		//	return output;
		//}

		#endregion
	}

	internal struct PosThckData
	{
		public double X;
		public double Y;
		public double Thickness;
	}
}
namespace FittingDataStruct
{
	public static class Handler
	{
		public static DatasAnMissing<T> ToWithMissing<T>( IEnumerable<Maybe<T>> src )
			=> new DatasAnMissing<T>()
			{
				Datas = src.ToList()
			};

		public static NumPosThckRflt<T> ToPosThckRflt<T>
			( T x , T y , T thck , List<T> rflt , List<T> wave )
			=> new NumPosThckRflt<T>( x , y , thck , rflt , wave );
	}

	public class NumPosThckRflt<T>
	{
		public readonly T X;
		public readonly T Y;
		public readonly T Thickness;
		//public readonly T KlaThickness;
		public readonly List<T> Reflectivity;
		public List<T> WaveLength => _WaveLength;

		public static List<T>  _WaveLength ;

		public NumPosThckRflt( T x , T y , T thck , List<T> rflt , List<T> wave )
		{
			if ( _WaveLength == null ) _WaveLength = wave;
			X = x;
			Y = y;
			Thickness = thck;
			//KlaThickness = klathck;
			Reflectivity = rflt;
		}
	}

	public class DatasAnMissing<T> // For Tracking Missing Datas 
	{
		public List<Maybe<T>> Datas;
		public List<int> MissingIndex
			=> Datas.IndicesOf( x => !x.isJust );
	}

}
