using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbPoductionImage
{
    public long ImageId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int Orderby { get; set; }
}
