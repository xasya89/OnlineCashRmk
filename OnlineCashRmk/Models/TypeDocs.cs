using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public enum TypeDocs
    {
        [Description("Открытие смены")]
        [Display(Name = "Открытие смены")]
        OpenShift,
        [Description("Покупка")]
        [Display(Name = "Покупка")]
        Buy,
        [Description("Возврат")]
        [Display(Name = "Возврат")]
        Return,
        [Description("Закрытие смены")]
        [Display(Name = "Закрытие смены")]
        CloseShift,
        [Description("Списание")]
        [Display(Name = "Списание")]
        WriteOf,
        [Description("Приход")]
        [Display(Name = "Приход")]
        Arrival,
        [Description("Инверторизация")]
        [Display(Name = "Инверторизация")]
        StockTaking,
        [Description("Деньги в кассе")]
        [Display(Name = "Деньги в кассе")]
        CashMoney,
        [Description("Новый товар")]
        [Display(Name = "Новый товар")]
        NewGoodFromCash
    }
}
