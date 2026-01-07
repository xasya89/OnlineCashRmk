using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class CreateWriteOfTransportModel
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public DateTime DateCreate { get; set; }
    public string Note { get; set; }
    public IEnumerable<CreateWriteOfPositionTransportModel> Positions { get; set; }
}

public class CreateWriteOfPositionTransportModel
{
    public Guid Uuid { get; set; }
    public decimal Price { get; set; }
    public decimal Count { get; set; }
}
