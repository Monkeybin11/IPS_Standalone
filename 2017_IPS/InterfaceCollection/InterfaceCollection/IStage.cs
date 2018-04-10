using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceCollection
{
    public interface IStageAPI<TModel,TconnectInfoType>
    {
        TModel Operator { get; set; }
        TModel Connect( TconnectInfoType connect );
        TModel TurnOnOff( string axis, bool onSwitch);
        TModel SetSpeed( double speed );
        TModel MoveAbs( string axis , double pos);
        TModel MoveRel( string axis , double pos);
        TModel WaitInPos( string axis , double pos );
        double CurrentPosition( string axis , double pos );
        TModel StartTrigger( int buffnum );
        TModel StopTrigger( int buffnum );
    }
}
