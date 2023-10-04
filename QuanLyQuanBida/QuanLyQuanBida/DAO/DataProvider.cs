using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DAO
{
    public class DataProvider
    {
        // Design patern Singleton làm một lần sử dụng nhiều lần

        // Tạo một biến private static là thể hiện của class đó để đảm bảo rằng nó là duy nhất
        // và chỉ được tạo ra trong class đó thôi.
        private static DataProvider instance; // CTRL + R + E


        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }
        private DataProvider() { }

        private string connSTR = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyQuanBida;Integrated Security=True";
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {

            DataTable data = new DataTable();

            // Sử dụng using vì nếu khi có lõi nó sẽ tiếp tục chạy và giải phóng bộ nhớ cho chúng ta
            using (SqlConnection conn = new SqlConnection(connSTR))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);


                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(data);
                conn.Close();
            }
            return data;
        }
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            // Sử dụng using vì nếu khi có lõi nó sẽ tiếp tục chạy và giải phóng bộ nhớ cho chúng ta
            using (SqlConnection conn = new SqlConnection(connSTR))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);


                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();
                conn.Close();
            }
            return data;
        }
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            // Sử dụng using vì nếu khi có lõi nó sẽ tiếp tục chạy và giải phóng bộ nhớ cho chúng ta
            using (SqlConnection conn = new SqlConnection(connSTR))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);


                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();
                conn.Close();
            }
            return data;
        }
    }
}
