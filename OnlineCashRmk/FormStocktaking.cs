using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OnlineCashRmk.Models;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Services;
using System.IO.Ports;
using Microsoft.Extensions.DependencyInjection;
using OnlineCashTransportModels.Shared;

namespace OnlineCashRmk
{
    public partial class FormStocktaking : Form
    {
        private readonly IDbContextFactory<DataContext> _dbFactory;
        private int stockTackingId=0;
        ObservableCollection<GroupItem> groupItems = new();
        ObservableCollection<GoodItem> goodItems = new();
        private readonly SearchGoodsControll _searchControll;

        BindingSource bindingGroups;
        BindingSource bindingGoods;

        private record class GroupItem(int id, string name)
        {
            public decimal AmountFact { get; set; }
        }

        private record class GoodItem(int id, int goodId, string name, Units unit, decimal price)
        {
            public string UnitStr => unit.GetDisplay();
            public decimal Quantity { get; set; }
            public decimal Amount { get => Quantity * price; }
        }

        #region Сканер штрих кода
        SerialDataReceivedEventHandler serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(async (s, e) => {
            var activeform = Form.ActiveForm;
            if (activeform != null && nameof(FormStocktaking) == activeform.Name)
            {
                var port = (SerialPort)s;
                string code = port.ReadExisting();
                var form = activeform as FormStocktaking;
                using var db = form._dbFactory.CreateDbContext();
                var barcode = await db.BarCodes.Include(b => b.Good).Where(b => b.Code == code)
                .FirstOrDefaultAsync();
                Action<Good> addGood = form.AddGood;
                if (barcode != null && barcode.Good.IsDeleted == false)
                    form.Invoke(addGood, barcode.Good);
            }
        });
        BarCodeScanner _barCodeScanner;
        #endregion

        public FormStocktaking(IDbContextFactory<DataContext> dbFactory, BarCodeScanner barCodeScanner)
        {
            _dbFactory= dbFactory;

            InitializeComponent();
            _barCodeScanner = barCodeScanner;
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived += serialDataReceivedEventHandler;

            bindingGroups = new BindingSource();
            bindingGroups.DataSource = groupItems;
            groupItems.CollectionChanged += (s, e) => { bindingGroups.ResetBindings(false); };
            listBoxGroups.DataSource = bindingGroups;
            listBoxGroups.DisplayMember = nameof(GroupItem.name);

            bindingGoods = new BindingSource();
            bindingGoods.DataSource = goodItems;
            dataGridViewGoods.AutoGenerateColumns = false;
            dataGridViewGoods.DataSource = bindingGoods;
            ColumnGoodName.DataPropertyName = nameof(GoodItem.name);
            ColumnUnit.DataPropertyName = nameof(GoodItem.UnitStr);
            ColumnCountFact.DataPropertyName = nameof(GoodItem.Quantity);
            ColumnPrice.DataPropertyName = nameof(GoodItem.price);
            ColumnSum.DataPropertyName = nameof(GoodItem.Amount);

            listBoxGroups.SelectedIndexChanged += (s, e) =>
            {
                if (listBoxGroups.SelectedItem != null)
                {
                    var _groupItem = listBoxGroups.SelectedItem as GroupItem;
                    using var db = _dbFactory.CreateDbContext();
                    var items = db.StocktakingGoods.Include(x => x.Good)
                    .Where(x => x.StocktakingGroupId == _groupItem.id)
                    .Select(x => new GoodItem(x.Id, x.GoodId, x.Good.Name, x.Good.Unit, x.Price)
                    {
                        Quantity = x.CountFact,
                    });
                    goodItems.Clear();
                    foreach (var item in items) 
                        goodItems.Add(item);
                    bindingGoods.ResetBindings(false);
                }
            };
            listBoxGroups.SelectedItem = null;

            _searchControll = new SearchGoodsControll(dbFactory);
            _searchControll.ProductSelected += (g) =>
            {
                if (g == null) return;
                AddGood(g);
            };
            _searchControll.Dock = DockStyle.Fill;
            searchPanel.Controls.Add(_searchControll);

            LoadStockTacking();
        }

        private async void LoadStockTacking()
        {
            using var db = _dbFactory.CreateDbContext();
            var stocktaking = await db.Stocktakings.Include(s => s.StocktakingGroups)
                .Where(s => s.isSynch == false)
                .AsNoTracking().FirstOrDefaultAsync();
            if (stocktaking != null)
                if (MessageBox.Show($"Продолжить редактировать предыдущую инверторизацию от {stocktaking.Create.ToString("dd.MM.yy")}?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    foreach (var group in stocktaking.StocktakingGroups)
                    {
                        groupItems.Add(new GroupItem(group.Id, group.Name)
                        {
                            AmountFact=group.Sum ?? 0
                        });
                    }
                }
                else
                {
                    stocktaking.isSynch = true;
                    await db.Stocktakings.Where(x => x.Id == stocktaking.Id)
                        .ExecuteUpdateAsync(x => x.SetProperty(x => x.isSynch, true));
                    stocktaking = null;
                }
            if (stocktaking == null)
            {
                FormText fr = new FormText();
                fr.label1.Text = "Денег в кассе";
                if (fr.ShowDialog() == DialogResult.OK)
                {
                    stocktaking = new Stocktaking
                    {
                        Create = DateTime.Now,
                        CashMoney = fr.textBox1.Text.ToDecimal(),
                        isSynch = false,
                    };
                    db.Stocktakings.Add(stocktaking);
                    await db.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
            stockTackingId = stocktaking.Id;
            await RecalcAoumnt();
        }

        /// <summary>
        /// Добавить группу
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBoxGroupAdd.Text != "")
            {
                using var db = _dbFactory.CreateDbContext();
                var newgroup = new StocktakingGroup { StocktakingId=stockTackingId, Name = textBoxGroupAdd.Text };
                db.StocktakingGroups.Add(newgroup);
                await db.SaveChangesAsync();
                groupItems.Add(new (newgroup.Id, newgroup.Name));
                listBoxGroups.SelectedIndex = listBoxGroups.Items.Count - 1;
            };
        }

        public async void AddGood(Good good)
        {
            if (good == null || listBoxGroups.SelectedItem==null)
                return;
            var _groupItem = listBoxGroups.SelectedItem as GroupItem;
            FormEditCount fr = new FormEditCount();
            fr.labelGoodName.Text = good.Name;
            fr.priceLabel.Text = $"{good.Price} р. {good.UnitDescription}";
            if (fr.ShowDialog() == DialogResult.OK)
            {
                using var db = _dbFactory.CreateDbContext();
                var count = fr.textBoxCount.Text.ToDecimal();
                var item = new StocktakingGood { StocktakingGroupId = _groupItem.id, GoodId = good.Id, CountFact = count, Price=good.Price };
                db.StocktakingGoods.Add(item);
                await db.SaveChangesAsync();
                goodItems.Add(new GoodItem(item.Id, item.GoodId, good.Name, good.Unit, good.Price)
                {
                    Quantity=count,
                });

                bindingGoods.ResetBindings(false);
                dataGridViewGoods.FirstDisplayedScrollingRowIndex = goodItems.Count - 1;
                await RecalcAoumnt();
            }
        }

        private async void FormStocktaking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                _searchControll.Focus();
            if(e.KeyCode==Keys.F2 && dataGridViewGoods.SelectedCells.Count>0)
            {
                var item = goodItems[dataGridViewGoods.SelectedCells[0].RowIndex];
                FormEditCount fr = new FormEditCount();
                fr.labelGoodName.Text = item.name;
                if(fr.ShowDialog()==DialogResult.OK)
                {
                    using var db = _dbFactory.CreateDbContext();
                    await db.StocktakingGoods.Where(x => x.Id == item.id)
                        .ExecuteUpdateAsync(x => x.SetProperty(x => x.CountFact, fr.textBoxCount.Text.ToDecimal()));
                    item.Quantity = fr.textBoxCount.Text.ToDecimal();
                    bindingGoods.ResetBindings(false);
                    bindingGroups.ResetBindings(false);
                    await RecalcAoumnt();
                }
            }
            if (e.KeyCode == Keys.Delete && dataGridViewGoods.SelectedCells.Count > 0)
            {
                var item = goodItems[dataGridViewGoods.SelectedCells[0].RowIndex];
                if (MessageBox.Show($"Удалить {item.name}?","",MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    using var db = _dbFactory.CreateDbContext();
                    await db.StocktakingGoods.Where(x => x.Id == item.id).ExecuteDeleteAsync();
                    goodItems.Remove(item);
                    bindingGoods.ResetBindings(false);
                    bindingGroups.ResetBindings(false);
                    await RecalcAoumnt();
                }
            }
        }

        private void dataGridViewGoods_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            using var db = _dbFactory.CreateDbContext();
            await db.Stocktakings.Where(x => x.Id == stockTackingId)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.isSynch, true));
            db.DocSynches.Add(new DocSynch { DocId = stockTackingId, TypeDoc=TypeDocs.StopStocktacking });
            await db.SaveChangesAsync();
            Close();
        }

        private void FormStocktaking_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_barCodeScanner.port != null)
                _barCodeScanner.port.DataReceived -= serialDataReceivedEventHandler;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private async Task RecalcAoumnt()
        {
            using var db = _dbFactory.CreateDbContext();
            var goodPrices = await db.Goods.AsNoTracking().ToDictionaryAsync(x => x.Id);
            var amount = await db.StocktakingGroups
                .Where(x => x.StocktakingId == stockTackingId)
                .SelectMany(x => x.StocktakingGoods)
                .SumAsync(x => x.CountFact * x.Price);
            label3.Text= amount.ToString();
        }
    }
}
