using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DTO
{
    public class Table
    { 
        public Table(int id, string name, string status)
        {
            this.ID = id;
            this.Name = name;
            this.Status = status;
        }

        public Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Status = row["status"].ToString();
            this.Timestart = row["timestart"].ToString();
        }

        public string timestart;

        public string Timestart
        {
            get { return timestart; }
            set { timestart = value; }
        }

        public string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
    }
}
