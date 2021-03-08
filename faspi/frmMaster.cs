using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace faspi
{
    public partial class frmMaster : Form
    {
        string gstr = "";
        BindingSource bs = new BindingSource();
        DataTable dtitem = new DataTable();

        public frmMaster()
        {
            InitializeComponent();
        }

        public void LoadData(string str, string frmCaption)
        {
            gstr = str;
            string sql = "";
            dtitem.Clear();
            this.Text = frmCaption;
            label2.Text = "List of " + str;
            if (str == "Delivery Point")
            {
                sql = "SELECT [name] from DeliveryPoints order by [name]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;                
            }

            else if (str == "DeliveredBy")
            {
                sql = "SELECT [name] from DeliveredBys order by [name]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }


            else if (str == "WorkStations")
            {
                sql = "SELECT [Sys_Name] as Name,[Sys_Code] as Code from WorkStations   order by [Sys_Name]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }

            else if (str == "User")
            {
                sql = "SELECT UserName as Name, UserType FROM USERs ORDER BY UserName, UserType";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Account")
            {
                sql = "SELECT ACCOUNTs.Name as AccName, ACCOUNTYPEs.Name AS Type FROM ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id where Ac_id<>'SER1'  ORDER BY ACCOUNTs.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Charges")
            {
                sql = "SELECT Name  from Charges  ORDER BY Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Packing")
            {
                sql = "SELECT Name  from packings  ORDER BY Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Gaddi")
            {
                sql = "SELECT Gaddi_name as Name from Gaddis order by Gaddi_name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Item")
            {
                sql = "SELECT Name FROM Items ORDER BY Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Customer/Supplier Rate")
            {
                sql = "SELECT DISTINCT ACCOUNTs.Name AS Account_name, items.name AS Item_name FROM (PARTYRATEs LEFT JOIN ACCOUNTs ON PARTYRATEs.Ac_id = ACCOUNTs.Ac_id) LEFT JOIN items ON PARTYRATEs.Des_id = items.Id ORDER BY ACCOUNTs.Name, items.name";                
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Control Room")
            {
                sql = "SELECT [Group] as GroupName,Features as [Features],Description as Description,selected_value as [Values] FROM FirmSetups order by Features";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.Columns["Delete"].Visible = false;
            }
            else if (str == "TransactionSetup")
            {
                sql = "SELECT VOUCHERTYPEs.Name FROM VOUCHERTYPEs where Type<>'Report' and A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " order by Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                label2.Text = "Transaction Setup";
                ansGridView5.Columns["Delete"].Visible = false;
            }
            else if (str == "State")
            {
                sql = "SELECT States.Sname as StateName, States.SPrintName as PrintName FROM States ORDER BY States.Sname";                              
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Staff")
            {
                sql = "select Name from others where type=17 order by [name]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Broker")
            {
                sql = "SELECT CONTRACTORs.Name FROM CONTRACTORs ORDER BY CONTRACTORs.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
            }
            else if (str == "Account Group")
            {

             
                sql = "select Name from Accountypes where Fixed="+ access_sql.Singlequote+"False"+ access_sql.Singlequote+" order by [name]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Account Groups";

           
            }
            textBox1.Focus();
            ansGridView5.Columns["Edit"].DataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ansGridView5.Columns["Edit"].DisplayIndex = ansGridView5.Columns.Count - 1;
            ansGridView5.Columns["Delete"].DisplayIndex = ansGridView5.Columns.Count - 1;

            if (Database.utype == "User")
            {
                ansGridView5.Columns["Delete"].Visible = false;
            }
        }

        private void ansGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gstr == "User")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_user_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }
                    frm_user frm = new frm_user();
                    frm.LoadData(funs.Select_user_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "User");
                    frm.ShowDialog(this);
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Users";
                            Database.GetSqlData("select * from Users where username='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "'", dtDelete);

                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Delivery Point");
                }
            }
            else if (gstr == "Account Group")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_AccType_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frmnewgroup frm = new frmnewgroup();
                    frm.LoadData(funs.Select_AccType_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Account Group");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();


                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Accountypes";
                            Database.GetSqlData("select * from Accountypes where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);

                        }
                    }
                    LoadData(gstr, "Account Group");
                }



            }

            else if (gstr == "WorkStations")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_ws_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Code"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }

                    frm_workstation frm = new frm_workstation();
                    frm.LoadData(funs.Select_ws_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Code"].Value.ToString()).ToString(), "Edit WorkStations");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "WorkStations";
                            Database.GetSqlData("select * from WorkStations where Code='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Code"].Value.ToString() + "'", dtDelete);

                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "WorkStations");
                }
            }




            else if (gstr == "DeliveredBy")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_db_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }

                    frm_deliveredby frm = new frm_deliveredby();
                    frm.LoadData(funs.Select_db_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit DeliveredBy");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "DeliveredBys";
                            Database.GetSqlData("select * from DeliveredBys where name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "'", dtDelete);

                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "DeliveredBy");
                }
            }





            else if (gstr == "Delivery Point")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_dp_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }

                    frmDP frm = new frmDP();
                    frm.LoadData(funs.Select_dp_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Delivery Point");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "DeliveryPoints";
                            Database.GetSqlData("select * from DeliveryPoints where name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "'", dtDelete);
                            
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Delivery Point");
                }
            }

            else if (gstr == "Gaddi")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_gaddi_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }

                    Gaddi frm = new Gaddi();
                    frm.LoadData(funs.Select_gaddi_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Gaddi");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Gaddis";
                            Database.GetSqlData("select * from Gaddis where Gaddi_name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "'", dtDelete);
                            
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Gaddi");
                }
            }

            else if (gstr == "Staff")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frm_NewGroup frm = new frm_NewGroup();
                    frm.LoadData(funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Staff");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Others";
                            Database.GetSqlData("select * from Others where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Staff");
                }
            }

            else if (gstr == "Customer/Supplier Rate")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    string acid = funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Account_name"].Value.ToString());
                    string did = funs.Select_item_name_pack_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Item_name"].Value.ToString());

                    frmCustSuppRate frm = new frmCustSuppRate();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData(acid,did, "Edit");
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                    if (res == DialogResult.OK)
                    {
                        string acid = funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Account_name"].Value.ToString());
                        string did = funs.Select_item_name_pack_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Item_name"].Value.ToString());

                        DataTable dtDelete = new DataTable();
                        dtDelete.TableName = "PARTYRATEs";
                        Database.GetSqlData("select * from PARTYRATEs where Ac_id='" + acid + "' and Des_id='" + did + "' ", dtDelete);
                        for (int i = 0; i < dtDelete.Rows.Count; i++)
                        {
                            dtDelete.Rows[i].Delete();
                        }
                        Database.SaveData(dtDelete);
                    }
                    LoadData(gstr, "Customer/Supplier Rate");
                }
            }

            else if (gstr == "Account")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }
                    frm_NewAcc frm = new frm_NewAcc();
                    frm.LoadData(funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString()).ToString(), "Edit Account");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }

                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Accounts";
                            Database.GetSqlData("select * from Accounts where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString() + "' ", dtDelete);

                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            string acid = funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString());
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Account");
                }
            }

            else if (gstr == "Item")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_item_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }
                    frmItem frm = new frmItem();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData(funs.Select_item_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()), "Edit Item");
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {

                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "ItemDetails";
                            Database.GetSqlData("select * from ItemDetails where Item_id='" + funs.Select_item_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) + "'", dtDelete);
                            
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);

                            dtDelete = new DataTable();
                            dtDelete.TableName = "Items";
                            Database.GetSqlData("select * from Items where id='" + funs.Select_item_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) + "'", dtDelete);
                            
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Item");
                }
            }


            else if (gstr == "Packing")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_packing_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }
                    Frmpacking frm = new Frmpacking();
                    frm.LoadData(funs.Select_packing_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Packing");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Packings";
                            Database.GetSqlData("select * from Packings where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Packing");
                }
            }


            else if (gstr == "Charges")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_ch_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }
                    frm_Charge frm = new frm_Charge();
                    frm.LoadData(funs.Select_ch_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Charges");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Charges";
                            Database.GetSqlData("select * from Charges where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Charges");
                }
            }




            else if (gstr == "Broker")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_broker_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }
                    frmBroker frm = new frmBroker();
                    frm.LoadData(funs.Select_con_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Broker");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "CONTRACTORs";
                            Database.GetSqlData("select * from CONTRACTORs where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Broker");
                }
            }

            else if (gstr == "State")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_state_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString()).ToString() == "")
                    {
                        return;
                    }
                    frm_state frm = new frm_state();
                    frm.LoadData(funs.Select_state_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString()).ToString(), "Edit State");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "States";
                            Database.GetSqlData("select * from States where Sname='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString() + "' ", dtDelete);
                            
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "State");
                }
            }

            else if (gstr == "Control Room")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_controlroom_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Features"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    ControlRoom frm = new ControlRoom();
                    frm.Loaddata(funs.Select_controlroom_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Features"].Value.ToString()).ToString(), "Edit Control Room");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
            }

            else if (gstr == "TransactionSetup")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_vt_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    Frmvouchertype frm = new Frmvouchertype();
                    frm.LoadData(funs.Select_vt_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Transaction Setup");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
            }

        }

        private bool validate(string name)
        {
            if (gstr == "Account Group")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNTs WHERE Act_id='" + funs.Select_act_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account Group is in Use in Account");
                    return false;
                }
            }
            if (gstr == "User")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE modifyby_id='" + funs.Select_user_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected User is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE user_id=" + funs.Select_user_id(name)) != 0)
                {
                    MessageBox.Show("Selected user is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM accounts WHERE userid=" + funs.Select_user_id(name)) != 0)
                {
                    MessageBox.Show("Selected user is in Use");
                    return false;
                }
            }
            else if (gstr == "Gaddi")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM voucherinfos WHERE Gaddi_id='" + funs.Select_gaddi_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Gaadi is Use in Transaction");
                    return false;
                }
               
            }
            else if (gstr == "Account")
            {
                //if (Database.GetScalarInt("SELECT count(*) FROM journal WHERE Ac_id='" + funs.Select_ac_id(name) + "'") != 0)
                //{
                //    MessageBox.Show("Selected Account is in Use in Transaction");
                //    return false;
                //}                
                if (Database.GetScalarInt("SELECT count(*) FROM Partyrates WHERE Ac_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is in Use in PartyRate");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE Ac_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Location WHERE CashAc_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Location");
                    return false;
                }

                if (Database.GetScalarInt("SELECT count(*) FROM Journals WHERE Ac_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }

                if (Database.GetScalarInt("SELECT count(*) FROM Journals WHERE Opp_Acid='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }


                if (Database.GetScalarInt("SELECT count(*) FROM ChallanUnloadings WHERE Consignee_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM ChallanUnloadings WHERE Consigner_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Stocks WHERE Consignee_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Stocks WHERE Consigner_id='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE Ac_id2='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE Driver_name='" + funs.Select_ac_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Account is Use in Transaction");
                    return false;
                }
            }
            if (gstr == "Broker")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNTs WHERE Con_id='" + funs.Select_broker_id(name)+"' ") != 0)
                {
                    MessageBox.Show("Selected Broker Name is in Use in Account");
                    return false;
                }
            }
            if (gstr == "Gaddi")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE Gaddi_id='" + funs.Select_gaddi_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Gaddi Name is in Use in Transaction");
                    return false;
                }
            }

            else if (gstr == "Charges")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM voucherdets WHERE ch_id='" + funs.Select_ch_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected  Name is in Use in Transaction");
                    return false;
                }
            }
            
            else if (gstr == "Packing")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM voucherdets WHERE packing='" + name + "'") != 0)
                {
                    MessageBox.Show("Selected Packing Name is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Challanunloadings WHERE packing='" + name + "'") != 0)
                {
                    MessageBox.Show("Selected Packing Name is in Use in Transaction");
                    return false;
                }
            }
            if (gstr == "State")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNTs WHERE State_id='" + funs.Select_state_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected State Name is in Use in Account");
                    return false;
                }
                //else if (Database.GetScalarInt("SELECT count(*) FROM COMPANY WHERE CState_id='" + funs.Select_state_id(name) + "'") != 0)
                //{
                //    MessageBox.Show("Selected State Name Use In Company");
                //    return false;
                //}
            }
            else if (gstr == "Staff")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNTs WHERE loc_id='" + funs.Select_oth_id(name)+"' ") != 0)
                {
                    MessageBox.Show("Selected Account Group is in Use in Account");
                    return false;
                }
            }
            else if (gstr == "Account Group")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNTs WHERE Act_id=" + funs.Select_act_id(name)) != 0)
                {
                    MessageBox.Show("Selected Account Group is in Use in Account");
                    return false;
                }
            }            
            else if (gstr == "Item")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherdets WHERE Des_ac_id='" + funs.Select_item_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Item Name is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Challanunloadings WHERE Des_ac_id='" + funs.Select_item_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Item Name is in Use ");
                    return false;
                }
            }
            else if (gstr == "Delivery Point")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Accounts WHERE SId='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM PartyRates WHERE Destination_id='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM ChallanUnloadings WHERE Destination_id='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM ChallanUnloadings WHERE Source_id='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM PartyRate WHERE Source_id='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE SId='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERINFOs WHERE Consigner_id='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM location WHERE [Dp_id]='" + funs.Select_dp_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Delivery Point is in Use");
                    return false;
                }
            }
            return true;
        }

        private void ADD()
        {
            if (gstr == "User")
            {
                frm_user frm = new frm_user();
                frm.LoadData("0", "User");
                frm.ShowDialog(this);
                LoadData(gstr, "User");
            }
            else if (gstr == "WorkStations")
            {
                frm_workstation frm = new frm_workstation();
                frm.LoadData("0", "WorkStations");
                frm.ShowDialog(this);
                LoadData(gstr, "WorkStations");
            }
            else if (gstr == "Account")
            {
                frm_NewAcc frm = new frm_NewAcc();
                frm.LoadData("0", "Account");
                frm.ShowDialog(this);
                LoadData(gstr, "Account");
            }
            else if (gstr == "Account Group")
            {
                frmnewgroup frm = new frmnewgroup();
                frm.LoadData("0", "Account Group");
               // frm.MdiParent = this.MdiParent;
                frm.ShowDialog(this);
                LoadData(gstr, "Account Group");
            }
            else if (gstr == "Charges")
            {
                frm_Charge frm = new frm_Charge();
                frm.LoadData("0", "Charges");
                frm.ShowDialog(this);
                LoadData(gstr, "Charges");
            }
            else if (gstr == "State")
            {
                frm_state frm = new frm_state();
                frm.LoadData("0", "State");
                frm.ShowDialog(this);
                LoadData(gstr, "State");
            }
            else if (gstr == "Gaddi")
            {
                Gaddi frm = new Gaddi();
                frm.LoadData("0", "Gaddi");
                frm.ShowDialog(this);
                LoadData(gstr, "Gaddi");
            }
            else if (gstr == "Packing")
            {
                Frmpacking frm = new Frmpacking();
                frm.LoadData("0", "Packing");
                frm.ShowDialog(this);
                LoadData(gstr, "Packing");
            }
            else if (gstr == "Broker")
            {
                frmBroker frm = new frmBroker();
                frm.LoadData("0", "Broker");
                frm.ShowDialog(this);
                LoadData(gstr, "Broker");
            }
            else if (gstr == "Delivery Point")
            {
                frmDP frm = new frmDP();
                frm.LoadData("0", "Delivery Point");
                frm.ShowDialog(this);
                LoadData(gstr, "Delivery Point");
            }
            else if (gstr == "DeliveredBy")
            {
                frm_deliveredby frm = new frm_deliveredby();
                frm.LoadData("0", "DeliveredBy");
                frm.ShowDialog(this);
                LoadData(gstr, "DeliveredBy");
            }
            else if (gstr == "Item")
            {
                frmItem frm = new frmItem();                
                frm.LoadData("0", "Item");
                frm.ShowDialog(this);
                LoadData(gstr, "Item");
            }          
            else if (gstr == "Staff")
            {
                frm_NewGroup frm = new frm_NewGroup();
                frm.LoadData("0", "Staff");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Customer/Supplier Rate")
            {
                frmCustSuppRate frm = new frmCustSuppRate();
                frm.LoadData("0", "0", "Customer/Supplier Rate");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void filter()
        {
            String strTemp = textBox1.Text;
            strTemp = strTemp.Replace("%", "?");
            strTemp = strTemp.Replace("[", string.Empty);
            strTemp = strTemp.Replace("]", string.Empty);
            string strfilter = "";
            int a = 0;
            a = dtitem.Columns.Count;
            if (gstr == "Tax")
            {
                for (int i = 0; i < dtitem.Columns.Count - 1; i++)
                {
                    if (strfilter != "")
                    {
                        strfilter += " or ";
                    }
                    strfilter += "(" + dtitem.Columns[i].ColumnName + " like '*" + strTemp + "*' " + ")";
                }
            }
            else
            {
                for (int i = 0; i < dtitem.Columns.Count; i++)
                {
                    if (strfilter != "")
                    {
                        strfilter += " or ";
                    }
                    strfilter += "(" + dtitem.Columns[i].ColumnName + " like '*" + strTemp + "*' " + ")";
                }
            }
            bs.Filter = null;
            bs.DataSource = dtitem;
            bs.Filter = strfilter;
        }

        private void frmMaster_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            SideFill();
            label3.Text = "";
            label3.Text ="Total =  "+ ansGridView5.RowCount.ToString();
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));
            
            //createnew
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "add";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Create New";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^C";
            if (gstr == "Control Room")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }

            //refresh
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "refresh";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Refresh";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^R";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {
                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {
                    Button btn = new Button();
                    btn.Size = new Size(150, 30);
                    btn.Name = dtsidefill.Rows[i]["Name"].ToString();
                    btn.Text = "";
                    Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                    Graphics G = Graphics.FromImage(bmp);
                    G.Clear(btn.BackColor);
                    string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
                    string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();
                    StringFormat SF = new StringFormat();
                    SF.Alignment = StringAlignment.Near;
                    SF.LineAlignment = StringAlignment.Center;
                    System.Drawing.Rectangle RC = btn.ClientRectangle;
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
                    btn.Image = bmp;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "add")
            {
                ADD();
            }
            else if (name == "refresh")
            {
                LoadData(gstr, gstr);
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        public void ExportToPdf(string tPath)
        {
            string str = "";
            FileStream fs = new FileStream(tPath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Rectangle rec;
            Document document;
            int Twidth = 0;
            int basevalue = 2;
            if (gstr == "StockItem")
            {
                if (Feature.Available("Company Colour") == "No")
                {
                    basevalue = 5;
                }
            }
            for (int i = basevalue; i < ansGridView5.Columns.Count; i++)
            {
                Twidth += ansGridView5.Columns[i].Width;
            }
            if (Twidth == 2000)
            {
                document = new Document(PageSize.A4.Rotate(), 20f, 10f, 20f, 10f);
            }

            document = new Document(PageSize.A4, 20f, 10f, 20f, 10f);

            //  Pagesize = GetPapersize();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new MainTextEventsHandler();
            document.Open();
            HTMLWorker hw = new HTMLWorker(document);
            str = "";
            str += @"<body> <font size='1'><table border=1> <tr>";
            for (int i = basevalue; i < ansGridView5.Columns.Count; i++)
            {
                string align = "";
                string bold = "";
                int width = 0;
                if (Twidth == 2000)
                {
                    width = ansGridView5.Columns[i].Width / 20;
                }
                else
                {
                    width = ansGridView5.Columns[i].Width / 10;
                }
                if (ansGridView5.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    align = "text-align:right;";
                }
                bold = "font-weight: bold;";

                if (width != 0)
                {
                    str += "<th width=" + width + "%  style='" + align + bold + "'>" + ansGridView5.Columns[i].HeaderText.ToString() + "</th> ";
                }
            }

            str += "</tr>";
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                str += "<tr> ";
                for (int j = basevalue; j < ansGridView5.Columns.Count; j++)
                {
                    int width = 0;
                    if (Twidth == 2000)
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 20;
                    }
                    else
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 10;
                    }
                    if (width != 0)
                    {
                        if (ansGridView5.Rows[i].Cells[j].Value != null)
                        {
                            string align = "";
                            string bold = "";
                            string colspan = "";
                            if (ansGridView5.Columns[j].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                            {
                                align = "text-align:right;";
                            }
                            if (ansGridView5.Rows[i].Cells[j].Style.Font != null && ansGridView5.Rows[i].Cells[j].Style.Font.Bold == true)
                            {
                                bold = "font-weight: bold;";
                            }
                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
                            {
                                colspan = "colspan= '2'";
                            }
                            if (ansGridView5.Rows[i].Cells[j].Value.ToString().Trim() == "")
                            {
                                str += "<td> &nbsp; </td>";
                            }
                            else
                            {
                                str += "<td " + colspan + "  style='" + align + bold + "'>" + ansGridView5.Rows[i].Cells[j].Value.ToString() + "</td> ";
                            }
                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
                            {
                                j++;
                            }
                        }
                        else
                        {
                            str += "<td> &nbsp; </td>";
                        }
                    }
                }
                str += "</tr> ";
            }
            str += "</table></font></body>";

            StringReader sr = new StringReader(str);
            hw.Parse(sr);
            document.Close();
        }

        internal class MainTextEventsHandler : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                DataTable dtRheader = new DataTable();
                Database.GetSqlData("select * from company", dtRheader);
                PdfPTable table = new PdfPTable(1);
                PdfPCell cell = new PdfPCell();

                cell.Phrase = new Phrase(dtRheader.Rows[0]["name"].ToString());
                cell.BorderWidth = 0f;
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(cell);
                cell.Phrase = new Phrase(dtRheader.Rows[0]["Address1"].ToString());
                table.AddCell(cell);
                cell.Phrase = new Phrase(dtRheader.Rows[0]["Address2"].ToString());
                table.AddCell(cell);
                cell.Phrase = new Phrase(Report.DecsOfReport2);
                table.AddCell(cell);
                cell.Phrase = new Phrase("\n");
                table.AddCell(cell);
               
                document.Add(table);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                string text = "";
                text += "Page No-" + document.PageNumber;
                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();
                BaseFont bf = BaseFont.CreateFont();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(530, 8);
                cb.ShowText(text);
                cb.EndText();
            }
        }

        private void frmMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.M)
            {
                if (gstr == "Account")
                {
                    frm_merge frm = new frm_merge();
                    frm.typ = "Account";
                    frm.ShowDialog();
                }
                else if (gstr == "Item")
                {
                    frm_merge frm = new frm_merge();
                    frm.typ = "Item";
                    frm.ShowDialog();
                }
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                LoadData(gstr, gstr);
                if (ansGridView5.Rows.Count == 0)
                {
                    return;
                }
                string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
                ExportToPdf(tPath);
                GC.Collect();
                PdfReader frm = new PdfReader();
                frm.LoadFile(tPath);
                frm.Show();
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                LoadData(gstr, gstr);
                Excelexport();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                ADD();
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                LoadData(gstr, gstr);
            }           
        }

        private void Excelexport()
        {
            if (ansGridView5.Rows.Count == 0)
            {
                return;
            }
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];
            int lno = 1;
            DataTable dtExcel = new DataTable();
            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from company", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            int basevalue = 2;
            if (gstr == "StockItem")
            {
                if (Feature.Available("Company Colour") == "No")
                {
                    basevalue = 5;
                }
            }

            for (int i = basevalue; i < ansGridView5.Columns.Count; i++)
            {
                if (ansGridView5.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    ws.get_Range(ws.Cells[5, i + 1], ws.Cells[5, i + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                }
                ws.get_Range(ws.Cells[i + 1, i + 1], ws.Cells[i + 1, i + 1]).ColumnWidth = ansGridView5.Columns[i].Width / 11.5;
                ws.Cells[5, i + 1] = ansGridView5.Columns[i].HeaderText.ToString();
            }

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                for (int j = basevalue; j < ansGridView5.Columns.Count; j++)
                {
                    if (ansGridView5.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).NumberFormat = "0,0.00";
                    }
                    else
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }


                    if (ansGridView5.Columns[j].DefaultCellStyle.Font != null)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).Font.Bold = true;
                    }

                    if (ansGridView5.Rows[i].Cells[j].Value != null)
                    {
                        ws.Cells[i + 6, j + 1] = ansGridView5.Rows[i].Cells[j].Value.ToString().Replace(",", "");
                    }
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            ws.Columns.AutoFit();
            apl.Visible = true;
        }

        private void frmMaster_Enter(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
