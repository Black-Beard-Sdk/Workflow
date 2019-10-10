namespace Bb.Workflows.Outputs
{

    public interface ILockObject
    {

        void Lock(uint crc);


        void UnLock(uint crc);


    }

}
