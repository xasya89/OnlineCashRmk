namespace OnlineCashRmk
{
    partial class FormRevaluation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRevaluation));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolButtonDeleteRow = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSynchGoods = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabelAmount = new System.Windows.Forms.ToolStripLabel();
            revaluationDataGridView = new System.Windows.Forms.DataGridView();
            searchPanel = new System.Windows.Forms.Panel();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)revaluationDataGridView).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolButtonDeleteRow, toolStripButton2, toolStripButton3, toolStripSeparator1, toolStripSynchGoods, toolStripSeparator2, toolStripLabelAmount });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1029, 28);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolButtonDeleteRow
            // 
            toolButtonDeleteRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolButtonDeleteRow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            toolButtonDeleteRow.Image = (System.Drawing.Image)resources.GetObject("toolButtonDeleteRow.Image");
            toolButtonDeleteRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButtonDeleteRow.Name = "toolButtonDeleteRow";
            toolButtonDeleteRow.Size = new System.Drawing.Size(142, 25);
            toolButtonDeleteRow.Text = "Удалить позицию";
            toolButtonDeleteRow.Click += toolButtonDeleteRow_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButton2.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(90, 25);
            toolStripButton2.Text = "Сохранить";
            toolStripButton2.Click += toolStripButton2_Click;
            // 
            // toolStripButton3
            // 
            toolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButton3.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Margin = new System.Windows.Forms.Padding(0, 1, 20, 2);
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(69, 25);
            toolStripButton3.Text = "Отмена";
            toolStripButton3.Click += toolStripButton3_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripSynchGoods
            // 
            toolStripSynchGoods.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripSynchGoods.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            toolStripSynchGoods.Image = (System.Drawing.Image)resources.GetObject("toolStripSynchGoods.Image");
            toolStripSynchGoods.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSynchGoods.Name = "toolStripSynchGoods";
            toolStripSynchGoods.Size = new System.Drawing.Size(127, 25);
            toolStripSynchGoods.Text = "Обновить цены";
            toolStripSynchGoods.Click += toolStripSynchGoods_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabelAmount
            // 
            toolStripLabelAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            toolStripLabelAmount.Margin = new System.Windows.Forms.Padding(20, 1, 20, 2);
            toolStripLabelAmount.Name = "toolStripLabelAmount";
            toolStripLabelAmount.Size = new System.Drawing.Size(175, 25);
            toolStripLabelAmount.Text = "Итого: было 0 стало 0";
            // 
            // revaluationDataGridView
            // 
            revaluationDataGridView.AllowUserToAddRows = false;
            revaluationDataGridView.AllowUserToDeleteRows = false;
            revaluationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            revaluationDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            revaluationDataGridView.Location = new System.Drawing.Point(0, 28);
            revaluationDataGridView.Name = "revaluationDataGridView";
            revaluationDataGridView.RowHeadersVisible = false;
            revaluationDataGridView.Size = new System.Drawing.Size(1029, 325);
            revaluationDataGridView.TabIndex = 1;
            // 
            // searchPanel
            // 
            searchPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            searchPanel.Location = new System.Drawing.Point(0, 353);
            searchPanel.Name = "searchPanel";
            searchPanel.Size = new System.Drawing.Size(1029, 277);
            searchPanel.TabIndex = 2;
            // 
            // FormRevaluation
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1029, 630);
            Controls.Add(searchPanel);
            Controls.Add(revaluationDataGridView);
            Controls.Add(toolStrip1);
            Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "FormRevaluation";
            Text = "FormRevaluation";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)revaluationDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolButtonDeleteRow;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.DataGridView revaluationDataGridView;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripSynchGoods;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabelAmount;
    }
}