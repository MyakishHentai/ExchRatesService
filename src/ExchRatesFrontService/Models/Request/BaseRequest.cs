using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesFrontService.Models.Request
{
    public class BaseRequest
    {
        [Required]
        [FromQuery]
        public int RequestId { get; set; } = 0;
    }
}
