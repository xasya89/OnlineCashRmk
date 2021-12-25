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

namespace OnlineCashRmk
{
    public partial class FormStocktaking : Form
    {
        DataContext _db;
        ISynchService _synchService;
        Stocktaking stocktaking;
        ObservableCollection<StocktakingGroup> groups=new ObservableCollection<StocktakingGroup>();
        StocktakingGroup selectedGroup;
        ObservableCollection<Good> findGoods = new ObservableCollection<Good>();
        BindingSource bindingGroups;
        BindingSource bindingGoods;
        public FormStocktaking(DataContext db, ISynchService synchService)
        {
            _db = db;
            _synchService = synchService;
            stocktaking = db.Stocktakings.Include(s=>s.StocktakingGroups).ThenInclude(gr=>gr.StocktakingGoods).ThenInclude(g=>g.Good).Where(s => s.isSynch == false).FirstOrDefault();
            if(stocktaking!=null && MessageBox.Show("Продолжить редактировать предыдущую инверторизацию?","",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
            {
                foreach (var group in stocktaking.StocktakingGroups)
                {
                    var stGroup = new StocktakingGroup { Id = group.Id, Name = group.Name };
                    groups.Add(stGroup);
                    foreach (var good in group.StocktakingGoods)
                        stGroup.StocktakingGoods.Add(new StocktakingGood
                        {
                            Id = good.Id,
                            StocktakingGroupId = good.StocktakingGroupId,
                            GoodId = good.Id,
                            Good = good.Good,
                            CountFact = good.CountFact
                        });
                }    
                    
            }
            InitializeComponent();
            bindingGroups = new BindingSource();
            bindingGroups.DataSource = groups;
            groups.CollectionChanged += (s, e) => { bindingGroups.ResetBindings(false); };
            listBoxGroups.DataSource = bindingGroups;
            listBoxGroups.DisplayMember = nameof(StocktakingGroup.Name);

            bindingGoods = new BindingSource();
            bindingGoods.DataSource = selectedGroup?.StocktakingGoods;
            dataGridViewGoods.AutoGenerateColumns = false;
            dataGridViewGoods.DataSource = bindingGoods;
            ColumnGoodName.DataPropertyName = nameof(StocktakingGood.GoodName);
            ColumnUnit.DataPropertyName = nameof(StocktakingGood.UnitStr);
            ColumnCountFact.DataPropertyName = nameof(StocktakingGood.CountFactStr);
            ColumnPrice.DataPropertyName = nameof(StocktakingGood.Price);
            ColumnSum.DataPropertyName = nameof(StocktakingGood.Sum);
            ColumnCountMoveDoc.DataPropertyName = nameof(StocktakingGood.CountDocMove);
            
            listBoxGroups.SelectedIndexChanged += (s, e) =>
            {
                if (listBoxGroups.SelectedItem!=null)
                {
                    selectedGroup = listBoxGroups.SelectedItem as StocktakingGroup;
                    bindingGoods.DataSource = selectedGroup.StocktakingGoods;
                    bindingGoods.ResetBindings(false);
                }
            };

            BindingSource bindingFind = new BindingSource();
            bindingFind.DataSource = findGoods;
            listBoxFind.DataSource = bindingFind;
            listBoxFind.DisplayMember = nameof(Good.Name);
            findGoods.CollectionChanged += (s, e) =>{ bindingFind.ResetBindings(false); };
        }

        /// <summary>
        /// Добавить группу
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxGroupAdd.Text != "")
            {
                var newgroup = new StocktakingGroup { Name = textBoxGroupAdd.Text };
                groups.Add(newgroup);
                bindingGroups.ResetBindings(false);
                selectedGroup = newgroup;
                bindingGoods.DataSource = newgroup.StocktakingGoods;
                bindingGoods.ResetBindings(false);
                textBoxFind.Select();
            };
        }

        public void AddGood(Good good)
        {
            if (good == null || selectedGroup == null)
                return;
            FormEditCount fr = new FormEditCount();
            fr.labelGoodName.Text = good.Name;
            if (fr.ShowDialog() == DialogResult.OK)
            {
                var stGood = selectedGroup.StocktakingGoods.Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                if (stGood == null)
                {
                    selectedGroup.StocktakingGoods.Add(new StocktakingGood { Good = good, CountFactStr = fr.textBoxCount.Text });
                    bindingGoods.ResetBindings(false);
                    dataGridViewGoods.FirstDisplayedScrollingRowIndex = selectedGroup.StocktakingGoods.Count - 1;

                    return;
                };


                stGood.CountFactStr = fr.textBoxCount.Text;
                bindingGoods.ResetBindings(false);
                int rowSelected = -1;
                foreach (var stgood in selectedGroup.StocktakingGoods)
                    ++rowSelected;
                dataGridViewGoods.FirstDisplayedScrollingRowIndex = rowSelected;
            }
        }

        private void FormStocktaking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                textBoxFind.Select();
            if(e.KeyCode==Keys.F2 && dataGridViewGoods.SelectedCells.Count>0)
            {
                var stGood = selectedGroup.StocktakingGoods[dataGridViewGoods.SelectedCells[0].RowIndex];
                FormEditCount fr = new FormEditCount();
                fr.labelGoodName.Text = stGood.GoodName;
                if(fr.ShowDialog()==DialogResult.OK)
                {
                    stGood.CountFactStr = fr.textBoxCount.Text;
                    bindingGoods.ResetBindings(false);
                }
            }
        }

        #region Поиск
        private void textBoxFind_Enter(object sender, EventArgs e)
        {
            textBoxFind.BackColor = Color.LightGreen;
        }

        private void textBoxFind_Leave(object sender, EventArgs e)
        {
            textBoxFind.BackColor = SystemColors.Control;
        }

        private void textBoxFind_TextChanged(object sender, EventArgs e)
        {
            if (textBoxFind.Text != "")
                if (textBoxFind.Text.Length >= 2)
                {
                    findGoods.Clear();
                    var goods = _db.Goods.OrderBy(g => g.Name).ToList();
                    foreach (var good in goods.Where(g => g.IsDeleted == false && g.Name.ToLower().IndexOf(textBoxFind.Text.ToLower()) > -1).Take(20).ToList())
                        findGoods.Add(good);
                    foreach (var barcode in _db.BarCodes.Include(g => g.Good).Where(b => b.Code == textBoxFind.Text).ToList())
                        if (barcode.Good != null && barcode.Good.IsDeleted == false)
                            findGoods.Add(barcode.Good);
                }
        }

        private void textBoxFind_KeyDown(object sender, KeyEventArgs e)
        {

            if (textBoxFind.Text != "")
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        var good = (Good)listBoxFind.SelectedItem;
                        if (good != null)
                            AddGood(good);
                        textBoxFind.Text = "";
                        findGoods.Clear();
                        break;
                    case Keys.Down:
                        int cursor = listBoxFind.SelectedIndex;
                        int itemcount = listBoxFind.Items.Count;
                        if (cursor + 1 < itemcount)
                            listBoxFind.SelectedIndex = cursor + 1;
                        else
                            listBoxFind.SelectedIndex = 0;
                        break;
                    case Keys.Up:
                        int cursor1 = listBoxFind.SelectedIndex;
                        int itemcount1 = listBoxFind.Items.Count;
                        if (cursor1 == 0)
                            listBoxFind.SelectedIndex = itemcount1 - 1;
                        else
                            listBoxFind.SelectedIndex = cursor1 - 1;
                        break;
                }
        }

        private void listBoxFind_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxFind.SelectedItems.Count > 0)
            {
                var good = (Good)listBoxFind.SelectedItem;
                AddGood(good);
                findGoods.Clear();
                textBoxFind.Text = "";
            }
        }
        #endregion

        private void dataGridViewGoods_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private async Task<bool> Save()
        {
            bool result = true;
            try
            {
                if (stocktaking == null)
                {
                    stocktaking = new Stocktaking();
                    _db.Stocktakings.Add(stocktaking);
                }
                foreach (var group in groups)
                {
                    var groupDb = await _db.StocktakingGroups.Where(gr => gr.Id == group.Id).FirstOrDefaultAsync();
                    if (groupDb == null)
                    {
                        groupDb = new StocktakingGroup { StocktakingId = stocktaking.Id, Name = group.Name };
                        _db.StocktakingGroups.Add(groupDb);
                    };
                    foreach (var good in group.StocktakingGoods)
                    {
                        var goodDb = await _db.StocktakingGoods.Where(g => g.Id == good.Id).FirstOrDefaultAsync();
                        if (goodDb == null)
                            _db.StocktakingGoods.Add(new StocktakingGood { StocktakingGroupId = groupDb.Id, GoodId = good.Good.Id, CountFact = good.CountFact });
                        else
                            goodDb.CountFact = good.CountFact;
                    }
                }
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateException dbe)
            {
                System.Diagnostics.Debug.WriteLine(dbe.InnerException.Message);
            }
            catch (SystemException ex)
            {
                result = false;
            }
            return result;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await Save();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await Save();
            _synchService.AppendDoc(new DocSynch { DocId = stocktaking.Id, TypeDoc = TypeDocs.StockTaking });
        }
    }
}
