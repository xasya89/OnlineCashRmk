using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atol.Drivers10.Fptr;
using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.ViewModels;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Tulpep.NotificationWindow;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;

namespace OnlineCashRmk
{
    public partial class Form1 : Form
    {
        IConfiguration configuration;
        string serverName = "";
        int idShop = 1;
        string cashierName = "";
        string cashierInn = "";
        public Form1()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
            configuration = builder.Build();
            serverName = configuration.GetSection("serverName").Value;
            idShop = Convert.ToInt32(configuration.GetSection("idShop").Value);
            cashierName = configuration.GetSection("cashierName").Value;
            cashierInn = configuration.GetSection("cashierInn").Value;
            InitializeComponent();
            dataGridView1.Select();
            foreach (Panel p in new Panel[] { panel1, panel2, panel3 })
                foreach (Control c in p.Controls)
                    if (c is Button && c.Name != button4.Name)
                        c.Click += (sender, e) => { dataGridView1.Focus(); dataGridView1.Select(); };
        }

        Fptr fptr=new Fptr();

        private void button1_Click(object sender, EventArgs e)
        {
            DataContext db = new DataContext();
            if (db.Shifts.Where(s => s.Stop == null).Count() == 0)
            {
                fptr.openShift();
                fptr.checkDocumentClosed();
                var shift = new Shift { Start = DateTime.Now, ShopId=idShop };
                db.Shifts.Add(shift);
                db.SaveChanges();
                buttonShift.Text = "Закрыть смену";
            }
            else
            {
                fptr.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_CLOSE_SHIFT);
                fptr.report();
                var shift = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
                shift.Stop = DateTime.Now;
                var checklist = db.CheckSells.Where(c=>c.ShiftId==shift.Id).ToList();
                shift.SumSell = checklist.Sum(c => c.SumAll);
                shift.SumAll = shift.SumIncome + shift.SumSell - shift.SumOutcome - shift.SummReturn;
                db.SaveChanges();
                buttonShift.Text = "Открыть смену";
                //Синхронизация смен
                Task.Run(async () =>
                {
                    if (await ShiftSynchViewModel.SynchAsync())
                        buttonShift.BackColor = Color.LightGreen;
                    else
                        buttonShift.BackColor = Color.LightPink;
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fptr.showProperties(Constants.LIBFPTR_GUI_PARENT_NATIVE, this.Handle);
            fptr.open();
            DataContext db = new DataContext();
            var shiftOpen = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
            if (shiftOpen != null)
                buttonShift.Text = "Закрыть смену";
            LoadGoods();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            fptr.close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataContext db = new DataContext();
            MessageBox.Show(db.Shifts.Count().ToString());
        }

        /// <summary>
        /// Обмен с сервером
        /// </summary>
        private void buttonSynch_Click(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                DataContext db = new DataContext();
                var str = await new HttpClient().GetAsync($"{serverName}/api/Goodssynchnew/" + 2).Result.Content.ReadAsStringAsync();
                List<Good> goods = JsonSerializer.Deserialize<List<Good>>(str);
                foreach (var good in goods)
                {
                    var goodDb = db.Goods.Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                    if (goodDb == null)
                    {
                        db.Goods.Add(new Good
                        {
                            Uuid = good.Uuid,
                            Name = good.Name,
                            Article = good.Name,
                            BarCode = good.BarCode,
                            Unit = good.Unit,
                            Price = good.Price
                        });
                    }
                    else
                    {
                        goodDb.Name = good.Name;
                        goodDb.BarCode = good.BarCode;
                        goodDb.Price = good.Price;
                    }
                };
                await db.SaveChangesAsync();
                MessageBox.Show("Обновление заврешено");
                LoadGoods();
            });
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                var cell = dataGridView1.SelectedCells[0];
                if(dataGridView1.Columns[cell.ColumnIndex].Name== "ColumnCount" && cell.Value!=null)
                {
                    cell.Value = cell.Value.ToString().Replace(",", ".");
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        string barcodeScan = "";
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad0: case Keys.D0:
                    barcodeScan = barcodeScan + "0";
                    break;
                case Keys.NumPad1: case Keys.D1:
                    barcodeScan = barcodeScan + "1";
                    break;
                case Keys.NumPad2: case Keys.D2:
                    barcodeScan = barcodeScan + "2";
                    break;
                case Keys.NumPad3: case Keys.D3:
                    barcodeScan = barcodeScan + "3";
                    break;
                case Keys.NumPad4: case Keys.D4:
                    barcodeScan = barcodeScan + "4";
                    break;
                case Keys.NumPad5: case Keys.D5:
                    barcodeScan = barcodeScan + "5";
                    break;
                case Keys.NumPad6: case Keys.D6:
                    barcodeScan = barcodeScan + "6";
                    break;
                case Keys.NumPad7: case Keys.D7:
                    barcodeScan = barcodeScan + "7";
                    break;
                case Keys.NumPad8: case Keys.D8:
                    barcodeScan = barcodeScan + "8";
                    break;
                case Keys.NumPad9: case Keys.D9:
                    barcodeScan = barcodeScan + "9";
                    break;
            }
            if (e.KeyCode == Keys.Enter)
                Task.Run(async () =>
                {
                    DataContext db = new DataContext();
                    var good = await db.Goods.Where(g => g.BarCode == barcodeScan).FirstOrDefaultAsync();
                    if (good == null)
                    {
                        var resp = await new HttpClient().GetAsync(@$"https://barcode-list.ru/barcode/RU/%D0%9F%D0%BE%D0%B8%D1%81%D0%BA.htm?barcode={barcodeScan}");
                        var str = await resp.Content.ReadAsStringAsync();
                        Regex regex = new Regex(@"<title>[\s\S]+?</title>");
                        MatchCollection matches = regex.Matches(str);
                        string goodName = "";
                        foreach (Match match in matches)
                            goodName = match.Value;
                        if (goodName.IndexOf("Поиск")==-1)
                        {
                            goodName = goodName.Replace("<title>", "").Replace("</title>", "");
                            regex = new Regex(@".+?(?=Штрих-код)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
                            var mc = regex.Matches(goodName);
                            var match = mc.Cast<Match>().FirstOrDefault();
                            goodName = match.Value.Replace(" - ", "");
                            FormPrice frPrice = new FormPrice();
                            if (frPrice.ShowDialog() == DialogResult.OK && frPrice.textBoxPrice.Text != "")
                            {
                                decimal price = 0;
                                frPrice.textBoxPrice.Text = frPrice.textBoxPrice.Text.Replace(".", ",");
                                decimal.TryParse(frPrice.textBoxPrice.Text, out price);
                                good = new Good
                                {
                                    Name = goodName,
                                    Article = "",
                                    Unit = Units.PCE,
                                    Price = price,
                                    BarCode = barcodeScan
                                };
                                db.Goods.Add(good);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                    barcodeScan = "";
                    if (good != null)
                        AddGood(good);
                });
            if (e.KeyCode == Keys.F2 && dataGridView1.SelectedRows.Count > 0)
                EditGood(dataGridView1.SelectedRows[0]);
            if(e.KeyCode==Keys.Delete && dataGridView1.SelectedRows.Count>0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                CalcSumAll();
            }
            if (e.KeyCode == Keys.F4)
                FindGood();
            //Наличная оплата
            if (e.KeyCode == Keys.F5)
                CheckPrint(false);
            if (e.KeyCode == Keys.F6)
                CheckPrint(true);
            //Очистить чек
            if (e.KeyCode == Keys.Escape)
                dataGridView1.Rows.Clear();
        }

        void AddGood(Good good)
        {

            bool flagAdd = true;
            double count = 1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToInt32(row.Cells["ColumnId"].Value) == good.Id)
                {
                    double countTmp = 0;
                    double.TryParse(row.Cells["ColumnCount"].Value.ToString(), out countTmp);
                    count = countTmp + 1;
                }
            if (good.Unit != Units.PCE)
            {
                FormEditCount frCountEdit = new FormEditCount();
                if (frCountEdit.ShowDialog() == DialogResult.OK)
                {
                    frCountEdit.textBoxCount.Text = frCountEdit.textBoxCount.Text.Replace(".", ",");
                    double.TryParse(frCountEdit.textBoxCount.Text, out count);
                }
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (Convert.ToInt32(row.Cells["ColumnId"].Value) == good.Id)
                {
                    row.Cells["ColumnCount"].Value = count;
                    decimal price = 0;
                    decimal.TryParse(row.Cells["ColumnPrice"].Value.ToString(), out price);
                    row.Cells["ColumnSum"].Value = count * (double)price;
                    flagAdd = false;
                }
            if (flagAdd)
            {
                Action dtadd = () =>
                {
                    dataGridView1.Rows.Add(
                        good.Id,
                        good.Name,
                        good.Unit.DisplayName(),
                        count,
                        good.Price,
                        Math.Round(count * (double)good.Price, 2)
                        ) ;
                };
                Invoke(dtadd);
            }
            CalcSumAll();
        }

        void EditGood(DataGridViewRow row)
        {
            string goodName = row.Cells["ColumnName"].Value.ToString();
            double count = Convert.ToDouble(row.Cells["ColumnCount"].Value);
            decimal price = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);
            FormEditCount fr = new FormEditCount();
            fr.labelGoodName.Text = goodName;
            if(fr.ShowDialog()==DialogResult.OK)
            {
                double.TryParse(fr.textBoxCount.Text.Replace(".",","), out count);
                row.Cells["ColumnCount"].Value = count;
                row.Cells["ColumnSum"].Value = (decimal)count * price;
                CalcSumAll();
            }
        }

        List<Good> GoodList = new List<Good>();
        void LoadGoods()
        {
            Task.Run(async () =>
            {
                DataContext db = new DataContext();
                GoodList = await db.Goods.ToListAsync();
            });
        }

        void FindGood()
        {
            FormFindGood fr = new FormFindGood(GoodList);
            if (fr.ShowDialog() == DialogResult.OK && fr.SelectedGood!=null)
            {
                AddGood(fr.SelectedGood);
            }
        }

        /// <summary>
        /// Пересчетаем итоговую сумму
        /// </summary>
        void CalcSumAll()
        {
            decimal sumAll = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                decimal priceRow = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);
                decimal countRow = Convert.ToDecimal(row.Cells["ColumnCount"].Value);
                sumAll += priceRow * countRow;
            }
            Action action = () =>
            {
                labelSumAll.Text = Math.Round(sumAll).ToString();
            };
            Invoke(action);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CheckPrint(false);
        }

        public void CheckPrint(bool isElectron)
        {
            Task.Run(async () =>
            {
                DataContext db = new DataContext();
                var shift = await db.Shifts.Where(s => s.Stop == null).FirstOrDefaultAsync();
                if (shift == null)
                    MessageBox.Show("Смена не открыта", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                if (dataGridView1.RowCount > 0)
                {
                    double sumAll = 0;
                    //Открытие чека
                    fptr.setParam(1021, cashierName);
                    fptr.setParam(1203, cashierInn);
                    fptr.operatorLogin();
                    fptr.setParam(Constants.LIBFPTR_PARAM_RECEIPT_TYPE, Constants.LIBFPTR_RT_SELL);
                    fptr.openReceipt();
                    //Регистрация позиций
                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        string goodname = row.Cells["ColumnName"].Value.ToString();
                        double price = Convert.ToDouble(row.Cells["ColumnPrice"].Value);
                        double count = Convert.ToDouble(row.Cells["ColumnCount"].Value);
                        fptr.setParam(Constants.LIBFPTR_PARAM_COMMODITY_NAME, goodname);
                        fptr.setParam(Constants.LIBFPTR_PARAM_PRICE, price);
                        fptr.setParam(Constants.LIBFPTR_PARAM_QUANTITY, count);
                        fptr.setParam(Constants.LIBFPTR_PARAM_TAX_TYPE, Constants.LIBFPTR_TAX_NO);
                        fptr.registration();
                        sumAll += price * count;
                    };
                    //Оплата чека
                    if (isElectron)
                        fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, Constants.LIBFPTR_PT_ELECTRONICALLY);
                    else
                        fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, Constants.LIBFPTR_PT_CASH);
                    fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_SUM, sumAll);
                        fptr.payment();
                    //Итог чека
                    fptr.setParam(Constants.LIBFPTR_PARAM_SUM, sumAll);
                    fptr.receiptTotal();
                    fptr.closeReceipt();

                    List<CheckGood> chgoods = new List<CheckGood>();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int goodId = Convert.ToInt32(row.Cells["ColumnId"].Value);
                        var good = await db.Goods.Where(g => g.Id == goodId).FirstOrDefaultAsync();
                        double count = Convert.ToDouble(row.Cells["ColumnCount"].Value);
                        decimal price = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);
                        CheckGood chgood = new CheckGood
                        {
                            Good = good,
                            Count = count,
                            Cost = price
                        };
                        chgoods.Add(chgood);
                    }
                    CheckSell checkSell = new CheckSell
                    {
                        IsElectron = isElectron,
                        DateCreate = DateTime.Now,
                        Shift = shift,
                        Sum = (decimal)chgoods.Sum(c => (double)c.Cost * c.Count),
                        SumDiscont = 0,
                        SumAll = (decimal)chgoods.Sum(c => (double)c.Cost * c.Count)
                    };
                    await db.CheckSells.AddAsync(checkSell);
                    foreach (var chgood in chgoods)
                        chgood.CheckSell = checkSell;
                    await db.CheckGoods.AddRangeAsync(chgoods);
                    await db.SaveChangesAsync();
                    dataGridView1.Rows.Clear();
                }
            });
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            CheckPrint(true);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
                EditGood(dataGridView1.SelectedRows[0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FindGood();
        }

        private void buttonMenu_Click(object sender, EventArgs e)
        {
            FormMenu fr = new FormMenu();
            fr.ShowDialog();
            LoadGoods();
        }
        //Отменить весь чек
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
        //Выдача кредита
        private void button6_Click(object sender, EventArgs e)
        {
            DataContext db = new DataContext();
            FormCreditAdd fr = new FormCreditAdd();
            List<CreditGood> creditgoods = new List<CreditGood>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int goodId = Convert.ToInt32(row.Cells["ColumnId"].Value);
                var good = db.Goods.Where(g => g.Id == goodId).FirstOrDefault();
                double count = Convert.ToDouble(row.Cells["ColumnCount"].Value);
                decimal price = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);
                CreditGood creditgood = new CreditGood
                {
                    Good = good,
                    Count = count,
                    Cost = price
                };
                creditgoods.Add(creditgood);
            };
            var sumpayment = creditgoods.Sum(ch => ch.Cost * (decimal)ch.Count);
            fr.SumPayment.Text = sumpayment.ToString();
            if (fr.ShowDialog() == DialogResult.OK)
            {
                if (fr.SumCredit.Text == "")
                    fr.SumCredit.Text = fr.SumPayment.Text;
                decimal sumcredit = 0;
                decimal.TryParse(fr.SumCredit.Text, out sumcredit);
                if (sumcredit == 0)
                    MessageBox.Show("Не заполнена сумма кредита", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    Task.Run(async () =>
                    {
                        try
                        {
                            var credit = new Credit
                            {
                                Creditor = fr.Creditor.Text,
                                DateCreate = DateTime.Now,
                                Sum = sumpayment,
                                SumDiscont = 0,
                                SumAll = sumpayment,
                                SumCredit = sumcredit,
                                isSynch = false
                            };
                            await db.AddAsync(credit);
                            
                            if (sumpayment - sumcredit > 0)
                            {
                                var creitpayment = new CreditPayment
                                {
                                    Credit = credit,
                                    DatePayment = DateTime.Now,
                                    Sum = sumpayment - sumcredit
                                };
                                await db.AddAsync(creitpayment);
                            };
                            foreach (var ch in creditgoods)
                                ch.Credit = credit;
                            await db.AddRangeAsync(creditgoods);
                            await db.SaveChangesAsync();
                        }
                        catch(SystemException ex)
                        {
                        }
                    });
            }
        }
    }
}
