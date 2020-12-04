namespace NewRn
{
    partial class frmRNCompare
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btGetData = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.cPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTypeCalc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOstStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPrihod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRealiz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOstEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPrcRn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btClose = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chbInv = new System.Windows.Forms.CheckBox();
            this.chbOtgruz = new System.Windows.Forms.CheckBox();
            this.chbOpt = new System.Windows.Forms.CheckBox();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.tbFio = new System.Windows.Forms.TextBox();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpStart
            // 
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(74, 11);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(79, 20);
            this.dtpStart.TabIndex = 0;
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Период с ";
            // 
            // btGetData
            // 
            this.btGetData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btGetData.Location = new System.Drawing.Point(294, 6);
            this.btGetData.Name = "btGetData";
            this.btGetData.Size = new System.Drawing.Size(239, 31);
            this.btGetData.TabIndex = 2;
            this.btGetData.Text = "Рассчитать и сравнить";
            this.btGetData.UseVisualStyleBackColor = true;
            this.btGetData.Click += new System.EventHandler(this.btGetData_Click);
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeRows = false;
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cPeriod,
            this.cTypeCalc,
            this.cOstStart,
            this.cPrihod,
            this.cRealiz,
            this.cOstEnd,
            this.cRN,
            this.cPrcRn});
            this.dgvData.Location = new System.Drawing.Point(12, 43);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(1079, 406);
            this.dgvData.TabIndex = 3;
            this.dgvData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_CellMouseDoubleClick);
            this.dgvData.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvData_RowPostPaint);
            this.dgvData.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvData_RowPrePaint);
            this.dgvData.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
            // 
            // cPeriod
            // 
            this.cPeriod.DataPropertyName = "namePeriod";
            this.cPeriod.HeaderText = "Период расчета";
            this.cPeriod.Name = "cPeriod";
            this.cPeriod.ReadOnly = true;
            // 
            // cTypeCalc
            // 
            this.cTypeCalc.DataPropertyName = "typeCalc";
            this.cTypeCalc.HeaderText = "Тип расчета";
            this.cTypeCalc.Name = "cTypeCalc";
            this.cTypeCalc.ReadOnly = true;
            // 
            // cOstStart
            // 
            this.cOstStart.DataPropertyName = "TotalRestStart";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.cOstStart.DefaultCellStyle = dataGridViewCellStyle2;
            this.cOstStart.HeaderText = "∑ Остаток на начало";
            this.cOstStart.Name = "cOstStart";
            this.cOstStart.ReadOnly = true;
            // 
            // cPrihod
            // 
            this.cPrihod.DataPropertyName = "TotalPrihod";
            dataGridViewCellStyle3.Format = "N2";
            this.cPrihod.DefaultCellStyle = dataGridViewCellStyle3;
            this.cPrihod.HeaderText = "∑ Приход";
            this.cPrihod.Name = "cPrihod";
            this.cPrihod.ReadOnly = true;
            // 
            // cRealiz
            // 
            this.cRealiz.DataPropertyName = "TotalRealiz";
            dataGridViewCellStyle4.Format = "N2";
            this.cRealiz.DefaultCellStyle = dataGridViewCellStyle4;
            this.cRealiz.HeaderText = "∑ Реализация";
            this.cRealiz.Name = "cRealiz";
            this.cRealiz.ReadOnly = true;
            // 
            // cOstEnd
            // 
            this.cOstEnd.DataPropertyName = "TotalRestStop";
            dataGridViewCellStyle5.Format = "N2";
            this.cOstEnd.DefaultCellStyle = dataGridViewCellStyle5;
            this.cOstEnd.HeaderText = "∑ Остаток на конец";
            this.cOstEnd.Name = "cOstEnd";
            this.cOstEnd.ReadOnly = true;
            // 
            // cRN
            // 
            this.cRN.DataPropertyName = "TotalRN";
            dataGridViewCellStyle6.Format = "N2";
            this.cRN.DefaultCellStyle = dataGridViewCellStyle6;
            this.cRN.HeaderText = "∑ РН";
            this.cRN.Name = "cRN";
            this.cRN.ReadOnly = true;
            // 
            // cPrcRn
            // 
            this.cPrcRn.DataPropertyName = "TotalPercentRN";
            dataGridViewCellStyle7.Format = "N2";
            this.cPrcRn.DefaultCellStyle = dataGridViewCellStyle7;
            this.cPrcRn.HeaderText = "∑ Процент РН";
            this.cPrcRn.Name = "cPrcRn";
            this.cPrcRn.ReadOnly = true;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Image = global::NewRn.Properties.Resources.pict_close;
            this.btClose.Location = new System.Drawing.Point(1034, 487);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(48, 48);
            this.btClose.TabIndex = 4;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(39, 502);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(170, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "имеются различия в данных";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 500);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(21, 21);
            this.panel1.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chbInv);
            this.groupBox1.Controls.Add(this.chbOtgruz);
            this.groupBox1.Controls.Add(this.chbOpt);
            this.groupBox1.Controls.Add(this.tbDate);
            this.groupBox1.Controls.Add(this.tbFio);
            this.groupBox1.Location = new System.Drawing.Point(215, 455);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры сохранённых данных";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Дата сохранения";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Сохранил";
            // 
            // chbInv
            // 
            this.chbInv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbInv.AutoSize = true;
            this.chbInv.Enabled = false;
            this.chbInv.Location = new System.Drawing.Point(315, 70);
            this.chbInv.Name = "chbInv";
            this.chbInv.Size = new System.Drawing.Size(170, 17);
            this.chbInv.TabIndex = 5;
            this.chbInv.Text = "учитывать инвент. списание";
            this.chbInv.UseVisualStyleBackColor = true;
            // 
            // chbOtgruz
            // 
            this.chbOtgruz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbOtgruz.AutoSize = true;
            this.chbOtgruz.Enabled = false;
            this.chbOtgruz.Location = new System.Drawing.Point(315, 47);
            this.chbOtgruz.Name = "chbOtgruz";
            this.chbOtgruz.Size = new System.Drawing.Size(131, 17);
            this.chbOtgruz.TabIndex = 5;
            this.chbOtgruz.Text = "только отгруженные";
            this.chbOtgruz.UseVisualStyleBackColor = true;
            // 
            // chbOpt
            // 
            this.chbOpt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbOpt.AutoSize = true;
            this.chbOpt.Enabled = false;
            this.chbOpt.Location = new System.Drawing.Point(315, 24);
            this.chbOpt.Name = "chbOpt";
            this.chbOpt.Size = new System.Drawing.Size(172, 17);
            this.chbOpt.TabIndex = 5;
            this.chbOpt.Text = "учитывать оптовые отгрузки";
            this.chbOpt.UseVisualStyleBackColor = true;
            // 
            // tbDate
            // 
            this.tbDate.Location = new System.Drawing.Point(111, 55);
            this.tbDate.Name = "tbDate";
            this.tbDate.ReadOnly = true;
            this.tbDate.Size = new System.Drawing.Size(175, 20);
            this.tbDate.TabIndex = 8;
            // 
            // tbFio
            // 
            this.tbFio.Location = new System.Drawing.Point(71, 29);
            this.tbFio.Name = "tbFio";
            this.tbFio.ReadOnly = true;
            this.tbFio.Size = new System.Drawing.Size(215, 20);
            this.tbFio.TabIndex = 8;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(188, 11);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(79, 20);
            this.dtpEnd.TabIndex = 0;
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtpEnd_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "по";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(810, 11);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(281, 24);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 19;
            this.progressBar1.Visible = false;
            // 
            // frmRNCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 567);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.btGetData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmRNCompare";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сравнение расчётов РН";
            this.Load += new System.EventHandler(this.frmRNCompare_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btGetData;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbFio;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.CheckBox chbInv;
        private System.Windows.Forms.CheckBox chbOtgruz;
        private System.Windows.Forms.CheckBox chbOpt;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTypeCalc;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOstStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPrihod;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRealiz;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOstEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRN;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPrcRn;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}