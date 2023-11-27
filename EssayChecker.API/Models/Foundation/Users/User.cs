using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using EssayChecker.API.Models.Foundation.Essays;

namespace EssayChecker.API.Models.Foundation.Users;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TelephoneNumber { get; set; }
    [JsonIgnore]
    public ICollection<Essay> Essays { get; set; }
}