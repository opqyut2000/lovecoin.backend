using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbProduction
{
    /// <summary>
    /// 產品ID
    /// </summary>
    public long ProductionId { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 價格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 商品描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 圖片ID
    /// </summary>
    public long? ImageId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Orderby { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool IsMarket { get; set; }

    /// <summary>
    /// 是否售出
    /// </summary>
    public bool IsSold { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
