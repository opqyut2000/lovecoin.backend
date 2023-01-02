using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbPoductionImage
{
    /// <summary>
    /// 圖片ID
    /// </summary>
    public long ImageId { get; set; }

    /// <summary>
    /// 圖片位置
    /// </summary>
    public string ImageUrl { get; set; } = null!;

    /// <summary>
    /// 排序
    /// </summary>
    public int Orderby { get; set; }
}
