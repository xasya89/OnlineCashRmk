using OnlineCashTransportModels;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using OnlineCashTransportModels.SharedExtensions;

namespace OnlineCashBackendApiService.Handlers.ApplicationStatus;

public class ApplicationStatusService
{
    private readonly ConcurrentDictionary<string, ApplicationStatusTransportModel> _dict;
    public ApplicationStatusService()
    {
        _dict = new();
    }

    public void Add(string shopDbName, ApplicationStatusTransportModel model)
    {
        _dict.AddOrUpdate(shopDbName, model, (key, oldValue) => model);
    }

    public IEnumerable<(string shopDbName, string version, string? typeDoc, DateTime? lastDocSynch)> Get()
    {
        List<(string shopDbName, string version, string? typeDoc, DateTime? lastDocSynch)> result = new();
        foreach(var key in _dict.Keys)
        {
            var item = _dict[key];
            result.Add((key, item.Version, item.LastSynchTypeDoc?.GetDescription(), item.LastDocSynch));
        }
        return result;
    }

}
