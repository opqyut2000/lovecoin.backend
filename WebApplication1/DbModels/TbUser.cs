using System;
using System.Collections.Generic;

namespace WebApplication1.DbModels;

public partial class TbUser
{
    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Username { get; set; }
}
