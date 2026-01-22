namespace OnlineCashRmk
{
    partial class SearchGoodsControll
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            searchTextBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            goodsListBox = new System.Windows.Forms.ListBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // searchTextBox
            // 
            searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            searchTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            searchTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            searchTextBox.Location = new System.Drawing.Point(102, 0);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new System.Drawing.Size(597, 33);
            searchTextBox.TabIndex = 0;
            searchTextBox.TextChanged += searchTextBox_TextChanged;
            searchTextBox.DoubleClick += searchTextBox_DoubleClick;
            searchTextBox.KeyDown += searchTextBox_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Left;
            label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            label1.Size = new System.Drawing.Size(102, 30);
            label1.TabIndex = 1;
            label1.Text = "Поиск (F4)";
            // 
            // panel1
            // 
            panel1.Controls.Add(searchTextBox);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(699, 39);
            panel1.TabIndex = 2;
            // 
            // goodsListBox
            // 
            goodsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            goodsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            goodsListBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            goodsListBox.FormattingEnabled = true;
            goodsListBox.ItemHeight = 25;
            goodsListBox.Location = new System.Drawing.Point(0, 39);
            goodsListBox.Name = "goodsListBox";
            goodsListBox.Size = new System.Drawing.Size(699, 205);
            goodsListBox.TabIndex = 3;
            goodsListBox.DoubleClick += goodsListBox_DoubleClick;
            goodsListBox.KeyDown += goodsListBox_KeyDown;
            // 
            // SearchGoodsControll
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(goodsListBox);
            Controls.Add(panel1);
            Name = "SearchGoodsControll";
            Size = new System.Drawing.Size(699, 244);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox goodsListBox;
    }
}
