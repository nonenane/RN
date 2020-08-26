namespace NewRn
{
    partial class GoodsDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoodsDialog));
            this.grdPrices = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ean = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.r1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.r2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prihod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realiz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_grp1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxEAN = new System.Windows.Forms.TextBox();
            this.textBoxGoods = new System.Windows.Forms.TextBox();
            this.labelEAN = new System.Windows.Forms.Label();
            this.labelGoods = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdPrices)).BeginInit();
            this.SuspendLayout();
            // 
            // grdPrices
            // 
            this.grdPrices.AllowUserToAddRows = false;
            this.grdPrices.AllowUserToDeleteRows = false;
            this.grdPrices.AllowUserToResizeRows = false;
            this.grdPrices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdPrices.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdPrices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdPrices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdPrices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPrices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.ean,
            this.cname,
            this.r1,
            this.r2,
            this.prihod,
            this.realiz,
            this.rn,
            this.id_grp1});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdPrices.DefaultCellStyle = dataGridViewCellStyle4;
            this.grdPrices.Location = new System.Drawing.Point(12, 46);
            this.grdPrices.Name = "grdPrices";
            this.grdPrices.ReadOnly = true;
            this.grdPrices.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdPrices.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.grdPrices.RowHeadersVisible = false;
            this.grdPrices.Size = new System.Drawing.Size(758, 508);
            this.grdPrices.TabIndex = 11;
            this.grdPrices.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdPrices_RowsAdded);
            this.grdPrices.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.grdPrices_RowsRemoved);
            this.grdPrices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdPrices_CellContentClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.id.DefaultCellStyle = dataGridViewCellStyle2;
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // ean
            // 
            this.ean.DataPropertyName = "ean";
            this.ean.HeaderText = "EAN";
            this.ean.Name = "ean";
            this.ean.ReadOnly = true;
            // 
            // cname
            // 
            this.cname.DataPropertyName = "cname";
            this.cname.HeaderText = "Наименование товара";
            this.cname.Name = "cname";
            this.cname.ReadOnly = true;
            // 
            // r1
            // 
            this.r1.DataPropertyName = "r1";
            this.r1.HeaderText = "Остатки на начало";
            this.r1.Name = "r1";
            this.r1.ReadOnly = true;
            // 
            // r2
            // 
            this.r2.DataPropertyName = "r2";
            this.r2.HeaderText = "Остатки на конец";
            this.r2.Name = "r2";
            this.r2.ReadOnly = true;
            // 
            // prihod
            // 
            this.prihod.DataPropertyName = "prihod_all";
            this.prihod.HeaderText = "Приход";
            this.prihod.Name = "prihod";
            this.prihod.ReadOnly = true;
            // 
            // realiz
            // 
            this.realiz.DataPropertyName = "realiz_all";
            this.realiz.HeaderText = "Реализация";
            this.realiz.Name = "realiz";
            this.realiz.ReadOnly = true;
            // 
            // rn
            // 
            this.rn.DataPropertyName = "rn";
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.rn.DefaultCellStyle = dataGridViewCellStyle3;
            this.rn.HeaderText = "РН";
            this.rn.Name = "rn";
            this.rn.ReadOnly = true;
            // 
            // id_grp1
            // 
            this.id_grp1.HeaderText = "id_grp1";
            this.id_grp1.Name = "id_grp1";
            this.id_grp1.ReadOnly = true;
            this.id_grp1.Visible = false;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrint.Enabled = false;
            this.buttonPrint.Image = ((System.Drawing.Image)(resources.GetObject("buttonPrint.Image")));
            this.buttonPrint.Location = new System.Drawing.Point(696, 560);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(33, 33);
            this.buttonPrint.TabIndex = 15;
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(735, 560);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 33);
            this.button2.TabIndex = 16;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxEAN
            // 
            this.textBoxEAN.Location = new System.Drawing.Point(48, 15);
            this.textBoxEAN.MaxLength = 13;
            this.textBoxEAN.Name = "textBoxEAN";
            this.textBoxEAN.Size = new System.Drawing.Size(175, 20);
            this.textBoxEAN.TabIndex = 17;
            this.textBoxEAN.TextChanged += new System.EventHandler(this.textBoxEAN_TextChanged);
            this.textBoxEAN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxEAN_KeyPress);
            // 
            // textBoxGoods
            // 
            this.textBoxGoods.Location = new System.Drawing.Point(359, 15);
            this.textBoxGoods.Name = "textBoxGoods";
            this.textBoxGoods.Size = new System.Drawing.Size(409, 20);
            this.textBoxGoods.TabIndex = 18;
            this.textBoxGoods.TextChanged += new System.EventHandler(this.textBoxEAN_TextChanged);
            // 
            // labelEAN
            // 
            this.labelEAN.AutoSize = true;
            this.labelEAN.Location = new System.Drawing.Point(16, 19);
            this.labelEAN.Name = "labelEAN";
            this.labelEAN.Size = new System.Drawing.Size(29, 13);
            this.labelEAN.TabIndex = 19;
            this.labelEAN.Text = "EAN";
            // 
            // labelGoods
            // 
            this.labelGoods.AutoSize = true;
            this.labelGoods.Location = new System.Drawing.Point(229, 19);
            this.labelGoods.Name = "labelGoods";
            this.labelGoods.Size = new System.Drawing.Size(124, 13);
            this.labelGoods.TabIndex = 20;
            this.labelGoods.Text = "Наименование товара:";
            // 
            // GoodsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 598);
            this.Controls.Add(this.labelGoods);
            this.Controls.Add(this.labelEAN);
            this.Controls.Add(this.textBoxGoods);
            this.Controls.Add(this.textBoxEAN);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.grdPrices);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GoodsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Товары по группе";
            this.Load += new System.EventHandler(this.GoodsDialog_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GoodsDialog_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GoodsDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grdPrices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdPrices;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxEAN;
        private System.Windows.Forms.TextBox textBoxGoods;
        private System.Windows.Forms.Label labelEAN;
        private System.Windows.Forms.Label labelGoods;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ean;
        private System.Windows.Forms.DataGridViewTextBoxColumn cname;
        private System.Windows.Forms.DataGridViewTextBoxColumn r1;
        private System.Windows.Forms.DataGridViewTextBoxColumn r2;
        private System.Windows.Forms.DataGridViewTextBoxColumn prihod;
        private System.Windows.Forms.DataGridViewTextBoxColumn realiz;
        private System.Windows.Forms.DataGridViewTextBoxColumn rn;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_grp1;
    }
}