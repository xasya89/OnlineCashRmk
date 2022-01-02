using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Services
{
    public interface ICashRegisterService
    {
        public void OpenShift();
        public void CloseShift();
        public void RegisterCheckSell(CheckSell checkSell);
        public void RegisterCheckReturn(CheckSell checkSell);
    }
}
