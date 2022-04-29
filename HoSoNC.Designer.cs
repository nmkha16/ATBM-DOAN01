namespace ATBM_DOAN01
{
    partial class HoSoNC
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
            this.dataNC = new System.Windows.Forms.DataGridView();
            this.comboBoxNC = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataNC)).BeginInit();
            this.SuspendLayout();
            // 
            // dataNC
            // 
            this.dataNC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataNC.Location = new System.Drawing.Point(12, 116);
            this.dataNC.Name = "dataNC";
            this.dataNC.RowHeadersWidth = 51;
            this.dataNC.RowTemplate.Height = 29;
            this.dataNC.Size = new System.Drawing.Size(776, 322);
            this.dataNC.TabIndex = 0;
            // 
            // comboBoxNC
            // 
            this.comboBoxNC.FormattingEnabled = true;
            this.comboBoxNC.Location = new System.Drawing.Point(176, 50);
            this.comboBoxNC.Name = "comboBoxNC";
            this.comboBoxNC.Size = new System.Drawing.Size(151, 28);
            this.comboBoxNC.TabIndex = 1;
            this.comboBoxNC.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "Chọn bảng:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(633, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HoSoNC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxNC);
            this.Controls.Add(this.dataNC);
            this.Name = "HoSoNC";
            this.Text = "HoSoNC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HoSoNC_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataNC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataNC;
        private ComboBox comboBoxNC;
        private Label label1;
        private Button button1;
    }
}