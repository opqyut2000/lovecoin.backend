using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbAccount
{
    public string AccountId { get; set; } = null!;

    public string Password { get; set; } = null!;
}
