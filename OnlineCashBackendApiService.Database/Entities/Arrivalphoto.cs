using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Arrivalphoto
{
    public int Id { get; set; }

    public int? ArrivalId { get; set; }

    public DateTime DateUpload { get; set; }

    public string UserFullName { get; set; } = null!;

    public string PhotoFileName { get; set; } = null!;
}
