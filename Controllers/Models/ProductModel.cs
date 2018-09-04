using System;
using System.Collections.Generic;

public partial class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string SellerInternalId { get; set; }
    public List<Review> Reviews {get; set;}
    
}