using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Helpers
{
    /// <summary>
    /// Class to manage sorting HTTP query
    /// </summary>
    public static class Sorting
    {
        /// <summary>
        /// Transform HTTP sort query string to LINQ dynamic order query string for given Entity
        /// </summary>
        /// <remarks>
        /// "name:desc,date" -> "Name, Date desc"
        /// </remarks>
        public static string GetDynamicFormat<T>(string queryString)
        {
            // split by sorting layer
            string[] queryStringParts = queryString.Split(',');
            SortOption<T>[] sortOptions = queryStringParts.Select(q => new SortOption<T>(q)).ToArray();

            // join formatted sorting layers
            var output = string.Join(", ", sortOptions.Select(o => o.ToString()).ToArray());
            return output;
        }
    }

    /// <summary>
    /// Sorting layer
    /// </summary>
    public class SortOption<T>
    {
        public string PropertyName { get; set; }
        public string Direction { get; set; } = "asc";
        
        public SortOption(string sortStringPart)
        {
            // sortStringPart e.g. "name:desc" / "name"
            string[] parts = sortStringPart.Split(':');
            // valid count of parts (1 - 2)
            if (parts.Length >= 1 && parts.Length <= 2)
            {
                SetPropertyName(parts[0]);
                if (parts.Length == 2)
                {
                    SetDirection(parts[1]);
                }
            }
            else
            {
                throw new AppLogicException("Invalid sort parametr format");
            }
        }

        private void SetPropertyName(string param)
        {
            // get properties of the given entity
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         // only properties with Sortable atribute
                                         .Where(pi => Attribute.IsDefined(pi, typeof(SortableAttribute)))
                                         .ToArray();

            // property that is currently sorted by
            var givenProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(param, StringComparison.InvariantCultureIgnoreCase));
            // property does not exist
            if (givenProperty == null)
            {
                throw new AppLogicException("Invalid sort parametr format");
            }

            PropertyName = param;
        }

        private void SetDirection(string param)
        {
            if (param == "asc") Direction = "";
            else if (param == "desc") Direction = "desc";
            else throw new AppLogicException("Invalid sort parametr format");
        }

        public override string ToString()
        {
            return $"{PropertyName} {Direction}";
        }
    }

    // Atribute to mark that property is available for sorting
    [AttributeUsage(AttributeTargets.Property)]
    public class SortableAttribute : Attribute
    {
    }
}
