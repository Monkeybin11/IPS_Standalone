using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceCollection;
using SPIIPLUSCOM660Lib;
using ModelLib.Monad;
using System.Threading;
using SpeedyCoding;

namespace MachineLib.DeviceLib.ACS_Stage
{
    public interface Child : IStageAPI<Maybe<IACSStageController> , string>
    { }


    public enum ConnectMode { IP , Com }

    public partial class ACSStageController : IACSStageController
    {
        object pWait = 0;
        public string Address { get; }
        public Dictionary<string , int> Axis;
        private AsyncChannel Ch;
        public Maybe<IACSStageController> Operator { get;  set; }

        public ACSStageController( ConnectMode mode , string address )
        {
            Ch = new AsyncChannel();
            Axis = new Dictionary<string , int>()
                   .Append( "Y" , 0 )
                   .Append( "X" , 1 )
                   .Append( "Z" , 2 );
            Address = address;
        }

        public Maybe<IACSStageController> Connect( string connectPath )
        {
            object container;
            var Currentcount = Ch.GetConnectionsList(out container, out container,out container);
            Ch.CloseComm();
            Ch.OpenCommEthernetTCP( connectPath , Ch.ACSC_SOCKET_STREAM_PORT );
            Thread.Sleep(300);
            var Nextcount = Ch.GetConnectionsList(out container, out container,out container);
            if ( Currentcount == Nextcount ) return new Nothing<IACSStageController>();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> Origin( string axis )
        {
            Ch.RunBuffer( Axis [ axis ] , "" , Ch.ACSC_ASYNCHRONOUS , ref pWait );
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> TurnOnOff( string axis , bool onSwitch )
        {
            if(onSwitch) Ch.Enable( Axis [ axis ] , Ch.ACSC_ASYNCHRONOUS , ref pWait );
            else Ch.Disable( Axis [ axis ] , Ch.ACSC_ASYNCHRONOUS , ref pWait );
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> SetSpeed( double speed )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> MoveAbs( string axis , double pos )
        {
            Ch.ToPoint( 0 , Axis [ axis ] , pos , Ch.ACSC_ASYNCHRONOUS , ref pWait );
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> MoveRel( string axis , double pos )
        {
            Ch.ToPoint( Ch.ACSC_AMF_RELATIVE , Axis [ axis ] , pos , Ch.ACSC_ASYNCHRONOUS , ref pWait );
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> WaitInPos( string axis , double targetPos )
        {
            while ( true )
            {
                double error = Math.Abs( targetPos - Ch.GetFPosition( Axis[axis], Ch.ACSC_SYNCHRONOUS, ref pWait ) );
                if ( error < 1 ) break;
            }
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public double CurrentPosition( string axis , double pos )
        {
            return Ch.GetFPosition( Axis.Values.ElementAt( Axis [ axis ] ) , Ch.ACSC_SYNCHRONOUS , ref pWait );
        }

        public Maybe<IACSStageController> StartTrigger( int buffnum )
        {
            Ch.RunBuffer( buffnum , "" , Ch.ACSC_ASYNCHRONOUS , ref pWait );
            return this.Delay50().ToMaybe<IACSStageController>();
        }
        public Maybe<IACSStageController> StopTrigger( int buffnum )
        {
            Ch.StopBuffer( buffnum , Ch.ACSC_ASYNCHRONOUS , ref pWait );
            return this.Delay50().ToMaybe<IACSStageController>();
        }

    }
    
}


