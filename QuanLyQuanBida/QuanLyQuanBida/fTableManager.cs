using QuanLyQuanBida.DAO;
using QuanLyQuanBida.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyQuanBida.DTO.Menu;

namespace QuanLyQuanBida
{
    public partial class fTableManager : Form
    {
        // Tạo constructor để lấy account từ form khác
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type);  }
        }
        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadTablePrice();
        }

        #region Method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + loginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadTablePrice()
        {
            cbTablePrice.Text = "70000";
            cbTablePrice.Items.Add("70000");
            cbTablePrice.Items.Add("80000");

        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();

            List<Table> tableList = TableDAO.Instance.LoadTableList();
        
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Click += btn_Click;
                btn.Tag = item;

                btn.Text = item.Name + "\n" + item.Status + "\n";

                switch (item.Status)
                {
                    case "Trống":
                        //btn.BackColor = Color.Green;

                        btn.Image = System.Drawing.Image.FromFile(@"D:\Đồ án cuối kì môn lập trình desktop\QuanLyQuanBida\QuanLyQuanBida\Resources\bidatable1.png");
                        btn.BackgroundImageLayout = ImageLayout.Zoom;
                        btn.Font = new Font(btn.Font.FontFamily, 13);

                        break;
                    default:
                        btn.Image = System.Drawing.Image.FromFile(@"D:\Đồ án cuối kì môn lập trình desktop\QuanLyQuanBida\QuanLyQuanBida\Resources\bidatable3.png");
                        btn.BackgroundImageLayout = ImageLayout.Zoom;
                        btn.Font = new Font(btn.Font.FontFamily, 13);
                        if (item.timestart != "")
                        {
                            DateTime tim = DateTime.Parse(item.timestart);
                            btn.Text += tim.ToString("HH:mm:ss");
                        }
                        break;
                }
                flpTable.Controls.Add(btn);
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();

            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());

                totalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            txtTotalPrice.Text = totalPrice.ToString("c");

        }

        #endregion 

        #region Events

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += f_InsertFood;
            f.DeleteFoood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }
        
        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;


            LoadFoodListByCategoryID(id);
        }


        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Please select table", "Notification");
                return;
            }
            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            DataProvider.Instance.ExecuteQuery("UPDATE dbo.TableFood SET timestart = GETDATE() WHERE id = " + table.ID.ToString());
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);

            LoadTable();
        }

        // Thanh toán
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table  == null)
            {
                MessageBox.Show("Please select table", "Notification");
                return;
            }
            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            
            double foodPrice = Convert.ToDouble(txtTotalPrice.Text.Split(',')[0]);

            double totalFoodPrice = foodPrice - (foodPrice / 100 * discount);
            if (idBill != -1)
            {
                string timeStart = TableDAO.Instance.GetTimeStart(table.ID);
                if (timeStart != "")
                {
                    //00:00:00:00 - dd:HH:mm:ss
                    double day = Convert.ToDouble(timeStart.Split(':')[0]);
                    double hour = Convert.ToDouble(timeStart.Split(':')[1]);
                    double minute = Convert.ToDouble(timeStart.Split(':')[2]);
                    double second = Double.Parse(timeStart.Split(':')[3], CultureInfo.InvariantCulture.NumberFormat);

                    int timePlay = Convert.ToInt32(hour * 60 + minute);
                    double tablePrice = (day * 24 + hour * 60 + minute + second / 60) * Convert.ToDouble(cbTablePrice.Text.ToString()) / 60; // Tính theo 90000 một phút

                    double finalTotalPrice = totalFoodPrice + tablePrice;

                    if (MessageBox.Show(String.Format("Are you sure to print bill for {0} with discount {1}% on total price of {2}đ is {3}đ \nNumber of hours played :{4} mins \nPrice of table {5} + {6} = {7}", table.Name, discount, foodPrice, totalFoodPrice, timePlay, tablePrice.ToString("0.00"), totalFoodPrice.ToString("0.00"), finalTotalPrice.ToString("0.00")), "Notification", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        DataProvider.Instance.ExecuteQuery("UPDATE dbo.TableFood SET timestart = NULL WHERE id = " + table.ID.ToString());
                        // MessageBox.Show(idBill.ToString() + "\n"+ discount.ToString() + "\n" + finalTotalPrice.ToString(), "e");
                        BillDAO.Instance.CheckOut(idBill, discount, (int)tablePrice);
                        ShowBill(table.ID);

                        LoadTable();
                    }
                }
          
            }
        }


        #endregion

        // Tính giờ 
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lbTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void lbTime_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnBillDetail_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Please select table", "Notification");
                return;
            }
            try
            {
                BillDetail f = new BillDetail(table.ID, (int)nmDiscount.Value, Convert.ToInt32(cbTablePrice.Text.ToString()));
                f.Show();
            }
            catch
            {

            }
        }

        private void nmDiscount_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
