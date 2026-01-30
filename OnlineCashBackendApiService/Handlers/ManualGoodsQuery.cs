using Dapper;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using OnlineCashTransportModels.Shared;

namespace OnlineCashBackendApiService.Handlers;

public static class ManualGoodsQuery
{
    public static async Task<IEnumerable<GoodsResponseTransportModel>> Handler(IDbContextFactory dbContextFactory)
    {
        using var db = dbContextFactory.CreateDbContext();
        var goods = await db.QueryAsync<Good>(@"SELECT g.*, gr.name AS GroupName
            FROM goods g INNER JOIN goodGroups gr ON g.goodGroupId=gr.id");
        var barcodes = await db.QueryAsync<GoodIdBarcode>("SELECT goodId, code FROM barcodes");
        foreach(var good in goods)
        {
            var codes = barcodes.Where(x => x.goodId == good.Id).Select(x=>x.code);
            good.Barcodes.AddRange(codes);
        };
        return goods.Select(x => new GoodsResponseTransportModel
        {
            Uuid = x.Uuid,
            Unit = x.Unit,
            SpecialType = x.SpecialType,
            VPackage = x.VPackage,
            Name = x.Name,
            Price = x.Price,
            IsDeleted = x.IsDeleted,
            Barcodes = x.Barcodes,
            IsPromotion2Plus1 = x.GroupName.ToLower()=="акция 2+1"
        });
    }

    private class Good
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public Units Unit { get; set; }
        public SpecialTypes SpecialType { get; set; }
        public double? VPackage { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> Barcodes { get; set; } = new();
    }


    private record GoodIdBarcode(int goodId, string code);
}
