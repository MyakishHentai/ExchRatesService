﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExchRatesWCFService.Models.Entities
{
    public class CodesEntity
    {
        public CodesEntity()
        {
            Items = new HashSet<CurrencyCodesEntity>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CurrencyCodesEntity> Items { get; set; }
    }
}
