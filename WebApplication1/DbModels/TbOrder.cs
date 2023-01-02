using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbOrder
{
    /// <summary>
    /// 訂單ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 用戶ID
    /// </summary>
    public string Userid { get; set; } = null!;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }
}
