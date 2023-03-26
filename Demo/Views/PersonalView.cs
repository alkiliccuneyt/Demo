// ReSharper disable All
#pragma warning disable CS8618
namespace Demo.Views;

public class PersonalView
{
    public long PersonalId { get; set; }
    public string PersonalName { get; set; }
    public string PersonalSurname { get; set; }
    public string FullName => $"{PersonalName} {PersonalSurname}";
}