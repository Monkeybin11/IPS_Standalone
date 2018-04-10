using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using XGBoost;
//using static XGBoost.BaseXgbModel;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public enum LabelType { IPS , KLA }

	public static class Core_Fitting
	{


		//  나중에는 세팅 , 데이터 , 로드 여부 순으로 모델 만들기
		//public static XGBRegressor CreateModel( List<IpsDataSet> datas )
		//{
		//	var regr = new XGBRegressor();
		//	regr.Fit( GetReflectivity( datas ) , GetKlaThickness( datas ) );
		//	return regr;
		//}
		//
		//public static Func<string,XGBRegressor,XGBRegressor> SaveModel
		//	=> ( saveroot , regr )
		//	=>
		//	{
		//		regr.SaveModelToFile( saveroot );
		//		return regr;
		//	};
		//
		//public static double CalcMSE( List<IpsDataSet> datas , XGBRegressor regr)
		//{
		//	var target = GetKlaThickness(datas);
		//	var pred = regr.Predict( GetReflectivity(datas) );
		//	return MSE( pred , target );
		//}
		//
		//public static XGBRegressor UpdateModel(string modelPath , List<IpsDataSet> datas)
		//{
		//	var regr = LoadRegressorFromFile(modelPath);
		//	regr.Fit( GetReflectivity(datas) , GetKlaThickness(datas) );
		//	return regr;
		//}

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
}
