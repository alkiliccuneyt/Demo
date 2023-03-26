// ReSharper disable All
#pragma warning disable CS8618
namespace Demo.Views;

public class GenericResponse
{
    public GenericResponse() { }
    
    public GenericResponse(object entity, bool success, string? message)
    {
        Entity = entity;
        Success = success;
        Message = string.IsNullOrEmpty(message) ? null : message.Trim();
    }
    
    public object Entity { get; set; }

    public bool Success { get; set; }

    public string? Message { get; set; }
}