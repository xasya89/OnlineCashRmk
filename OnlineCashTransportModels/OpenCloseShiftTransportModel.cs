using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public record OpenShiftTransportModel(DateTime start, Guid uuid);

public record CloseShiftTransportModel(DateTime stop, Guid uuid);