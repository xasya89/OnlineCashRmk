using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Launcher;

internal class UpdateConfig
{
    public string VersionUrl { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
    public string AppName { get; set; } = "OnlineCashRmk.exe";
}
