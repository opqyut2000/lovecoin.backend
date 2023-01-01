using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbShoppingcart
{
    public string UserId { get; set; } = null!;

    public long ProductionId { get; set; }
}
