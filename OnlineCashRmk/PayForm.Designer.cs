
namespace OnlineCashRmk
{
    partial class PayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BuyerName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DiscountBonus = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BuySum = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DiscounSum = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ItogSum = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Карта лояльности";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(168, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(235, 29);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // BuyerName
            // 
            this.BuyerName.AutoSize = true;
            this.BuyerName.Location = new System.Drawing.Point(168, 69);
            this.BuyerName.Name = "BuyerName";
            this.BuyerName.Size = new System.Drawing.Size(52, 21);
            this.BuyerName.TabIndex = 20;
            this.BuyerName.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Бонус";
            // 
            // DiscountBonus
            // 
            this.DiscountBonus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DiscountBonus.Location = new System.Drawing.Point(168, 93);
            this.DiscountBonus.Name = "DiscountBonus";
            this.DiscountBonus.ReadOnly = true;
            this.DiscountBonus.Size = new System.Drawing.Size(235, 29);
            this.DiscountBonus.TabIndex = 20;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.DiscountBonus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.BuyerName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(604, 140);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Покупатель";
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(409, 90);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(119, 32);
            this.button5.TabIndex = 21;
            this.button5.Text = "Списать (Del)";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightSalmon;
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(477, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 52);
            this.button4.TabIndex = 5;
            this.button4.Text = "Отмена ESC";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 21);
            this.label4.TabIndex = 6;
            this.label4.Text = "Покупка";
            // 
            // BuySum
            // 
            this.BuySum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BuySum.Location = new System.Drawing.Point(87, 12);
            this.BuySum.Name = "BuySum";
            this.BuySum.ReadOnly = true;
            this.BuySum.Size = new System.Drawing.Size(96, 29);
            this.BuySum.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "Скидка";
            // 
            // DiscounSum
            // 
            this.DiscounSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DiscounSum.Location = new System.Drawing.Point(263, 12);
            this.DiscounSum.Name = "DiscounSum";
            this.DiscounSum.ReadOnly = true;
            this.DiscounSum.Size = new System.Drawing.Size(91, 29);
            this.DiscounSum.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(369, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 21);
            this.label6.TabIndex = 10;
            this.label6.Text = "К оплате";
            // 
            // ItogSum
            // 
            this.ItogSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ItogSum.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ItogSum.Location = new System.Drawing.Point(447, 12);
            this.ItogSum.Name = "ItogSum";
            this.ItogSum.ReadOnly = true;
            this.ItogSum.Size = new System.Drawing.Size(100, 29);
            this.ItogSum.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Image = global::OnlineCashRmk.Properties.Resources.money;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(12, 239);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 58);
            this.button1.TabIndex = 12;
            this.button1.Text = "Наличные (F5)";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = global::OnlineCashRmk.Properties.Resources.credit_cart;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(211, 239);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(170, 58);
            this.button2.TabIndex = 13;
            this.button2.Text = "Карта (F6)";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(409, 239);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 58);
            this.button3.TabIndex = 14;
            this.button3.Text = "Сложная (F7)";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.DiscounSum);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.BuySum);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.ItogSum);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 140);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 56);
            this.panel1.TabIndex = 21;
            // 
            // PayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 311);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PayForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label BuyerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DiscountBonus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox BuySum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DiscounSum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ItogSum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Panel panel1;
    }
}