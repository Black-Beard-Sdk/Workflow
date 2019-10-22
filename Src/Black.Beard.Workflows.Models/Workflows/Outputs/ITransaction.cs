using System;

namespace Bb.Workflows.Outputs
{

    public interface ITransaction : IDisposable
    {

        ITransaction Child { get; set; }

        void Commit();

        void Rollback();

    }

}
