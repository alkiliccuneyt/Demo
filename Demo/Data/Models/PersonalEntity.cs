using Demo.Data.Extensions;
// ReSharper disable All
#pragma warning disable CS8765
#pragma warning disable CS8618
namespace Demo.Data.Models;

[DbTypeName(Name = "finances.PersonalEntity")]
public class PersonalEntity
{
    public long IdentityNumber { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    protected bool Equals(PersonalEntity other)
    {
        return IdentityNumber == other.IdentityNumber;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PersonalEntity) obj);
    }

    public override int GetHashCode()
    {
        return IdentityNumber.GetHashCode();
    }

    public static bool operator ==(PersonalEntity left, PersonalEntity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PersonalEntity left, PersonalEntity right)
    {
        return !Equals(left, right);
    }
}