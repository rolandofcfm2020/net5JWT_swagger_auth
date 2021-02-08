using System;
using System.Collections.Generic;

#nullable disable

namespace ApiNet5.DataAccess
{
    public partial class Product
    {
        public Product()
        {
            InvoiceProductItems = new HashSet<InvoiceProductItem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public decimal? Cost { get; set; }
        public string CurrencyCode { get; set; }

        public virtual ICollection<InvoiceProductItem> InvoiceProductItems { get; set; }
    }
}
