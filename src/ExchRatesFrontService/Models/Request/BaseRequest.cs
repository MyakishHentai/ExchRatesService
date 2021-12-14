using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ExchRatesFrontService.Models.Request
{
    public class BaseRequest
    {
        [Required] [FromQuery] public int RequestId { get; set; }
    }
}