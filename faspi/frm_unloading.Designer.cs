namespace faspi
{
    partial class frm_unloading
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtTruckNo = new System.Windows.Forms.TextBox();
            this.ansGridView1 = new faspiGrid.ansGridView(this.components);
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtTotalWeight = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.booking_date1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vi_id1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grno1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consigner1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consignee1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.destination1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delivery1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grtype1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.private1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actweight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wt1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amt1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.freight1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dd1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.foc1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pay1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paid1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billed1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Private = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grcharge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.othcharge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.freight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(184, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(91, 51);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Unloading NO";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = ".";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd-MMM-yyyy";
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(10, 17);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(135, 26);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            this.dateTimePicker1.Enter += new System.EventHandler(this.dateTimePicker1_Enter);
            this.dateTimePicker1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker1_KeyDown);
            this.dateTimePicker1.Leave += new System.EventHandler(this.dateTimePicker1_Leave);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(872, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(144, 685);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Location = new System.Drawing.Point(12, 509);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(423, 117);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Narration";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(10, 18);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(407, 89);
            this.textBox1.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtTruckNo);
            this.groupBox3.Location = new System.Drawing.Point(295, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stock Transfer No";
            // 
            // txtTruckNo
            // 
            this.txtTruckNo.BackColor = System.Drawing.Color.White;
            this.txtTruckNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTruckNo.Location = new System.Drawing.Point(10, 18);
            this.txtTruckNo.Name = "txtTruckNo";
            this.txtTruckNo.ReadOnly = true;
            this.txtTruckNo.Size = new System.Drawing.Size(177, 26);
            this.txtTruckNo.TabIndex = 3;
            this.txtTruckNo.TextChanged += new System.EventHandler(this.txtTruckNo_TextChanged_1);
            this.txtTruckNo.Enter += new System.EventHandler(this.txtTruckNo_Enter);
            this.txtTruckNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTruckNo_KeyPress);
            this.txtTruckNo.Leave += new System.EventHandler(this.txtTruckNo_Leave);
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
            this.booking_date1,
            this.vi_id1,
            this.grno1,
            this.consigner1,
            this.consignee1,
            this.source1,
            this.destination1,
            this.delivery1,
            this.grtype1,
            this.private1,
            this.remark1,
            this.qty1,
            this.actweight,
            this.wt1,
            this.amt1,
            this.freight1,
            this.dd1,
            this.foc1,
            this.pay1,
            this.paid1,
            this.billed1,
            this.itemname,
            this.packing,
            this.Private,
            this.Remark,
            this.grcharge,
            this.othcharge,
            this.freight});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ansGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.ansGridView1.Location = new System.Drawing.Point(12, 75);
            this.ansGridView1.MultiSelect = false;
            this.ansGridView1.Name = "ansGridView1";
            this.ansGridView1.RowHeadersVisible = false;
            this.ansGridView1.Size = new System.Drawing.Size(832, 359);
            this.ansGridView1.TabIndex = 18;
            // 
            // textBox20
            // 
            this.textBox20.BackColor = System.Drawing.Color.White;
            this.textBox20.Enabled = false;
            this.textBox20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox20.Location = new System.Drawing.Point(483, 467);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(100, 22);
            this.textBox20.TabIndex = 198;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(380, 446);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(92, 18);
            this.label21.TabIndex = 197;
            this.label21.Text = "Total To Pay";
            // 
            // textBox19
            // 
            this.textBox19.BackColor = System.Drawing.Color.White;
            this.textBox19.Enabled = false;
            this.textBox19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox19.Location = new System.Drawing.Point(373, 469);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(100, 22);
            this.textBox19.TabIndex = 196;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(502, 446);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(82, 18);
            this.label20.TabIndex = 195;
            this.label20.Text = "Total Other";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(261, 446);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(91, 18);
            this.label17.TabIndex = 190;
            this.label17.Text = "Total Weight";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(135, 446);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 18);
            this.label11.TabIndex = 189;
            this.label11.Text = "Total Quantity";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(12, 446);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 18);
            this.label10.TabIndex = 188;
            this.label10.Text = "Total GR";
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.White;
            this.textBox7.Enabled = false;
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.Location = new System.Drawing.Point(134, 469);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 22);
            this.textBox7.TabIndex = 186;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(14, 469);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 22);
            this.textBox2.TabIndex = 185;
            // 
            // txtTotalWeight
            // 
            this.txtTotalWeight.BackColor = System.Drawing.Color.White;
            this.txtTotalWeight.Enabled = false;
            this.txtTotalWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalWeight.Location = new System.Drawing.Point(252, 469);
            this.txtTotalWeight.Name = "txtTotalWeight";
            this.txtTotalWeight.Size = new System.Drawing.Size(100, 22);
            this.txtTotalWeight.TabIndex = 184;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Red;
            this.label28.Location = new System.Drawing.Point(525, 19);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(23, 31);
            this.label28.TabIndex = 233;
            this.label28.Text = ".";
            this.label28.Visible = false;
            // 
            // booking_date1
            // 
            this.booking_date1.HeaderText = "Booking Date";
            this.booking_date1.Name = "booking_date1";
            this.booking_date1.ReadOnly = true;
            // 
            // vi_id1
            // 
            this.vi_id1.HeaderText = "vi_id";
            this.vi_id1.Name = "vi_id1";
            this.vi_id1.ReadOnly = true;
            this.vi_id1.Visible = false;
            // 
            // grno1
            // 
            this.grno1.HeaderText = "GRno";
            this.grno1.Name = "grno1";
            this.grno1.ReadOnly = true;
            // 
            // consigner1
            // 
            this.consigner1.HeaderText = "Consigner";
            this.consigner1.Name = "consigner1";
            this.consigner1.ReadOnly = true;
            // 
            // consignee1
            // 
            this.consignee1.HeaderText = "Consignee";
            this.consignee1.Name = "consignee1";
            this.consignee1.ReadOnly = true;
            // 
            // source1
            // 
            this.source1.HeaderText = "Source";
            this.source1.Name = "source1";
            this.source1.ReadOnly = true;
            this.source1.Visible = false;
            // 
            // destination1
            // 
            this.destination1.HeaderText = "Destination";
            this.destination1.Name = "destination1";
            this.destination1.ReadOnly = true;
            this.destination1.Visible = false;
            // 
            // delivery1
            // 
            this.delivery1.HeaderText = "Delivery Type";
            this.delivery1.Name = "delivery1";
            this.delivery1.ReadOnly = true;
            // 
            // grtype1
            // 
            this.grtype1.HeaderText = "GR Type";
            this.grtype1.Name = "grtype1";
            this.grtype1.ReadOnly = true;
            this.grtype1.Visible = false;
            // 
            // private1
            // 
            this.private1.HeaderText = "Private";
            this.private1.Name = "private1";
            this.private1.ReadOnly = true;
            this.private1.Visible = false;
            // 
            // remark1
            // 
            this.remark1.HeaderText = "Remark";
            this.remark1.Name = "remark1";
            this.remark1.ReadOnly = true;
            this.remark1.Visible = false;
            // 
            // qty1
            // 
            this.qty1.HeaderText = "Total Quantity";
            this.qty1.Name = "qty1";
            this.qty1.ReadOnly = true;
            // 
            // actweight
            // 
            this.actweight.HeaderText = "Total Actweight";
            this.actweight.Name = "actweight";
            this.actweight.ReadOnly = true;
            // 
            // wt1
            // 
            this.wt1.HeaderText = "Total Weight";
            this.wt1.Name = "wt1";
            this.wt1.ReadOnly = true;
            // 
            // amt1
            // 
            this.amt1.HeaderText = "Total Amount";
            this.amt1.Name = "amt1";
            this.amt1.ReadOnly = true;
            this.amt1.Visible = false;
            // 
            // freight1
            // 
            this.freight1.HeaderText = "Freight";
            this.freight1.Name = "freight1";
            this.freight1.ReadOnly = true;
            this.freight1.Visible = false;
            // 
            // dd1
            // 
            this.dd1.HeaderText = "Door Delivery";
            this.dd1.Name = "dd1";
            this.dd1.ReadOnly = true;
            this.dd1.Visible = false;
            // 
            // foc1
            // 
            this.foc1.HeaderText = "Total FOC";
            this.foc1.Name = "foc1";
            this.foc1.ReadOnly = true;
            // 
            // pay1
            // 
            this.pay1.HeaderText = "Total To Pay";
            this.pay1.Name = "pay1";
            this.pay1.ReadOnly = true;
            // 
            // paid1
            // 
            this.paid1.HeaderText = "Total Paid";
            this.paid1.Name = "paid1";
            this.paid1.ReadOnly = true;
            // 
            // billed1
            // 
            this.billed1.HeaderText = "Total T.B.B.";
            this.billed1.Name = "billed1";
            this.billed1.ReadOnly = true;
            // 
            // itemname
            // 
            this.itemname.HeaderText = "Itemname";
            this.itemname.Name = "itemname";
            this.itemname.ReadOnly = true;
            // 
            // packing
            // 
            this.packing.HeaderText = "Packing";
            this.packing.Name = "packing";
            this.packing.ReadOnly = true;
            // 
            // Private
            // 
            this.Private.HeaderText = "Private";
            this.Private.Name = "Private";
            this.Private.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.HeaderText = "Remark";
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // grcharge
            // 
            this.grcharge.HeaderText = "grcharge";
            this.grcharge.Name = "grcharge";
            this.grcharge.ReadOnly = true;
            // 
            // othcharge
            // 
            this.othcharge.HeaderText = "Othcharge";
            this.othcharge.Name = "othcharge";
            this.othcharge.ReadOnly = true;
            // 
            // freight
            // 
            this.freight.HeaderText = "freight";
            this.freight.Name = "freight";
            this.freight.ReadOnly = true;
            this.freight.Visible = false;
            // 
            // frm_unloading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1018, 744);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.textBox19);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.txtTotalWeight);
            this.Controls.Add(this.ansGridView1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "frm_unloading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_unloading";
            this.Load += new System.EventHandler(this.frm_unloading_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_unloading_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtTruckNo;
        private faspiGrid.ansGridView ansGridView1;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtTotalWeight;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.DataGridViewTextBoxColumn booking_date1;
        private System.Windows.Forms.DataGridViewTextBoxColumn vi_id1;
        private System.Windows.Forms.DataGridViewTextBoxColumn grno1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consigner1;
        private System.Windows.Forms.DataGridViewTextBoxColumn consignee1;
        private System.Windows.Forms.DataGridViewTextBoxColumn source1;
        private System.Windows.Forms.DataGridViewTextBoxColumn destination1;
        private System.Windows.Forms.DataGridViewTextBoxColumn delivery1;
        private System.Windows.Forms.DataGridViewTextBoxColumn grtype1;
        private System.Windows.Forms.DataGridViewTextBoxColumn private1;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark1;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty1;
        private System.Windows.Forms.DataGridViewTextBoxColumn actweight;
        private System.Windows.Forms.DataGridViewTextBoxColumn wt1;
        private System.Windows.Forms.DataGridViewTextBoxColumn amt1;
        private System.Windows.Forms.DataGridViewTextBoxColumn freight1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dd1;
        private System.Windows.Forms.DataGridViewTextBoxColumn foc1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pay1;
        private System.Windows.Forms.DataGridViewTextBoxColumn paid1;
        private System.Windows.Forms.DataGridViewTextBoxColumn billed1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemname;
        private System.Windows.Forms.DataGridViewTextBoxColumn packing;
        private System.Windows.Forms.DataGridViewTextBoxColumn Private;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn grcharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn othcharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn freight;

    }
}