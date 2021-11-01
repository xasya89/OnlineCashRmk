﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineCashRmk.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    public partial class FormWriteOf : Form
    {
        DataContext db;
        ILogger<FormWriteOf> logger;
        ISynchService synch;

        ObservableCollection<WriteofGood> writeofGoods = new ObservableCollection<WriteofGood>();
        WriteofGood writeofGoodSelected { get; set; }
        ObservableCollection<Good> findGoods = new ObservableCollection<Good>();

        public FormWriteOf(DataContext db, ILogger<FormWriteOf> logger, ISynchService synch)
        {
            this.db = db;
            this.logger = logger;
            this.synch = synch;

            InitializeComponent();
            BindingSource binding = new BindingSource();
            binding.DataSource = writeofGoods;
            writeofGoods.CollectionChanged += (s, e) =>
            {
                binding.ResetBindings(false);
                LabelSumAll.Text = Math.Round(writeofGoods.Sum(w => w.Sum),2).ToString();
                LabelCountAll.Text = Math.Round(writeofGoods.Sum(w => w.Count),3).ToString();
            };
            writeofGoodTable.AutoGenerateColumns = false;
            writeofGoodTable.DataSource = binding;
            ColumnGoodName.DataPropertyName = nameof(WriteofGood.GoodName);
            ColumnGoodUnit.DataPropertyName = nameof(WriteofGood.GoodUnit);
            ColumnCount.DataPropertyName = nameof(WriteofGood.Count);
            ColumnPrice.DataPropertyName = nameof(WriteofGood.Price);
            ColumnSum.DataPropertyName = nameof(WriteofGood.Sum);

            BindingSource findBinding = new BindingSource();
            findBinding.DataSource = findGoods;
            findGoods.CollectionChanged += (s, e) =>
            {
                findBinding.ResetBindings(false);
            };
            findListBox.DataSource = findBinding;
            findListBox.DisplayMember = nameof(Good.Name);
        }

        string barcodeScan = "";
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!findInpText.Focused)
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
                    AddGood(db.BarCodes.Include(b => b.Good).Where(b => b.Code == barcodeScan & b.Good.IsDeleted == false).FirstOrDefault()?.Good);
                    barcodeScan = "";
                }
            };
            if (e.KeyCode == Keys.F2 && writeofGoodTable.SelectedRows.Count > 0)
                EditGood();
            if (e.KeyCode == Keys.Delete && writeofGoodTable.SelectedRows.Count > 0)
            {
                int pos = writeofGoodTable.SelectedRows[0].Index;
                writeofGoods.RemoveAt(pos);
            }
            if (e.KeyCode == Keys.F4 & !e.Alt)
                if (!findInpText.Focused)
                {
                    findInpText.Focus();
                    findInpText.BackColor = Color.LightBlue;
                }
                else
                {
                    writeofGoodTable.Select();
                    findInpText.BackColor = SystemColors.Window;
                }
            //Очистить чек
            if (e.KeyCode == Keys.Escape)
                writeofGoods.Clear();
        }

        void AddGood(Good good) => AddGood(good,1 );

        void AddGood(Good good, double count)
        {
            if (good != null)
            {
                if (good.Unit != Units.PCE & good.SpecialType != SpecialTypes.Beer)
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
                if (good.SpecialType == SpecialTypes.Beer)
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
                    LabelSumAll.Text = Math.Round(writeofGoods.Sum(w=>w.Sum),2).ToString();
                    LabelCountAll.Text = Math.Round(writeofGoods.Sum(w => w.Count), 3).ToString();
                }
            }
        }

        void EditGood()
        {

        }

        private void writeofGoodTable_SelectionChanged(object sender, EventArgs e)
        {
            if(writeofGoodTable.SelectedCells.Count>0)
                writeofGoodSelected = writeofGoods[writeofGoodTable.SelectedCells[0].RowIndex];
        }

        private async void findInpText_KeyDown(object sender, KeyEventArgs e)
        {
            if (findInpText.Text != "")
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        var good = (Good)findListBox.SelectedItem;
                        if (good != null)
                            AddGood(good);
                        findInpText.Text = "";
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

        private async void findInpText_TextChanged(object sender, EventArgs e)
        {
            findGoods.Clear();
            if (findInpText.Text.Length >= 3)
            {
                string str = findInpText.Text.ToLower();
                var goods = await db.Goods.Where(g => g.Name.ToLower().IndexOf(str) > -1).OrderBy(g => g.Name).Take(10).ToListAsync();
                foreach (var g in goods)
                    findGoods.Add(g);
                var good = (await db.BarCodes.Include(b => b.Good).Where(b => b.Code == findInpText.Text).FirstOrDefaultAsync())?.Good;
                if (good != null)
                    findGoods.Add(good);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
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

        private void findListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var good = findListBox.SelectedItem as Good;
            if (good != null)
                AddGood(good);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}