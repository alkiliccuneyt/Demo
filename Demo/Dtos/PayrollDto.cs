using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Demo.Dtos;

public class PayrollQueryDto
{
    [JsonProperty("identityNumber", Required = Newtonsoft.Json.Required.AllowNull)]
    [Required(AllowEmptyStrings = true)]
    [DefaultValue(null)]
    public long? IdentityNumber { get; set; }
    [JsonProperty("payTypeId", Required = Newtonsoft.Json.Required.AllowNull)]
    [Required(AllowEmptyStrings = true)]
    [DefaultValue(null)]
    public int? PayTypeId { get; set; }
    [JsonProperty("period", Required = Newtonsoft.Json.Required.AllowNull)]
    [Required(AllowEmptyStrings = true)]
    [DefaultValue(null)]
    public int? Period { get; set; }
}

public class PayrollSummaryQueryDto
{
    [JsonProperty("identityNumber", Required = Newtonsoft.Json.Required.AllowNull)]
    [Required(AllowEmptyStrings = true)]
    [DefaultValue(null)]
    public long? IdentityNumber { get; set; }
    [JsonProperty("period", Required = Newtonsoft.Json.Required.AllowNull)]
    [Required(AllowEmptyStrings = true)]
    [DefaultValue(null)]
    public int? Period { get; set; }
}

public class PayrollExpandedQueryDto
{
    [JsonProperty("identityNumber", Required = Newtonsoft.Json.Required.AllowNull)]
    [Required(AllowEmptyStrings = true)]
    [DefaultValue(null)]
    public long? IdentityNumber { get; set; }
}

public class PayrollAddDto
{
    [JsonProperty("personalId", Required = Required.Always)]
    [Required]
    public long PersonalId { get; set; }
    [JsonProperty("payTypeId", Required = Required.Always)]
    [Required]
    public int PayTypeId { get; set; }
    [JsonProperty("period", Required = Required.Always)]
    [Required]
    public int Period { get; set; }
    [JsonProperty("quantity", Required = Required.Always)]
    [Required]
    public float Quantity { get; set; }
    [JsonProperty("amount", Required = Required.Always)]
    [Required]
    public float Amount { get; set; }
}

public class PayrollUpdateDto: PayrollAddDto
{
    [JsonProperty("payrollId", Required = Required.Always)]
    [Required]
    public int PayrollId { get; set; }
}