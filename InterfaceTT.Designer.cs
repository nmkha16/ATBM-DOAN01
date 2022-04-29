namespace ATBM_DOAN01
{
    partial class InterfaceTT
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
            this.dataTT = new System.Windows.Forms.DataGridView();
            this.comboBoxTT = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataTT)).BeginInit();
            this.SuspendLayout();
            // 
            // dataTT
            // 
            this.dataTT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTT.Location = new System.Drawing.Point(12, 113);
            this.dataTT.Name = "dataTT";
            this.dataTT.RowHeadersWidth = 51;
            this.dataTT.RowTemplate.Height = 29;
            this.dataTT.Size = new System.Drawing.Size(1091, 469);
            this.dataTT.TabIndex = 0;
            // 
            // comboBoxTT
            // 
            this.comboBoxTT.FormattingEnabled = true;
            this.comboBoxTT.Location = new System.Drawing.Point(450, 50);
            this.comboBoxTT.Name = "comboBoxTT";
            this.comboBoxTT.Size = new System.Drawing.Size(151, 28);
            this.comboBoxTT.TabIndex = 1;
            this.comboBoxTT.SelectedIndexChanged += new System.EventHandler(this.comboBoxTT_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(255, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "Chọn bảng:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(702, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InterfaceTT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 606);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxTT);
            this.Controls.Add(this.dataTT);
            this.Name = "InterfaceTT";
            this.Text = "InterfaceTT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InterfaceTT_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataTT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataTT;
        private ComboBox comboBoxTT;
        private Label label1;
        private Button button1;
    }
}