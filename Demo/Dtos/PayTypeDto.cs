using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace Demo.Dtos;

public class PayTypeAddDto
{
    [JsonProperty("deffination", Required = Required.Always)]
    [Required]
    [StringLength(256)]
    public string Deffination { get; set; }
    [JsonProperty("shortDeffination", Required = Required.Always)]
    [Required]
    [StringLength(64)]
    public string ShortDeffination { get; set; }
}

public class PayTypeUpdateDto: PayTypeAddDto
{
    [JsonProperty("payTypeId", Required = Required.Always)]
    [Required]
    public int PayTypeId { get; set; }
}