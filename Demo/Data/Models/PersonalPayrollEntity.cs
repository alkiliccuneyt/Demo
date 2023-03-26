using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Demo.Data.Extensions;
// ReSharper disable All
#pragma warning disable CS8765
#pragma warning disable CS8618
namespace Demo.Data.Models;

[DbTypeName(Name = "finances.PersonalPayrollEntity")]
public class PersonalPayrollEntity
{
    public int Id { get; set; }
    public long PersonalId { get; set; }
    public int PayTypeId { get; set; }
    public int PeriodId { get; set; }
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18,3)")]
    public float Amount { get; set; }
    [Column(TypeName = "decimal(18,3)")]
    [DefaultValue(0)]
    public float Pay { get; set; }
    
    protected bool Equals(PersonalPayrollEntity other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PersonalPayrollEntity) obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(PersonalPayrollEntity left, PersonalPayrollEntity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PersonalPayrollEntity left, PersonalPayrollEntity right)
    {
        return !Equals(left, right);
    }
}