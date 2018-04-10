using System.Collections.Generic;
using System.Linq;
using System.IO;
using SpeedyCoding;
using ModelLib.AmplifiedType;
using static ModelLib.AmplifiedType.Handler;
using FittingDataStruct;

using static FittingDataStruct.Handler;
using static System.IO.Directory;
using static System.IO.Path;
using System;


namespace Fitting_Core
{
	using FileNames = IEnumerable<string>;
	using DPosThckRflt = NumPosThckRflt<double>;
	using MssData = DatasAnMissing<List<double>>;
	public static partial class DataLoader
	{
		readonly static string RefFileName = "Reflectivity.csv";
		readonly static string ResFileName = "Result.csv";
		readonly static string KlaFileName = "KLAResult.csv";


		public static List<string [ ]> GetIpsDataSetPaths( IEnumerable<string> path )
		{

			var output = new List<string[]>();

			var simbolList = path.Select(x => x.Split('_').First()).Distinct().ToList();

			foreach ( var simbol in simbolList )
			{
				var samefile = path.Where( x => x.Split('_').First() == simbol);

				var resultPath = samefile.Where(x => x.Split('_').Last() == ResFileName ).ToList();
				var rfltPath = samefile.Where(x => x.Split('_').Last() == RefFileName ).ToList();

				if ( resultPath.Count == 1 && rfltPath.Count == 1 )
				{
					output.Add( new string [ ] { simbol.Split( '-' ).First().Split( '\\' ).Last() , resultPath.First() , rfltPath.First() } );
				}
			}
			return output;
		}

		public static Maybe<List<string [ ]>> MGetIpsDataSetPaths( IEnumerable<string> path )
		{

			var output = new List<string[]>();

			var simbolList = path.Select(x => x.Split('_').FromLast(1) ).Distinct().ToList();

			foreach ( var simbol in simbolList )
			{
				var samefile = path.Where( x => x.Split('_').FromLast(1) == simbol);

				var resultPath = samefile.Where(x => x.Split('_').Last() == ResFileName ).ToList();
				var rfltPath = samefile.Where(x => x.Split('_').Last() == RefFileName ).ToList();

				if ( resultPath.Count == 1 && rfltPath.Count == 1 )
				{
					output.Add( new string [ ] { simbol.Split( '-' ).FromLast(1).Split( '\\' ).Last() , resultPath.First() , rfltPath.First() } );
				}
			}
			return output.Count > 0
					? Just( output )
					: None;
		}


		public static Maybe<List<DPosThckRflt>> GetIPSDatas( string ipsPath , string klaPah )
		{
			ipsPath = Path.GetDirectoryName( ipsPath );

			FileNames filenames = Just( ipsPath )
							.Bind( GetAllDirs )
							.Map( GetAllFileNames );

			if ( !CheckFiles( filenames ) ) return None;

			var numOfClass = filenames.Select( GetNumOfClass ).ToList();
			var wave     = GetDataWith( ReadWaveLen , RfltFilter , filenames ).First().Datas.First().Value;
			var thckness = GetDataWith( ReadPos , ThckFilter , filenames );
			var rflts    = GetDataWith( ReadReflectivity , RfltFilter , filenames );
			var Total = thckness.Zip( rflts , (f,s) => f.ToTuple( s ) ).ToList();
			//var Total = ToClsNumWihtThckRflt(numOfClass , thckness , rflts );

			List<List< DPosThckRflt >> totallist = new List<List<NumPosThckRflt<double>>>();

			//for ( int i = 0 ; i < Total.Count ; i++ ) // loop for each files 
			//{
			foreach ( var item in Total )
			{
				//var item = Total[i];
				var thmiss = item.Item1.MissingIndex;
				var rfmiss = item.Item2.MissingIndex;

				var missing = thmiss.Concat(rfmiss).Distinct().OrderBy( x => x).Reverse();

				foreach ( var idx in missing ) // remove 
				{
					//item.Item1.RemoveAt( idx );
					item.Item1.Datas.RemoveAt( idx );
					item.Item2.Datas.RemoveAt( idx );
				}

				var newthck = item.Item1.Datas.Select( x => x.Value).ToList();
				var newrflt = item.Item2.Datas.Select( x => x.Value).ToList();

				if ( newthck.Count != newrflt.Count ) return None;

				List<DPosThckRflt> output  = new List<DPosThckRflt>();

				for ( int j = 0 ; j < newrflt.Count ; j++ )
				{
					var ptrw = ToPosThckRflt(  newthck[j][0] , newthck[j][1] , newthck[j][2] , newrflt[j] , wave );

					output.Add( ptrw );
				}
				totallist.Add( output );
			}
			return Just( totallist.Flatten().ToList() );
		}
	}
}




