using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Customer_API.Models;

public partial class Customer
{
    [Key]
    public int Id { get; set; }
    public string? Cname { get; set; }
    public string? Email { get; set; }
    public string? Passwd { get; set; }
    public string? City { get; set; }
}
