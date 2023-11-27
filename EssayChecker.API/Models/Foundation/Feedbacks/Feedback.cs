using System;
using System.Text.Json.Serialization;
using EssayChecker.API.Models.Foundation.Essays;

namespace EssayChecker.API.Models.Foundation.Feedbacks;

public class Feedback
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public float Mark { get; set; }
    public Guid EssayId { get; set; }
    [JsonIgnore]
    public Essay Essay { get; set; }
}