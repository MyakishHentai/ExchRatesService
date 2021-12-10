using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExchRatesFrontService.Models.Request
{
    public class DateRequest : BaseRequest
    {
        [Required]
        [JsonPropertyName("date")]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime Date { get; set; }
    }
}
