using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.AmplifiedType
{

    using Writer = Func<string, string>;

    public partial class Handler
    {
        public static AccumulWriter<A> Accmululatable<A>(A val, Writer writer) => new AccumulWriter<A>(val, writer);
        public static AccumulWriter<A> Accmululatable<A>(A val, string txt, Writer writer) => new AccumulWriter<A>(val, txt, writer);
    }


    /// <summary>
    /// AccumulWriter Tpye Class , Writer: String -> String , Value : Stack<A> ( Value History ) , Paper : Stack<string> (Write History)
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public struct AccumulWriter<A>
    {
        internal Stack<A> Value { get; set; }
        internal Stack<string> Paper { get; set; }
        internal Writer Writer { get; set; }

        internal AccumulWriter(A value, Writer writer)
        {
            if (value == null)
                throw new ArgumentException(nameof(value));
            Value = new Stack<A>();
            Value.Push(value);

            Paper = new Stack<string>();
            Paper.Push("");

            Writer = writer;
        }

        internal AccumulWriter(A value, string txt, Writer writer)
        {
            if (value == null)
                throw new ArgumentException(nameof(value));
            Value = new Stack<A>();
            Value.Push(value);

            Paper = new Stack<string>();
            Paper.Push(txt);

            Writer = writer;
        }

        internal AccumulWriter(A value, string txt, AccumulWriter<A> past)
        {
            if (value == null)
                throw new ArgumentException(nameof(value));

            Value = past.Value;
            Value.Push(value);

            Paper = past.Paper;
            Writer = past.Writer;
            Paper.Push(Paper.First() + Writer(txt));
        }

        public Stack<A> ValueHistory => Value;
        public Stack<string> PaperHistory => Paper;
    }

    public static class AccumulWriterExt
    {
        public static A Lift<A>
            (this AccumulWriter<A> self, Func<A, A> f)
            => f(self.Value.First());

        // Func => new M<A> ( val , txt , self  )
        public static AccumulWriter<A> Add<A>
            (this AccumulWriter<A> self, Func<A, A> f, string txt)
            => new AccumulWriter<A>(f(self.Value.First()), txt, self);

        public static string GetLastPaper<A>
            (this AccumulWriter<A> self)
            => self.Paper.First();

        public static A GetLastValue<A>
           (this AccumulWriter<A> self)
           => self.Value.First();

        public static string GetFirstPaper<A>
          (this AccumulWriter<A> self)
          => self.Paper.Last();

        public static A GetFirstValue<A>
           (this AccumulWriter<A> self)
           => self.Value.Last();

        public static AccumulWriter<A> Restore<A>
            (this AccumulWriter<A> src)
        {
            src.Paper.Pop();
            src.Value.Pop();
            return src;
        }

        public static int Count<A>
           (this AccumulWriter<A> src)
            => src.Value.Count;
    }
}
