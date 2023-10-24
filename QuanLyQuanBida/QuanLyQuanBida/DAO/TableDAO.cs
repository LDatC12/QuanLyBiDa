using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanBida.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return instance; }

            private set { instance = value; }
        }
        public static int TableWidth = 150;
        public static int TableHeight = 88;

        private TableDAO() { }

        // Lấy tất cả thông tin từ table 
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            string query = "USP_GetTableList";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }

        // Tính giờ chơi 
        public string GetTimeStart(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM TableFood WHERE id = " + id);
            string rs = "";
            foreach (DataRow item in data.Rows)
            {
                Table tableInfo = new Table(item);
                 
                //MessageBox.Show(DateTime.Parse(tableInfo.timestart).ToString(), "asd");
                //MessageBox.Show(DateTime.Now.ToString(), "asd");
                // Giờ chơi = Giờ hiện tại - Giờ bắt đầu
                //if (tableInfo == null) return rs;
                DateTime date1 = DateTime.Now;
                DateTime date2 = DateTime.Parse(tableInfo.timestart);
                TimeSpan timePlay = date1.Subtract(date2);
                // Custom-timespan-format-strings
                rs = timePlay.ToString(@"dd\:hh\:mm\:ss");
            }
            return rs;
        }
        public string GetTimeStart1(int id)
        {
            string rs = "";
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM TableFood WHERE id = " + id);
            foreach (DataRow item in data.Rows)
            {
                Table tableInfo = new Table(item);
                DateTime date1 = DateTime.Parse(tableInfo.timestart);
                rs = date1.ToString();
            }
            return rs;
        }
    }
}
