using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.SqlServer.Server;
#pragma warning disable CS8625
#pragma warning disable CS0649
#pragma warning disable CS8618

// ReSharper disable All
#pragma warning disable 649
#pragma warning restore 649
#pragma warning disable 0618

namespace Demo.Data.Extensions;

public class Tvp : SqlMapper.ICustomQueryParameter
{

    static readonly Action<System.Data.SqlClient.SqlParameter, string> SetTypeName;
    private readonly IEnumerable<SqlDataRecord> _table;
    private readonly string _typeName;

    /// <summary>
    ///     Create a new instance of TableValuedParameter
    /// </summary>
    public Tvp(IEnumerable<SqlDataRecord> table) : this(table, null)
    {
    }

    /// <summary>
    ///     Create a new instance of TableValuedParameter
    /// </summary>
    public Tvp(IEnumerable<SqlDataRecord> table, string typeName)
    {
        this._table = table;
        this._typeName = typeName;
    }

    void SqlMapper.ICustomQueryParameter.AddParameter(IDbCommand command, string name)
    {
        var param = command.CreateParameter();
        param.ParameterName = name;
        Set(param, _table, _typeName);
        command.Parameters.Add(param);
    }

    internal static void Set(IDbDataParameter parameter, IEnumerable<SqlDataRecord> table, string typeName)
    {

        parameter.Value = SqlMapper.SanitizeParameterValue(table);
        if (parameter is SqlParameter sqlParam)
        {
            SetTypeName?.Invoke(sqlParam, typeName);
            sqlParam.SqlDbType = SqlDbType.Structured;
        }
    }
}