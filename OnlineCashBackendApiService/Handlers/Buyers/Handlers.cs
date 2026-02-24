using Microsoft.AspNetCore.Mvc;
using OnlineCashTransportModels;

namespace OnlineCashBackendApiService.Handlers.Buyers;

public static class GetBuyers
{
    public static async Task<IEnumerable<GetBuyerItemTransportModel>> Handler(DiscountService service) =>
        (await service.GetAllItemsAsync()).Select(x=>new GetBuyerItemTransportModel()
        {
            IsBlocked=x.isBlocked,
            Uuid=x.uuid,
            PhoneNumber = x.phoneNumber,
            SpecialDiscount = x.specilaPercent
        });
}

public static class UpdateSumDiscountBuyer
{
    public static async Task<IResult> Handler([FromBody] UpdateSumDiscountBuyerTransportModel body, DiscountService service)
    {
        await service.UpdateDiscountAsync(body.PhoneNumber, body.SumDiscount);
        return Results.Ok();
    }
}