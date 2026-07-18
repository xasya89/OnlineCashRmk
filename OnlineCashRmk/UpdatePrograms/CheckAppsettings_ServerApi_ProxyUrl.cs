using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace OnlineCashRmk.UpdatePrograms;

internal static class CheckAppsettings_ServerApi_ProxyUrl
{
    public static void CheckAndUpdate()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        string targetServerApi = "https://erp.beermag31.ru/online-cash/v3/";

        bool isModified = false; // Флаг: меняли ли мы JSON в памяти

        // 2. Читаем и анализируем файл, если он существует
        if (File.Exists(configPath))
        {
            string jsonContent = File.ReadAllText(configPath);
            JsonNode? jsonNode = JsonNode.Parse(jsonContent);

            if (jsonNode != null)
            {
                // --- ПРОВЕРКА 1: ServerApi ---
                if (jsonNode["ConnectionStrings"]?["ServerApi"] is JsonValue serverApiNode)
                {
                    string currentValue = serverApiNode.GetValue<string>();
                    if (!string.Equals(currentValue, targetServerApi, StringComparison.OrdinalIgnoreCase))
                    {
                        jsonNode["ConnectionStrings"]["ServerApi"] = targetServerApi;
                        isModified = true;
                    }
                }

                // --- ПРОВЕРКА 2: ProxySettings ---
                if (jsonNode["ProxySettings"] == null)
                {
                    // Создаем секцию с нуля
                    jsonNode["ProxySettings"] = new JsonObject
                    {
                        ["Type"] = "none",
                        ["Url"] = ""
                    };
                    isModified = true;
                }

                if (isModified)
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    File.WriteAllText(configPath, jsonNode.ToJsonString(options));
                }
            }
        }
    }
}
