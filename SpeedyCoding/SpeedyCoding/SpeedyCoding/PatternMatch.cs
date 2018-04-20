using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This code is from http://codebetter.com/matthewpodwysocki/2008/09/16/functional-c-pattern-matching/

namespace SpeedyCoding
{
    public class IncompletePatternMatchException :
   Exception
    {
    }

    public class PatternMatchOnValue<TIn, TOut>
    {
        private readonly IList<PatternMatchCase> _cases =
            new List<PatternMatchCase>();
        private readonly TIn _value;
        private Func<TIn, TOut> _elseCase;

        internal PatternMatchOnValue( TIn value )
        {
            _value = value;
        }

        /// <summary>
        /// Define matching condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public PatternMatchOnValue<TIn , TOut> With(
            Predicate<TIn> condition ,
            Func<TIn , TOut> result )
        {
            _cases.Add( new PatternMatchCase
            {
                Condition = condition ,
                Result = result
            } );

            return this;
        }

        /// <summary>
        /// Define matching condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public PatternMatchOnValue<TIn , TOut> With(
            Predicate<TIn> condition ,
            TOut result )
        {
            return With( condition , x => result );
        }

        /// <summary>
        /// In case of matching failing
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public PatternMatchOnValue<TIn , TOut> Else(
            Func<TIn , TOut> result )
        {
            if ( _elseCase != null )
            {
                throw new InvalidOperationException(
                    "Cannot have multiple else cases" );
            }

            _elseCase = result;

            return this;
        }

        /// <summary>
        /// In case of matching failing
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public PatternMatchOnValue<TIn , TOut> Else(
            TOut result )
        {
            return Else( x => result );
        }

        public TOut Do()
        {
            if ( _elseCase != null )
            {
                With( x => true , _elseCase );
                _elseCase = null;
            }

            foreach ( var test in _cases )
            {
                if ( test.Condition( _value ) )
                {
                    return test.Result( _value );
                }
            }

            throw new IncompletePatternMatchException();
        }

        private struct PatternMatchCase
        {
            public Predicate<TIn> Condition;
            public Func<TIn, TOut> Result;
        }
    }

    public static class PatternMatch
    {
        public static PatternMatchContext<TIn> Match<TIn>(
            this TIn value )
        {
            return new PatternMatchContext<TIn>( value );
        }
    }

    public class PatternMatchContext<TIn>
    {
        private readonly TIn _value;

        internal PatternMatchContext( TIn value )
        {
            _value = value;
        }

        public PatternMatchOnValue<TIn , TOut> With<TOut>(
            Predicate<TIn> condition ,
            TOut result )
        {
            return new PatternMatchOnValue<TIn , TOut>( _value )
                .With( condition , result );
        }
    }


}
