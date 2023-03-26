using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace Demo.Dtos;

public class PersonalPayTypeAddDto
{
    [JsonProperty("personalId", Required = Required.Always)]
    [Required]
    public long PersonalId { get; set; }
    [JsonProperty("payTypeId", Required = Required.Always)]
    [Required]
    public int PayTypeId { get; set; }
}
