using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExpensesTracker.Api.Dtos
{
    /// <summary>
    /// Paging and HATEOAS
    /// </summary>
    public abstract class PageModel : LinkModel
    {
        public Page Page { get; set; }
    }

    /// <summary>
    /// Paging
    /// </summary>
    public class Page
    {
        public int Size { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int Number { get; set; }
    }

    /// <summary>
    /// HATEOAS
    /// </summary>
    public abstract class LinkModel
    {
        [XmlIgnore]
        public IEnumerable<Link> Links { get; set; }

        // since XmlDataContractSerializerFormatter does not work with property attributes
        // it is neccessary to use XmlSerializerFormatter which can not serialize interfaces
        // so this fake property is needed
        [XmlArray(ElementName = "Links")]
        public List<Link> LinksXml { 
            get 
            {
                return Links.ToList() ?? null;
            } 
        }
    }

    /// <summary>
    /// Link
    /// </summary>
    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }
}
