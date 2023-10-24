using QuanLyQuanBida.DAO;
using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Menu = QuanLyQuanBida.DTO.Menu;

namespace QuanLyQuanBida
{
    public partial class BillDetail : Form
    {
        int TablePrice;
        public BillDetail(int id, int discount, int tableprice)
        {
            InitializeComponent();
            ShowBill(id);
            TablePrice = tableprice;
            lbTableID.Text = (id-1).ToString();
            lbDiscount.Text = discount.ToString();
            lbTablePrice.Text = tableprice.ToString();

            Table table = lsvDetail.Tag as Table;

            double foodPrice = Convert.ToDouble(lbFoodPrice.Text.Split(',')[0]);

            double totalFoodPrice = foodPrice - (foodPrice / 100 * discount);

            lbAfterDiscount.Text = totalFoodPrice.ToString();

            string timeStart = TableDAO.Instance.GetTimeStart(id);

            string timeStart1 = TableDAO.Instance.GetTimeStart1(id);
            if (timeStart != "")
            {
                double day = Convert.ToDouble(timeStart.Split(':')[0]);
                double hour = Convert.ToDouble(timeStart.Split(':')[1]);
                double minute = Convert.ToDouble(timeStart.Split(':')[2]);
                double second = Double.Parse(timeStart.Split(':')[3], CultureInfo.InvariantCulture.NumberFormat);

                int timePlaying = Convert.ToInt32(hour * 60 + minute);
                double tablePrice = (day * 24 + hour * 60 + minute + second / 60) * Convert.ToDouble(TablePrice) / 60; // Tính theo 90000 một phút

                double finalTotalPrice = totalFoodPrice + tablePrice;
                lbTimeStart.Text = timeStart1;
                lbTimeEnd.Text = DateTime.Now.ToString();
                lbTimePlaying.Text = timePlaying.ToString() + "p";
                CultureInfo culture = new CultureInfo("vi-VN");
                Thread.CurrentThread.CurrentCulture = culture;
                lbTimePlay.Text = tablePrice.ToString("c");
                lbFinalPrice.Text = finalTotalPrice.ToString("c");
            }
        }

        void ShowBill(int id)
        {
            lsvDetail.Items.Clear();

            List<Menu> listBill = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            int i = 1;
            foreach (Menu item in listBill)
            {
                ListViewItem lsvItem = new ListViewItem(i.ToString());
                lsvItem.SubItems.Add(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());

                totalPrice += item.TotalPrice;

                lsvDetail.Items.Add(lsvItem);
                i++;
            }

            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            lbFoodPrice.Text = totalPrice.ToString("c");
        }

        private void BillDetail_Load(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
