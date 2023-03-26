using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace Demo.Dtos;

public class PersonalAddUpdateDto
{
    [JsonProperty("personalId", Required = Required.Always)]
    [Required]
    public long PersonalId { get; set; }
    [JsonProperty("personalName", Required = Required.Always)]
    [Required]
    [StringLength(64)]
    public string PersonalName { get; set; }
    [JsonProperty("personalSurname", Required = Required.Always)]
    [Required]
    [StringLength(64)]
    public string PersonalSurname { get; set; }
}