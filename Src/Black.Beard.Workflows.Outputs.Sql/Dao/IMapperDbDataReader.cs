using System.Data.Common;

namespace Bb.Dao
{

    public interface IMapperDbDataReader
    {

        void Map(DbDataReaderContext item);

    }

}