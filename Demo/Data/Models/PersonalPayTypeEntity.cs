using Demo.Data.Extensions;
// ReSharper disable All
#pragma warning disable CS8765
#pragma warning disable CS8618
namespace Demo.Data.Models;

[DbTypeName(Name = "finances.PersonalPayTypeEntity")]
public class PersonalPayTypeEntity
{
    public long PersonalId { get; set; }
    public int PayTypeId { get; set; }
    
    protected bool Equals(PersonalPayTypeEntity other)
    {
        return PersonalId == other.PersonalId && PayTypeId == PayTypeId;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PersonalPayTypeEntity) obj);
    }

    public override int GetHashCode()
    {
        return ($"{PersonalId}{PayTypeId}").GetHashCode();
    }

    public static bool operator ==(PersonalPayTypeEntity left, PersonalPayTypeEntity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PersonalPayTypeEntity left, PersonalPayTypeEntity right)
    {
        return !Equals(left, right);
    }
}