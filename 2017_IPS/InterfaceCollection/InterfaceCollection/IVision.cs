using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceCollection
{
    public interface IVision<Tdata,Tregion,TregionInfo,Tresult>
    {
        Tdata Preprocessing( Tdata input );
        Tregion RegionProposal( Tdata input );
        TregionInfo ClassifyRegion( Tregion input );
        Tresult ClassifyInfo( TregionInfo input );
    }
}
