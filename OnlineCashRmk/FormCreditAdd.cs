using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCashRmk
{
    public partial class FormCreditAdd : Form
    {
        public FormCreditAdd()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SumCredit.Text = SumPayment.Text;
        }
    }
}
