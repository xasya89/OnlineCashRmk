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

namespace OnlineCashRmk
{
    public partial class FormStocktaking : Form
    {
        DataContext _db;
        int? stocktakingId=null;
        ObservableCollection<StocktakingGroup> groups=new ObservableCollection<StocktakingGroup>();
        StocktakingGroup selectedGroup;
        ObservableCollection<Good> findGoods = new ObservableCollection<Good>();
        BindingSource bindingGroups;
        BindingSource bindingGoods;
        public FormStocktaking(DataContext db)
        {
            _db = db;
            var stocktaking = db.Stocktakings.Where(s => s.isSynch == false).FirstOrDefault();
            if(stocktaking!=null && MessageBox.Show("Продолжить редактировать предыдущую инверторизацию?","",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
            {
                //TODO: Доделать открытие предыдущего документа
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
            if (good == null || selectedGroup==null)
                return;
            selectedGroup.StocktakingGoods.Add(new StocktakingGood { Good = good });
            bindingGoods.ResetBindings(false);
        }

        private void FormStocktaking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                textBoxFind.Select();
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

    }
}
