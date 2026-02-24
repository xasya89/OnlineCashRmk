using OnlineCashTransportModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class ApplicationStatusTransportModel
{
    public DateTime LastSendStatus { get; set; } = DateTime.Now;
    public string Version { get; set; }
    public TypeDocs? LastSynchTypeDoc { get; set; }
    public DateTime? LastDocSynch { get; set; }
}
