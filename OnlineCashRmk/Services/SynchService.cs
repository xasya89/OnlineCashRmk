using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.Models;
using OnlineCashTransportModels;
using OnlineCashTransportModels.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OnlineCashRmk.Services;

public class SynchService : ISynchService
{
    private readonly IDbContextFactory<DataContext> dbContextFactory;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly HttpClient httpClient;
    ILogger<SynchService> logger_;
    IConfiguration configuration;
    string hostname;
    int shopId;
    private static List<DocSynch> DocSynches = new List<DocSynch>();

    public SynchService(IDbContextFactory<DataContext> dbFactory, ILogger<SynchService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        dbContextFactory = dbFactory;
        httpClient = httpClientFactory.CreateClient(Program.HttpClientName);
        logger_ = logger;
        this.configuration = configuration;
        this.httpClientFactory = httpClientFactory;
        hostname = configuration.GetSection("serverName").Value;
        shopId = Convert.ToInt32(configuration.GetSection("idShop").Value);
    }
    public void AppendDoc(DocSynch docSynch)
    {
        using var db = dbContextFactory.CreateDbContext();
        db.DocSynches.Add(docSynch);
        db.SaveChanges();
        DocSynches.Add(docSynch);
    }

    public async Task<List<Supplier>> SynchSuppliersAsync()
    {
        var client = httpClientFactory.CreateClient(Program.HttpClientName);
        var suppliers = await client.GetFromJsonAsync<IEnumerable<SupplierResponseTransportModel>>("/manuals/suppliers");
        using var db = dbContextFactory.CreateDbContext();
        foreach(var supplier in suppliers)
        {
            var supplierDb = db.Suppliers.Where(s => s.Id == supplier.Id).FirstOrDefault();
            if (supplierDb == null)
                db.Suppliers.Add(new Supplier { Id = supplier.Id, Name = supplier.Name, Inn = "", Kpp = "" });

        }
        db.SaveChanges();
        return await db.Suppliers.OrderBy(x=>x.Name).AsNoTracking().ToListAsync();
    }

    public async Task SynchGoods()
    {
        var response = await httpClient.GetFromJsonAsync<IEnumerable<GoodsResponseTransportModel>>($"manuals/goods");
        await _synchGoods(response.Select(x => new GoodSynchItem(
            x.Id,
            x.Uuid,
            x.Name,
            x.Unit,
            x.SpecialType,
            x.VPackage,
            x.Barcodes.ToArray(),
            x.Price,
            x.IsDeleted,
            x.IsPromotion2Plus1)));
    }

    private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(GoodsResponse));
    public async Task SynchGoodsFromFile()
    {
        string fileName = Path.Combine("input", "goods.xml");
        if (!Directory.Exists("input"))
        {
            MessageBox.Show("Не найден файл в каталоге", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Directory.CreateDirectory("input");
            return;
        }
        try
        {
            GoodsResponse result;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                result = (GoodsResponse)Serializer.Deserialize(fileStream);
            }

            var goods = result.Items.Select(x => new GoodSynchItem(
                x.Id,
                x.Uuid,
                x.Name,
                x.Unit,
                x.SpecialType,
                x.VPackage,
                x.Barcodes.ToArray(),
                x.Price,
                x.IsDeleted,
                x.IsPromotion2Plus1));
            await _synchGoods(goods);

            MessageBox.Show($"Успешно загружено {result.Items.Count} товаров!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show($"Ошибка формата XML:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла ошибка при чтении файла:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private record GoodSynchItem(int id, 
        Guid uuid, 
        string name, 
        Units unit, 
        SpecialTypes specialType,
        double? vPackage,
        string[] bacrodes,
        decimal price,
        bool isDeleted,
        bool isPromotion2Plus1
        );
    private async Task _synchGoods(IEnumerable<GoodSynchItem> goods)
    {
        using var db = dbContextFactory.CreateDbContext();
        foreach (var good in goods)
        {
            var goodDb = db.Goods.Include(g => g.BarCodes).Where(g => g.Uuid == good.uuid).FirstOrDefault();
            if (goodDb == null)
            {
                var newgood = new Good
                {
                    Uuid = good.uuid,
                    Name = good.name,
                    NameLower = good.name.Trim().ToLower(),
                    Article = null,
                    Unit = good.unit,
                    Price = good.price,
                    SpecialType = good.specialType,
                    VPackage = good.vPackage,
                    IsDeleted = good.isDeleted
                };
                db.Goods.Add(newgood);
                foreach (string barcode in good.bacrodes)
                    db.BarCodes.Add(new BarCode
                    {
                        Good = newgood,
                        Code = barcode
                    });
            }
            else
            {
                goodDb.Name = good.name;
                goodDb.NameLower = good.name.Trim().ToLower();
                goodDb.Unit = good.unit;
                goodDb.Price = good.price;
                goodDb.SpecialType = good.specialType;
                goodDb.VPackage = good.vPackage;
                goodDb.IsDeleted = good.isDeleted;
                //добавление новых или измененных штрих кодов
                foreach (string barcode in good.bacrodes)
                    if (goodDb.BarCodes.Count(b => b.Code == barcode) == 0)
                        db.BarCodes.Add(new BarCode { Good = goodDb, Code = barcode });
                //Удаление не зарегестрированных на сервере штрихкодов
                foreach (var barcodeDb in goodDb.BarCodes)
                    if (good.bacrodes.Count(b => b == barcodeDb.Code) == 0)
                        db.BarCodes.Remove(barcodeDb);
            }
        };
        await db.SaveChangesAsync();
    }
}

[XmlRoot("Goods")]
public class GoodsResponse
{
    [XmlElement("Good")]
    public List<GoodItem> Items { get; set; } = new List<GoodItem>();
}

public class GoodItem
{
    [XmlElement("Id")]
    public int Id { get; set; }

    [XmlElement("Uuid")]
    public Guid Uuid { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("Unit")]
    public Units Unit { get; set; }

    // Если у вас есть enum SpecialTypes, замените string на SpecialTypes
    [XmlElement("SpecialType")]
    public SpecialTypes SpecialType { get; set; }

    // double? (nullable) автоматически обработает <VPackage xsi:nil="true" />
    [XmlElement("VPackage")]
    public double? VPackage { get; set; }

    [XmlArray("Barcodes")]
    [XmlArrayItem("Barcode")]
    public List<string> Barcodes { get; set; } = new List<string>();

    [XmlElement("Price")]
    public decimal Price { get; set; }

    [XmlElement("IsDeleted")]
    public bool IsDeleted { get; set; }

    [XmlElement("IsPromotion2Plus1")]
    public bool IsPromotion2Plus1 { get; set; }
}