namespace NewRn
{
    partial class frmRnCompareTovar
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btClose = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btPrint = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbDeps = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbGrp2 = new System.Windows.Forms.ComboBox();
            this.cmbGrp1 = new System.Windows.Forms.ComboBox();
            this.tbEan = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cEan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTypeCalc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOstStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOstEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPrihod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOtgruz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVozvr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSpis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSpisInv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPrihodGlob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRealiz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOtgruzOpt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVozvrKass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRealizGlob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRnPrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Image = global::NewRn.Properties.Resources.pict_close;
            this.btClose.Location = new System.Drawing.Point(1253, 534);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(48, 48);
            this.btClose.TabIndex = 6;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
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
            this.cEan,
            this.cName,
            this.cTypeCalc,
            this.cOstStart,
            this.cOstEnd,
            this.cPrihod,
            this.cOtgruz,
            this.cVozvr,
            this.cSpis,
            this.cSpisInv,
            this.cPrihodGlob,
            this.cRealiz,
            this.cOtgruzOpt,
            this.cVozvrKass,
            this.cRealizGlob,
            this.cRN,
            this.cRnPrc});
            this.dgvData.Location = new System.Drawing.Point(12, 138);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(1289, 390);
            this.dgvData.TabIndex = 5;
            this.dgvData.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvData_ColumnWidthChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(15, 548);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(21, 21);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 552);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "имеются различия в данных";
            // 
            // btPrint
            // 
            this.btPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPrint.Image = global::NewRn.Properties.Resources.WZPRINT;
            this.btPrint.Location = new System.Drawing.Point(1199, 534);
            this.btPrint.Name = "btPrint";
            this.btPrint.Size = new System.Drawing.Size(48, 48);
            this.btPrint.TabIndex = 6;
            this.btPrint.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbDeps);
            this.groupBox2.Location = new System.Drawing.Point(15, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(217, 84);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Отдел:";
            // 
            // cmbDeps
            // 
            this.cmbDeps.DisplayMember = "name";
            this.cmbDeps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeps.FormattingEnabled = true;
            this.cmbDeps.Location = new System.Drawing.Point(6, 26);
            this.cmbDeps.Name = "cmbDeps";
            this.cmbDeps.Size = new System.Drawing.Size(199, 21);
            this.cmbDeps.TabIndex = 7;
            this.cmbDeps.ValueMember = "id";
            this.cmbDeps.SelectionChangeCommitted += new System.EventHandler(this.cmbDeps_SelectionChangeCommitted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbGrp2);
            this.groupBox1.Controls.Add(this.cmbGrp1);
            this.groupBox1.Location = new System.Drawing.Point(238, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 84);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Группы товаров";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Инв.группа";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Т/У группа";
            // 
            // cmbGrp2
            // 
            this.cmbGrp2.DisplayMember = "name";
            this.cmbGrp2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrp2.FormattingEnabled = true;
            this.cmbGrp2.Location = new System.Drawing.Point(115, 46);
            this.cmbGrp2.Name = "cmbGrp2";
            this.cmbGrp2.Size = new System.Drawing.Size(199, 21);
            this.cmbGrp2.TabIndex = 7;
            this.cmbGrp2.ValueMember = "id";
            this.cmbGrp2.SelectionChangeCommitted += new System.EventHandler(this.cmbGrp2_SelectionChangeCommitted);
            // 
            // cmbGrp1
            // 
            this.cmbGrp1.DisplayMember = "name";
            this.cmbGrp1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrp1.FormattingEnabled = true;
            this.cmbGrp1.Location = new System.Drawing.Point(115, 19);
            this.cmbGrp1.Name = "cmbGrp1";
            this.cmbGrp1.Size = new System.Drawing.Size(199, 21);
            this.cmbGrp1.TabIndex = 7;
            this.cmbGrp1.ValueMember = "id";
            this.cmbGrp1.SelectionChangeCommitted += new System.EventHandler(this.cmbGrp1_SelectionChangeCommitted);
            // 
            // tbEan
            // 
            this.tbEan.Location = new System.Drawing.Point(12, 112);
            this.tbEan.MaxLength = 13;
            this.tbEan.Name = "tbEan";
            this.tbEan.Size = new System.Drawing.Size(223, 20);
            this.tbEan.TabIndex = 29;
            this.tbEan.TextChanged += new System.EventHandler(this.tbEan_TextChanged);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(238, 112);
            this.tbName.MaxLength = 250;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(223, 20);
            this.tbName.TabIndex = 29;
            this.tbName.TextChanged += new System.EventHandler(this.tbEan_TextChanged);
            // 
            // cEan
            // 
            this.cEan.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cEan.DataPropertyName = "ean";
            this.cEan.Frozen = true;
            this.cEan.HeaderText = "EAN";
            this.cEan.MinimumWidth = 90;
            this.cEan.Name = "cEan";
            this.cEan.ReadOnly = true;
            this.cEan.Width = 90;
            // 
            // cName
            // 
            this.cName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cName.DataPropertyName = "cname";
            this.cName.Frozen = true;
            this.cName.HeaderText = "Наименование товара";
            this.cName.MinimumWidth = 150;
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.Width = 150;
            // 
            // cTypeCalc
            // 
            this.cTypeCalc.HeaderText = "Тип расчета";
            this.cTypeCalc.Name = "cTypeCalc";
            this.cTypeCalc.ReadOnly = true;
            // 
            // cOstStart
            // 
            this.cOstStart.DataPropertyName = "r1";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.cOstStart.DefaultCellStyle = dataGridViewCellStyle2;
            this.cOstStart.HeaderText = "Остаток на начало";
            this.cOstStart.Name = "cOstStart";
            this.cOstStart.ReadOnly = true;
            // 
            // cOstEnd
            // 
            this.cOstEnd.DataPropertyName = "r2";
            dataGridViewCellStyle3.Format = "N2";
            this.cOstEnd.DefaultCellStyle = dataGridViewCellStyle3;
            this.cOstEnd.HeaderText = "Остаток на конец";
            this.cOstEnd.Name = "cOstEnd";
            this.cOstEnd.ReadOnly = true;
            // 
            // cPrihod
            // 
            this.cPrihod.DataPropertyName = "prihod";
            dataGridViewCellStyle4.Format = "N2";
            this.cPrihod.DefaultCellStyle = dataGridViewCellStyle4;
            this.cPrihod.HeaderText = "Приход";
            this.cPrihod.Name = "cPrihod";
            this.cPrihod.ReadOnly = true;
            // 
            // cOtgruz
            // 
            this.cOtgruz.DataPropertyName = "otgruz";
            dataGridViewCellStyle5.Format = "N2";
            this.cOtgruz.DefaultCellStyle = dataGridViewCellStyle5;
            this.cOtgruz.HeaderText = "Отгрузка";
            this.cOtgruz.Name = "cOtgruz";
            this.cOtgruz.ReadOnly = true;
            // 
            // cVozvr
            // 
            this.cVozvr.DataPropertyName = "vozvr";
            dataGridViewCellStyle6.Format = "N2";
            this.cVozvr.DefaultCellStyle = dataGridViewCellStyle6;
            this.cVozvr.HeaderText = "Возврат";
            this.cVozvr.Name = "cVozvr";
            this.cVozvr.ReadOnly = true;
            // 
            // cSpis
            // 
            this.cSpis.DataPropertyName = "spis";
            dataGridViewCellStyle7.Format = "N2";
            this.cSpis.DefaultCellStyle = dataGridViewCellStyle7;
            this.cSpis.HeaderText = "Списание";
            this.cSpis.Name = "cSpis";
            this.cSpis.ReadOnly = true;
            // 
            // cSpisInv
            // 
            this.cSpisInv.DataPropertyName = "spis_inv";
            dataGridViewCellStyle8.Format = "N2";
            this.cSpisInv.DefaultCellStyle = dataGridViewCellStyle8;
            this.cSpisInv.HeaderText = "Списание Инв.";
            this.cSpisInv.Name = "cSpisInv";
            this.cSpisInv.ReadOnly = true;
            // 
            // cPrihodGlob
            // 
            this.cPrihodGlob.DataPropertyName = "prihod_all";
            dataGridViewCellStyle9.Format = "N2";
            this.cPrihodGlob.DefaultCellStyle = dataGridViewCellStyle9;
            this.cPrihodGlob.HeaderText = "Общий приход";
            this.cPrihodGlob.Name = "cPrihodGlob";
            this.cPrihodGlob.ReadOnly = true;
            // 
            // cRealiz
            // 
            this.cRealiz.DataPropertyName = "realiz";
            dataGridViewCellStyle10.Format = "N2";
            this.cRealiz.DefaultCellStyle = dataGridViewCellStyle10;
            this.cRealiz.HeaderText = "Реализация";
            this.cRealiz.Name = "cRealiz";
            this.cRealiz.ReadOnly = true;
            // 
            // cOtgruzOpt
            // 
            this.cOtgruzOpt.DataPropertyName = "realiz_opt";
            dataGridViewCellStyle11.Format = "N2";
            this.cOtgruzOpt.DefaultCellStyle = dataGridViewCellStyle11;
            this.cOtgruzOpt.HeaderText = "Опт. отгрузки";
            this.cOtgruzOpt.Name = "cOtgruzOpt";
            this.cOtgruzOpt.ReadOnly = true;
            // 
            // cVozvrKass
            // 
            this.cVozvrKass.DataPropertyName = "vozvkass";
            dataGridViewCellStyle12.Format = "N2";
            this.cVozvrKass.DefaultCellStyle = dataGridViewCellStyle12;
            this.cVozvrKass.HeaderText = "Возвраты с касс";
            this.cVozvrKass.Name = "cVozvrKass";
            this.cVozvrKass.ReadOnly = true;
            // 
            // cRealizGlob
            // 
            this.cRealizGlob.DataPropertyName = "realiz_all";
            dataGridViewCellStyle13.Format = "N2";
            this.cRealizGlob.DefaultCellStyle = dataGridViewCellStyle13;
            this.cRealizGlob.HeaderText = "Общ. реализация";
            this.cRealizGlob.Name = "cRealizGlob";
            this.cRealizGlob.ReadOnly = true;
            // 
            // cRN
            // 
            this.cRN.DataPropertyName = "rn";
            dataGridViewCellStyle14.Format = "N2";
            this.cRN.DefaultCellStyle = dataGridViewCellStyle14;
            this.cRN.HeaderText = "РН";
            this.cRN.Name = "cRN";
            this.cRN.ReadOnly = true;
            // 
            // cRnPrc
            // 
            this.cRnPrc.DataPropertyName = "procent";
            dataGridViewCellStyle15.Format = "N2";
            this.cRnPrc.DefaultCellStyle = dataGridViewCellStyle15;
            this.cRnPrc.HeaderText = "Процент РН";
            this.cRnPrc.Name = "cRnPrc";
            this.cRnPrc.ReadOnly = true;
            // 
            // frmRnCompareTovar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1313, 601);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbEan);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btPrint);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.dgvData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmRnCompareTovar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сравнение РН по товарам";
            this.Load += new System.EventHandler(this.frmRnCompareTovar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btPrint;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbDeps;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbGrp1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbGrp2;
        private System.Windows.Forms.TextBox tbEan;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEan;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTypeCalc;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOstStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOstEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPrihod;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOtgruz;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVozvr;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSpis;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSpisInv;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPrihodGlob;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRealiz;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOtgruzOpt;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVozvrKass;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRealizGlob;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRN;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRnPrc;
    }
}