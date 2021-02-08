using System;
using System.Collections.Generic;

#nullable disable

namespace ApiNet5.DataAccess
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceProductItems = new HashSet<InvoiceProductItem>();
        }

        public Guid Id { get; set; }
        public string Emisor { get; set; }
        public Guid ClientId { get; set; }
        public long Number { get; set; }
        public string Series { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Total { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<InvoiceProductItem> InvoiceProductItems { get; set; }
    }
}
