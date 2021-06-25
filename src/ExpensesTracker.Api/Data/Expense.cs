using ExpensesTracker.Api.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Data
{
    [Table("Expense")]
    public class Expense
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        [Sortable]
        public decimal Amount { get; set; }
        
        [Sortable]
        public DateTimeOffset TimeStamp { get; set; }
    }
}
