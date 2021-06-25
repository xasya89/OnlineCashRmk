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
        List<Good> Goods;
        public Good SelectedGood;
        public FormFindGood(List<Good> goods)
        {
            Goods = goods;
            InitializeComponent();
        }

        private void FormFindGood_Load(object sender, EventArgs e)
        {
            foreach (var good in Goods)
                dataGridView1.Rows.Add(
                    good.Id,
                    good.Name,
                    good.Price
                    );
            textBoxFind.Focus();
            textBoxFind.Select();
        }

        private void textBoxFind_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach (var good in Goods)
                if(good.Name.ToLower().IndexOf(textBoxFind.Text.ToLower())>-1)
                dataGridView1.Rows.Add(
                    good.Id,
                    good.Name,
                    good.Price
                    );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedGoodEvent();
        }
        void SelectedGoodEvent()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int idGood = Convert.ToInt32(row.Cells["ColumnId"].Value);
                SelectedGood = Goods.Where(g => g.Id == idGood).FirstOrDefault();
                DialogResult = DialogResult.OK;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            SelectedGoodEvent();
        }
    }
}
