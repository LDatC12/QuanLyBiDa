using QuanLyQuanBida.DAO;
using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QuanLyQuanBida
{
    public partial class fAdmin : Form
    {
        // Binding cho foodList
        BindingSource foodList = new BindingSource();

        // Binding cho account
        BindingSource accountList = new BindingSource();

        public fAdmin()
        {
            InitializeComponent();
            Loading();
        }
        #region methods

        // Tìm theo tên gần đúng
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        } 
        void Loading()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
        }

        // Binding cho account

        // DataSourceUpdateMode.Never
        // Dòng cuối để cho nó không bao giờ tự update khi mình lose focus
        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            // reset về ngày đầu tháng
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            // reset cuối tháng = đầu tháng + 30 - 1;
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        
        // Load data cho combobox
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        // Binding theo food vào trong text box
        void AddFoodBinding()
        {
            // DataSourceUpdateMode.Never
            // Dòng cuối để cho nó không bao giờ tự update khi mình lose focus
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        #endregion

        #region events

        private void btnViewbill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        // Giúp cho cbFood chạy theo FoodCategory
        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            // Try vì rất khó nêu ra tất cả điều kiện  vì vậy để không lỗi thì nên try catch
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong.");
            }
        }
        // Thêm thức ăn
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value; 

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Add food successfull!");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            } else
            {
                MessageBox.Show("Cannot add food.");
            }
        }

        // Cập nhật thức ăn 
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Update food successfull!");
                LoadListFood();
                if (updateFood != null) {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Cannot update food.");
            }
        }

        // Xóa món ăn
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Delete food successfull!");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Cannot delete food.");
            }
        }
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Reset account password successfull!");
            }
            else
            {
                MessageBox.Show("Cannot reset account password.");
            }

        }

        // Thêm tài khoản
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string name = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int) nmAccountType.Value;

            if (AccountDAO.Instance.InsertAccount(name, displayName, type))
            {
                MessageBox.Show("Add account successfull!");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Cannot add account.");
            }
        }

        // Sửa tài khoản
        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            string name = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmAccountType.Value;

            if (AccountDAO.Instance.UpdateAccount(name, displayName, type))
            {
                MessageBox.Show("Update account successfull!");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Cannot Update account.");
            }
        }

        // Xóa tài khoản
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string name = txtUserName.Text;

            if (AccountDAO.Instance.DeleteAccount(name)) {
                MessageBox.Show("Delete account successfull!");
                LoadAccount(); 
            }
            else
            {
                MessageBox.Show("Cannot add account.");
            }
        }

        // Tìm kiến gần đúng thức ăn
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txtSearchFoodName.Text);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        // Tạo sự kiện Add Update Delete
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFoood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }


        #endregion

        private void btnBillDetail_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            // report xuất //
            ToExcel(dtgvBill, "D:\\Đồ án cuối kì môn lập trình desktop\\report.xlsx");
        }
        private void ToExcel(DataGridView dataGridView1, string fileName)
        {
            //khai báo thư viện hỗ trợ Microsoft.Office.Interop.Excel
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;
            try
            {
                //Tạo đối tượng COM.
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                //tạo mới một Workbooks bằng phương thức add()
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                //đặt tên cho sheet
                worksheet.Name = "Quản lý quán bi da";

                // export header trong DataGridView
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                // export nội dung trong DataGridView
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value == null)
                        {
                            worksheet.Cells[i + 2, j + 1] = "";
                        } else worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                // sử dụng phương thức SaveAs() để lưu workbook với filename
                workbook.SaveAs(fileName);
                //đóng workbook
                workbook.Close();
                excel.Quit();
                MessageBox.Show("Xuất dữ liệu ra Excel thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
