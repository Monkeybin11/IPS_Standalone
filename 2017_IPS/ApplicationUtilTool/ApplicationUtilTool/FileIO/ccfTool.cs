using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.FileIO
{
    //  ccfTool ccf = new ccfTool(@"C:\Users\idiol\Desktop","T__tdi_2inch");
    //  var changer = ccf.AppendChangeList("Stream Conditioning");
    //  changer( @"Crop Height" )( "564" );
    //  changer( @"Scale Vertical" )( "444" );
    //  ccf.RunChnage();
    public class ccfTool
    {
        List<Tuple<string,string,string>> ChangeList;
        string BasePath;
        string FileName;
        string Extension;
        string FullPath { get { return Path.Combine( BasePath , FileName + Extension ); } }

        public ccfTool( string basepath , string filename )
        {
            ChangeList = new List<Tuple<string , string , string>>();
            BasePath = basepath;
            FileName = filename;
            if ( !File.Exists( basepath + "\\" + filename ) ) throw new Exception( "Config ccf file is not exist" );
        }

        public ccfTool( string fullpath )
        {
            string[] strlist = fullpath.Split('.')[0].Split('\\');
            Extension = Path.GetExtension( fullpath );
            ChangeList = new List<Tuple<string , string , string>>();
            BasePath = strlist.Take(strlist.Count()-1)
                                           .Aggregate( (f,s) => Path.Combine(f,s) );
            FileName = strlist.Last();
        }


        public Func<string , Func<string , Action<string>>> AppendChangeList
            => section
            => key
            => value
            => ChangeList.Add( Tuple.Create( section , key , value ) );

        public void RunChnage()
        {
            if ( ChangeList != null )
            {
                var nonExtensionPath = Path.Combine(BasePath,FileName);
                ccf2ini( nonExtensionPath + ".ccf" );
                iniTool initool = new iniTool(Path.Combine(BasePath,FileName)+".ini");
                foreach ( var item in ChangeList )
                {
                    initool.WriteValue( item.Item1 , item.Item2 , item.Item3 );
                }
                ini2ccf( nonExtensionPath + ".ini");
            }
        }

        public bool? ccf2ini( string originPath )
        {
            if ( Path.GetExtension( originPath ) == ".ccf" )
            {
                File.Move( originPath , Path.ChangeExtension( originPath , ".ini" ) );
                return true;
            }
            return null;
        }


         public bool? ccf2ini(  )
        {
            if ( Extension == ".ccf" )
            {
                File.Move( FullPath , Path.ChangeExtension( FullPath , ".ini" ) );
                return true;
            }
            return null;
        }

        public bool? ini2ccf( string originPath )
        {
            if ( Path.GetExtension( originPath ) == ".ini" )
            {
                File.Move( originPath , Path.ChangeExtension( originPath , ".ccf" ));
                return true;
            }
            return null;
        }

        public bool? ini2ccf()
        {
            var inipath = Path.Combine(BasePath , FileName+".ini");
            if ( File.Exists( inipath  )) 
            {
                File.Move( inipath , Path.ChangeExtension( inipath , ".ccf" ) );
                return true;
            }
            return null;
        }
    }
}
