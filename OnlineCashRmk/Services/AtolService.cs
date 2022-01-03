using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atol.Drivers10.Fptr;
using Microsoft.Extensions.Configuration;

namespace OnlineCashRmk.Services
{
    public class AtolService : ICashRegisterService
    {
        string cashierName;
        string cashierInn;
        Fptr fptr;
        public AtolService(IConfiguration configuration)
        {
            cashierName = configuration.GetSection("cashierName").Value;
            cashierInn = configuration.GetSection("cashierInn").Value;
            fptr = new Fptr();
            fptr.open();
        }

        public void CloseShift()
        {

            fptr.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_CLOSE_SHIFT);
            fptr.report();
        }

        public void OpenShift()
        {
            fptr.openShift();
            fptr.checkDocumentClosed();
        }

        public void RegisterCheckReturn(CheckSell checkSell)
        {
            throw new NotImplementedException();
        }

        public void RegisterCheckSell(CheckSell checkSell)
        {
            //Открытие чека
            fptr.setParam(1021, cashierName);
            fptr.setParam(1203, cashierInn);
            fptr.operatorLogin();
            fptr.setParam(Constants.LIBFPTR_PARAM_RECEIPT_TYPE, Constants.LIBFPTR_RT_SELL);
            fptr.openReceipt();
            //Регистрация позиций
            decimal sumAll = 0;
            foreach (var check in checkSell.CheckGoods)
            {
                string goodname = check.Good.Name;
                decimal price = check.Cost;
                decimal count = (decimal)check.Count;
                decimal sum = Math.Ceiling(price * count);
                //sumAll += sum;
                if (sum - price * count > 0)
                    count = Math.Round(sum/price,3);
                fptr.setParam(Constants.LIBFPTR_PARAM_COMMODITY_NAME, goodname);
                fptr.setParam(Constants.LIBFPTR_PARAM_PRICE, (double)price);
                fptr.setParam(Constants.LIBFPTR_PARAM_QUANTITY, (double)count);
                fptr.setParam(Constants.LIBFPTR_PARAM_TAX_TYPE, Constants.LIBFPTR_TAX_NO);
                fptr.registration();
                sumAll += count * price;
            };
            //Итог чека
            fptr.setParam(Constants.LIBFPTR_PARAM_SUM, (double)sumAll);
            fptr.receiptTotal();
            //Оплата чека
            /*
            if (true)
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, Constants.LIBFPTR_PT_ELECTRONICALLY);
            else
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, Constants.LIBFPTR_PT_CASH);
            fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_SUM, (double) sumAll);
            fptr.payment();
            */
            foreach (var payment in checkSell.CheckPayments.OrderBy(p=>p.TypePayment))
            {
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_SUM, (double)payment.Sum);
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, payment.TypePayment==TypePayment.Electron? Constants.LIBFPTR_PT_ELECTRONICALLY : Constants.LIBFPTR_PT_CASH);


                fptr.payment();

            }
            fptr.closeReceipt();
        }
    }
}
