using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace workNet
{
    class Program
    {
        static string connectionString = "server = localhost; uid = sa; pwd = 1111; database = PrivateData;";
        static void Main(string[] args)
        {
            smallGiants small = new smallGiants();
            small.getsmallGiants();

            //majorInfo major = new majorInfo();
            //major.getmajorInfo();
            //major.getmajorInfoDetail();

            //jobInfo job = new jobInfo();
            //job.getjobInfo();
            //job.getjobInfoDetail();

            //ncsInfo ncs = new ncsInfo();
            //ncs.getncsInfo();

            //dicDataByWord dataByWord = new dicDataByWord();
            //dataByWord.getdicDataByWord();

            //wantedInfo wanted = new wantedInfo();
            //wanted.getwantedInfo();
            //wanted.getwantedInfoDetail();
        }

        public static void insert(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static DataSet selectDS(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter();
                _SqlDataAdapter.SelectCommand = new SqlCommand(query, conn);
                _SqlDataAdapter.Fill(ds);

                return ds;
            }
        }

        public static string getStringList(XDocument doc, string descName)
        {
            var itemList = from r in doc.Descendants(descName)
                           select (string)r;

            string val = "";
            for (int i = 0; i < itemList.ToList().Count(); i++)
            {
                val += itemList.ToList()[i] + "|";
            }

            return val;
        }
    }
}
