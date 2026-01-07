using OnlineCashTransportModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class CreateCheckTransportModel
{
    public DateTime DateCreate { get; set; }
    public TypeSell TypeSell { get; set; }
    public decimal SumDiscont { get; set; }
    public decimal SumElectron { get; set; }
    public decimal SumCash { get; set; }
    public IEnumerable<CreateCheckPositionTransportModel> Positions { get; set; }
}

public class CreateCheckPositionTransportModel
{
    public Guid Uuid { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
}