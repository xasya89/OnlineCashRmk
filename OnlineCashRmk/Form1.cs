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
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.Services;

namespace OnlineCashRmk
{
    public partial class Form1 : Form
    {
        IServiceProvider serviceProvider;
        ISynchService synchService;
        DataContext db;
        ObservableCollection<CheckGoodModel> checkGoods = new ObservableCollection<CheckGoodModel>();
        int saleSelected = 1;
        Dictionary<int, List<CheckGoodModel>> saleCheckGoods = new Dictionary<int, List<CheckGoodModel>>()
        {
            {1,new List<CheckGoodModel>() },
            {2,new List<CheckGoodModel>() },
            {3,new List<CheckGoodModel>() }
        };
        ObservableCollection<Good> findGoods  = new ObservableCollection<Good>();
        Good goodPackcage = null;
        IConfiguration configuration;
        string serverName = "";
        int idShop = 1;
        string cashierName = "";
        string cashierInn = "";
        public Form1(IServiceProvider serviceProvider, ILogger<Form1> logger, DataContext db, ISynchService synchService)
        {
            this.serviceProvider = serviceProvider;
            this.db = db;
            this.synchService = synchService;
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
            configuration = builder.Build();
            serverName = configuration.GetSection("serverName").Value;
            idShop = Convert.ToInt32(configuration.GetSection("idShop").Value);
            cashierName = configuration.GetSection("cashierName").Value;
            cashierInn = configuration.GetSection("cashierInn").Value;
            Guid uuidGoodPackage = Guid.Empty;
            //DataContext db = new DataContext();
            Guid.TryParse(configuration.GetSection("BuyGoodPackage").Value, out uuidGoodPackage);
            if (uuidGoodPackage != Guid.Empty)
                goodPackcage = db.Goods.Where(g => g.Uuid == uuidGoodPackage).FirstOrDefault();
            InitializeComponent();
            if (goodPackcage == null)
                btnAddPackage.Visible = false;
            dataGridView1.Select();
            foreach (Panel p in new Panel[] { panel2, panel3 })
                foreach (Control c in p.Controls)
                    if (c is Button && c.Name != button4.Name)
                        c.Click += (sender, e) => { dataGridView1.Focus(); dataGridView1.Select(); };
            BindingSource binding = new BindingSource();
            findGoods.CollectionChanged += (sender, e) =>
            {
                binding.ResetBindings(false);
            };
            binding.DataSource = findGoods;
            findListBox.DataSource = binding;
            findListBox.DisplayMember = nameof(Good.Name);
            BindingSource bindingCheckGoods = new BindingSource();
            checkGoods.CollectionChanged += (s, e) =>
            {
                bindingCheckGoods.ResetBindings(false);
                labelSumAll.Text = checkGoods.Sum(c => c.Sum ).ToString();
            };
            bindingCheckGoods.DataSource = checkGoods;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = bindingCheckGoods;
            ColumnId.DataPropertyName = nameof(CheckGoodModel.GoodId);
            ColumnName.DataPropertyName = nameof(CheckGoodModel.GoodName);
            ColumnUnit.DataPropertyName = nameof(CheckGoodModel.GoodUnit);
            ColumnCount.DataPropertyName = nameof(CheckGoodModel.Count);
            ColumnPrice.DataPropertyName = nameof(CheckGoodModel.Cost);
            ColumnSum.DataPropertyName = nameof(CheckGoodModel.Sum);
        }

        private void FindGoods_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        Fptr fptr=new Fptr();

        private void button1_Click(object sender, EventArgs e)
        {
            if (db.Shifts.Where(s => s.Stop == null).Count() == 0)
            {
                fptr.openShift();
                fptr.checkDocumentClosed();
                var shift = new Shift { Uuid=Guid.NewGuid(), Start = DateTime.Now, ShopId=idShop };
                db.Shifts.Add(shift);
                db.SaveChanges();
                synchService.AppendDoc(new DocSynch { TypeDoc = TypeDocs.OpenShift, DocId = shift.Id });
                buttonShift.Text = "Закрыть смену";
                labelStatusShift.Text = "Открыта";
                labelStatusShift.BackColor = Color.LightGreen;
            }
            else
            {
                fptr.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_CLOSE_SHIFT);
                fptr.report();
                var shift = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
                shift.Stop = DateTime.Now;
                var checklist = db.CheckSells.Where(c=>c.ShiftId==shift.Id).ToList();
                shift.SumNoElectron = checklist.Where(s => !s.IsElectron).Sum(s => s.SumAll);
                shift.SumElectron = checklist.Where(s => s.IsElectron).Sum(s => s.SumAll);
                shift.SumSell = checklist.Sum(c => c.SumAll);
                shift.SumAll = shift.SumIncome + shift.SumSell - shift.SumOutcome - shift.SummReturn;
                if(db.Credits.Where(c=>c.ShiftId==shift.Id).Count()>0)
                    shift.SumCredit = db.Credits.Where(c=>c.ShiftId==shift.Id).ToList().Sum(c => c.SumCredit);
                db.SaveChanges();
                synchService.AppendDoc(new DocSynch { TypeDoc = TypeDocs.CloseShift, DocId = shift.Id });
                buttonShift.Text = "Открыть смену";
                labelStatusShift.Text = "Закрыта";
                labelStatusShift.BackColor = Color.LightPink;
                //Вывод итогов смены
                MessageBox.Show($"Итоги смены:\n ---------------- \nНаличные:\t{shift.SumNoElectron} \nБезналичные:\t{shift.SumElectron}  \nОстаток в кассе:\t{shift.SumAll}\n ---------------- \nВыданы кредиты:\t{shift.SumCredit}");
                //Синхронизация смен
                /*
                Task.Run(async () =>
                {
                    if (await ShiftSynchViewModel.SynchAsync())
                        buttonShift.BackColor = Color.LightGreen;
                    else
                        buttonShift.BackColor = Color.LightPink;
                });
                */
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fptr.showProperties(Constants.LIBFPTR_GUI_PARENT_NATIVE, this.Handle);
            fptr.open();
            var shiftOpen = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
            if (shiftOpen != null)
            {
                buttonShift.Text = "Закрыть смену";
                labelStatusShift.Text = "Открыта";
                labelStatusShift.BackColor = Color.LightGreen;
            }
            else
            {
                buttonShift.Text = "Открыть смену";
                labelStatusShift.Text = "Закрыта";
                labelStatusShift.BackColor = Color.LightPink;
            }
            LoadGoods();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            fptr.close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(db.Shifts.Count().ToString());
        }

        /// <summary>
        /// Обмен с сервером
        /// </summary>
        private void buttonSynch_Click(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
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
            if (!findTextBox.Focused)
            {
                switch (e.KeyCode)
                {
                    case Keys.NumPad0:
                    case Keys.D0:
                        barcodeScan = barcodeScan + "0";
                        break;
                    case Keys.NumPad1:
                    case Keys.D1:
                        barcodeScan = barcodeScan + "1";
                        break;
                    case Keys.NumPad2:
                    case Keys.D2:
                        barcodeScan = barcodeScan + "2";
                        break;
                    case Keys.NumPad3:
                    case Keys.D3:
                        barcodeScan = barcodeScan + "3";
                        break;
                    case Keys.NumPad4:
                    case Keys.D4:
                        barcodeScan = barcodeScan + "4";
                        break;
                    case Keys.NumPad5:
                    case Keys.D5:
                        barcodeScan = barcodeScan + "5";
                        break;
                    case Keys.NumPad6:
                    case Keys.D6:
                        barcodeScan = barcodeScan + "6";
                        break;
                    case Keys.NumPad7:
                    case Keys.D7:
                        barcodeScan = barcodeScan + "7";
                        break;
                    case Keys.NumPad8:
                    case Keys.D8:
                        barcodeScan = barcodeScan + "8";
                        break;
                    case Keys.NumPad9:
                    case Keys.D9:
                        barcodeScan = barcodeScan + "9";
                        break;
                }
                if (e.KeyCode == Keys.Enter)
                {
                    AddGood(db.BarCodes.Include(b=>b.Good).Where(b => b.Code == barcodeScan & b.Good.IsDeleted==false).FirstOrDefault()?.Good);
                    barcodeScan = "";
                }    
            };
            if (e.KeyCode == Keys.F2 & e.Control && dataGridView1.SelectedRows.Count > 0)
                EditGood(dataGridView1.SelectedRows[0]);
            if(e.KeyCode==Keys.Delete && dataGridView1.SelectedRows.Count>0)
            {
                int pos = dataGridView1.SelectedRows[0].Index;
                checkGoods.RemoveAt(pos);
            }
            if (e.KeyCode == Keys.F4 & !e.Alt)
                if (!findTextBox.Focused)
                {
                    findTextBox.Focus();
                    findTextBox.BackColor = Color.LightBlue;
                }
                else
                {
                    dataGridView1.Select();
                    findTextBox.BackColor = SystemColors.Window;
                }
            if(e.Control & e.KeyCode==Keys.F4)
                FindGood();
                //Наличная оплата
            if (e.KeyCode == Keys.F5)
                CheckPrint(false);
            if (e.KeyCode == Keys.F6)
                CheckPrint(true);
            //Пакет
            if (e.KeyCode == Keys.F8)
                btnAddPackage_Click(null, null);
            //Очистить чек
            if (e.KeyCode == Keys.Escape)
                checkGoods.Clear();
            if(e.KeyCode==Keys.F1)
                btnSale_Click(btnSale1, null);
            if (e.KeyCode == Keys.F2 & !e.Control)
                btnSale_Click(btnSale2, null); 
            if (e.KeyCode == Keys.F3)
                btnSale_Click(btnSale3, null);
        }

        void AddGood(Good good, double count=1)
        {
            if (good != null)
            {
                if (good.Unit != Units.PCE & good.SpecialType!=SpecialTypes.Beer)
                {
                    FormEditCount frCountEdit = new FormEditCount();
                    if (frCountEdit.ShowDialog() == DialogResult.OK)
                    {
                        frCountEdit.textBoxCount.Text = frCountEdit.textBoxCount.Text.Replace(".", ",");
                        double.TryParse(frCountEdit.textBoxCount.Text, out count);
                    }
                    else
                        return;
                }
                if(good.SpecialType==SpecialTypes.Beer)
                {
                    FormBuyBeer frBuy = new FormBuyBeer(good);
                    if (frBuy.ShowDialog() == DialogResult.OK)
                    {
                        var goodButtle = (Good)frBuy.BottleListBox.SelectedItem;
                        double.TryParse(frBuy.CountTextBox.Text, out count);
                        AddGood(goodButtle, count);
                        count = Math.Round((goodButtle.VPackage == null ? 0 : (double)goodButtle.VPackage) * count, 2);
                    }
                    else
                        return;
                }
                if (checkGoods.Count(g => g.GoodId == good.Id) == 0)
                    checkGoods.Add(new CheckGoodModel { GoodId = good.Id, Good = good, Count = count, Cost = good.Price });
                else
                {
                    var checkgood = checkGoods.FirstOrDefault(g => g.GoodId == good.Id);
                    if (good.Unit == Units.PCE || good.SpecialType==SpecialTypes.Beer)
                        checkgood.Count += count;
                    else
                        checkgood.Count = count;
                    ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
                    labelSumAll.Text = Math.Round(checkGoods.Sum(c => (decimal)c.Count * c.Cost)).ToString();
                }
            }
        }

        void EditGood(DataGridViewRow row)
        {
            int goodId = Convert.ToInt32(row.Cells[ColumnId.Name].Value);
            var checkGood = checkGoods.Where(c => c.GoodId == goodId).FirstOrDefault();
            if (checkGood != null)
            {
                FormEditCount fr = new FormEditCount();
                fr.labelGoodName.Text = checkGood.Good.Name;
                if (fr.ShowDialog() == DialogResult.OK)
                {
                    double count = 0;
                    double.TryParse(fr.textBoxCount.Text.Replace(".", ","), out count);
                    checkGood.Count = count;
                    ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
                    labelSumAll.Text =Math.Round(checkGoods.Sum(c => c.Sum)).ToString();
                }
            }
        }

        List<Good> GoodList = new List<Good>();
        void LoadGoods()
        {
            Task.Run(async () =>
            {
                GoodList = await db.Goods.Where(g=>g.IsDeleted==false).ToListAsync();
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

        private void button1_Click_1(object sender, EventArgs e)
        {
                CheckPrint(false);
        }

        public void CheckPrint(bool isElectron)
        {

            //При наличной оплате вызовем диалог расчета сдачи
            if (!isElectron)
            {
                decimal sumAll = checkGoods.Sum(c => c.Sum);
                FormPaymentNoElectron fr = new FormPaymentNoElectron(sumAll);
                if (fr.ShowDialog() != DialogResult.OK)
                    return;
            }

            var shift = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
            if (shift == null)
                MessageBox.Show("Смена не открыта", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            if (checkGoods.Count > 0)
            {
                //Открытие чека
                fptr.setParam(1021, cashierName);
                fptr.setParam(1203, cashierInn);
                fptr.operatorLogin();
                fptr.setParam(Constants.LIBFPTR_PARAM_RECEIPT_TYPE, Constants.LIBFPTR_RT_SELL);
                fptr.openReceipt();
                //Регистрация позиций
                foreach (var check in checkGoods)
                {
                    string goodname = check.Good.Name;
                    double price = (double)check.Cost;
                    double count = check.Count;
                    fptr.setParam(Constants.LIBFPTR_PARAM_COMMODITY_NAME, goodname);
                    fptr.setParam(Constants.LIBFPTR_PARAM_PRICE, price);
                    fptr.setParam(Constants.LIBFPTR_PARAM_QUANTITY, count);
                    fptr.setParam(Constants.LIBFPTR_PARAM_TAX_TYPE, Constants.LIBFPTR_TAX_NO);
                    fptr.registration();
                };
                double sumAll = (double)checkGoods.Sum(c => c.Sum);
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
                CheckSell checkSell = new CheckSell
                {
                    IsElectron = isElectron,
                    DateCreate = DateTime.Now,
                    Shift = shift,
                    Sum = (decimal)sumAll,
                    SumDiscont = 0,
                    SumAll = (decimal)sumAll
                };
                db.CheckSells.Add(checkSell);
                var chgoods = new List<CheckGood>();
                foreach (var chgood in checkGoods)
                {
                    var check = new CheckGood
                    {
                        CheckSell = checkSell,
                        Good = db.Goods.Where(g => g.Id == chgood.GoodId).FirstOrDefault(),
                        Count = chgood.Count,
                        Cost = chgood.Cost
                    };
                    db.CheckGoods.Add(check);
                };
                db.SaveChanges();
                db.DocSynches.Add(new DocSynch { DocId = checkSell.Id, TypeDoc = TypeDocs.Buy });
                db.SaveChanges();
                checkGoods.Clear();
                ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
            }
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
            checkGoods.Clear();
            ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
        }
        //Выдача кредита
        private void button6_Click(object sender, EventArgs e)
        {
            var shift = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
            if (shift == null)
                MessageBox.Show("Смена не открыта", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            if (checkGoods.Count > 0)
            {
                decimal sumpayment = checkGoods.Sum(c => c.Sum);
                FormCreditAdd fr = new FormCreditAdd();
                fr.SumPayment.Text = Math.Round(sumpayment).ToString();
                fr.SumCredit.Text = Math.Round(sumpayment).ToString();
                if (fr.ShowDialog() == DialogResult.OK)
                {
                    if (fr.SumCredit.Text == "")
                        fr.SumCredit.Text = fr.SumPayment.Text;
                    decimal sumcredit = 0;
                    decimal.TryParse(fr.SumCredit.Text, out sumcredit);
                    if (sumcredit == 0)
                        MessageBox.Show("Не заполнена сумма кредита", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                            try
                            {
                                var credit = new Credit
                                {
                                    Shift=shift,
                                    Creditor = fr.Creditor.Text,
                                    DateCreate = DateTime.Now,
                                    Sum = sumpayment,
                                    SumDiscont = 0,
                                    SumAll = sumpayment,
                                    SumCredit = sumcredit,
                                    isSynch = false
                                };
                                db.Add(credit);

                                if (sumpayment - sumcredit > 0)
                                {
                                    var creitpayment = new CreditPayment
                                    {
                                        Credit = credit,
                                        DatePayment = DateTime.Now,
                                        Sum = sumpayment - sumcredit
                                    };
                                    db.Add(creitpayment);
                                };
                            foreach (var ch in checkGoods)
                                db.CreditGoods.Add(new CreditGood { 
                                    Credit=credit,
                                    Good=ch.Good,
                                    Cost=ch.Cost,
                                    Count=ch.Count,
                                });
                                db.SaveChanges();
                            checkGoods.Clear();
                            }
                            catch (SystemException ex)
                            {
                            }
                }
            }
        }
        //поиск товара
        private void findTextBox_TextChanged(object sender, EventArgs e)
        {
            if (findTextBox.Text != "")
                if (findTextBox.Text.Length >= 2)
                {
                    findGoods.Clear();
                    var goods = db.Goods.OrderBy(g => g.Name).ToList();
                    //foreach (var good in goods.Where(g => g.Name.ToLower().IndexOf(findTextBox.Text.ToLower()) > -1).ToList())
                    foreach (var good in goods.Where(g => g.Name.ToLower().IndexOf(findTextBox.Text.ToLower()) > -1).Take(20).ToList())
                        findGoods.Add(good);
                    foreach (var barcode in db.BarCodes.Include(g => g.Good).Where(b => b.Code == findTextBox.Text).ToList())
                        findGoods.Add(barcode.Good);
                }
        }

        private void findTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (findTextBox.Text != "")
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        var good = (Good)findListBox.SelectedItem;
                        if (good != null)
                            AddGood(good);
                        findTextBox.Text = "";
                        findGoods.Clear();
                        break;
                    case Keys.Down:
                        int cursor = findListBox.SelectedIndex;
                        int itemcount = findListBox.Items.Count;
                        if (cursor + 1 < itemcount)
                            findListBox.SelectedIndex = cursor + 1;
                        else
                            findListBox.SelectedIndex = 0;
                        break;
                    case Keys.Up:
                        int cursor1 = findListBox.SelectedIndex;
                        int itemcount1 = findListBox.Items.Count;
                        if (cursor1 == 0)
                            findListBox.SelectedIndex = itemcount1 - 1;
                        else
                            findListBox.SelectedIndex = cursor1 - 1;
                        break;
                }
        }

        private void findListBox_DoubleClick(object sender, EventArgs e)
        {
            if (findListBox.SelectedItems.Count > 0)
            {
                var good = (Good)findListBox.SelectedItem;
                AddGood(good);
                findGoods.Clear();
                findTextBox.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Синхронизация смен
            Task.Run(async () =>
            {
                if (await ShiftSynchViewModel.SynchAsync())
                    buttonShift.BackColor = Color.LightGreen;
                else
                    buttonShift.BackColor = Color.LightPink;
            });
        }

        private void btnAddPackage_Click(object sender, EventArgs e)
        {
            if (goodPackcage != null)
                AddGood(goodPackcage);
        }

        private void buttonCheckHistory_Click(object sender, EventArgs e)
        {
            FormCheckHistory fr = new FormCheckHistory();
            fr.ShowDialog();
            fr.BringToFront();
            fr.Select();
        }

        private void списанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formWriteOf = serviceProvider.GetRequiredService<FormWriteOf>();
            formWriteOf.Show();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            foreach (var btn in new List<Button>() { btnSale1, btnSale2, btnSale3 })
                btn.BackColor = SystemColors.Control;
            (sender as Button).BackColor = Color.Green;
            int num = Convert.ToInt32((sender as Button).Name.Replace("btnSale", ""));
            saleCheckGoods[saleSelected].Clear();
            foreach (var ch in checkGoods)
                saleCheckGoods[saleSelected].Add(ch);
            saleSelected = num;
            checkGoods.Clear();
            foreach (var ch in saleCheckGoods[saleSelected])
                checkGoods.Add(ch);
        }

        private void приходыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formarrival = serviceProvider.GetRequiredService<FormArrival>();
            formarrival.Show();
        }
    }
}
