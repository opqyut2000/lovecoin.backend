using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbOrder
{
    public long OrderId { get; set; }

    public string Userid { get; set; } = null!;

    public DateTime CreateTime { get; set; }
}
