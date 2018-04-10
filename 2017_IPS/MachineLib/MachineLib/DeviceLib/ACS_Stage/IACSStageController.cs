namespace MachineLib.DeviceLib.ACS_Stage
{
    public interface IACSStageController : InterfaceCollection.IStageAPI<ModelLib.Monad.Maybe<IACSStageController> , string>
    {
        string Address { get; }
        ModelLib.Monad.Maybe<IACSStageController> Operator { get; set; }
        ModelLib.Monad.Maybe<IACSStageController> Connect( string connectPath );
        double CurrentPosition( string axis , double pos );
        ModelLib.Monad.Maybe<IACSStageController> MoveAbs( string axis , double pos );
        ModelLib.Monad.Maybe<IACSStageController> MoveRel( string axis , double pos );
        ModelLib.Monad.Maybe<IACSStageController> Origin( string axis );
        ModelLib.Monad.Maybe<IACSStageController> SetSpeed( double speed );
        ModelLib.Monad.Maybe<IACSStageController> StartTrigger( int buffnum );
        ModelLib.Monad.Maybe<IACSStageController> StopTrigger( int buffnum );
        ModelLib.Monad.Maybe<IACSStageController> TurnOnOff( string axis , bool onSwitch );
        ModelLib.Monad.Maybe<IACSStageController> WaitInPos( string axis , double targetPos );
    }
}