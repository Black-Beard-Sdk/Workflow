using Bb.Dao.Dao;
using System;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace Bb.Dao
{

    public interface IQueryGenerator
    {

        SqlManager Manager { get; }

        QueryCommand Generate(QuerySqlCommand query);

        //QueryCommand Insert(object instance, ObjectMapping mapping);

        //QueryCommand Remove(object instance, ObjectMapping mapping);

        //QueryCommand Update(object instance, ObjectMapping mapping);

        QueryCommand Select<T>(ObjectMapping mapping, Expression<Func<T, bool>>[] e);

        string Declare(string variableName, DbType variableType, string value);

        string GetValue(DbType dbType, object value);

        string ParameterName(string parameterName);

        string Member(string v);

    }

}