using Demo.Data.Configurations.Abstract;

#pragma warning disable CS8618

namespace Demo.Data.Configurations;

public class ConnectionConfiguration: IConnectionConfiguration
{
    public string DefaultConnection { get; set; }
    public string ApplicationName { get; set; }
}