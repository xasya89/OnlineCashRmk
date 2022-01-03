using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public enum TypePayment
    {
        [Display(Name = "Электронные")]
        Electron=0,
        [Display(Name = "Наличные")]
        Cash=1
    }
}
