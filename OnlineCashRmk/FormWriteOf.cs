using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.Models;
using OnlineCashRmk.Services;
using OnlineCashTransportModels.Shared;
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
    public partial class FormWriteOf : Form
    {
        private readonly IDbContextFactory<DataContext> dbContextFactory;
        private readonly ILogger<FormWriteOf> logger;
        private readonly ISynchService synch;
        private readonly SearchGoodsControll _searchControll;

        SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) => {
            var activeform = Form.ActiveForm;
            if (activeform != null && nameof(FormWriteOf) == activeform.Name)
            {
                var port = (SerialPort)s;
                string code = port.ReadExisting();
                var form = activeform as FormWriteOf;
                using var db = form.dbContextFactory.CreateDbContext();
                var barcode = await db.BarCodes.Include(b => b.Good).Where(b => b.Code == code).FirstOrDefaultAsync();
                Action<Good> addGood = form.AddGood;
                if (barcode != null && barcode.Good.IsDeleted == false)
                    form.Invoke(addGood, barcode.Good);
            }
        });
        BarCodeScanner _barCodeScanner;

        ObservableCollection<WriteofGood> writeofGoods = new ObservableCollection<WriteofGood>();
        WriteofGood writeofGoodSelected { get; set; }

        public FormWriteOf(IDbContextFactory<DataContext> dbFactory, ILogger<FormWriteOf> logger, ISynchService synch, BarCodeScanner barCodeScanner)
        {
            dbContextFactory = dbFactory;
            this.logger = logger;
            this.synch = synch;

            InitializeComponent();

            _barCodeScanner = barCodeScanner;
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived += serialDataReceivedEventHandler;

            BindingSource binding = new BindingSource();
            binding.DataSource = writeofGoods;
            writeofGoods.CollectionChanged += (s, e) =>
            {
                binding.ResetBindings(false);
                toolStripLabудAmount.Text = Math.Round(writeofGoods.Sum(w => w.Sum), 2).ToString();
            };
            writeofGoodTable.AutoGenerateColumns = false;
            writeofGoodTable.DataSource = binding;
            ColumnGoodName.DataPropertyName = nameof(WriteofGood.GoodName);
            ColumnGoodUnit.DataPropertyName = nameof(WriteofGood.GoodUnit);
            ColumnCount.DataPropertyName = nameof(WriteofGood.Count);
            ColumnPrice.DataPropertyName = nameof(WriteofGood.Price);
            ColumnSum.DataPropertyName = nameof(WriteofGood.Sum);

            _searchControll = new SearchGoodsControll(dbFactory);
            _searchControll.ProductSelected += (g) =>
            {
                if (g == null) return;
                AddGood(g);
            };
            _searchControll.Dock = DockStyle.Fill;
            searchPanel.Controls.Add(_searchControll);
        }

        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && writeofGoodTable.SelectedRows.Count > 0)
            {
                int pos = writeofGoodTable.SelectedRows[0].Index;
                writeofGoods.RemoveAt(pos);
            }
            if (e.KeyCode == Keys.F4 & !e.Alt)
                _searchControll.Focus();
            //Очистить чек
            if (e.KeyCode == Keys.Escape)
                writeofGoods.Clear();
            if (e.KeyCode == Keys.Delete && writeofGoodSelected != null)
                writeofGoods.Remove(writeofGoodSelected);
        }

        void AddGood(Good good) => AddGood(good,1 );

        void AddGood(Good good, decimal count)
        {
            if (good != null)
            {
                FormEditCount frCountEdit = new FormEditCount();
                if (frCountEdit.ShowDialog() == DialogResult.OK)
                {
                    frCountEdit.textBoxCount.Text = frCountEdit.textBoxCount.Text.Replace(".", ",");
                    decimal.TryParse(frCountEdit.textBoxCount.Text, out count);
                }
                if (writeofGoods.Count(g => g.GoodId == good.Id) == 0)
                    writeofGoods.Add(new WriteofGood { GoodId = good.Id, Good = good, Count = count, Price = good.Price });
                else
                {
                    var checkgood = writeofGoods.FirstOrDefault(g => g.GoodId == good.Id);
                    if (good.Unit == Units.PCE || good.SpecialType == SpecialTypes.Beer)
                        checkgood.Count += count;
                    else
                        checkgood.Count = count;
                    ((BindingSource)writeofGoodTable.DataSource).ResetBindings(false);
                    toolStripLabудAmount.Text = Math.Round(writeofGoods.Sum(w => w.Sum), 2).ToString();
                }
            }
        }

        private void writeofGoodTable_SelectionChanged(object sender, EventArgs e)
        {
            if(writeofGoodTable.SelectedCells.Count>0)
                writeofGoodSelected = writeofGoods[writeofGoodTable.SelectedCells[0].RowIndex];
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using var db = dbContextFactory.CreateDbContext();
            Writeof writeof = new Writeof { DateCreate = DateTime.Now, SumAll = writeofGoods.Sum(w => w.Sum), Note = NoteInp.Text };
            db.Writeofs.Add(writeof);
            foreach (var wgood in writeofGoods)
                db.WriteofGoods.Add(new WriteofGood
                {
                    Writeof = writeof,
                    GoodId = wgood.GoodId,
                    Count = wgood.Count,
                    Price = wgood.Price
                });
            await db.SaveChangesAsync();
            synch.AppendDoc(new DocSynch { DocId = writeof.Id, Create = DateTime.Now, TypeDoc = TypeDocs.WriteOf });
            Close();
        }

        //Удаление позиции
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (writeofGoodSelected != null)
                writeofGoods.Remove(writeofGoodSelected);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormWriteOf_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived -= serialDataReceivedEventHandler;
        }
    }
}
