using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbOrderProduction
{
    /// <summary>
    /// 訂單ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 產品ID
    /// </summary>
    public long ProductionId { get; set; }
}
