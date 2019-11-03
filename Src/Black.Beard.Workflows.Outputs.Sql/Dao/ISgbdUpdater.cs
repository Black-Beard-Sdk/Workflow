using System.Collections.Generic;
using System.Text;

namespace Bb.Dao
{
    public interface ISgbdUpdater
    {

        StringBuilder[] GetScripts(Dictionary<string, string> arguments);

    }

}