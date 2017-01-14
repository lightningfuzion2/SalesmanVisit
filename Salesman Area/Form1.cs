using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;


namespace Salesman_Area
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            SmanArea.ShowCellToolTips = false;
        }




        private void Load_Click(object sender, EventArgs e)

        {
            SqlConnection con = new SqlConnection(@"Data Source=DARSHSPC;Initial Catalog=DEMedSql;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;
            ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();
            SqlDataAdapter smaadp = new SqlDataAdapter(new SqlCommand(@"

use DEMedSql

SELECT * 
INTO #temp
FROM 
(
Select CustUId,Custcode,Party,Citycode,City,SmanUId,Smancode,Sman,[1] as Sun,[2] as Mon,[3] as Tue,[4] as Wed,[5] as Thu,[6] as Fri,[7] as Sat
from
(Select FF.led_id as CustUId,FF.Usr_Code as Custcode,FF.Led_Name as Party,AA.LedId_Party ,
bb.led_id as SmanUId,bb.Usr_Code as Smancode,BB.Led_Name as Sman, AA.LedId_Sman,
cc.GrpId as Citycode,CC.GrpName as City, AA.VisitDay
From Dbo.tbl_WeeklyVisit as AA
Left join dbo.tbl_LedgerSetup AS FF WITH (NOLOCK) On AA.LedId_Party = FF.Led_Id
Left Join dbo.tbl_LedgerSetup as BB With (Nolock) on AA.LedId_Sman = BB.Led_Id
LEFT JOIN dbo.tbl_GroupDetail AS CC WITH (NOLOCK) On FF.Grp_Id_City = CC.GrpId
) PT

Pivot (Count(VisitDay) For VisitDay in ([1], [2], [3], [4], [5], [6], [7])
      )piv)piv
      
      --alter table #temp 
      --alter column Sun bit
      --alter table #temp 
      --alter column Mon bit
	  --alter table #temp 
      --alter column Tue bit
	  --alter table #temp 
      --alter column Wed bit
	  --alter table #temp 
      --alter column Thu bit
	  --alter table #temp 
      --alter column Fri bit
	  --alter table #temp 
      --alter column Sat bit
      select * from #temp
      drop table #temp", con));

            DataTable smatb = new DataTable();
            smaadp.Fill(smatb);
            //SmanArea.DataSource = smatb;
            SmanArea.DoubleBuffered(true);
            SmanArea.Rows.Clear();
            foreach (DataRow item in smatb.Rows)
            {
                int n = SmanArea.Rows.Add();
                SmanArea.Rows[n].Cells[0].Value = (n+1).ToString();
                SmanArea.Rows[n].Cells[1].Value = item[0].ToString();
                SmanArea.Rows[n].Cells[2].Value = item[1].ToString();
                SmanArea.Rows[n].Cells[3].Value = item[2].ToString();
                SmanArea.Rows[n].Cells[4].Value = item[3].ToString();
                SmanArea.Rows[n].Cells[5].Value = item[4].ToString();
                SmanArea.Rows[n].Cells[6].Value = item[5].ToString();
                SmanArea.Rows[n].Cells[7].Value = item[6].ToString();
                SmanArea.Rows[n].Cells[8].Value = item[7].ToString();
                SmanArea.Rows[n].Cells[9].Value = item[8].ToString();
                SmanArea.Rows[n].Cells[10].Value = item[9].ToString();
                SmanArea.Rows[n].Cells[11].Value = item[10].ToString();
                SmanArea.Rows[n].Cells[12].Value = item[11].ToString();
                SmanArea.Rows[n].Cells[13].Value = item[12].ToString();
                SmanArea.Rows[n].Cells[14].Value = item[13].ToString();
                SmanArea.Rows[n].Cells[15].Value = item[14].ToString();
                //smanchkd.Items.Add(item[8].ToString());
            }


            //foreach (DataGridViewRow row in SmanArea.Rows)
           // {
              //  row.HeaderCell.Value = (row.Index + 1).ToString();
          //  }

        //    SmanArea.TopLeftHeaderCell.Value = (SmanArea.RowCount - 1).ToString();
            SmanArea.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            
            SqlDataAdapter smaadp2 = new SqlDataAdapter(new SqlCommand(@"


    Select 
    distinct BB.led_id as SmanUId,  BB.Usr_Code as Smancode,BB.Led_Name as Sman, AA.LedId_Sman
    From Dbo.tbl_WeeklyVisit as AA 
    Left Join dbo.tbl_LedgerSetup as BB With (NoLock) on AA.LedId_Sman = BB.Led_Id", con));
            DataTable smatb2 = new DataTable();
            smaadp2.Fill(smatb);

                foreach (DataRow item2 in smatb2.Rows)
                {
                //smanchkd.Items.Add(item2[1].ToString());
                //smanchkd.Controls.Add(item2[1].ToString());
            }


        }



        private void find(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.F)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                findbox.BringToFront();
                findbox.Text = (SmanArea.CurrentCell.Value.ToString()) ?? "";
                findbox.Focus();
                findbox.SelectAll();
            }
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                findbox.SendToBack();
                SmanArea.Focus();
            }
        }
        private void findent(object sender, KeyPressEventArgs e)
        {
            string searchValue = findbox.Text;

            SmanArea.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in SmanArea.Rows)
                {
                    if (row.Cells[2].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private void pp(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (SmanArea.Rows[e.RowIndex].Selected)
            {

                int width = SmanArea.Width;
                Rectangle r = senderGrid.GetRowDisplayRectangle(e.RowIndex, false);
                var rect = new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1);
                // draw the border around the selected row using the highlight color and using a border width of 2
                ControlPaint.DrawBorder(e.Graphics, rect,
                            Color.SlateGray, 2, ButtonBorderStyle.Solid,
                            Color.SlateGray, 2, ButtonBorderStyle.Solid,
                            Color.SlateGray, 2, ButtonBorderStyle.Solid,
                            Color.SlateGray, 2, ButtonBorderStyle.Solid);


            }
        }
        private void days(object sender, DataGridViewCellFormattingEventArgs e)
        {
            {
                SmanArea.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
                SmanArea.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
                SmanArea.CurrentCell.Style.SelectionBackColor = Color.SlateGray;
                
                var senderGrid = (DataGridView)sender;

                var cc = SmanArea.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cc.Selected && senderGrid.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn)
                {

                    cc.Style.SelectionBackColor = Color.LightGray;
                    cc.Style.SelectionForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
                }


        


                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    var c = SmanArea.Rows[e.RowIndex].Cells[e.ColumnIndex];
        
                    if (c.Value.ToString() == "1" && c is DataGridViewCheckBoxCell)
                    {
                        c.Style.BackColor = Color.LightSlateGray;
                    }
                    if (c.Value.ToString() == "0" && c is DataGridViewCheckBoxCell)
                    {
                        c.Style.BackColor = Color.White;
                    }
                }
        
            }
        }

        
        private void sarearow_enter(object sender, DataGridViewCellEventArgs e)
        {
            var c = SmanArea.Rows[e.RowIndex].Cells[e.ColumnIndex];

            var cell = SmanArea.CurrentCell;
            var cellDisplayRect = SmanArea.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                toolTip1.Show(string.Format(cell.OwningColumn.HeaderText, cell.RowIndex, cell.ColumnIndex),
                          SmanArea,
                          (cellDisplayRect.X + cell.Size.Width / 100),
                          (cellDisplayRect.Y + cell.Size.Height / 1),
                          3000);

            }
            else { toolTip1.Hide(SmanArea); }

        }

        private void SmanArea_SortStringChanged(object sender, EventArgs e)
        {

        }

        private void SmanArea_FilterStringChanged(object sender, EventArgs e)
        {

        }
    }


    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}

