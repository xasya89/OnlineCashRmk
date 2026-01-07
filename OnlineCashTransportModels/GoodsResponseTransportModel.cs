using OnlineCashTransportModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class GoodsResponseTransportModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; }
    public Units Unit { get; set; }
    public SpecialTypes SpecialType { get; set; }
    public double? VPackage { get; set; }
    public IEnumerable<string> Barcodes { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }
}
