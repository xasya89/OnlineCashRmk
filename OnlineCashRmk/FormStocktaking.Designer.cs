
namespace OnlineCashRmk
{
    partial class FormStocktaking
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
            panel1 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            listBoxGroups = new System.Windows.Forms.ListBox();
            panel2 = new System.Windows.Forms.Panel();
            button1 = new System.Windows.Forms.Button();
            textBoxGroupAdd = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            dataGridViewGoods = new System.Windows.Forms.DataGridView();
            ColumnGoodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnCountFact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            searchPanel = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewGoods).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label3);
            panel1.Controls.Add(button2);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 581);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1112, 49);
            panel1.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 14);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(52, 21);
            label3.TabIndex = 1;
            label3.Text = "label3";
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.LightGreen;
            button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button2.Location = new System.Drawing.Point(762, 9);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(255, 31);
            button2.TabIndex = 0;
            button2.Text = "Отправить";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBoxGroups);
            splitContainer1.Panel1.Controls.Add(panel2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridViewGoods);
            splitContainer1.Panel2.Controls.Add(searchPanel);
            splitContainer1.Size = new System.Drawing.Size(1112, 581);
            splitContainer1.SplitterDistance = 199;
            splitContainer1.TabIndex = 2;
            // 
            // listBoxGroups
            // 
            listBoxGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxGroups.FormattingEnabled = true;
            listBoxGroups.ItemHeight = 21;
            listBoxGroups.Location = new System.Drawing.Point(0, 112);
            listBoxGroups.Name = "listBoxGroups";
            listBoxGroups.Size = new System.Drawing.Size(199, 469);
            listBoxGroups.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Controls.Add(textBoxGroupAdd);
            panel2.Controls.Add(label1);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(199, 112);
            panel2.TabIndex = 0;
            // 
            // button1
            // 
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            button1.Location = new System.Drawing.Point(32, 68);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(153, 34);
            button1.TabIndex = 2;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBoxGroupAdd
            // 
            textBoxGroupAdd.Location = new System.Drawing.Point(3, 33);
            textBoxGroupAdd.Name = "textBoxGroupAdd";
            textBoxGroupAdd.Size = new System.Drawing.Size(179, 29);
            textBoxGroupAdd.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(107, 21);
            label1.TabIndex = 0;
            label1.Text = "Новая группа";
            // 
            // dataGridViewGoods
            // 
            dataGridViewGoods.AllowUserToAddRows = false;
            dataGridViewGoods.AllowUserToDeleteRows = false;
            dataGridViewGoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewGoods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { ColumnGoodName, ColumnUnit, ColumnCountFact, ColumnPrice, ColumnSum });
            dataGridViewGoods.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewGoods.EnableHeadersVisualStyles = false;
            dataGridViewGoods.Location = new System.Drawing.Point(0, 0);
            dataGridViewGoods.MultiSelect = false;
            dataGridViewGoods.Name = "dataGridViewGoods";
            dataGridViewGoods.RowHeadersVisible = false;
            dataGridViewGoods.Size = new System.Drawing.Size(909, 383);
            dataGridViewGoods.TabIndex = 1;
            dataGridViewGoods.KeyDown += dataGridViewGoods_KeyDown;
            // 
            // ColumnGoodName
            // 
            ColumnGoodName.HeaderText = "Наименование";
            ColumnGoodName.Name = "ColumnGoodName";
            ColumnGoodName.ReadOnly = true;
            ColumnGoodName.Width = 500;
            // 
            // ColumnUnit
            // 
            ColumnUnit.HeaderText = "Ед.";
            ColumnUnit.Name = "ColumnUnit";
            ColumnUnit.ReadOnly = true;
            ColumnUnit.Width = 70;
            // 
            // ColumnCountFact
            // 
            ColumnCountFact.HeaderText = "Кол-во";
            ColumnCountFact.Name = "ColumnCountFact";
            ColumnCountFact.ReadOnly = true;
            // 
            // ColumnPrice
            // 
            ColumnPrice.HeaderText = "Цена";
            ColumnPrice.Name = "ColumnPrice";
            ColumnPrice.ReadOnly = true;
            // 
            // ColumnSum
            // 
            ColumnSum.HeaderText = "Стоимость";
            ColumnSum.Name = "ColumnSum";
            ColumnSum.ReadOnly = true;
            // 
            // searchPanel
            // 
            searchPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            searchPanel.Location = new System.Drawing.Point(0, 383);
            searchPanel.Name = "searchPanel";
            searchPanel.Size = new System.Drawing.Size(909, 198);
            searchPanel.TabIndex = 0;
            // 
            // FormStocktaking
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1112, 630);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            Font = new System.Drawing.Font("Segoe UI", 12F);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "FormStocktaking";
            Text = "Инверторизация";
            FormClosed += FormStocktaking_FormClosed;
            KeyDown += FormStocktaking_KeyDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewGoods).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBoxGroups;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxGroupAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewGoods;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCountFact;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSum;
    }
}