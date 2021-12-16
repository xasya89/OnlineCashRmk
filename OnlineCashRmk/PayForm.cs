using OnlineCashRmk.Models;
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
    public partial class PayForm : Form
    {
        DataContext _db;
        CheckSell _checkSell;
        BindingSource bindingCheckSell = new BindingSource();
        Buyer _buyer=new Buyer();
        BindingSource bindingBuyer = new BindingSource();
        public PayForm(DataContext db)
        {
            InitializeComponent();
            _db = db;
            bindingCheckSell.DataSource = _checkSell;
            BuySum.DataBindings.Add("Text", bindingCheckSell, nameof(CheckSell.Sum),false, DataSourceUpdateMode.OnPropertyChanged);
            DiscounSum.DataBindings.Add("Text", bindingCheckSell, nameof(CheckSell.SumDiscont), false, DataSourceUpdateMode.OnPropertyChanged);
            ItogSum.DataBindings.Add("Text", bindingCheckSell, nameof(CheckSell.SumAll), false, DataSourceUpdateMode.OnPropertyChanged);

            bindingBuyer.DataSource = _buyer;
            BuyerName.DataBindings.Add("Text", bindingBuyer, nameof(Buyer.Name), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public DialogResult Pay(CheckSell checkSell)
        {
            _checkSell = new CheckSell { Sum=22 };
            bindingCheckSell.DataSource = _checkSell;
            bindingCheckSell.ResetBindings(false);
            return ShowDialog();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var buyer = _db.Buyers.Where(b => b.DiscountCardNum == textBox1.Text).FirstOrDefault();
                if(buyer!=null)
                {
                    _buyer = buyer;
                    //TODO: Скорее всего добавить buyer ы checksell
                    bindingBuyer.DataSource = buyer;
                    bindingBuyer.ResetBindings(false);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            

        }
    }
}
