using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Telegramuser
{
    public int Id { get; set; }

    public long ChatId { get; set; }
}
