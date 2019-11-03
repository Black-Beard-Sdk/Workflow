using System.Data;

namespace Bb.Dao
{

    public interface IQueryPredicateGenerator
    {
        string CurrentTimestamp { get; }

        string WriteMember(string name);

        string WriteParameter(string variableName);

        string WriteEquality();

        string WriteNotEquality();
        string WriteType(DbType variableType, object value);

        string WriteStringValue(object value);
        string WriteBooleanValue(object value);
        string WriteGuid(object value);
        string WriteDatetime(object value);
        string WriteTimeOffset(object value);
        string WriteTime(object value);
        string WriteDecimal(object value);
        string WriteInt(object value);
        string WriteUInt(object value);

    }

}