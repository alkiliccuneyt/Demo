using Demo.Data.Extensions;
// ReSharper disable All
#pragma warning disable CS8765
#pragma warning disable CS8618
namespace Demo.Data.Models;

[DbTypeName(Name = "constants.PayTypeEntity")]
public class PayTypeEntity
{
    public int Id { get; set; }
    public string Deffination { get; set; }
    public string ShortDeffination { get; set; }
    
    protected bool Equals(PayTypeEntity other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PayTypeEntity) obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(PayTypeEntity left, PayTypeEntity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PayTypeEntity left, PayTypeEntity right)
    {
        return !Equals(left, right);
    }
}