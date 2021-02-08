using System;
using System.Collections.Generic;

#nullable disable

namespace ApiNet5.DataAccess
{
    public partial class InvoiceProductItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }
    }
}
