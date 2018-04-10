using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using SpeedyCoding;

namespace MachineLib.DeviceLib.ACS_Stage
{
    public class ACSStageController_Virtual : IACSStageController
    {
        public string Address
        {
            get
            {
                return "Address";
            }
        }

        public Maybe<IACSStageController> Operator { get; set; }

        public Maybe<IACSStageController> Connect( string connectPath )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public double CurrentPosition( string axis , double pos )
        {
            return 0;
        }

        public Maybe<IACSStageController> MoveAbs( string axis , double pos )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> MoveRel( string axis , double pos )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> Origin( string axis )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> SetSpeed( double speed )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> StartTrigger( int buffnum )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> StopTrigger( int buffnum )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> TurnOnOff( string axis , bool onSwitch )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> WaitInPos( string axis , double targetPos )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }
    }
}
