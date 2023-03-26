#pragma warning disable CS8618

// ReSharper disable All

namespace Demo.Data.Extensions;

[AttributeUsage(AttributeTargets.Class)]
public class DbTypeName : Attribute
{
    public string Name { get; set; }
}