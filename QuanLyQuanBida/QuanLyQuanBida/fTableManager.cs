using QuanLyQuanBida.DAO;
using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyQuanBida.DTO.Menu;

namespace QuanLyQuanBida
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();

            LoadTable();
        }

        #region Method
        void LoadTable()
        {
            List<Table> tableList = TableDAO.Instance.LoadTableList();
        
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Click += btn_Click;
                btn.Tag = item;

                btn.Text = item.Name + "\n" + item.Status;

                switch (item.Status)
                {
                    case "Trống":
                        //btn.BackColor = Color.Green;

                        btn.Image = System.Drawing.Image.FromFile(@"D:\contest\Login\bidatable1.png");
                        btn.BackgroundImageLayout = ImageLayout.Zoom;
                        btn.Font = new Font(btn.Font.FontFamily, 15);

                        break;
                    default:
                        btn.Image = System.Drawing.Image.FromFile(@"D:\contest\Login\bidatable3.png");
                        btn.BackgroundImageLayout = ImageLayout.Zoom;
                        btn.Font = new Font(btn.Font.FontFamily, 15);
                        break;
                }
                flpTable.Controls.Add(btn);
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<QuanLyQuanBida.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);

            foreach (QuanLyQuanBida.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());

                lsvBill.Items.Add(lsvItem);
            }
        }

        #endregion 

        #region Events

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            ShowBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }
        #endregion
    }
}
