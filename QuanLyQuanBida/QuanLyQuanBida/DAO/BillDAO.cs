using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DAO
{
    public class BillDAO
    {
        private static BillDAO instance = new BillDAO();

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        // Thành công: Bill ID
        // Thất bại: -1
        public int GetUnCheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Bill WHERE idTable = " + id + " AND status = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable", new object[] {id});
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public void CheckOut(int id, int discount, int totalPrice)
        {
            // Khi cập nhật nhớ là phải xét tất cả đều không rỗng hoặc null

            // Cẩn thận khi cập nhật phải chừa 1 khoảng trống giữa các biến truyền vào ví dụ như   + discount +

            // Cẩn thận khi truyền float vì nó có dấu phẩy , 

            // Cách tốt nhất là làm như vầy

            // DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });


            string query = "UPDATE dbo.Bill SET status = 1 , " + "discount = " + discount + " , DateCheckOut = GETDATE() , " + "totalPrice = " + totalPrice + " WHERE id = " + id;


            DataProvider.Instance.ExecuteNonQuery(query);
        } 
        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM Bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
