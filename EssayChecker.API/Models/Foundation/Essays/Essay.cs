using System;
using System.Text.Json.Serialization;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Models.Foundation.Users;

namespace EssayChecker.API.Models.Foundation.Essays;

public class Essay
{
    public Guid Id { get; set; }
    public string EssayType { get; set; }
    public string EssayContent { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    
    [JsonIgnore] 
    public Feedback Feedback { get; set; }
}