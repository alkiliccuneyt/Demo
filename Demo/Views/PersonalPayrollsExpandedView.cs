// ReSharper disable All

using System.Runtime.Serialization;
using Demo.Extensions;

#pragma warning disable CS8618
namespace Demo.Views;
[System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute("Payrolls", Namespace="", AnonymousType=true)]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute("Payrolls", Namespace="")]
public partial class Payrolls
{
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private System.Collections.ObjectModel.Collection<PayrollsPayroll> _payroll;
    
    [System.Xml.Serialization.XmlElementAttribute("Payroll", Namespace="")]
    public System.Collections.ObjectModel.Collection<PayrollsPayroll> Payroll
    {
        get
        {
            return this._payroll;
        }
        private set
        {
            this._payroll = value;
        }
    }
    
    public Payrolls()
    {
        this._payroll = new System.Collections.ObjectModel.Collection<PayrollsPayroll>();
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute("PayrollsPayroll", Namespace="", AnonymousType=true)]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PayrollsPayroll
{
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriod> _periods;
    
    [System.Xml.Serialization.XmlArrayAttribute("Periods", Namespace="")]
    [System.Xml.Serialization.XmlArrayItemAttribute("Period", Namespace="")]
    public System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriod> Periods
    {
        get
        {
            return this._periods;
        }
        private set
        {
            this._periods = value;
        }
    }
    
    public PayrollsPayroll()
    {
        this._periods = new System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriod>();
    }
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private string _Personal { get; set; }
    
    [System.Xml.Serialization.XmlAttributeAttribute("Personal", Namespace="")]
    public string Personal {
        get
        {
            return GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(this._Personal));
        }
        set
        {
            this._Personal = value;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute("IdentityNumber", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public long IdentityNumber { get; set; }
    
    [System.Xml.Serialization.XmlAttributeAttribute("TotalPay", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal TotalPay { get; set; }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute("PayrollsPayrollPeriods", Namespace="", AnonymousType=true)]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PayrollsPayrollPeriods
{
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriod> _period;
    
    [System.Xml.Serialization.XmlElementAttribute("Period", Namespace="")]
    public System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriod> Period
    {
        get
        {
            return this._period;
        }
        private set
        {
            this._period = value;
        }
    }
    
    public PayrollsPayrollPeriods()
    {
        this._period = new System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriod>();
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute("PayrollsPayrollPeriodsPeriod", Namespace="", AnonymousType=true)]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PayrollsPayrollPeriodsPeriod
{
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriodPaysPay> _pays;
    
    [System.Xml.Serialization.XmlArrayAttribute("Pays", Namespace="")]
    [System.Xml.Serialization.XmlArrayItemAttribute("Pay", Namespace="")]
    public System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriodPaysPay> Pays
    {
        get
        {
            return this._pays;
        }
        private set
        {
            this._pays = value;
        }
    }
    
    public PayrollsPayrollPeriodsPeriod()
    {
        this._pays = new System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriodPaysPay>();
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute("Id", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public int Id { get; set; }
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private string _Name { get; set; }
    
    [System.Xml.Serialization.XmlAttributeAttribute("Name", Namespace="")]
    public string Name {
        get
        {
            return GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(this._Name));
        }
        set
        {
            this._Name = value;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute("Pay", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal Pay { get; set; }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute("PayrollsPayrollPeriodsPeriodPays", Namespace="", AnonymousType=true)]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PayrollsPayrollPeriodsPeriodPays
{
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriodPaysPay> _pay;
    
    [System.Xml.Serialization.XmlElementAttribute("Pay", Namespace="")]
    public System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriodPaysPay> Pay
    {
        get
        {
            return this._pay;
        }
        private set
        {
            this._pay = value;
        }
    }
    
    public PayrollsPayrollPeriodsPeriodPays()
    {
        this._pay = new System.Collections.ObjectModel.Collection<PayrollsPayrollPeriodsPeriodPaysPay>();
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute("PayrollsPayrollPeriodsPeriodPaysPay", Namespace="", AnonymousType=true)]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PayrollsPayrollPeriodsPeriodPaysPay
{
    
    [System.Xml.Serialization.XmlAttributeAttribute("Id", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public int Id { get; set; }
    
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    private string _PayType { get; set; }
    
    [System.Xml.Serialization.XmlAttributeAttribute("PayType", Namespace="")]
    public string PayType {
        get
        {
            return GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(this._PayType));
        }
        set
        {
            this._PayType = value;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute("Amount", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal Amount { get; set; }
    
    [System.Xml.Serialization.XmlAttributeAttribute("Quantity", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Quantity { get; set; }
    
    [System.Xml.Serialization.XmlAttributeAttribute("Pay", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal Pay { get; set; }
}