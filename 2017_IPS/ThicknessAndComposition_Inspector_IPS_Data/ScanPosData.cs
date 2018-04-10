using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class ScanPosData
	{
		public double[] RhoList = new double[]
		{
			0,
			49.4,
			98.8,
			148
		};

		public double[][] MovePosList = new double[][]
			{
				new double[] { 0} ,
				new double[] { 90.0,135.0,180.0,225.0,270.0,315.0 , 360.0 , 405.0},
				new double[] {  90.0, 112.5, 135.0, 157.5, 180.0, 202.5, 225.0, 247.5, 270.0, 292.5, 315.0, 337.5 , 360.0 , 382.5, 405.0, 427.5},
				new double[] { 90, 105, 120, 135, 150, 165, 180, 195, 210, 225, 240, 255, 270, 285, 300, 315, 330, 345 , 360,375, 390, 405, 420, 435 }

			};

		public double[][] ThetaList = new double[][]
			{
				new double[] { 0} ,
				new double[] { 90,135,180,225,270,315 , 0 , 45},
				new double[] {  90.0, 112.5, 135.0, 157.5, 180.0, 202.5, 225.0, 247.5, 270.0, 292.5, 315.0, 337.5 ,0.0, 22.5, 45.0, 67.5},
				new double[] { 90, 105, 120, 135, 150, 165, 180, 195, 210, 225, 240, 255, 270, 285, 300, 315, 330, 345 , 0,15, 30, 45, 60, 75 }

			};


		public double[][] FixScanPos = new double[][]
		{
			new double[]{0.00000 ,0.00000		   },
			new double[]{-0.00001 ,4.94000         },
			new double[]{-3.49312 ,3.49309         },
			new double[]{-4.94000 ,-0.00001        },
			new double[]{-3.49309 ,-3.49312        },
			new double[]{0.00001 ,-4.94000         },
			new double[]{3.49312 ,-3.49309         },
			new double[]{4.94000 ,0.00001          },
			new double[]{3.49309 ,3.49312          },
			new double[]{-0.00003 ,9.88000         },
			new double[]{-3.78095 ,9.12792         },
			new double[]{-6.98624 ,6.98619         },
			new double[]{-9.12795 ,3.78088         },
			new double[]{-9.88000 ,-0.00004        },
			new double[]{-9.12792 ,-3.78095        },
			new double[]{-6.98619 ,-6.98624        },
			new double[]{-3.78088 ,-9.12795        },
			new double[]{0.00004 ,-9.88000         },
			new double[]{3.78095 ,-9.12792         },
			new double[]{6.98625 ,-6.98619         },
			new double[]{9.12795 ,-3.78088         },
			new double[]{9.88000 ,0.00004          },
			new double[]{9.12792 ,3.78095          },
			new double[]{6.98618 ,6.98625          },
			new double[]{3.78088 ,9.12795          },
			new double[]{-0.00006 ,14.82000        },
			new double[]{-3.83576 ,14.31501        },
			new double[]{-7.41004 ,12.83448        },
			new double[]{-10.47936 ,10.47928       },
			new double[]{-12.83452 ,7.40996        },
			new double[]{-14.31504 ,3.83564        },
			new double[]{-14.82000 ,-0.00006       },
			new double[]{-14.31501 ,-3.83576       },
			new double[]{-12.83446 ,-7.41004       },
			new double[]{-10.47928 ,-10.47937      },
			new double[]{-7.40994 ,-12.83452       },
			new double[]{-3.83564 ,-14.31504       },
			new double[]{0.00006 ,-14.82000        },
			new double[]{3.83576 ,-14.31501        },
			new double[]{7.41006 ,-12.83446        },
			new double[]{10.47937 ,-10.47928       },
			new double[]{12.83453 ,-7.40994        },
			new double[]{14.31504 ,-3.83564        },
			new double[]{14.82000 ,0.00006         },
			new double[]{14.31501 ,3.83576         },
			new double[]{12.83446 ,7.41006         },
			new double[]{10.47928 ,10.47937        },
			new double[]{7.40994 ,12.83453         },
			new double[]{3.83564 ,14.31504         }
		};
	}
}
