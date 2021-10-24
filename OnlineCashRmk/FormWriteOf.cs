using Microsoft.Extensions.Logging;
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
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    public partial class FormWriteOf : Form
    {
        ObservableCollection<WriteofGood> writeofGoods = new ObservableCollection<WriteofGood>();
        WriteofGood writeofGoodSelected { get; set; }

        public FormWriteOf(DataContext db, ILogger<FormWriteOf> logger, ISynchService synch)
        {
            InitializeComponent();
            BindingSource binding = new BindingSource();
            binding.DataSource = writeofGoods;
            writeofGoods.CollectionChanged += (s, e) =>
            {
                binding.ResetBindings(false);
                LabelSumAll.Text = writeofGoods.Sum(w => w.Sum).ToString();
                LabelCountAll.Text = writeofGoods.Sum(w => w.Count).ToString();
            };
            writeofGoodTable.AutoGenerateColumns = false;
            writeofGoodTable.DataSource = binding;
            ColumnGoodName.DataPropertyName = nameof(WriteofGood.Good.Name);
            ColumnGoodUnit.DataPropertyName = nameof(WriteofGood.Good.UnitDescription);
            ColumnCount.DataPropertyName = nameof(WriteofGood.Count);
            ColumnPrice.DataPropertyName = nameof(WriteofGood.Price);
            ColumnSum.DataPropertyName = nameof(WriteofGood.Sum);
        }
    }
}
