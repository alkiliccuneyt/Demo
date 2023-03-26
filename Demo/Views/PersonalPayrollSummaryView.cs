// ReSharper disable All

using Demo.Extensions;

#pragma warning disable CS8618
namespace Demo.Views;

public class PersonalPayrollSummaryView
{
    private string _personal { get; set; }
    public long PersonalId { get; set; }
    public string FullName
    {
        get
        {
            return GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(_personal));
        }
        set
        {
            this._personal = value;
        }
    }
    public string Period { get; set; }
    public float TotalPay { get; set; }
}