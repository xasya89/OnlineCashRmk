using System;
using System.Collections.Generic;
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
    public partial class FormFindGood : Form
    {
        DataContext _db;
        IServiceProvider _provider;
        public FormFindGood(DataContext db, IServiceProvider provider)
        {
            _db = db;
            _provider = provider;
        }

        List<Good> Goods = new List<Good>();
        public Good Show()
        {
            var goods = _db.Goods.Include(g => g.BarCodes).Where(g=>g.IsDeleted==false).ToList();
            Goods = goods;
            InitializeComponent();
            foreach (var good in Goods)
                dataGridView1.Rows.Add(
                    good.Id,
                    good.Name,
                    good.Price,
                    "изменить"
                    );
            textBoxFind.Focus();
            textBoxFind.Select();
            if (ShowDialog()==DialogResult.OK)
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = dataGridView1.SelectedRows[0];
                    int idGood = Convert.ToInt32(row.Cells["ColumnId"].Value);
                    return Goods.Where(g => g.Id == idGood).FirstOrDefault(); 
                }
            return null;
        }

        public FormFindGood()
        {
            
        }

        private void FormFindGood_Load(object sender, EventArgs e)
        {
            
        }

        private void textBoxFind_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach (var good in Goods)
                if(good.Name.ToLower().IndexOf(textBoxFind.Text.ToLower())>-1)
                dataGridView1.Rows.Add(
                    good.Id,
                    good.Name,
                    good.Price,
                    "Изменить"
                    );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ColumnEdit.Index)
            {
                int goodId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[ColumnId.Name].Value);
                var good = Goods.Where(g => g.Id == goodId).FirstOrDefault();
                var frNewGood =(FormNewGood) _provider.GetService(typeof(FormNewGood));
                DialogResult = DialogResult.Cancel;
                frNewGood.Show(good);
            }
        }
    }
}
