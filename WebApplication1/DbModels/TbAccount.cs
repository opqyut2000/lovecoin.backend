using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbAccount
{
    /// <summary>
    /// 管理帳號
    /// </summary>
    public string AccountId { get; set; } = null!;

    public string Password { get; set; } = null!;
}
