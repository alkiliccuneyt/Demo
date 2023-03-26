namespace Demo.Data.Configurations.Abstract;

public interface IConnectionConfiguration
{
    string DefaultConnection { get; set; }
    string ApplicationName { get; set; }
}