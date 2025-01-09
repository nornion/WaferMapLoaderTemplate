namespace ExtendedByDLL
{
    partial class Form1
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
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.buttonPreTable = new System.Windows.Forms.Button();
            this.labelDesc = new System.Windows.Forms.Label();
            this.buttonNextTable = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonInvokeMethod = new System.Windows.Forms.Button();
            this.labelIcon = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonOpenFiles = new System.Windows.Forms.Button();
            this.labelTabIdx = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(44, 17);
            this.StatusLabel.Text = "Ready";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.StatusProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 400);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(783, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusProgressBar
            // 
            this.StatusProgressBar.Name = "StatusProgressBar";
            this.StatusProgressBar.Size = new System.Drawing.Size(75, 16);
            this.StatusProgressBar.Visible = false;
            // 
            // buttonPreTable
            // 
            this.buttonPreTable.Enabled = false;
            this.buttonPreTable.Location = new System.Drawing.Point(251, 50);
            this.buttonPreTable.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPreTable.Name = "buttonPreTable";
            this.buttonPreTable.Size = new System.Drawing.Size(67, 22);
            this.buttonPreTable.TabIndex = 4;
            this.buttonPreTable.Text = "Pre Wafer";
            this.buttonPreTable.UseVisualStyleBackColor = true;
            this.buttonPreTable.Click += new System.EventHandler(this.buttonPreTable_Click);
            // 
            // labelDesc
            // 
            this.labelDesc.AutoSize = true;
            this.labelDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDesc.Location = new System.Drawing.Point(9, 28);
            this.labelDesc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(44, 17);
            this.labelDesc.TabIndex = 2;
            this.labelDesc.Text = "Desc:";
            // 
            // buttonNextTable
            // 
            this.buttonNextTable.Enabled = false;
            this.buttonNextTable.Location = new System.Drawing.Point(352, 50);
            this.buttonNextTable.Margin = new System.Windows.Forms.Padding(2);
            this.buttonNextTable.Name = "buttonNextTable";
            this.buttonNextTable.Size = new System.Drawing.Size(70, 22);
            this.buttonNextTable.TabIndex = 5;
            this.buttonNextTable.Text = "Next Wafer";
            this.buttonNextTable.UseVisualStyleBackColor = true;
            this.buttonNextTable.Click += new System.EventHandler(this.buttonNextTable_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(9, 3);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(39, 17);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Title:";
            // 
            // buttonInvokeMethod
            // 
            this.buttonInvokeMethod.Enabled = false;
            this.buttonInvokeMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInvokeMethod.Location = new System.Drawing.Point(107, 3);
            this.buttonInvokeMethod.Margin = new System.Windows.Forms.Padding(2);
            this.buttonInvokeMethod.Name = "buttonInvokeMethod";
            this.buttonInvokeMethod.Size = new System.Drawing.Size(311, 44);
            this.buttonInvokeMethod.TabIndex = 3;
            this.buttonInvokeMethod.Text = "Execute Load()";
            this.buttonInvokeMethod.UseVisualStyleBackColor = true;
            this.buttonInvokeMethod.Click += new System.EventHandler(this.buttonInvokeMethod_Click);
            // 
            // labelIcon
            // 
            this.labelIcon.AutoSize = true;
            this.labelIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIcon.Location = new System.Drawing.Point(9, 54);
            this.labelIcon.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(38, 17);
            this.labelIcon.TabIndex = 0;
            this.labelIcon.Text = "Icon:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.labelIcon);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.labelDesc);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(783, 76);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.buttonOpenFiles);
            this.panel2.Controls.Add(this.buttonInvokeMethod);
            this.panel2.Controls.Add(this.labelTabIdx);
            this.panel2.Controls.Add(this.buttonPreTable);
            this.panel2.Controls.Add(this.buttonNextTable);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(354, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(429, 76);
            this.panel2.TabIndex = 8;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(109, 51);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(137, 20);
            this.comboBox1.TabIndex = 10;
            // 
            // buttonOpenFiles
            // 
            this.buttonOpenFiles.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOpenFiles.Location = new System.Drawing.Point(3, 3);
            this.buttonOpenFiles.Name = "buttonOpenFiles";
            this.buttonOpenFiles.Size = new System.Drawing.Size(99, 69);
            this.buttonOpenFiles.TabIndex = 9;
            this.buttonOpenFiles.Text = "Open Files";
            this.buttonOpenFiles.UseVisualStyleBackColor = true;
            this.buttonOpenFiles.Click += new System.EventHandler(this.buttonOpenFiles_Click);
            // 
            // labelTabIdx
            // 
            this.labelTabIdx.AutoSize = true;
            this.labelTabIdx.Location = new System.Drawing.Point(321, 54);
            this.labelTabIdx.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTabIdx.Name = "labelTabIdx";
            this.labelTabIdx.Size = new System.Drawing.Size(23, 12);
            this.labelTabIdx.TabIndex = 7;
            this.labelTabIdx.Text = "0/0";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 76);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(783, 324);
            this.dataGridView1.TabIndex = 9;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "CSV|*.csv";
            this.openFileDialog1.Multiselect = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 422);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "NEDA WaferMapLoader Tester";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button buttonPreTable;
        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.Button buttonNextTable;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonInvokeMethod;
        private System.Windows.Forms.Label labelIcon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripProgressBar StatusProgressBar;
        private System.Windows.Forms.Label labelTabIdx;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonOpenFiles;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

