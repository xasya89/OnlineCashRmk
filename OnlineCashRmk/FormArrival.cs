using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.DataModels;
using OnlineCashRmk.Extenisions;
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
        private readonly ISynchService synch;
        private readonly IDbContextFactory<DataContext> _dbFactory;
        private readonly SearchGoodsControll _searchControll;
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

            _searchControll = new SearchGoodsControll(_dbFactory);
            _searchControll.ProductSelected += (g) =>
            {
                if (g == null) return;
                AddGood(g);
            };
            _searchControll.Dock = DockStyle.Fill;
            searchPanel.Controls.Add(_searchControll);
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
        private async void FormArrival_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                if ((dataGridViewPositions.Focused & dataGridViewPositions.SelectedCells.Count > 0) && dataGridViewPositions.Columns[dataGridViewPositions.SelectedCells[0].ColumnIndex].Name == Column_ExpiresDate.Name)
                {
                    var position = ArrivalPositions[dataGridViewPositions.SelectedCells[0].RowIndex];
                    position.ExpiresDate = null;
                    dtp.Visible = false;
                }
                else
                    button1_Click(null, null);

            if (e.KeyCode == Keys.F4 & !e.Alt)
                _searchControll.Focus();
        }

        private void SupplierComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewPositions.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewPositions.SelectedCells.Count > 0)
            {
                int pos = dataGridViewPositions.SelectedCells[0].RowIndex;
                ArrivalPositions.RemoveAt(pos);
            }
            dataGridViewPositions.Select();
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
                    .GroupBy(x=>x.GoodId)
                    .ToDictionary(x=>x.Key);
                var ids = ArrivalPositions.Select(x => x.GoodId).ToArray();
                var goods = await db.Goods.Where(x => ids.Contains(x.Id))
                    .ToListAsync();
                foreach(var good in goods)
                    good.PriceArrival = goodIdsWithPrice[good.Id].First().PriceArrival;

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
