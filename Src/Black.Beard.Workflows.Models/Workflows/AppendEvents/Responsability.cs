using Bb.Workflows.Models;

namespace Bb.Workflows
{

    public abstract class Responsability<TContext>
        where TContext : RunContext, new()
    {

        public Responsability(Responsability<TContext> child = null)
        {
            this.Child = child;
        }

        public void Eval(TContext model)
        {
            if (Check(model))
                Execute(model);
            else
                Child?.Eval(model);
        }

        protected abstract void Execute(TContext model);

        public abstract bool Check(TContext model);

        public Responsability<TContext> Child { get; }

    }

}
