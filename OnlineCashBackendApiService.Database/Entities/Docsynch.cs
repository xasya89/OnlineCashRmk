using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Docsynch
{
    public int Id { get; set; }

    public DateTime DateAppend { get; set; }

    public Guid Uuid { get; set; }
}
