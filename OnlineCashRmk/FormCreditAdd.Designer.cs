﻿
namespace OnlineCashRmk
{
    partial class FormCreditAdd
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
            this.Creditor = new System.Windows.Forms.TextBox();
            this.SumCredit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SumPayment = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Кому выдан";
            // 
            // Creditor
            // 
            this.Creditor.Location = new System.Drawing.Point(146, 12);
            this.Creditor.Name = "Creditor";
            this.Creditor.Size = new System.Drawing.Size(657, 35);
            this.Creditor.TabIndex = 1;
            this.Creditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Creditor_KeyDown);
            // 
            // SumCredit
            // 
            this.SumCredit.Location = new System.Drawing.Point(570, 68);
            this.SumCredit.Name = "SumCredit";
            this.SumCredit.Size = new System.Drawing.Size(178, 35);
            this.SumCredit.TabIndex = 2;
            this.SumCredit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Creditor_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(412, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "Сумма заяйма";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightSalmon;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(558, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(245, 58);
            this.button1.TabIndex = 4;
            this.button1.Text = "Отмена";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightGreen;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(252, 135);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(245, 58);
            this.button2.TabIndex = 4;
            this.button2.Text = "Выдать (Enter)";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 30);
            this.label3.TabIndex = 3;
            this.label3.Text = "К оплате";
            // 
            // SumPayment
            // 
            this.SumPayment.Location = new System.Drawing.Point(146, 68);
            this.SumPayment.Name = "SumPayment";
            this.SumPayment.ReadOnly = true;
            this.SumPayment.Size = new System.Drawing.Size(190, 35);
            this.SumPayment.TabIndex = 10;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(353, 68);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(53, 37);
            this.button3.TabIndex = 11;
            this.button3.Text = "=";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::OnlineCashRmk.Properties.Resources.clear;
            this.button4.Location = new System.Drawing.Point(754, 66);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(53, 37);
            this.button4.TabIndex = 11;
            this.button4.Text = "=";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FormCreditAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 220);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SumPayment);
            this.Controls.Add(this.SumCredit);
            this.Controls.Add(this.Creditor);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FormCreditAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Внести кредит";
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.FormCreditAdd_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox Creditor;
        public System.Windows.Forms.TextBox SumCredit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox SumPayment;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}