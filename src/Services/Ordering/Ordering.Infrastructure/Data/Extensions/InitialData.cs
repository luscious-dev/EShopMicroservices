using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<Customer> Customers => new List<Customer>()
        {
            Customer.Create(CustomerId.Of(new Guid("68f8f912-1cc5-40e6-a5f8-d80bd05da823")), "wale", "wale@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("475c665d-cedb-403b-868f-bd510ca1d6c3")), "kemi", "kemi@gmail.com")
        };
    }
}
