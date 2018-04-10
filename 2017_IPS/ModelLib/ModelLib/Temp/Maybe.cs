using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    public interface Maybe<T> : Monad
    {

    }


    public class Just<T> : Maybe<T>, IEnumerable<T>
    {
        T _Value { get; set; }
        public T Value { get; private set; }
        public bool HasValue { get { return true; } }
        public Just( T value )
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals( object obj )
        {
            var target = obj as Maybe<T>;
            return target == null ? false : true;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public IEnumerable<T> ToEnumerable()
        {
            yield return Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Value;
        }

    }

    public class Nothing<T> : Maybe<T> , IEnumerable<T>
    {
        public T Value { get; private set; }
        public bool HasValue { get { return false; } }

        public override string ToString()
        {
            return "";
        }

        public override bool Equals( object obj )
        {
            return obj == null ? true : false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public IEnumerable<T> ToEnumerable()
        {
            yield return Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return null;
        }
    }

   

    public static class MaybeExt
    {
        public static Maybe<T> ToMaybe<T>
            ( this T value )
        {
            return new Just<T>( value );
        }

        public static Maybe<IB> ToMaybe<A, IB>
          ( this A value )
            where A : class
            where IB : class
        {
            var obj = value as IB;
            if ( obj == null ) return new Just<IB>( obj );
            else               return new Nothing<IB>();
        }


        public static Maybe<B> Bind<A, B>(
            this Maybe<A> a ,
            Func<A , Maybe<B>> func )
        {
            var justa = a as Just<A>;
            return justa == null ?
                   new Nothing<B>() :
                   func( justa.Value );
        }

        // This is Currying. 
        // select : M<A> -> A -> B -> C -> M<C> 
        // select 
        //
        public static Maybe<C> SelectMany<A, B, C>(
            this Maybe<A> src ,
            Func<A , Maybe<B>> func ,
            Func<A , B , C> select )
        {
            return src.Bind( a =>
                    func( a ).Bind( b =>
                    select( a , b ).ToMaybe() ) );
        }

        public static Maybe<A> Else<A>(
            this Maybe<A> a ,
            Action act )
        {
            var justa = a as Just<A>;
            if ( justa == null )
            {
                act();
                return a;
            }
            return new Nothing<A>();
        }

        public static Maybe<int> Div( this int numerator , int denominator )
        {
            return denominator == 0
                       ? ( Maybe<int> )new Nothing<int>()
                       : new Just<int>( numerator / denominator );
        }

		//public override static IList<Maybe<T>> IndexOf(
		//this IList<Maybe<T>>src )
     }
}
