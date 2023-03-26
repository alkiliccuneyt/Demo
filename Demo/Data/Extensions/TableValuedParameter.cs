using System.Collections.Concurrent;
using System.Data;
using System.Reflection;
using Dapper;
using Microsoft.SqlServer.Server;
#pragma warning disable CS8625
#pragma warning disable CS8603

// ReSharper disable All

namespace Demo.Data.Extensions;

public static class TableValuedParameter
{
    private static readonly Dictionary<Type, SqlDbType> Types = new Dictionary<Type, SqlDbType>
    {
        {typeof(Boolean), SqlDbType.Bit},
        {typeof(Boolean?), SqlDbType.Bit},
        {typeof(Byte), SqlDbType.TinyInt},
        {typeof(Byte?), SqlDbType.TinyInt},
        {typeof(String), SqlDbType.NVarChar},
        {typeof(DateTime), SqlDbType.DateTime2},
        {typeof(DateTime?), SqlDbType.DateTime2},
        {typeof(Int16), SqlDbType.SmallInt},
        {typeof(Int16?), SqlDbType.SmallInt},
        {typeof(Int32), SqlDbType.Int},
        {typeof(Int32?), SqlDbType.Int},
        {typeof(Int64), SqlDbType.BigInt},
        {typeof(Int64?), SqlDbType.BigInt},
        {typeof(Decimal), SqlDbType.Decimal},
        {typeof(Decimal?), SqlDbType.Decimal},
        {typeof(Double), SqlDbType.Float},
        {typeof(Double?), SqlDbType.Float},
        {typeof(Single), SqlDbType.Real},
        {typeof(Single?), SqlDbType.Real},
        {typeof(TimeSpan), SqlDbType.Time},
        {typeof(Guid), SqlDbType.UniqueIdentifier},
        {typeof(Guid?), SqlDbType.UniqueIdentifier},
        {typeof(Byte[]), SqlDbType.Binary},
        {typeof(Byte?[]), SqlDbType.Binary},
        {typeof(Char[]), SqlDbType.Char},
        {typeof(Char?[]), SqlDbType.Char}
    };

    private static readonly ConcurrentDictionary<Type, SqlMetaData[]> Tipler =
        new ConcurrentDictionary<Type, SqlMetaData[]>();

    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> Props =
        new ConcurrentDictionary<Type, PropertyInfo[]>();

    public static SqlDbType GetSqlDbType(this Type systype)
    {
        var resulttype = SqlDbType.NVarChar;
        Types.TryGetValue(systype, out resulttype);
        return resulttype;
    }

    public static IEnumerable<SqlDataRecord> ConvertToTvp<T>(this IEnumerable<T> data) where T : new()
    {
        SqlMetaData[] gelenData;
        PropertyInfo[] properties;

        if (Tipler.ContainsKey(typeof(T)))
        {
            gelenData = Tipler[typeof(T)];
            if (Props.ContainsKey(typeof(T)))
            {
                properties = Props[typeof(T)];
            }
            else
            {
                properties = typeof(T).GetTypeInfo().GetProperties();
                Props.TryAdd(typeof(T), properties);
            }
        }
        else
        {
            var records = new List<SqlMetaData>();
            if (Props.ContainsKey(typeof(T)))
            {
                properties = Props[typeof(T)];
            }
            else
            {
                properties = typeof(T).GetTypeInfo().GetProperties();
                Props.TryAdd(typeof(T), properties);
            }

            foreach (var prop in properties)
            {
                var pt = prop.PropertyType;
                var sdbtyp = prop.PropertyType.GetSqlDbType();
                records.Add(pt.Name.Equals("String")
                    ? new SqlMetaData(prop.Name, sdbtyp, 4000)
                    : new SqlMetaData(prop.Name, sdbtyp));
            }

            gelenData = records.ToArray();
            Tipler.TryAdd(typeof(T), gelenData);
        }

        var ret = new SqlDataRecord(gelenData);
        foreach (var d in data)
        {
            for (var i = 0; i < properties.Length; i++)
            {
                var im = properties[i];
                ret.SetValue(i, properties[i].GetValue(d, null));
            }

            yield return ret;
        }
    }

    /// <summary>
    ///     This extension converts an enumerable set to a Dapper TVP
    /// </summary>
    /// <typeparam name="T">type of enumerbale</typeparam>
    /// <param name="enumerable">list of values</param>
    /// <param name="orderedColumnNames">if more than one column in a TVP, columns order must mtach order of columns in TVP</param>
    /// <returns>a custom query parameter</returns>
    public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(this IEnumerable<T> enumerable,
        IEnumerable<string> orderedColumnNames = null) where T : new()
    {
        var attrName = GetDbTypeName<T>();
        var table = enumerable.ConvertToTvp();
        return new Tvp(table, attrName);
    }

    public static string GetDbTypeName<T>()
    {
        var dnAttribute = typeof(T).GetTypeInfo().GetCustomAttributes(
            typeof(DbTypeName), true
        ).FirstOrDefault() as DbTypeName;
        return dnAttribute?.Name;
    }
}