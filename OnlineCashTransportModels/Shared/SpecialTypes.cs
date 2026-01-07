using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels.Shared;

public enum SpecialTypes
{
    [Display(Name = "")]
    None,
    [Display(Name = "Пиво")]
    Beer,
    [Display(Name = "Бутылка")]
    Bottle,
    [Display(Name = "Пакет")]
    Bag
}
