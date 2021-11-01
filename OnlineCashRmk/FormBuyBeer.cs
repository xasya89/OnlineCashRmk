using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    public partial class FormBuyBeer : Form
    {
        public FormBuyBeer(Good good)
        {
            InitializeComponent();
            GoodNameLabel.Text = good.Name;
            DataContext db = new DataContext();
            BottleListBox.DataSource = db.Goods
                .Where(g => g.SpecialType == SpecialTypes.Bottle)
                .OrderBy(g=>g.Name)
                .ToList();
            BottleListBox.DisplayMember = nameof(Good.Name);
            CountTextBox.Text = "1";
        }

        private void FormBuyBeer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        void CountPlus()
        {
            int count = 0;
            int.TryParse(CountTextBox.Text, out count);
            CountTextBox.Text = (count + 1).ToString();
        }
        void CountMinus()
        {
            int count = 0;
            int.TryParse(CountTextBox.Text, out count);
            CountTextBox.Text = (count<=0 ? 1 : count - 1).ToString();
        }

        private void FormBuyBeer_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Oemplus:
                case Keys.Add:
                    CountPlus();
                    break;
                case Keys.OemMinus:
                case Keys.Subtract:
                    CountMinus();
                    break;
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
                case Keys.Enter:
                    DialogResult = DialogResult.OK;
                    break;
            }
        }

        private void ButtonPlus_Click(object sender, EventArgs e)
        {
            CountPlus();
        }

        private void ButtonMinus_Click(object sender, EventArgs e)
        {
            CountMinus();
        }
    }
}
