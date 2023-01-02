using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbShoppingcart
{
    /// <summary>
    /// 用戶名
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 產品ID
    /// </summary>
    public long ProductionId { get; set; }
}
