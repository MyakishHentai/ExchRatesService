using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExchRatesFrontService.Models.Request
{
    public class LoginRequest : BaseRequest
    {
        [Required]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("userPassword")]
        public string Password { get; set; }
    }
}