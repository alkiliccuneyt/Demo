using Demo.Data.Extensions;
// ReSharper disable All
#pragma warning disable CS8765
#pragma warning disable CS8618
namespace Demo.Data.Models;

[DbTypeName(Name = "constants.PeriodEntity")]
public class PeriodEntity
{
    public int Id { get; set; }
    public string Deffination { get; set; }
    
    protected bool Equals(PeriodEntity other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PeriodEntity) obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(PeriodEntity left, PeriodEntity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PeriodEntity left, PeriodEntity right)
    {
        return !Equals(left, right);
    }
}