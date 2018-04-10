using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Data
{
	public interface IStgCtrl
	{
		string Query( string msg );
		bool Send( string msg );
		bool WaitReady( int timeoutSec );
		bool SendAndReady( string cmd , int timeoutSec = 0);

		string Home			{get;set;}
		string GoAbs		{get;set;}
		string GoRel		{get;set;}
		string SetSpeed		{get;set;}
		string Status		{get;set;}
		string StatusOK		{get;set;}
		string Pos			{get;set;}
		string Go			{get;set;}
		string Stop			{get;set;}
	}
}
