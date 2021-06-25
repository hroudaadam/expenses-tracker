using ExpensesTracker.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExpensesTracker.Api.Dtos
{    
    /// <summary>
    /// Expense response with HATEOAS
    /// </summary>
    public class ExpenseRes : LinkModel
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Expense response with HATEOAS version 2
    /// </summary>
    [XmlRoot(ElementName = "ExpenseRes")]
    public class ExpenseResV2 : LinkModel
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public decimal Amount { get; set; }
        public int MagicNumber { get; set; } = new Random().Next(100);
    }

    /// <summary>
    /// Expense request
    /// </summary>
    public class ExpenseReq
    {
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }

    /// <summary>
    /// Expense list response with paging and HATEOAS
    /// </summary>
    public class ExpenseListRes : PageModel
    {
        [XmlIgnore]
        public IEnumerable<ExpenseRes> Expenses { get; set; }

        // since XmlDataContractSerializerFormatter does work with property attributes
        // it is neccessary to use XmlSerializerFormatter which can not serialize interfaces
        // so this fake property is needed
        [XmlArray(ElementName = "Expenses")]
        public List<ExpenseRes> ExpensesXml
        {
            get
            {
                return Expenses.ToList() ?? null;
            }
        }
    }

    /// <summary>
    /// Expense list query
    /// </summary>
        public class ExpenseListQuery
    {
        public int Size { get; set; } = 50;
        public int Page { get; set; } = 1;
        public string Sort { get; set; } = "TimeStamp"; // Name,Age:desc
        public decimal MinAmount { get; set; } = decimal.MinValue;
        public decimal MaxAmount { get; set; } = decimal.MaxValue;
    }
}
