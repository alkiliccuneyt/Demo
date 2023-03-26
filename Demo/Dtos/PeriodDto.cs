using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace Demo.Dtos;

public class PeriodAddDto
{
    [JsonProperty("deffination", Required = Required.Always)]
    [Required]
    [StringLength(256)]
    public string Deffination { get; set; }
}

public class PeriodUpdateDto: PeriodAddDto
{
    [JsonProperty("periodId", Required = Required.Always)]
    [Required]
    public int PeriodId { get; set; }
}