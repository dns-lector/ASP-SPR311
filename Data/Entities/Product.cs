﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_SPR311.Data.Entities
{
    public class Product
    {
        public Guid    Id          { get; set; }
        public Guid    CategoryId  { get; set; }
        public String  Name        { get; set; } = null!;
        public String? Description { get; set; } = null!;
        public String? Slug        { get; set; } = null!;
        public String  ImagesCsv   { get; set; } = String.Empty;   // Comma Separated Values (CSV)

        [Column(TypeName = "decimal(5, 2)")]
        public double  Price       { get; set; }
        public int     Stock       { get; set; } = 1;

        public Category Category   { get; set; } = null!;
    }
}
