namespace NewRn
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpFinish = new System.Windows.Forms.DateTimePicker();
            this.cmbDepartments = new System.Windows.Forms.ComboBox();
            this.grdPrices = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.r1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.r2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prihod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otgruz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vozvr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inv_spis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prihod_all = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realiz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realiz_opt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vozvkass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realiz_all = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.proc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_otdel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmPrint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.print = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCount = new System.Windows.Forms.Button();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.optCheckBox = new System.Windows.Forms.CheckBox();
            this.countAllWorker = new System.ComponentModel.BackgroundWorker();
            this.countOneWorker = new System.ComponentModel.BackgroundWorker();
            this.chkRemains = new System.Windows.Forms.CheckBox();
            this.cbShipped = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.srcPrices = new System.Windows.Forms.BindingSource(this.components);
            this.SettingsButton = new System.Windows.Forms.Button();
            this.txtPrihod = new System.Windows.Forms.TextBox();
            this.lblRemainStart = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSumRemainStart = new System.Windows.Forms.TextBox();
            this.txtSumProcent = new System.Windows.Forms.TextBox();
            this.lblRemainFinish = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSumRN = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSumReal = new System.Windows.Forms.TextBox();
            this.txtSumRemainFinish = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.gBGroup = new System.Windows.Forms.GroupBox();
            this.rBGrp2 = new System.Windows.Forms.RadioButton();
            this.rBGpr1 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btSave = new System.Windows.Forms.Button();
            this.btCalc = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chbWithInvSpis = new System.Windows.Forms.CheckBox();
            this.pLegend = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdPrices)).BeginInit();
            this.cmPrint.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.srcPrices)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.gBGroup.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "с:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "по:";
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(33, 28);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(145, 23);
            this.dtpStart.TabIndex = 5;
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
            // 
            // dtpFinish
            // 
            this.dtpFinish.Location = new System.Drawing.Point(218, 28);
            this.dtpFinish.Name = "dtpFinish";
            this.dtpFinish.Size = new System.Drawing.Size(147, 23);
            this.dtpFinish.TabIndex = 6;
            this.dtpFinish.ValueChanged += new System.EventHandler(this.dtpFinish_ValueChanged);
            // 
            // cmbDepartments
            // 
            this.cmbDepartments.DisplayMember = "name";
            this.cmbDepartments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartments.FormattingEnabled = true;
            this.cmbDepartments.Location = new System.Drawing.Point(6, 26);
            this.cmbDepartments.Name = "cmbDepartments";
            this.cmbDepartments.Size = new System.Drawing.Size(199, 24);
            this.cmbDepartments.TabIndex = 7;
            this.cmbDepartments.ValueMember = "id";
            this.cmbDepartments.SelectedIndexChanged += new System.EventHandler(this.cmbDepartments_SelectedIndexChanged);
            // 
            // grdPrices
            // 
            this.grdPrices.AllowUserToAddRows = false;
            this.grdPrices.AllowUserToDeleteRows = false;
            this.grdPrices.AllowUserToResizeRows = false;
            this.grdPrices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPrices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdPrices.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdPrices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPrices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.cname,
            this.r1,
            this.r2,
            this.prihod,
            this.otgruz,
            this.vozvr,
            this.spis,
            this.inv_spis,
            this.prihod_all,
            this.realiz,
            this.realiz_opt,
            this.vozvkass,
            this.realiz_all,
            this.rn,
            this.proc,
            this.id_otdel});
            this.grdPrices.ContextMenuStrip = this.cmPrint;
            this.grdPrices.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdPrices.Location = new System.Drawing.Point(16, 125);
            this.grdPrices.MultiSelect = false;
            this.grdPrices.Name = "grdPrices";
            this.grdPrices.ReadOnly = true;
            this.grdPrices.RowHeadersVisible = false;
            this.grdPrices.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grdPrices.RowTemplate.Height = 24;
            this.grdPrices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdPrices.Size = new System.Drawing.Size(1254, 492);
            this.grdPrices.TabIndex = 10;
            this.grdPrices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdPrices_CellContentClick);
            this.grdPrices.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdPrices_CellContentDoubleClick);
            this.grdPrices.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grdPrices_RowPostPaint);
            this.grdPrices.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.grdPrices_RowPrePaint);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.id.DataPropertyName = "id";
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.id.DefaultCellStyle = dataGridViewCellStyle17;
            this.id.HeaderText = "Номер";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cname
            // 
            this.cname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cname.DataPropertyName = "cname";
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cname.DefaultCellStyle = dataGridViewCellStyle18;
            this.cname.HeaderText = "Наименование";
            this.cname.Name = "cname";
            this.cname.ReadOnly = true;
            this.cname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // r1
            // 
            this.r1.DataPropertyName = "r1";
            dataGridViewCellStyle19.Format = "N2";
            dataGridViewCellStyle19.NullValue = null;
            this.r1.DefaultCellStyle = dataGridViewCellStyle19;
            this.r1.HeaderText = "Остатки на начало";
            this.r1.Name = "r1";
            this.r1.ReadOnly = true;
            // 
            // r2
            // 
            this.r2.DataPropertyName = "r2";
            dataGridViewCellStyle20.Format = "N2";
            dataGridViewCellStyle20.NullValue = null;
            this.r2.DefaultCellStyle = dataGridViewCellStyle20;
            this.r2.HeaderText = "Остатки на конец";
            this.r2.Name = "r2";
            this.r2.ReadOnly = true;
            // 
            // prihod
            // 
            this.prihod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.prihod.DataPropertyName = "prihod";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle21.Format = "N2";
            dataGridViewCellStyle21.NullValue = null;
            this.prihod.DefaultCellStyle = dataGridViewCellStyle21;
            this.prihod.HeaderText = "Приход";
            this.prihod.Name = "prihod";
            this.prihod.ReadOnly = true;
            this.prihod.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // otgruz
            // 
            this.otgruz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.otgruz.DataPropertyName = "otgruz";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle22.Format = "N2";
            dataGridViewCellStyle22.NullValue = null;
            this.otgruz.DefaultCellStyle = dataGridViewCellStyle22;
            this.otgruz.HeaderText = "Отгрузка";
            this.otgruz.Name = "otgruz";
            this.otgruz.ReadOnly = true;
            this.otgruz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // vozvr
            // 
            this.vozvr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.vozvr.DataPropertyName = "vozvr";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle23.Format = "N2";
            this.vozvr.DefaultCellStyle = dataGridViewCellStyle23;
            this.vozvr.HeaderText = "Возврат";
            this.vozvr.Name = "vozvr";
            this.vozvr.ReadOnly = true;
            this.vozvr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // spis
            // 
            this.spis.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.spis.DataPropertyName = "spis";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle24.Format = "N2";
            this.spis.DefaultCellStyle = dataGridViewCellStyle24;
            this.spis.HeaderText = "Списание";
            this.spis.Name = "spis";
            this.spis.ReadOnly = true;
            this.spis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // inv_spis
            // 
            this.inv_spis.DataPropertyName = "spis_inv";
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle25.Format = "N2";
            this.inv_spis.DefaultCellStyle = dataGridViewCellStyle25;
            this.inv_spis.HeaderText = "Списание Инв.";
            this.inv_spis.Name = "inv_spis";
            this.inv_spis.ReadOnly = true;
            // 
            // prihod_all
            // 
            this.prihod_all.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.prihod_all.DataPropertyName = "prihod_all";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle26.Format = "N2";
            this.prihod_all.DefaultCellStyle = dataGridViewCellStyle26;
            this.prihod_all.HeaderText = "Общ. приход";
            this.prihod_all.Name = "prihod_all";
            this.prihod_all.ReadOnly = true;
            // 
            // realiz
            // 
            this.realiz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.realiz.DataPropertyName = "realiz";
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle27.Format = "N2";
            dataGridViewCellStyle27.NullValue = null;
            this.realiz.DefaultCellStyle = dataGridViewCellStyle27;
            this.realiz.HeaderText = "Реализация";
            this.realiz.Name = "realiz";
            this.realiz.ReadOnly = true;
            this.realiz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // realiz_opt
            // 
            this.realiz_opt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.realiz_opt.DataPropertyName = "realiz_opt";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle28.Format = "N2";
            this.realiz_opt.DefaultCellStyle = dataGridViewCellStyle28;
            this.realiz_opt.HeaderText = "Опт. отгрузки";
            this.realiz_opt.Name = "realiz_opt";
            this.realiz_opt.ReadOnly = true;
            // 
            // vozvkass
            // 
            this.vozvkass.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.vozvkass.DataPropertyName = "vozvkass";
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle29.Format = "N2";
            this.vozvkass.DefaultCellStyle = dataGridViewCellStyle29;
            this.vozvkass.HeaderText = "Возвраты с касс";
            this.vozvkass.Name = "vozvkass";
            this.vozvkass.ReadOnly = true;
            // 
            // realiz_all
            // 
            this.realiz_all.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.realiz_all.DataPropertyName = "realiz_all";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle30.Format = "N2";
            this.realiz_all.DefaultCellStyle = dataGridViewCellStyle30;
            this.realiz_all.HeaderText = "Общ. реализация";
            this.realiz_all.Name = "realiz_all";
            this.realiz_all.ReadOnly = true;
            // 
            // rn
            // 
            this.rn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rn.DataPropertyName = "rn";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle31.Format = "N2";
            dataGridViewCellStyle31.NullValue = null;
            this.rn.DefaultCellStyle = dataGridViewCellStyle31;
            this.rn.HeaderText = "РН";
            this.rn.Name = "rn";
            this.rn.ReadOnly = true;
            this.rn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // proc
            // 
            this.proc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.proc.DataPropertyName = "procent";
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle32.Format = "N2";
            dataGridViewCellStyle32.NullValue = null;
            this.proc.DefaultCellStyle = dataGridViewCellStyle32;
            this.proc.HeaderText = "Процент";
            this.proc.Name = "proc";
            this.proc.ReadOnly = true;
            this.proc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // id_otdel
            // 
            this.id_otdel.DataPropertyName = "id_otdel";
            this.id_otdel.HeaderText = "id_otdel";
            this.id_otdel.Name = "id_otdel";
            this.id_otdel.ReadOnly = true;
            this.id_otdel.Visible = false;
            // 
            // cmPrint
            // 
            this.cmPrint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.print});
            this.cmPrint.Name = "cmPrint";
            this.cmPrint.Size = new System.Drawing.Size(169, 26);
            this.cmPrint.Opening += new System.ComponentModel.CancelEventHandler(this.cmPrint_Opening);
            // 
            // print
            // 
            this.print.Name = "print";
            this.print.Size = new System.Drawing.Size(168, 22);
            this.print.Text = "Печать товаров...";
            this.print.Click += new System.EventHandler(this.print_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(17, 650);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 33);
            this.button1.TabIndex = 14;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCount
            // 
            this.btnCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCount.Image = ((System.Drawing.Image)(resources.GetObject("btnCount.Image")));
            this.btnCount.Location = new System.Drawing.Point(69, 650);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(33, 33);
            this.btnCount.TabIndex = 19;
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // progress
            // 
            this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progress.Location = new System.Drawing.Point(17, 88);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(1254, 23);
            this.progress.TabIndex = 20;
            // 
            // optCheckBox
            // 
            this.optCheckBox.AutoSize = true;
            this.optCheckBox.Location = new System.Drawing.Point(6, 21);
            this.optCheckBox.Name = "optCheckBox";
            this.optCheckBox.Size = new System.Drawing.Size(198, 20);
            this.optCheckBox.TabIndex = 23;
            this.optCheckBox.Text = "Учитывать оптовые отгрузки";
            this.optCheckBox.UseVisualStyleBackColor = true;
            this.optCheckBox.CheckedChanged += new System.EventHandler(this.optCheckBox_CheckedChanged);
            // 
            // chkRemains
            // 
            this.chkRemains.AutoSize = true;
            this.chkRemains.Location = new System.Drawing.Point(11, 11);
            this.chkRemains.Name = "chkRemains";
            this.chkRemains.Size = new System.Drawing.Size(120, 20);
            this.chkRemains.TabIndex = 24;
            this.chkRemains.Text = "Вывод остатков";
            this.chkRemains.UseVisualStyleBackColor = true;
            this.chkRemains.CheckedChanged += new System.EventHandler(this.chkRemains_CheckedChanged);
            // 
            // cbShipped
            // 
            this.cbShipped.AutoSize = true;
            this.cbShipped.Location = new System.Drawing.Point(6, 44);
            this.cbShipped.Name = "cbShipped";
            this.cbShipped.Size = new System.Drawing.Size(149, 20);
            this.cbShipped.TabIndex = 25;
            this.cbShipped.Text = "только отгруженные";
            this.cbShipped.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optCheckBox);
            this.groupBox1.Controls.Add(this.cbShipped);
            this.groupBox1.Location = new System.Drawing.Point(863, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 76);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbDepartments);
            this.groupBox2.Location = new System.Drawing.Point(17, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(217, 61);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Отдел:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dtpStart);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.dtpFinish);
            this.groupBox3.Location = new System.Drawing.Point(240, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(377, 61);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Период расчёта:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkRemains);
            this.groupBox4.Location = new System.Drawing.Point(1081, 47);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(182, 35);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelServer});
            this.statusStrip1.Location = new System.Drawing.Point(0, 698);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1282, 22);
            this.statusStrip1.TabIndex = 34;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelServer
            // 
            this.toolStripStatusLabelServer.Name = "toolStripStatusLabelServer";
            this.toolStripStatusLabelServer.Size = new System.Drawing.Size(0, 17);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SettingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SettingsButton.Image = ((System.Drawing.Image)(resources.GetObject("SettingsButton.Image")));
            this.SettingsButton.Location = new System.Drawing.Point(121, 650);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(33, 33);
            this.SettingsButton.TabIndex = 35;
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // txtPrihod
            // 
            this.txtPrihod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrihod.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtPrihod.Location = new System.Drawing.Point(81, 14);
            this.txtPrihod.Name = "txtPrihod";
            this.txtPrihod.ReadOnly = true;
            this.txtPrihod.Size = new System.Drawing.Size(124, 23);
            this.txtPrihod.TabIndex = 22;
            // 
            // lblRemainStart
            // 
            this.lblRemainStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemainStart.AutoSize = true;
            this.lblRemainStart.Location = new System.Drawing.Point(26, 47);
            this.lblRemainStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemainStart.Name = "lblRemainStart";
            this.lblRemainStart.Size = new System.Drawing.Size(124, 16);
            this.lblRemainStart.TabIndex = 30;
            this.lblRemainStart.Text = "Остаток на начало:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 17);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 16);
            this.label7.TabIndex = 21;
            this.label7.Text = "Приход:";
            // 
            // txtSumRemainStart
            // 
            this.txtSumRemainStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSumRemainStart.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSumRemainStart.Location = new System.Drawing.Point(157, 44);
            this.txtSumRemainStart.Name = "txtSumRemainStart";
            this.txtSumRemainStart.ReadOnly = true;
            this.txtSumRemainStart.Size = new System.Drawing.Size(124, 23);
            this.txtSumRemainStart.TabIndex = 31;
            // 
            // txtSumProcent
            // 
            this.txtSumProcent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSumProcent.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtSumProcent.Location = new System.Drawing.Point(656, 14);
            this.txtSumProcent.Name = "txtSumProcent";
            this.txtSumProcent.ReadOnly = true;
            this.txtSumProcent.Size = new System.Drawing.Size(55, 23);
            this.txtSumProcent.TabIndex = 18;
            // 
            // lblRemainFinish
            // 
            this.lblRemainFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemainFinish.AutoSize = true;
            this.lblRemainFinish.Location = new System.Drawing.Point(298, 47);
            this.lblRemainFinish.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemainFinish.Name = "lblRemainFinish";
            this.lblRemainFinish.Size = new System.Drawing.Size(116, 16);
            this.lblRemainFinish.TabIndex = 32;
            this.lblRemainFinish.Text = "Остаток на конец:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(585, 16);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 25);
            this.label5.TabIndex = 17;
            this.label5.Text = "Процент:";
            // 
            // txtSumRN
            // 
            this.txtSumRN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSumRN.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtSumRN.Location = new System.Drawing.Point(467, 14);
            this.txtSumRN.Name = "txtSumRN";
            this.txtSumRN.ReadOnly = true;
            this.txtSumRN.Size = new System.Drawing.Size(111, 23);
            this.txtSumRN.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(432, 17);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "РН:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(212, 17);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Реализация:";
            // 
            // txtSumReal
            // 
            this.txtSumReal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSumReal.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtSumReal.Location = new System.Drawing.Point(301, 14);
            this.txtSumReal.Name = "txtSumReal";
            this.txtSumReal.ReadOnly = true;
            this.txtSumReal.Size = new System.Drawing.Size(124, 23);
            this.txtSumReal.TabIndex = 13;
            // 
            // txtSumRemainFinish
            // 
            this.txtSumRemainFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSumRemainFinish.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSumRemainFinish.Location = new System.Drawing.Point(421, 44);
            this.txtSumRemainFinish.Name = "txtSumRemainFinish";
            this.txtSumRemainFinish.ReadOnly = true;
            this.txtSumRemainFinish.Size = new System.Drawing.Size(124, 23);
            this.txtSumRemainFinish.TabIndex = 33;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.txtSumRemainFinish);
            this.groupBox5.Controls.Add(this.txtSumReal);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtSumRN);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.lblRemainFinish);
            this.groupBox5.Controls.Add(this.txtSumProcent);
            this.groupBox5.Controls.Add(this.txtSumRemainStart);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.lblRemainStart);
            this.groupBox5.Controls.Add(this.txtPrihod);
            this.groupBox5.Location = new System.Drawing.Point(273, 621);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(749, 72);
            this.groupBox5.TabIndex = 36;
            this.groupBox5.TabStop = false;
            // 
            // gBGroup
            // 
            this.gBGroup.Controls.Add(this.rBGrp2);
            this.gBGroup.Controls.Add(this.rBGpr1);
            this.gBGroup.Controls.Add(this.radioButton1);
            this.gBGroup.Location = new System.Drawing.Point(619, 6);
            this.gBGroup.Name = "gBGroup";
            this.gBGroup.Size = new System.Drawing.Size(240, 76);
            this.gBGroup.TabIndex = 20;
            this.gBGroup.TabStop = false;
            // 
            // rBGrp2
            // 
            this.rBGrp2.AutoSize = true;
            this.rBGrp2.Location = new System.Drawing.Point(19, 43);
            this.rBGrp2.Name = "rBGrp2";
            this.rBGrp2.Size = new System.Drawing.Size(194, 20);
            this.rBGrp2.TabIndex = 22;
            this.rBGrp2.TabStop = true;
            this.rBGrp2.Text = "Инвентаризационная группа";
            this.rBGrp2.UseVisualStyleBackColor = true;
            this.rBGrp2.Click += new System.EventHandler(this.rBGrp2_Click);
            // 
            // rBGpr1
            // 
            this.rBGpr1.AutoSize = true;
            this.rBGpr1.Location = new System.Drawing.Point(20, 20);
            this.rBGpr1.Name = "rBGpr1";
            this.rBGpr1.Size = new System.Drawing.Size(92, 20);
            this.rBGpr1.TabIndex = 21;
            this.rBGpr1.Text = "Т.У. группа";
            this.rBGpr1.UseVisualStyleBackColor = true;
            this.rBGpr1.Click += new System.EventHandler(this.rBGpr1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(19, 21);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(98, 20);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Enabled = false;
            this.btSave.Image = global::NewRn.Properties.Resources._006074_floppy_disk_ok;
            this.btSave.Location = new System.Drawing.Point(1229, 638);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(40, 40);
            this.btSave.TabIndex = 37;
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Visible = false;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btCalc
            // 
            this.btCalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCalc.Enabled = false;
            this.btCalc.Image = global::NewRn.Properties.Resources._11;
            this.btCalc.Location = new System.Drawing.Point(1183, 638);
            this.btCalc.Name = "btCalc";
            this.btCalc.Size = new System.Drawing.Size(40, 40);
            this.btCalc.TabIndex = 37;
            this.btCalc.UseVisualStyleBackColor = true;
            this.btCalc.Visible = false;
            this.btCalc.Click += new System.EventHandler(this.btCalc_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chbWithInvSpis);
            this.groupBox6.Location = new System.Drawing.Point(1081, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(182, 35);
            this.groupBox6.TabIndex = 30;
            this.groupBox6.TabStop = false;
            // 
            // chbWithInvSpis
            // 
            this.chbWithInvSpis.AutoSize = true;
            this.chbWithInvSpis.Checked = true;
            this.chbWithInvSpis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbWithInvSpis.Location = new System.Drawing.Point(11, 11);
            this.chbWithInvSpis.Name = "chbWithInvSpis";
            this.chbWithInvSpis.Size = new System.Drawing.Size(165, 20);
            this.chbWithInvSpis.TabIndex = 24;
            this.chbWithInvSpis.Text = "с учётом инв. списания";
            this.chbWithInvSpis.UseVisualStyleBackColor = true;
            // 
            // pLegend
            // 
            this.pLegend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pLegend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(178)))), ((int)(((byte)(255)))));
            this.pLegend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pLegend.Location = new System.Drawing.Point(16, 621);
            this.pLegend.Name = "pLegend";
            this.pLegend.Size = new System.Drawing.Size(17, 17);
            this.pLegend.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 622);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(230, 16);
            this.label3.TabIndex = 34;
            this.label3.Text = "Не совпадают данные при сравнение";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1282, 720);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pLegend);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.btCalc);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.gBGroup);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnCount);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.grdPrices);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "РН";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdPrices)).EndInit();
            this.cmPrint.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.srcPrices)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.gBGroup.ResumeLayout(false);
            this.gBGroup.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpFinish;
        private System.Windows.Forms.ComboBox cmbDepartments;
        private System.Windows.Forms.DataGridView grdPrices;
        private System.Windows.Forms.BindingSource srcPrices;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.CheckBox optCheckBox;
        private System.ComponentModel.BackgroundWorker countAllWorker;
        private System.ComponentModel.BackgroundWorker countOneWorker;
        private System.Windows.Forms.ContextMenuStrip cmPrint;
        private System.Windows.Forms.ToolStripMenuItem print;
        private System.Windows.Forms.CheckBox chkRemains;
        private System.Windows.Forms.CheckBox cbShipped;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelServer;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.TextBox txtPrihod;
        private System.Windows.Forms.Label lblRemainStart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSumRemainStart;
        private System.Windows.Forms.TextBox txtSumProcent;
        private System.Windows.Forms.Label lblRemainFinish;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSumRN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSumReal;
        private System.Windows.Forms.TextBox txtSumRemainFinish;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn cname;
        private System.Windows.Forms.DataGridViewTextBoxColumn r1;
        private System.Windows.Forms.DataGridViewTextBoxColumn r2;
        private System.Windows.Forms.DataGridViewTextBoxColumn prihod;
        private System.Windows.Forms.DataGridViewTextBoxColumn otgruz;
        private System.Windows.Forms.DataGridViewTextBoxColumn vozvr;
        private System.Windows.Forms.DataGridViewTextBoxColumn spis;
        private System.Windows.Forms.DataGridViewTextBoxColumn inv_spis;
        private System.Windows.Forms.DataGridViewTextBoxColumn prihod_all;
        private System.Windows.Forms.DataGridViewTextBoxColumn realiz;
        private System.Windows.Forms.DataGridViewTextBoxColumn realiz_opt;
        private System.Windows.Forms.DataGridViewTextBoxColumn vozvkass;
        private System.Windows.Forms.DataGridViewTextBoxColumn realiz_all;
        private System.Windows.Forms.DataGridViewTextBoxColumn rn;
        private System.Windows.Forms.DataGridViewTextBoxColumn proc;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_otdel;
        private System.Windows.Forms.GroupBox gBGroup;
        private System.Windows.Forms.RadioButton rBGrp2;
        private System.Windows.Forms.RadioButton rBGpr1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btCalc;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chbWithInvSpis;
        private System.Windows.Forms.Panel pLegend;
        private System.Windows.Forms.Label label3;
    }
}

