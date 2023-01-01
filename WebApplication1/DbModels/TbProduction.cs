using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbProduction
{
    public long ProductionId { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public long? ImageId { get; set; }

    public int Orderby { get; set; }

    public DateTime CreateTime { get; set; }

    public bool IsMarket { get; set; }

    public bool IsSold { get; set; }

    public DateTime? UpdateTime { get; set; }
}
