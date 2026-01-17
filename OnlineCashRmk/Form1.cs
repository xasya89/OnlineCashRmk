using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using OnlineCashRmk;
using OnlineCashRmk.DataModels;
using OnlineCashRmk.Models;
using OnlineCashRmk.Services;
using OnlineCashRmk.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Tulpep.NotificationWindow;
using OnlineCashTransportModels.Shared;
using OnlineCashTransportModels;

namespace OnlineCashRmk;

public partial class Form1 : Form
{
    IServiceProvider serviceProvider;
    private readonly HttpClient httpClient;
    ISynchService synchService;
    ICashRegisterService _cashService;
    private readonly IDbContextFactory<DataContext> dbContextFactory;
    private readonly IDocumentSenderService _documentSenderServer;
    //DataContext db;
    ILogger<Form1> _logger;
    ObservableCollection<CheckGoodModel> checkGoods = new ObservableCollection<CheckGoodModel>();
    Buyer SelectedBuyer = null;
    bool __isReturn = false;
    bool IsReturn
    {
        get => __isReturn;
        set
        {
            __isReturn = value;
            buttonReturn.BackColor = __isReturn == true ? Color.Red : SystemColors.Control;
        }
    }
    int saleSelected = 1;
    Dictionary<int, List<CheckGoodModel>> saleCheckGoods = new Dictionary<int, List<CheckGoodModel>>()
    {
        {1,new List<CheckGoodModel>() },
        {2,new List<CheckGoodModel>() },
        {3,new List<CheckGoodModel>() }
    };
    Dictionary<int, Buyer> saleBuyers = new Dictionary<int, Buyer>()
    {
        {1,null },{2,null },{3,null }
    };
    ObservableCollection<Good> findGoods = new ObservableCollection<Good>();
    Good goodPackcage = null;
    IConfiguration configuration;
    string serverName = "";
    int idShop = 1;
    string cashierName = "";
    string cashierInn = "";

    public delegate void SellNotifyHandler(List<CheckGood> Goods);
    //public event SellNotifyHandler SellNotify;

    SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) =>
    {
        var activeform = Form.ActiveForm;
        if (activeform != null && nameof(Form1) == activeform.Name)
        {
            var port = (SerialPort)s;
            string code = port.ReadExisting().Trim();
            var form = activeform as Form1;
            using var db = form.dbContextFactory.CreateDbContext();
            var barcode = await db.BarCodes.Include(b => b.Good).Where(b => b.Code == code)
            .AsNoTracking().FirstOrDefaultAsync();
            Action<Good, double> addGood = form.AddGood;
            if (barcode != null && barcode.Good.IsDeleted == false)
                Task.Run(() => form.Invoke(addGood, barcode.Good, 1));
        }
    });

    FormScreenForBuyer formScreenForBuyer;

    public Form1(IServiceProvider serviceProvider,
        IDocumentSenderService documentSenderService,
        ILogger<Form1> logger,
        IDbContextFactory<DataContext> _dbFactory,
        IHttpClientFactory httpClientFactory,
        ISynchService synchService,
        BarCodeScanner barCodeScanner,
        ICashRegisterService cashService,
        IHttpClientFactory clientFactory)
    {
        _documentSenderServer = documentSenderService;
        try
        {
            _cashService = cashService;
            this.serviceProvider = serviceProvider;
            //db = _dbFactory.CreateDbContext();

            dbContextFactory = _dbFactory;
            _logger = logger;
            httpClient = httpClientFactory.CreateClient(Program.HttpClientName);
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
                using (var db = dbContextFactory.CreateDbContext())
                    goodPackcage = db.Goods.Where(g => g.Uuid == uuidGoodPackage)
                            .AsNoTracking().FirstOrDefault();
            InitializeComponent();

            if (barCodeScanner.port != null)
            {
                barCodeScanner.port.DataReceived += serialDataReceivedEventHandler;
                if (barCodeScanner.port.IsOpen)
                    toolStripStatusLabelScannerIsOpen.BackColor = Color.LightGreen;
                else
                {
                    _logger.LogError("Barcode scanner port is not openned");
                    toolStripStatusLabelScannerIsOpen.BackColor = Color.Red;
                }

            }
            ;

            if (configuration.GetSection("buttonDiscountVisible").Value?.ToLower() == "true")
            {
                btnDiscount.Visible = true;
                ColumnDiscount.Visible = true;
            }
            else
            {
                btnDiscount.Visible = false;
                ColumnDiscount.Visible = false;
            }
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
                CalcSumBuy();
            };
            bindingCheckGoods.DataSource = checkGoods;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = bindingCheckGoods;
            ColumnId.DataPropertyName = nameof(CheckGoodModel.GoodId);
            ColumnName.DataPropertyName = nameof(CheckGoodModel.GoodName);
            ColumnUnit.DataPropertyName = nameof(CheckGoodModel.GoodUnit);
            ColumnCount.DataPropertyName = nameof(CheckGoodModel.Count);
            ColumnDiscount.DataPropertyName = nameof(CheckGoodModel.Discount);
            ColumnPrice.DataPropertyName = nameof(CheckGoodModel.Cost);
            ColumnSum.DataPropertyName = nameof(CheckGoodModel.Sum);


            //Открытие второго окна для отображения покупки покупателю
            if (configuration.GetSection("openScreenForBuyer").Value == true.ToString())
            {
                formScreenForBuyer = new FormScreenForBuyer(checkGoods);
                formScreenForBuyer.Show();
            }
            //Сначала загрузим список покупателей
            //GetBuyers();
            /*
            HubConnection hub = new HubConnectionBuilder().WithUrl("https://localhost:44394/discount_and_buyer_hub")
                .WithAutomaticReconnect()
                .Build();
            hub.On<Buyer>("buyer", async b =>
            {
                Action changeBuyer = () =>
                {
                    var buyer = db.Buyers.Where(x => x.Uuid == b.Uuid).FirstOrDefault();
                    if (buyer == null)
                    {
                        buyer = b;
                        buyer.Id = 0;
                        db.Buyers.Add(buyer);
                    }
                    buyer.DiscountSum = b.DiscountSum;
                    buyer.SpecialPercent = b.SpecialPercent;
                    buyer.TemporyPercent = b.TemporyPercent;
                    db.SaveChanges();
                };
                Invoke(changeBuyer);
            });
            BuyerHubConnected(hub);
            */
        }
        catch (Exception ex)
        {
            logger.LogError("Form 1 error init \n" + ex.StackTrace + "\n" + ex.Message);
            Close();
        }
    }

    private void GetBuyers()
    {
        try
        {
            var buyers = httpClient.GetFromJsonAsync<List<Buyer>>("/api/onlinecash/buyers").Result;
            using var db = dbContextFactory.CreateDbContext();
            var buyersDb = db.Buyers.AsNoTracking().ToList();
            foreach (var buyerDb in buyersDb)
            {
                var buyer = buyers.Where(x => x.Uuid == buyerDb.Uuid).FirstOrDefault();
                if (buyer == null)
                {
                    buyer.Id = 0;
                    db.Buyers.Add(buyer);
                }
                else
                    if (buyerDb.DiscountSum != buyer.DiscountSum || buyerDb.SpecialPercent != buyer.SpecialPercent || buyerDb.TemporyPercent != buyer.TemporyPercent)
                {
                    buyerDb.DiscountSum = buyer.DiscountSum;
                    buyerDb.SpecialPercent = buyer.SpecialPercent;
                    buyerDb.TemporyPercent = buyer.TemporyPercent;
                }
            }
            db.SaveChanges();
        }
        catch (SystemException ex)
        {

        }
    }

    private void FindGoods_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        using var db = dbContextFactory.CreateDbContext();
        if (db.Shifts.Where(s => s.Stop == null).Count() == 0)
        {
            var shift = new Shift { Uuid = Guid.NewGuid(), Start = DateTime.Now, ShopId = idShop };
            db.Shifts.Add(shift);
            db.SaveChanges();
            synchService.AppendDoc(new DocSynch { TypeDoc = TypeDocs.OpenShift, DocId = shift.Id });
            buttonShift.Text = "Закрыть смену";
            labelStatusShift.Text = "Открыта";
            labelStatusShift.BackColor = Color.LightGreen;
            _cashService.OpenShift();
        }
        else
        if (MessageBox.Show("Вы точно хотите закрыть смену?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            var shift = db.Shifts.Where(s => s.Stop == null)
                .Include(x => x.CheckSells).FirstOrDefault();
            shift.Stop = DateTime.Now;
            shift.SumNoElectron = shift.CheckSells.Where(x => !x.IsReturn).Sum(x => x.SumCash);
            shift.SumElectron = shift.CheckSells.Where(x => !x.IsReturn).Sum(x => x.SumElectron);
            shift.SumSell = shift.CheckSells.Where(x => !x.IsReturn).Sum(x => x.SumCash + x.SumElectron);
            shift.SummReturn = shift.CheckSells.Where(x => x.IsReturn).Sum(x => x.SumCash + x.SumElectron);
            shift.SumAll = shift.SumSell - shift.SummReturn;
            db.SaveChanges();
            synchService.AppendDoc(new DocSynch { TypeDoc = TypeDocs.CloseShift, DocId = shift.Id });
            buttonShift.Text = "Открыть смену";
            labelStatusShift.Text = "Закрыта";
            labelStatusShift.BackColor = Color.LightPink;
            _cashService.CloseShift();
            //Вывод итогов смены
            MessageBox.Show($"Итоги смены:\n ---------------- \nНаличные:\t{shift.SumNoElectron} \nБезналичные:\t{shift.SumElectron}  \nПродажи за сегодня:\t{shift.SumSell}\n ---------------- \nВозвраты:\t{shift.SummReturn}");
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Task.Delay(TimeSpan.FromSeconds(1.5)).Wait();
        using var db = dbContextFactory.CreateDbContext();
        var isShiftOpen = db.Shifts.Where(s => s.Stop == null).Any();
        if (isShiftOpen)
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

    private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        formScreenForBuyer?.Close();
        _cashService.Close();
        await Program.StopedHost();
    }

    /// <summary>
    /// Обмен с сервером
    /// </summary>
    private void buttonSynch_Click(object sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            using var db = dbContextFactory.CreateDbContext();
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
            }
            ;
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
            if (dataGridView1.Columns[cell.ColumnIndex].Name == "ColumnCount" && cell.Value != null)
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
                using var db = dbContextFactory.CreateDbContext();
                var good = db.BarCodes.Include(b => b.Good)
                    .Where(b => b.Code == barcodeScan & b.Good.IsDeleted == false)
                    .AsNoTracking().FirstOrDefault()?.Good;
                AddGood(good);
                barcodeScan = "";
            }
        }
        ;
        if (e.KeyCode == Keys.F2 & e.Control && dataGridView1.SelectedRows.Count > 0)
            EditGood(dataGridView1.SelectedRows[0]);
        if (e.KeyCode == Keys.Delete && dataGridView1.SelectedRows.Count > 0)
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
        //Наличная оплата
        if (e.KeyCode == Keys.F5)
            button1_Click_1(null, null);
        if (e.KeyCode == Keys.F6)
            button2_Click_1(null, null);
        if (e.KeyCode == Keys.F7)
            button7_Click(null, null);
        //Пакет
        if (e.KeyCode == Keys.F8)
            btnAddPackage_Click(null, null);
        //Очистить чек
        if (e.KeyCode == Keys.Escape)
            checkGoods.Clear();
        if (e.KeyCode == Keys.F1)
            btnSale_Click(btnSale1, null);
        if (e.KeyCode == Keys.F2 & !e.Control)
            btnSale_Click(btnSale2, null);
        if (e.KeyCode == Keys.F3)
            btnSale_Click(btnSale3, null);

        if (e.KeyCode == Keys.F9)
            //btnDiscount_Click(btnDiscount, null);
            buttonDiscountCard_Click(buttonDiscountCard, null);
    }

    void AddGood(Good good, double count = 1)
    {
        if (good != null && good.IsDeleted == false)
        {
            if (good.Unit != Units.PCE & good.SpecialType != SpecialTypes.Beer)
            {
                FormEditCount frCountEdit = new FormEditCount();
                if (good.Unit != Units.Litr)
                {
                    frCountEdit.textBoxCount.Text = "0,";
                    frCountEdit.textBoxCount.SelectionStart = "0,".Length;
                }
                if (frCountEdit.ShowDialog() == DialogResult.OK)
                    count = frCountEdit.textBoxCount.Text.ToDouble();
                else
                    return;
            }
            if (good.SpecialType == SpecialTypes.Beer)
            {
                FormBuyBeer frBuy = new FormBuyBeer(dbContextFactory, good);
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
                if (good.Unit == Units.PCE || good.SpecialType == SpecialTypes.Beer)
                    checkgood.Count += count;
                else
                    checkgood.Count = count;
                CalcSumBuy();
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
                CalcSumBuy();
            }
        }
    }

    private void CalcSumBuy()
    {
        ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
        decimal discount = GetDiscountSum();
        decimal sumBuy = Math.Ceiling(checkGoods.Sum(c => c.Sum) - discount);
        labelDiscountSum.Text = discount.ToSellFormat();
        labelSumAll.Text = sumBuy.ToSellFormat();
    }

    List<Good> GoodList = new List<Good>();
    void LoadGoods()
    {
        Task.Run(async () =>
        {
            using var db = dbContextFactory.CreateDbContext();
            GoodList = await db.Goods.Where(g => g.IsDeleted == false).AsNoTracking().ToListAsync();
        });
    }

    /// <summary>
    /// Оплата наличными
    /// </summary>
    private async void button1_Click_1(object sender, EventArgs e)
    {
        decimal discount = GetDiscountSum();
        decimal sumAll = Math.Ceiling(checkGoods.Sum(c => c.Sum) - discount);
        FormPaymentNoElectron fr = new FormPaymentNoElectron(sumAll);
        if (fr.ShowDialog() != DialogResult.OK)
            return;

        await CheckPrint(discount, 0, sumAll);
    }

    private decimal GetDiscountSum()
    {
        decimal discount = Math.Floor((SelectedBuyer?.TemporyPercent ?? 0) * checkGoods.Sum(c => c.Sum) / 100);
        if (discount > 0)
            return discount;
        discount = Math.Floor((SelectedBuyer?.SpecialPercent ?? 0) * checkGoods.Sum(c => c.Sum) / 100);
        if (discount > 0)
            return discount;
        var sumBuy = Math.Ceiling(checkGoods.Sum(c => c.Sum));
        decimal sumDiscount = 0;
        if ((SelectedBuyer?.DiscountSum ?? 0) > 0)
        {
            FormBuyerDiscount frDiscount = new FormBuyerDiscount((decimal)SelectedBuyer.DiscountSum, SelectedBuyer.Birthday == DateTime.Now.Date);
            if (frDiscount.ShowDialog() == DialogResult.OK)
                sumDiscount = (decimal)SelectedBuyer.DiscountSum;
        }
        if (sumDiscount > sumBuy)
            sumDiscount = sumBuy;
        return sumDiscount;
    }

    public async Task CheckPrint(decimal sumDiscount, decimal sumElectron, decimal sumCash)
    {
        /*
        using var db = dbContextFactory.CreateDbContext();
        var shift = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
        if (shift == null)
            MessageBox.Show("Смена не открыта", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
        else
        if (checkGoods.Count > 0)
        {
            var sumBuy = Math.Ceiling(checkGoods.Sum(c => c.Sum));
            CheckSell checkSell = new CheckSell
            {
                Buyer = SelectedBuyer,
                IsElectron=isElectron,
                DateCreate = DateTime.Now,
                TypeSell=IsReturn==false ? TypeSell.Sell : TypeSell.Return,
                Shift = shift,
                Sum = sumBuy,
                SumDiscont = sumDiscount,
                SumAll = sumBuy - sumDiscount
            };
            db.CheckSells.Add(checkSell);

            foreach (var payment in payments)
                db.CheckPayments.Add(new CheckPayment
                {
                    CheckSell=checkSell,
                    TypePayment=payment.TypePayment,
                    Income=payment.Income,
                    Sum=payment.Sum,
                    Retturn=payment.Income - payment.Sum
                });

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
                chgoods.Add(check);
            };
            if(SelectedBuyer!=null)
            {
                SelectedBuyer.DiscountSum -= sumDiscount;
            }
            db.SaveChanges();
            db.DocSynches.Add(new DocSynch { DocId = checkSell.Id, TypeDoc = TypeDocs.Buy });
            db.SaveChanges();
        */
        using var db = dbContextFactory.CreateDbContext();
        var error = await DbCashFormExtensions.CheckPrint(db, checkGoods.ToList(), null, 0, IsReturn, sumElectron, sumCash);
        if (error == "")
        {
            checkGoods.Clear();
            ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
            SelectedBuyer = null;
        }
        else
            MessageBox.Show(error, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Оплата безнал
    /// </summary>
    private async void button2_Click_1(object sender, EventArgs e)
    {
        decimal discount = GetDiscountSum();
        decimal sumAll = Math.Ceiling(checkGoods.Sum(c => c.Sum) - discount);
        FormPaymentElectron fr = new FormPaymentElectron(sumAll);
        if (fr.ShowDialog() == DialogResult.OK)
            await CheckPrint(discount, sumAll, 0);
    }

    private void dataGridView1_DoubleClick(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedRows.Count > 0)
            EditGood(dataGridView1.SelectedRows[0]);
    }

    private async void buttonMenu_Click(object sender, EventArgs e)
    {
        buttonMenu.BackColor = Color.Yellow;
        try
        {
            using var db = await dbContextFactory.CreateDbContextAsync();
            await DbCashFormExtensions.SynchGoods(db, httpClient);
            await db.SaveChangesAsync();
            GoodList = await db.Goods.Where(g => g.IsDeleted == false).AsNoTracking().ToListAsync();
            buttonMenu.BackColor = Color.LightGreen;
        }
        catch (SystemException ex)
        {
            buttonMenu.BackColor = Color.Red;
        }
    }
    //Отменить весь чек
    private void button5_Click(object sender, EventArgs e)
    {
        SelectedBuyer = null;
        checkGoods.Clear();
        ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
    }
    //поиск товара
    private void findTextBox_TextChanged(object sender, EventArgs e)
    {
        if (findTextBox.Text != "")
            if (findTextBox.Text.Length >= 2)
            {
                findGoods.Clear();
                using var db = dbContextFactory.CreateDbContext();
                var goods = db.Goods.OrderBy(g => g.Name).ToList();
                //foreach (var good in goods.Where(g => g.Name.ToLower().IndexOf(findTextBox.Text.ToLower()) > -1).ToList())
                foreach (var good in goods.Where(g => g.IsDeleted == false && g.Name.ToLower().IndexOf(findTextBox.Text.ToLower()) > -1).Take(20).ToList())
                    findGoods.Add(good);
                foreach (var barcode in db.BarCodes.Include(g => g.Good).Where(b => b.Code == findTextBox.Text).ToList())
                    if (barcode.Good != null && barcode.Good.IsDeleted == false)
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
        /*
        fr.BringToFront();
        fr.Select();
        */
        if (fr.checkGoodsReturn.Count > 0)
        {
            IsReturn = true;
            btnSale_Click(btnSale1, null);
            checkGoods.Clear();
            foreach (var ch in fr.checkGoodsReturn)
                checkGoods.Add(new CheckGoodModel
                {
                    GoodId = ch.GoodId,
                    Good = ch.Good,
                    Count = ch.Count,
                    Cost = ch.Cost
                });
        }
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
        saleBuyers[saleSelected] = SelectedBuyer;
        saleSelected = num;
        checkGoods.Clear();
        SelectedBuyer = saleBuyers[saleSelected];
        foreach (var ch in saleCheckGoods[saleSelected])
            checkGoods.Add(ch);
    }

    private void приходыToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formarrival = serviceProvider.GetRequiredService<FormArrival>();
        formarrival.Show();
        LoadGoods();
    }

    /// <summary>
    /// Комбинированная оплата
    /// </summary>
    private async void button7_Click(object sender, EventArgs e)
    {
        decimal discount = GetDiscountSum();
        decimal sumAll = Math.Ceiling(checkGoods.Sum(c => c.Sum) - discount);
        var payFor = serviceProvider.GetRequiredService<FormPaymentCombine>();
        var payments = payFor.Show(sumAll);
        if (payments.HasValue)
            await CheckPrint(discount, payments.Value.sumElectron, payments.Value.sumCash);
    }

    /// <summary>
    /// Скидка на позицию
    /// </summary>
    private void btnDiscount_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedRows.Count > 0)
        {
            int pos = dataGridView1.SelectedRows[0].Index;
            var chekGood = checkGoods[pos];
            FormText fr = new FormText();
            fr.label1.Text = "Скидка";
            if (fr.ShowDialog() == DialogResult.OK)
            {
                decimal discount = 0M;
                var cultureInfo = CultureInfo.InvariantCulture;
                NumberStyles styles = NumberStyles.Number;
                decimal.TryParse(fr.textBox1.Text, styles, cultureInfo, out discount);
                chekGood.Discount = discount;
                ((BindingSource)dataGridView1.DataSource).ResetBindings(false);
                labelSumAll.Text = Math.Round(checkGoods.Sum(c => (decimal)c.Count * c.Cost)).ToString();
            }
        }
    }

    // TODO: Проверить AsNoTracking при изменении цен в инверторизации
    private void инверторизацияToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var fr = serviceProvider.GetRequiredService<FormStocktaking>();
        try
        {
            fr.Show();
        }
        catch (Exception) { }
        ;
    }

    private void выдачаДенегToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        var fr = serviceProvider.GetRequiredService<FormCashMoney>();
        fr.Text = "Выдача";
        fr.Open(TypeCashMoneyOpertaion.Outcome);
    }

    private void внесениеДенегToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        var fr = serviceProvider.GetRequiredService<FormCashMoney>();
        fr.Text = "Внесение";
        fr.Open(TypeCashMoneyOpertaion.Income);
    }

    private void buttonReturn_Click(object sender, EventArgs e)
    {
        IsReturn = !IsReturn;
    }

    private void историяДокументовToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var formHistory = (FormHistory)serviceProvider.GetRequiredService(typeof(FormHistory));
        formHistory.Show();
    }

    /// <summary>
    /// Карта лояльности
    /// </summary>
    private void buttonDiscountCard_Click(object sender, EventArgs e)
    {
        FormBuyerRegistration fr = new FormBuyerRegistration();
        if (fr.ShowDialog() == DialogResult.OK)
        {
            using var db = dbContextFactory.CreateDbContext();
            var buyer = db.Buyers.Where(b => b.Phone == fr.phoneNumberTextBox.Text).FirstOrDefault();
            if (buyer == null)
            {
                buyer = new Buyer { Uuid = Guid.NewGuid(), Phone = fr.phoneNumberTextBox.Text };
                db.Buyers.Add(buyer);
                db.SaveChanges();
            }
            SelectedBuyer = buyer;
            CalcSumBuy();
        }
    }

    /// <summary>
    /// Загрузить товары
    /// </summary>
    private void загрузитьТоварыToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            using var db = dbContextFactory.CreateDbContext();
            try
            {
                var str = await new HttpClient().GetAsync($"{serverName}/api/Goodssynchnew/{idShop}").Result.Content.ReadAsStringAsync();
                List<GoodSynchDataModel> goods = JsonSerializer.Deserialize<List<GoodSynchDataModel>>(str);
                foreach (var good in goods)
                {
                    var goodDb = db.Goods.Include(g => g.BarCodes).Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                    if (goodDb == null)
                    {
                        var newgood = new Good
                        {
                            Uuid = good.Uuid,
                            Name = good.Name,
                            Article = good.Name,
                            Unit = good.Unit,
                            Price = good.Price,
                            SpecialType = good.SpecialType,
                            VPackage = good.VPackage,
                            IsDeleted = good.IsDeleted
                        };
                        db.Goods.Add(newgood);
                        //добавление штрих кодов
                        foreach (string barcode in good.Barcodes)
                            db.BarCodes.Add(new BarCode
                            {
                                Good = newgood,
                                Code = barcode
                            });
                    }
                    else
                    {
                        goodDb.Name = good.Name;
                        goodDb.Unit = good.Unit;
                        goodDb.Price = good.Price;
                        goodDb.SpecialType = good.SpecialType;
                        goodDb.VPackage = good.VPackage;
                        goodDb.IsDeleted = good.IsDeleted;
                        //добавление новых или измененных штрих кодов
                        foreach (string barcode in good.Barcodes)
                            if (goodDb.BarCodes.Count(b => b.Code == barcode) == 0)
                                db.BarCodes.Add(new BarCode { Good = goodDb, Code = barcode });
                        //Удаление не зарегестрированных на сервере штрихкодов
                        foreach (var barcodeDb in goodDb.BarCodes)
                            if (good.Barcodes.Count(b => b == barcodeDb.Code) == 0)
                                db.BarCodes.Remove(barcodeDb);
                    }
                }
                ;
                await db.SaveChangesAsync();
                MessageBox.Show("Загрузка успешно завершена", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Ошибка загрузки товаров", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        });
    }

    private async void отправитьДокументыНаСерверToolStripMenuItem_Click(object sender, EventArgs e)
    {
        отправитьДокументыНаСерверToolStripMenuItem.BackColor = Color.LightBlue;
        try
        {
            await _documentSenderServer.SendDocuments();
        }
        catch { };
        отправитьДокументыНаСерверToolStripMenuItem.BackColor = Color.LightGreen;
    }
}


static class DbCashFormExtensions
{
    public static async Task SynchGoods(DataContext context, HttpClient httpClient)
    {
        var goods = await httpClient.GetFromJsonAsync<IEnumerable<GoodsResponseTransportModel>>($"manuals/goods");
        foreach (var good in goods)
        {
            var goodDb = context.Goods.Include(g => g.BarCodes).Where(g => g.Uuid == good.Uuid).FirstOrDefault();
            if (goodDb == null)
            {
                var newgood = new Good
                {
                    Uuid = good.Uuid,
                    Name = good.Name,
                    NameLower=good.Name.Trim().ToLower(),
                    Article = good.Name,
                    Unit = good.Unit,
                    Price = good.Price,
                    SpecialType = good.SpecialType,
                    VPackage = good.VPackage,
                    IsDeleted = good.IsDeleted
                };
                context.Goods.Add(newgood);
                //добавление штрих кодов
                foreach (string barcode in good.Barcodes)
                    context.BarCodes.Add(new BarCode
                    {
                        Good = newgood,
                        Code = barcode
                    });
            }
            else
            {
                goodDb.Name = good.Name;
                goodDb.NameLower = good.Name.Trim().ToLower();
                goodDb.Unit = good.Unit;
                goodDb.Price = good.Price;
                goodDb.SpecialType = good.SpecialType;
                goodDb.VPackage = good.VPackage;
                goodDb.IsDeleted = good.IsDeleted;
                //добавление новых или измененных штрих кодов
                foreach (string barcode in good.Barcodes)
                    if (goodDb.BarCodes.Count(b => b.Code == barcode) == 0)
                        context.BarCodes.Add(new BarCode { Good = goodDb, Code = barcode });
                //Удаление не зарегестрированных на сервере штрихкодов
                foreach (var barcodeDb in goodDb.BarCodes)
                    if (good.Barcodes.Count(b => b == barcodeDb.Code) == 0)
                        context.BarCodes.Remove(barcodeDb);
            }
        };
        await context.SaveChangesAsync();
    }

    public static async Task<string> CheckPrint(DataContext db, IEnumerable<CheckGoodModel> checkGoods, Buyer? buyer, decimal sumDiscount, bool isReturn, decimal sumElectron, decimal sumCash)
    {
        var shift = db.Shifts.Where(s => s.Stop == null).FirstOrDefault();
        if (shift == null)
            return "Смена не открыта";
        if (checkGoods.Count() > 0)
        {
            var sumBuy = Math.Ceiling(checkGoods.Sum(c => c.Sum));
            CheckSell checkSell = new CheckSell
            {
                Buyer = buyer,
                IsElectron = sumElectron>0 & sumCash==0,
                DateCreate = DateTime.Now,
                TypeSell = isReturn == false ? TypeSell.Sell : TypeSell.Return,
                Shift = shift,
                Sum = sumBuy,
                SumDiscont = sumDiscount,
                SumElectron = sumElectron,
                SumCash = sumCash,
                SumAll = sumBuy - sumDiscount,
                CheckGoods = checkGoods.Select(c=>new CheckGood
                {
                    GoodId = c.GoodId,
                    Count = c.Count,
                    Cost = c.Cost
                }).ToList()
            };
            db.CheckSells.Add(checkSell);
            await db.SaveChangesAsync();
            db.DocSynches.Add(new DocSynch { DocId = checkSell.Id, TypeDoc = TypeDocs.Buy });
            await db.SaveChangesAsync();
        }
        return "";
    }
}