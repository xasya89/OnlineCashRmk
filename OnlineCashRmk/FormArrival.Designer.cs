
namespace OnlineCashRmk
{
    partial class FormArrival
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            SupplierComboBox = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            arrivalNum = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            arrivalDate = new System.Windows.Forms.DateTimePicker();
            label3 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            labelSumArrival = new System.Windows.Forms.Label();
            labelSumNds = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            labelSumSell = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            dataGridViewPositions = new System.Windows.Forms.DataGridView();
            Column_GoodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_PriceArrival = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_PriceSell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_PricePercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_NdsPercent = new System.Windows.Forms.DataGridViewComboBoxColumn();
            Column_SumNds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_SumArrival = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_SumSell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column_ExpiresDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            buttonCancel = new System.Windows.Forms.Button();
            buttonSave = new System.Windows.Forms.Button();
            searchPanel = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPositions).BeginInit();
            SuspendLayout();
            // 
            // SupplierComboBox
            // 
            SupplierComboBox.FormattingEnabled = true;
            SupplierComboBox.Location = new System.Drawing.Point(500, 12);
            SupplierComboBox.Name = "SupplierComboBox";
            SupplierComboBox.Size = new System.Drawing.Size(225, 23);
            SupplierComboBox.TabIndex = 3;
            SupplierComboBox.SelectedIndexChanged += SupplierComboBox_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(45, 15);
            label1.TabIndex = 1;
            label1.Text = "Номер";
            // 
            // arrivalNum
            // 
            arrivalNum.Location = new System.Drawing.Point(64, 12);
            arrivalNum.Name = "arrivalNum";
            arrivalNum.Size = new System.Drawing.Size(100, 23);
            arrivalNum.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(170, 15);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(32, 15);
            label2.TabIndex = 3;
            label2.Text = "Дата";
            // 
            // arrivalDate
            // 
            arrivalDate.Location = new System.Drawing.Point(208, 12);
            arrivalDate.Name = "arrivalDate";
            arrivalDate.Size = new System.Drawing.Size(200, 23);
            arrivalDate.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(424, 15);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(70, 15);
            label3.TabIndex = 5;
            label3.Text = "Поставшик";
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonSave);
            panel1.Controls.Add(buttonCancel);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(arrivalNum);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(SupplierComboBox);
            panel1.Controls.Add(arrivalDate);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1041, 77);
            panel1.TabIndex = 6;
            // 
            // panel4
            // 
            panel4.Controls.Add(label4);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(labelSumArrival);
            panel4.Controls.Add(labelSumNds);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(labelSumSell);
            panel4.Location = new System.Drawing.Point(298, 41);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(731, 29);
            panel4.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(8, 8);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(100, 15);
            label4.TabIndex = 6;
            label4.Text = "Всего по закупке";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(224, 8);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(32, 15);
            label7.TabIndex = 11;
            label7.Text = "НДС";
            // 
            // labelSumArrival
            // 
            labelSumArrival.AutoSize = true;
            labelSumArrival.Location = new System.Drawing.Point(126, 8);
            labelSumArrival.Name = "labelSumArrival";
            labelSumArrival.Size = new System.Drawing.Size(38, 15);
            labelSumArrival.TabIndex = 7;
            labelSumArrival.Text = "label5";
            // 
            // labelSumNds
            // 
            labelSumNds.AutoSize = true;
            labelSumNds.Location = new System.Drawing.Point(275, 8);
            labelSumNds.Name = "labelSumNds";
            labelSumNds.Size = new System.Drawing.Size(38, 15);
            labelSumNds.TabIndex = 10;
            labelSumNds.Text = "label6";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(378, 8);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(72, 15);
            label5.TabIndex = 8;
            label5.Text = "по продаже";
            // 
            // labelSumSell
            // 
            labelSumSell.AutoSize = true;
            labelSumSell.Location = new System.Drawing.Point(469, 8);
            labelSumSell.Name = "labelSumSell";
            labelSumSell.Size = new System.Drawing.Size(38, 15);
            labelSumSell.TabIndex = 9;
            labelSumSell.Text = "label6";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(13, 51);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(101, 23);
            button1.TabIndex = 4;
            button1.Text = "Удалить (del)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridViewPositions
            // 
            dataGridViewPositions.AllowUserToAddRows = false;
            dataGridViewPositions.AllowUserToDeleteRows = false;
            dataGridViewPositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPositions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column_GoodName, Column_Unit, Column_PriceArrival, Column_PriceSell, Column_PricePercent, Column_Count, Column_NdsPercent, Column_SumNds, Column_SumArrival, Column_SumSell, Column_ExpiresDate });
            dataGridViewPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewPositions.Location = new System.Drawing.Point(0, 77);
            dataGridViewPositions.MultiSelect = false;
            dataGridViewPositions.Name = "dataGridViewPositions";
            dataGridViewPositions.RowHeadersVisible = false;
            dataGridViewPositions.Size = new System.Drawing.Size(1041, 340);
            dataGridViewPositions.TabIndex = 0;
            dataGridViewPositions.CellClick += dataGridViewPositions_CellClick;
            dataGridViewPositions.CellValueChanged += dataGridViewPositions_CellValueChanged;
            // 
            // Column_GoodName
            // 
            Column_GoodName.HeaderText = "Наименование";
            Column_GoodName.Name = "Column_GoodName";
            Column_GoodName.ReadOnly = true;
            Column_GoodName.Width = 160;
            // 
            // Column_Unit
            // 
            Column_Unit.HeaderText = "Ед";
            Column_Unit.Name = "Column_Unit";
            Column_Unit.ReadOnly = true;
            Column_Unit.Width = 60;
            // 
            // Column_PriceArrival
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            Column_PriceArrival.DefaultCellStyle = dataGridViewCellStyle5;
            Column_PriceArrival.HeaderText = "Цена";
            Column_PriceArrival.Name = "Column_PriceArrival";
            // 
            // Column_PriceSell
            // 
            Column_PriceSell.HeaderText = "Цена продажи";
            Column_PriceSell.Name = "Column_PriceSell";
            // 
            // Column_PricePercent
            // 
            Column_PricePercent.HeaderText = "Наценка %";
            Column_PricePercent.Name = "Column_PricePercent";
            Column_PricePercent.ReadOnly = true;
            // 
            // Column_Count
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            Column_Count.DefaultCellStyle = dataGridViewCellStyle6;
            Column_Count.HeaderText = "Кол-во";
            Column_Count.Name = "Column_Count";
            // 
            // Column_NdsPercent
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            Column_NdsPercent.DefaultCellStyle = dataGridViewCellStyle7;
            Column_NdsPercent.HeaderText = "% НДС";
            Column_NdsPercent.Items.AddRange(new object[] { "20%", "10%", "0%", "Без ндс" });
            Column_NdsPercent.Name = "Column_NdsPercent";
            // 
            // Column_SumNds
            // 
            Column_SumNds.HeaderText = "НДС";
            Column_SumNds.Name = "Column_SumNds";
            Column_SumNds.ReadOnly = true;
            Column_SumNds.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            Column_SumNds.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_SumArrival
            // 
            Column_SumArrival.HeaderText = "Сумма покупки";
            Column_SumArrival.Name = "Column_SumArrival";
            Column_SumArrival.ReadOnly = true;
            // 
            // Column_SumSell
            // 
            Column_SumSell.HeaderText = "Сумма продажи";
            Column_SumSell.Name = "Column_SumSell";
            Column_SumSell.ReadOnly = true;
            // 
            // Column_ExpiresDate
            // 
            dataGridViewCellStyle8.Format = "d";
            dataGridViewCellStyle8.NullValue = null;
            Column_ExpiresDate.DefaultCellStyle = dataGridViewCellStyle8;
            Column_ExpiresDate.HeaderText = "Срок годности";
            Column_ExpiresDate.Name = "Column_ExpiresDate";
            // 
            // buttonCancel
            // 
            buttonCancel.BackColor = System.Drawing.Color.Salmon;
            buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonCancel.Location = new System.Drawing.Point(784, 4);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(112, 31);
            buttonCancel.TabIndex = 0;
            buttonCancel.Text = "Отмена";
            buttonCancel.UseVisualStyleBackColor = false;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = System.Drawing.Color.LightGreen;
            buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonSave.Location = new System.Drawing.Point(917, 4);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new System.Drawing.Size(112, 31);
            buttonSave.TabIndex = 0;
            buttonSave.Text = "Сохранить";
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += buttonSave_Click;
            // 
            // searchPanel
            // 
            searchPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            searchPanel.Location = new System.Drawing.Point(0, 417);
            searchPanel.Name = "searchPanel";
            searchPanel.Size = new System.Drawing.Size(1041, 224);
            searchPanel.TabIndex = 9;
            // 
            // FormArrival
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1041, 641);
            Controls.Add(dataGridViewPositions);
            Controls.Add(searchPanel);
            Controls.Add(panel1);
            KeyPreview = true;
            Name = "FormArrival";
            Text = "FormArrival";
            FormClosed += FormArrival_FormClosed;
            KeyDown += FormArrival_KeyDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPositions).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox SupplierComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox arrivalNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker arrivalDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridViewPositions;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelSumSell;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSumArrival;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSumNds;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_GoodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PriceArrival;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PriceSell;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PricePercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Count;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_NdsPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_SumNds;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_SumArrival;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_SumSell;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ExpiresDate;
    }
}