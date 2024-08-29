using System.Text.Json.Serialization;

namespace Hng.Application.Features.Profiles.Dtos
{
    public class ProfileDto
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
        [JsonPropertyName("user_name")]
        public string Username { get; set; }
        public string Pronoun { get; set; }
        [JsonPropertyName("job_title")]
        public string JobTitle { get; set; }
        [JsonPropertyName("bio")]
        public string Bio { get; set; }
        [JsonPropertyName("department")]
        public string Department { get; set; }
        [JsonPropertyName("facebook_link")]
        public string FacebookLink { get; set; }
        [JsonPropertyName("twitter_link")]
        public string TwitterLink { get; set; }
        [JsonPropertyName("linkedin_link")]
        public string LinkedinLink { get; set; }
    }
}