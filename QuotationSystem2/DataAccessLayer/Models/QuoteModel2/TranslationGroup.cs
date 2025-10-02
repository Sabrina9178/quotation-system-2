using System;
using System.Collections.Generic;

namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

public partial class TranslationGroup
{
    public int TranslationID { get; set; }

    public string? Dummy { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<PipeTopPricingMethod> PipeTopPricingMethods { get; set; } = new List<PipeTopPricingMethod>();

    public virtual ICollection<PipeTop> PipeTops { get; set; } = new List<PipeTop>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
}
