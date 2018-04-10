using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGBoost;

namespace Fitting_Core
{
	using static XGBoost.BaseXgbModel;
	public enum LabelType { IPS, KLA }

	public static class Core_Fitting
	{
		public static XGBRegressor Regr;
		static List<IpsDataSet> LoadedDatas;


		public static void Reset()
		{
			Regr = new XGBRegressor();
			LoadedDatas = new List<IpsDataSet>();
		}
		//  나중에는 세팅 , 데이터 , 로드 여부 순으로 모델 만들기
		public static XGBRegressor CreateModel( List<IpsDataSet> datas )
		{
			LoadedDatas = datas;
			Regr = new XGBRegressor();
			Regr.Fit( GetReflectivity( datas ) , GetKlaThickness( datas ) );
			return Regr;
		}


		public static XGBRegressor LoadModel( string path ) // Excuted when ScanAutorun is fired
		{
			Regr = LoadRegressorFromFile( path );
			return Regr;
		}

		public static float [ ] Predict( double [ ] src )
		{
			var xdata = src.Select( x =>(float)x).ToArray();
			var output =  Regr.Predict( new float[][] { xdata } );
			return output   ;
		}



		public static Action<string> SaveModel
			=> ( path )
			=> Regr.SaveModelToFile( path + ".model" );
		
		public static double CalcMSE( XGBRegressor regr)
		{
			var target = GetKlaThickness(LoadedDatas);
			var pred = regr.Predict( GetReflectivity(LoadedDatas) );
			return MSE( pred , target );
		}
		
		public static XGBRegressor UpdateModel( List<IpsDataSet> datas)
		{
			Regr.Fit( GetReflectivity(datas) , GetKlaThickness(datas) );
			return Regr;
		}

		private static Func<List<IpsDataSet> , float [ ]> GetKlaThickness
			=> src
			=> src.Select( x => x.KlaThickness.AsEnumerable() )
				  .Aggregate( ( f , s ) => f.Concat( s ) )
				  .ToArray();

		private static Func<List<IpsDataSet> , float [ ] [ ]> GetReflectivity
			=> src
			=> src.Select( x => x.RfltList.AsEnumerable() )
				  .Aggregate( ( f , s ) => f.Concat( s ) )
				  .ToArray();


		private static Func<float [ ] , float [ ] , double> MSE
			=> ( target , pred )
			=> Math.Sqrt( target.Select( ( x , i ) => ( double )Math.Pow( ( x - pred [ i ] ) , 2 ) ).Sum() / target.Length );
	}

	public class Parameters
	{
		public readonly List<double> PrameterList;

		public Parameters
			(
					int		maxDepth = 3,
					float	learningRate = 0.1F, 
					int		nEstimators = 100,
					bool	silent = true, 
					string	objective = "reg:linear", 
					int		nThread = -1, 
					float	gamma = 0, 
					int		minChildWeight = 1, 
					int		maxDeltaStep = 0, 
					float	subsample = 1, 
					float	colSampleByTree = 1,
					float	colSampleByLevel = 1, 
					float	regAlpha = 0, 
					float	regLambda = 1, 
					float	scalePosWeight = 1, 
					float	baseScore = 0.5F, 
					int		seed = 0, 
					float	missing = float.NaN
			)
		{
		}



	}

}
