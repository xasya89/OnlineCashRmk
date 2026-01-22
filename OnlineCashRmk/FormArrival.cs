using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.DataModels;
using OnlineCashRmk.Models;
using OnlineCashRmk.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCashRmk
{
    public partial class FormArrival : Form
    {
        ObservableCollection<Supplier> Suppliers { get; set; } = new ObservableCollection<Supplier>();
        ObservableCollection<ArrivalPositionDataModel> ArrivalPositions { get; set; } = new ObservableCollection<ArrivalPositionDataModel>();
        ObservableCollection<Good> findGoods = new ObservableCollection<Good>();
        ISynchService synch;
        private readonly IDbContextFactory<DataContext> _dbFactory;
        IServiceProvider _provider;
        private List<Good> _goods {  get; set; }

        SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) =>
        {
            var activeform = Form.ActiveForm;
            if (activeform != null && nameof(FormArrival) == activeform.Name)
            {
                var port = (SerialPort)s;
                string code = port.ReadExisting();
                var form = activeform as FormArrival;
                var db = await form._dbFactory.CreateDbContextAsync();
                var barcode = await db.BarCodes.Include(b => b.Good)
                .Where(b => b.Code == code & !b.Good.IsDeleted)
                .AsNoTracking().FirstOrDefaultAsync();
                Action<Good, decimal> addGood = form.AddGood;
                if (barcode != null && barcode.Good.IsDeleted == false)
                    await Task.Run(() => form.Invoke(addGood, barcode.Good, 1M));
            }
        });
        BarCodeScanner _barCodeScanner;

        public FormArrival(ISynchService synchService, ILogger<FormArrival> logger,
            IDbContextFactory<DataContext> dbFactory, ISynchService synch, BarCodeScanner barCodeScanner,
            IServiceProvider provider)
        {
            this.synch = synch;
            _dbFactory = dbFactory;
            _provider = provider;
            InitializeComponent();
            CalcSumAll();

            _barCodeScanner = barCodeScanner;
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived += serialDataReceivedEventHandler;

            GetSuupliers();
            BindingSource positionBinding = new BindingSource();
            ArrivalPositions.CollectionChanged += (s, e) =>
            {
                positionBinding.ResetBindings(false);
                CalcSumAll();
            };
            positionBinding.DataSource = ArrivalPositions;
            dataGridViewPositions.AutoGenerateColumns = false;
            dataGridViewPositions.DataSource = positionBinding;
            Column_GoodName.DataPropertyName = nameof(ArrivalPositionDataModel.GoodName);
            Column_Unit.DataPropertyName = nameof(ArrivalPositionDataModel.UnitStr);
            Column_PriceArrival.DataPropertyName = nameof(ArrivalPositionDataModel.PriceArrival);
            Column_PriceSell.DataPropertyName = nameof(ArrivalPositionDataModel.PriceSell);
            Column_PricePercent.DataPropertyName = nameof(ArrivalPositionDataModel.PricePercent);
            Column_Count.DataPropertyName = nameof(ArrivalPositionDataModel.Count);
            Column_SumArrival.DataPropertyName = nameof(ArrivalPositionDataModel.SumArrival);
            Column_NdsPercent.DataPropertyName = nameof(ArrivalPositionDataModel.NdsPercent);
            Column_SumNds.DataPropertyName = nameof(ArrivalPositionDataModel.SumNdsStr);
            Column_SumSell.DataPropertyName = nameof(ArrivalPositionDataModel.SumSell);
            Column_ExpiresDate.DataPropertyName = nameof(ArrivalPositionDataModel.ExpiresDate);
            dataGridViewPositions.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex < 0) return;
                var position = ArrivalPositions[e.RowIndex];
                if (dataGridViewPositions.Columns[e.ColumnIndex].Name == nameof(Column_PricePercent))
                    if (position.PricePercentDecimal == 0 || position.PricePercentDecimal > 56)
                        e.CellStyle.BackColor = Color.LightGreen;
                    else
                        e.CellStyle.BackColor = Color.LightPink;
            };
            BindingSource binding = new BindingSource();
            findGoods.CollectionChanged += (sender, e) =>
            {
                binding.ResetBindings(false);
            };
            binding.DataSource = findGoods;
            findListBox.DataSource = binding;
            findListBox.DisplayMember = nameof(Good.Name);
        }

        private async void GetSuupliers()
        {
            using var db = _dbFactory.CreateDbContext();
            var suupliers = await db.Suppliers.OrderBy(x => x.Name)
                .AsNoTracking().ToListAsync();
            foreach (var supplier in suupliers)
                Suppliers.Add(supplier);
            SupplierComboBox.DataSource = Suppliers;
            SupplierComboBox.DisplayMember = nameof(Supplier.Name);
            SupplierComboBox.ValueMember = nameof(Supplier.Id);
        }

        private CancellationTokenSource _searchCts;
        private async void findTextBox_TextChanged(object sender, EventArgs e)
        {
            // Отменяем предыдущий запрос (если он ещё не завершился)
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();

            var currentText = findTextBox.Text.Trim();
            if (string.IsNullOrEmpty(currentText))
            {
                // Очистить таблицу или скрыть результаты
                findGoods.Clear();
                return;
            }

            try
            {
                // Ждём паузу ввода (debounce)
                await Task.Delay(400, _searchCts.Token); // 400 мс — хороший баланс
                findGoods.Clear();
                // Выполняем запрос к БД асинхронноif (findTextBox.Text != "")
                if (findTextBox.Text.Length >= 2)
                {
                    findGoods.Clear();
                    var db = _dbFactory.CreateDbContext();
                    var term = findTextBox.Text.ToLowerInvariant();

                    var goods = await db.Goods
                        .Where(g => g.NameLower.Contains(term) && !g.IsDeleted)
                        .OrderBy(g => g.Name)
                        .Take(20)
                        .AsNoTracking()
                        .ToListAsync();
                    foreach (var good in goods)
                        findGoods.Add(good);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
            
        }

        void AddGood(Good good, decimal count = 1)
        {
            if (good != null /*&& ArrivalPositions.FirstOrDefault(p => p.GoodId == good.Id) == null*/)
            {
                var newArrival = new ArrivalPositionDataModel { GoodId = good.Id, GoodName = good.Name, Unit = good.Unit, Count = count, PriceArrival=good.PriceArrival, PriceSell = good.Price };
                ArrivalPositions.Add(newArrival);
                newArrival.PropertyChanged += (s, e) => CalcSumAll();
                dataGridViewPositions.FirstDisplayedScrollingRowIndex = ArrivalPositions.Count - 1;
            }

        }

        private async void findTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (findTextBox.Text != "")
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        var good = (Good)findListBox.SelectedItem;
                            AddGood(good);
                        if(int.TryParse(findTextBox.Text, out var _))
                        {
                            using var db= _dbFactory.CreateDbContext();
                            var barcode = await db.BarCodes.Include(g => g.Good)
                                .Where(b => b.Code == findTextBox.Text & !b.Good.IsDeleted)
                                .AsNoTracking().FirstOrDefaultAsync();
                            AddGood(barcode?.Good);
                        }
                        findTextBox.Clear();
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
                /*
                findGoods.Clear();
                findTextBox.Text = "";
                */
            }
        }

        string barcodeScan = "";
        private async void FormArrival_KeyDown(object sender, KeyEventArgs e)
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
                    using var db = _dbFactory.CreateDbContext();
                    var barcode = await db.BarCodes
                        .Include(b => b.Good)
                        .Where(b => b.Code == barcodeScan & b.Good.IsDeleted == false)
                        .AsNoTracking().FirstOrDefaultAsync();
                    AddGood(barcode?.Good);
                    barcodeScan = "";
                }
                if (e.KeyCode == Keys.Delete)
                    if ((dataGridViewPositions.Focused & dataGridViewPositions.SelectedCells.Count > 0) && dataGridViewPositions.Columns[dataGridViewPositions.SelectedCells[0].ColumnIndex].Name == Column_ExpiresDate.Name)
                    {
                        var position = ArrivalPositions[dataGridViewPositions.SelectedCells[0].RowIndex];
                        position.ExpiresDate = null;
                        dtp.Visible = false;
                    }
                    else
                        button1_Click(null, null);
            }
            ;
            if (e.KeyCode == Keys.F4 & !e.Alt)
                if (!findTextBox.Focused)
                {
                    findTextBox.Focus();
                    findTextBox.BackColor = Color.LightBlue;
                }
                else
                {
                    dataGridViewPositions.Select();
                    findTextBox.BackColor = SystemColors.Window;
                }
        }

        private void SupplierComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewPositions.Select();
            findTextBox.BackColor = SystemColors.Window;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewPositions.SelectedCells.Count > 0)
            {
                int pos = dataGridViewPositions.SelectedCells[0].RowIndex;
                ArrivalPositions.RemoveAt(pos);
            }
            dataGridViewPositions.Select();
            findTextBox.BackColor = SystemColors.Window;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            var supplier = SupplierComboBox.SelectedItem as Supplier;
            if (arrivalNum.Text != "" && supplier != null)
            {
                using var db = _dbFactory.CreateDbContext();
                var arrival = new Arrival { Num = arrivalNum.Text, DateArrival = arrivalDate.Value, SupplierId = supplier.Id };
                db.Arrivals.Add(arrival);
                Revaluation revaluation = new Revaluation();
                foreach (var position in ArrivalPositions)
                {
                    var arrivalGood = new ArrivalGood { Arrival = arrival, GoodId = position.GoodId, Count = position.Count, PriceArrival = position.PriceArrival, PriceSell=position.PriceSell, Nds = position.NdsPercent, ExpiresDate = position.ExpiresDate };
                    db.ArrivalGoods.Add(arrivalGood);
                };
                //Сохраним закупочную цену
                var goodIdsWithPrice = ArrivalPositions.Select(x => new { GoodId = x.GoodId, PriceArrival = x.PriceArrival })
                    .ToDictionary(x=>x.GoodId);
                var ids = ArrivalPositions.Select(x => x.GoodId).ToArray();
                var goods = await db.Goods.Where(x => ids.Contains(x.Id))
                    .ToListAsync();
                foreach(var good in goods)
                    good.PriceArrival = goodIdsWithPrice[good.Id].PriceArrival;

                await db.SaveChangesAsync();
                var doc = new DocSynch { DocId = arrival.Id, Create = DateTime.Now, TypeDoc = TypeDocs.Arrival };
                db.DocSynches.Add(doc);
                await db.SaveChangesAsync();
                Close();
            }
        }

        private void FormArrival_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived -= serialDataReceivedEventHandler;
        }

        void CalcSumAll()
        {
            labelSumNds.Text = ArrivalPositions.Sum(a => a.SumNds).ToSellFormat();
            labelSumArrival.Text = ArrivalPositions.Sum(a => a.SumArrival).ToSellFormat();
            labelSumSell.Text = ArrivalPositions.Sum(a => a.SumSell).ToSellFormat();
        }

        private DateTimePicker dtp { get; set; }
        private void dataGridViewPositions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewPositions.Columns[e.ColumnIndex].Name == Column_ExpiresDate.Name)
            {
                // initialize DateTimePicker
                dtp = new DateTimePicker();
                dtp.Format = DateTimePickerFormat.Short;
                dtp.Visible = true;
                var position = ArrivalPositions[e.RowIndex];
                dtp.Value = position.ExpiresDate == null ? DateTime.Now : (DateTime)position.ExpiresDate;
                //dtp.Value = DateTime.Parse(dataGridViewPositions.CurrentCell?.Value?.ToString());

                // set size and location
                var rect = dataGridViewPositions.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dtp.Size = new Size(rect.Width, rect.Height);
                dtp.Location = new Point(rect.X, rect.Y);

                // attach events
                dtp.CloseUp += new EventHandler(dtp_CloseUp);
                dtp.TextChanged += new EventHandler(dtp_OnTextChange);

                dataGridViewPositions.Controls.Add(dtp);
            }
        }
        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            dataGridViewPositions.CurrentCell.Value = dtp.Text.ToString();
        }

        // on close of cell, hide dtp
        void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fr = (FormNewGood)_provider.GetService(typeof(FormNewGood));
            AddGood(fr.Show());
            fr.Dispose();
        }

        /// <summary>
        /// Раскраска наценки в заивисимости от процента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewPositions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            RecolorRows();
        }

        // Перекраска ячеек, в завиисомти от наценки
        private void RecolorRows()
        {
            /*
            for (int i = 0; i < ArrivalPositions.Count; i++)
            {
                var position = ArrivalPositions[i];
                if (position.PricePercentDecimal == 0 || position.PricePercentDecimal > 56)
                    dataGridViewPositions.Rows[i].Cells["Column_PricePercent"].Style.BackColor = Color.LightGreen;
                else
                    dataGridViewPositions.Rows[i].Cells["Column_PricePercent"].Style.BackColor = Color.LightPink;
            }
            */
        }
    }
}
