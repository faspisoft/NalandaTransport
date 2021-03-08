namespace faspi
{
    partial class Frm_GrSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.ansGridView1 = new faspiGrid.ansGridView(this.components);
            this.sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consigner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consignee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totquantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totweight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.details = new System.Windows.Forms.DataGridViewButtonColumn();
            this.vi_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ansGridView2 = new faspiGrid.ansGridView(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.vdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entrytype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reffno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entrydate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enteredby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detail = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(45, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 18);
            this.label11.TabIndex = 113;
            this.label11.Text = "GRno";
            // 
            // textBox10
            // 
            this.textBox10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox10.Location = new System.Drawing.Point(110, 16);
            this.textBox10.Margin = new System.Windows.Forms.Padding(4);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(211, 24);
            this.textBox10.TabIndex = 1;
            // 
            // Button1
            // 
            this.Button1.BackColor = System.Drawing.Color.Blue;
            this.Button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button1.ForeColor = System.Drawing.Color.White;
            this.Button1.Location = new System.Drawing.Point(375, 7);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(96, 43);
            this.Button1.TabIndex = 2;
            this.Button1.Text = "Ok";
            this.Button1.UseVisualStyleBackColor = false;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // ansGridView1
            // 
            this.ansGridView1.AllowUserToAddRows = false;
            this.ansGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ansGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ansGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ansGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sno,
            this.grno,
            this.bookingdate,
            this.consigner,
            this.consignee,
            this.source,
            this.destination,
            this.totquantity,
            this.totweight,
            this.details,
            this.vi_id});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ansGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.ansGridView1.Location = new System.Drawing.Point(9, 72);
            this.ansGridView1.MultiSelect = false;
            this.ansGridView1.Name = "ansGridView1";
            this.ansGridView1.RowHeadersVisible = false;
            this.ansGridView1.Size = new System.Drawing.Size(1002, 167);
            this.ansGridView1.TabIndex = 114;
            this.ansGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView1_CellClick);
            // 
            // sno
            // 
            this.sno.HeaderText = "Sno";
            this.sno.Name = "sno";
            this.sno.ReadOnly = true;
            this.sno.Width = 40;
            // 
            // grno
            // 
            this.grno.HeaderText = "Grno";
            this.grno.Name = "grno";
            this.grno.ReadOnly = true;
            // 
            // bookingdate
            // 
            this.bookingdate.HeaderText = "BookingDate";
            this.bookingdate.Name = "bookingdate";
            this.bookingdate.ReadOnly = true;
            // 
            // consigner
            // 
            this.consigner.HeaderText = "Consigner";
            this.consigner.Name = "consigner";
            this.consigner.ReadOnly = true;
            this.consigner.Width = 120;
            // 
            // consignee
            // 
            this.consignee.HeaderText = "Consignee";
            this.consignee.Name = "consignee";
            this.consignee.ReadOnly = true;
            this.consignee.Width = 120;
            // 
            // source
            // 
            this.source.HeaderText = "Source";
            this.source.Name = "source";
            this.source.ReadOnly = true;
            // 
            // destination
            // 
            this.destination.HeaderText = "Destination";
            this.destination.Name = "destination";
            this.destination.ReadOnly = true;
            // 
            // totquantity
            // 
            this.totquantity.HeaderText = "Quantity";
            this.totquantity.Name = "totquantity";
            this.totquantity.ReadOnly = true;
            this.totquantity.Width = 80;
            // 
            // totweight
            // 
            this.totweight.HeaderText = "Weight";
            this.totweight.Name = "totweight";
            this.totweight.ReadOnly = true;
            this.totweight.Width = 80;
            // 
            // details
            // 
            this.details.HeaderText = "Details";
            this.details.Name = "details";
            this.details.Text = "Details";
            this.details.ToolTipText = "Details";
            this.details.UseColumnTextForButtonValue = true;
            this.details.Width = 80;
            // 
            // vi_id
            // 
            this.vi_id.HeaderText = "vi_id";
            this.vi_id.Name = "vi_id";
            this.vi_id.ReadOnly = true;
            this.vi_id.Visible = false;
            // 
            // ansGridView2
            // 
            this.ansGridView2.AllowUserToAddRows = false;
            this.ansGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ansGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ansGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ansGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vdate,
            this.location,
            this.entrytype,
            this.reffno,
            this.entrydate,
            this.enteredby,
            this.viid,
            this.detail});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ansGridView2.DefaultCellStyle = dataGridViewCellStyle4;
            this.ansGridView2.Location = new System.Drawing.Point(76, 268);
            this.ansGridView2.MultiSelect = false;
            this.ansGridView2.Name = "ansGridView2";
            this.ansGridView2.RowHeadersVisible = false;
            this.ansGridView2.Size = new System.Drawing.Size(807, 250);
            this.ansGridView2.TabIndex = 115;
            this.ansGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView2_CellClick);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Blue;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(494, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 43);
            this.button2.TabIndex = 116;
            this.button2.Text = "Close (Esc)";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // vdate
            // 
            this.vdate.HeaderText = "Vdate";
            this.vdate.Name = "vdate";
            this.vdate.ReadOnly = true;
            // 
            // location
            // 
            this.location.HeaderText = "Location";
            this.location.Name = "location";
            this.location.ReadOnly = true;
            this.location.Width = 120;
            // 
            // entrytype
            // 
            this.entrytype.HeaderText = "EntryType";
            this.entrytype.Name = "entrytype";
            this.entrytype.ReadOnly = true;
            this.entrytype.Width = 150;
            // 
            // reffno
            // 
            this.reffno.HeaderText = "ReffNo";
            this.reffno.Name = "reffno";
            this.reffno.ReadOnly = true;
            // 
            // entrydate
            // 
            this.entrydate.HeaderText = "EntryDate";
            this.entrydate.Name = "entrydate";
            this.entrydate.ReadOnly = true;
            // 
            // enteredby
            // 
            this.enteredby.HeaderText = "EnteredBy";
            this.enteredby.Name = "enteredby";
            this.enteredby.ReadOnly = true;
            this.enteredby.Width = 120;
            // 
            // viid
            // 
            this.viid.HeaderText = "vi_id";
            this.viid.Name = "viid";
            this.viid.Visible = false;
            // 
            // detail
            // 
            this.detail.HeaderText = "Details";
            this.detail.Name = "detail";
            this.detail.Text = "View";
            this.detail.ToolTipText = "View";
            this.detail.UseColumnTextForButtonValue = true;
            // 
            // Frm_GrSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1018, 744);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ansGridView2);
            this.Controls.Add(this.ansGridView1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.Button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "Frm_GrSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_GrSearch";
            this.Load += new System.EventHandler(this.Frm_GrSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_GrSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox10;
        internal System.Windows.Forms.Button Button1;
        private faspiGrid.ansGridView ansGridView1;
        private faspiGrid.ansGridView ansGridView2;
        internal System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn grno;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn consigner;
        private System.Windows.Forms.DataGridViewTextBoxColumn consignee;
        private System.Windows.Forms.DataGridViewTextBoxColumn source;
        private System.Windows.Forms.DataGridViewTextBoxColumn destination;
        private System.Windows.Forms.DataGridViewTextBoxColumn totquantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn totweight;
        private System.Windows.Forms.DataGridViewButtonColumn details;
        private System.Windows.Forms.DataGridViewTextBoxColumn vi_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn vdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn location;
        private System.Windows.Forms.DataGridViewTextBoxColumn entrytype;
        private System.Windows.Forms.DataGridViewTextBoxColumn reffno;
        private System.Windows.Forms.DataGridViewTextBoxColumn entrydate;
        private System.Windows.Forms.DataGridViewTextBoxColumn enteredby;
        private System.Windows.Forms.DataGridViewTextBoxColumn viid;
        private System.Windows.Forms.DataGridViewButtonColumn detail;
    }
}