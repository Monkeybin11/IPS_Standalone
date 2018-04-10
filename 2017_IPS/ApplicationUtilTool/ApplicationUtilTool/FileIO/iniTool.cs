using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.FileIO
{
    public class iniTool
    {
        // CurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        [DllImport( "kernel32" )]
        public static extern long WritePrivateProfileString( string section , string key , string val , string filePath );
        [DllImport( "kernel32" )]
        public static extern int GetPrivateProfileString( string section , string key , string def , StringBuilder retVal ,
                                                        int size , string filePath );


        public readonly string FilePath;

        public iniTool( string path )
        {
            FilePath = path;
            if ( !File.Exists( path ) ) File.Create( path );

        }

        //public iniTool ChangeKey( string section , string keyname , stirng )
        //{
        //    iniTool.WritePrivateProfileString( section , keyname , value , FilePath );
        //    return this;
        //}


        public iniTool WriteValue( string section , string keyname , string value )
        {
            iniTool.WritePrivateProfileString( section , keyname , value , FilePath );
            return this;
        }

        public iniTool WriteValues(string section , IEnumerable<Tuple<string,string>> keyval)
        {
            keyval.ToList().ForEach(x => iniTool.WritePrivateProfileString( section , x.Item1 , x.Item2 , FilePath ) );
            return this;
        }

        public string GetValue( string section , string keyname )
        {
            var str = new StringBuilder(1024);
            iniTool.GetPrivateProfileString( section , keyname , "" , str , 1024 , FilePath );
            return str.ToString();
        }

        public List<string> GetValues( string section , IEnumerable<string> keynames ) =>
            keynames.Select( x => GetValue( section , x ) )
                    .ToList();
    }
}
