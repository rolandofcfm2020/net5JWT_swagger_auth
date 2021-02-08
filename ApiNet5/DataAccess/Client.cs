using System;
using System.Collections.Generic;

#nullable disable

namespace ApiNet5.DataAccess
{
    public partial class Client
    {
        public Client()
        {
            Invoices = new HashSet<Invoice>();
        }

        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
