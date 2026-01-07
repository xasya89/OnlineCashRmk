using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class CreateArrivalTransportModel
{
    public Guid DocumentUuid { get; set; }
    public string Num { get; set; }
    public DateTime DateArrival { get; set; }
    public int SupplierId { get; set; }
    public IEnumerable<CreateArrivalPositionTransportModel> Positions { get; set; }
}
public class CreateArrivalPositionTransportModel
{
    public Guid Uuid { get; set; }
    public decimal PriceArrival { get; set; }
    public decimal PriceSell { get; set; }
    public decimal Count { get; set; }
    public string Nds { get; set; } = "Без ндс";
}