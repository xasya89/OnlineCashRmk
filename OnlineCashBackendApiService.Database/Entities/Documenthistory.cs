using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Documenthistory
{
    public int Id { get; set; }

    public string DocumentId { get; set; } = null!;

    public string DocumentType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public bool Processed { get; set; }
}
