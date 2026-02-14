
namespace OnlineCashRmk
{
    partial class FormWriteOf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWriteOf));
            DateWriteOfInp = new System.Windows.Forms.DateTimePicker();
            DateWriteOf = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            writeofGoodTable = new System.Windows.Forms.DataGridView();
            ColumnGoodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnGoodUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripLabудAmount = new System.Windows.Forms.ToolStripLabel();
            NoteInp = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            searchPanel = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)writeofGoodTable).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // DateWriteOfInp
            // 
            DateWriteOfInp.Location = new System.Drawing.Point(136, 12);
            DateWriteOfInp.Name = "DateWriteOfInp";
            DateWriteOfInp.Size = new System.Drawing.Size(200, 27);
            DateWriteOfInp.TabIndex = 0;
            // 
            // DateWriteOf
            // 
            DateWriteOf.AutoSize = true;
            DateWriteOf.Location = new System.Drawing.Point(12, 17);
            DateWriteOf.Name = "DateWriteOf";
            DateWriteOf.Size = new System.Drawing.Size(118, 20);
            DateWriteOf.TabIndex = 1;
            DateWriteOf.Text = "Дата документа";
            // 
            // panel1
            // 
            panel1.Controls.Add(writeofGoodTable);
            panel1.Controls.Add(toolStrip1);
            panel1.Location = new System.Drawing.Point(-1, 45);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(916, 293);
            panel1.TabIndex = 2;
            // 
            // writeofGoodTable
            // 
            writeofGoodTable.AllowUserToAddRows = false;
            writeofGoodTable.AllowUserToDeleteRows = false;
            writeofGoodTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            writeofGoodTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { ColumnGoodName, ColumnGoodUnit, ColumnCount, ColumnPrice, ColumnSum });
            writeofGoodTable.Dock = System.Windows.Forms.DockStyle.Fill;
            writeofGoodTable.Location = new System.Drawing.Point(0, 28);
            writeofGoodTable.Name = "writeofGoodTable";
            writeofGoodTable.Size = new System.Drawing.Size(916, 265);
            writeofGoodTable.TabIndex = 2;
            writeofGoodTable.SelectionChanged += writeofGoodTable_SelectionChanged;
            // 
            // ColumnGoodName
            // 
            ColumnGoodName.FillWeight = 300F;
            ColumnGoodName.HeaderText = "Товар";
            ColumnGoodName.Name = "ColumnGoodName";
            ColumnGoodName.ReadOnly = true;
            ColumnGoodName.Width = 300;
            // 
            // ColumnGoodUnit
            // 
            ColumnGoodUnit.HeaderText = "Ед";
            ColumnGoodUnit.Name = "ColumnGoodUnit";
            ColumnGoodUnit.ReadOnly = true;
            // 
            // ColumnCount
            // 
            ColumnCount.HeaderText = "Кол-во";
            ColumnCount.Name = "ColumnCount";
            // 
            // ColumnPrice
            // 
            ColumnPrice.HeaderText = "Цена";
            ColumnPrice.Name = "ColumnPrice";
            ColumnPrice.ReadOnly = true;
            // 
            // ColumnSum
            // 
            ColumnSum.HeaderText = "Сумма";
            ColumnSum.Name = "ColumnSum";
            ColumnSum.ReadOnly = true;
            // 
            // toolStrip1
            // 
            toolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton2, toolStripSeparator1, toolStripLabудAmount, toolStripLabel1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(916, 28);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(88, 25);
            toolStripButton2.Text = "Удалить";
            toolStripButton2.Click += toolStripButton2_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(57, 25);
            toolStripLabel1.Text = "Итог:  ";
            // 
            // toolStripLabудAmount
            // 
            toolStripLabудAmount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripLabудAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            toolStripLabудAmount.Name = "toolStripLabудAmount";
            toolStripLabудAmount.Size = new System.Drawing.Size(176, 25);
            toolStripLabудAmount.Text = "toolStripLabудAmount";
            // 
            // NoteInp
            // 
            NoteInp.Location = new System.Drawing.Point(103, 531);
            NoteInp.Name = "NoteInp";
            NoteInp.Size = new System.Drawing.Size(812, 27);
            NoteInp.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 534);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(72, 20);
            label1.TabIndex = 4;
            label1.Text = "Причина";
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            button1.Location = new System.Drawing.Point(582, 12);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(148, 30);
            button1.TabIndex = 5;
            button1.Text = "Сохранить";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            button2.Location = new System.Drawing.Point(763, 12);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(139, 30);
            button2.TabIndex = 5;
            button2.Text = "Отмена";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // searchPanel
            // 
            searchPanel.Location = new System.Drawing.Point(-1, 344);
            searchPanel.Name = "searchPanel";
            searchPanel.Size = new System.Drawing.Size(916, 181);
            searchPanel.TabIndex = 6;
            // 
            // FormWriteOf
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(914, 561);
            Controls.Add(searchPanel);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(NoteInp);
            Controls.Add(panel1);
            Controls.Add(DateWriteOf);
            Controls.Add(DateWriteOfInp);
            Font = new System.Drawing.Font("Segoe UI", 11.25F);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "FormWriteOf";
            Text = "Списание";
            FormClosed += FormWriteOf_FormClosed;
            KeyDown += Form1_KeyDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)writeofGoodTable).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DateWriteOfInp;
        private System.Windows.Forms.Label DateWriteOf;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView writeofGoodTable;
        private System.Windows.Forms.TextBox NoteInp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGoodUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSum;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabудAmount;
    }
}