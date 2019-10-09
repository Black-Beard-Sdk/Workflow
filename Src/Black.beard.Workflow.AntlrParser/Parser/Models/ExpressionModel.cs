using System;

namespace Bb.Workflows.Parser.Models
{

    /// <summary>
    /// Expression base
    /// </summary>
    public abstract class ExpressionModel
    {

        public object Evaluator { get; set; }

        public bool Evaluate<TContext>(TContext context)
        {
            var e = Evaluator as Func<TContext, bool>;
            return e(context);
        }

        public abstract T Accept<T>(IVisitor<T> visitor);

    }

}