﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class ProductTransaction
    {
        public int ProductTransactionId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int QuantityBefor { get; set; }
        [Required]
        public ProductTransactionType ActivityType { get; set; }
        [Required]
        public int QuantityAfter { get; set; }
        public string? ProductionNumber { get; set; }
        public string? SalesOrderNumber { get; set; }
        public double? UnitPrice { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public string DoneBy { get; set; }
        //Navigation Property
        public Product Product { get; set; }
    }
}
