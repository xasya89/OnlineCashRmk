using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    public partial class FormCheckHistory : Form
    {
        DataContext db;
        ObservableCollection<CheckSell> checkSells = new ObservableCollection<CheckSell>();
        ObservableCollection<CheckGood> checkGoods = new ObservableCollection<CheckGood>();
        BindingSource binding = new BindingSource();
        BindingSource bindingCheckGoods = new BindingSource();
        public FormCheckHistory()
        {
            db = new DataContext();
            InitializeComponent();
            binding.DataSource = checkSells;
            dataGridViewChecks.AutoGenerateColumns = false;
            dataGridViewChecks.DataSource = binding;
            ColumnDateCreate.DataPropertyName = nameof(CheckSell.DateCreate);
            ColumnSumAll.DataPropertyName = nameof(CheckSell.SumAll);
            ColumnTypePayment.DataPropertyName = nameof(CheckSell.IsElectron);

            dataGridViewCheck.AutoGenerateColumns = false;
            bindingCheckGoods.DataSource = checkGoods;
            dataGridViewCheck.DataSource = bindingCheckGoods;
            ColumnGoodName.DataPropertyName = nameof(CheckGood.GoodName);
            ColumnCount.DataPropertyName = nameof(CheckGood.Count);
            ColumnPrice.DataPropertyName = nameof(CheckGood.Cost);
            ColumnSum.DataPropertyName = nameof(CheckGood.Sum);

            Select();
        }

        void Select()
        {
            var shifts = db.Shifts
                .Include(s => s.CheckSells.OrderByDescending(c=>c.DateCreate))
                .ThenInclude(c => c.CheckGoods)
                .ThenInclude(cg => cg.Good)
                .Where(s => s.Start.Date == dateTimePicker1.Value.Date)
                .ToList();
            foreach (var shift in shifts)
                foreach (var chek in shift.CheckSells)
                    checkSells.Add(chek);
            binding.ResetBindings(false);
        }

        private void dataGridViewChecks_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridViewChecks.SelectedRows.Count>0)
            {
                var chGoods = checkSells[dataGridViewChecks.SelectedRows[0].Index].CheckGoods;
                checkGoods.Clear();
                foreach (var chGood in chGoods)
                    checkGoods.Add(chGood);
                bindingCheckGoods.ResetBindings(false);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Select();
        }
    }
}
