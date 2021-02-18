using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace workNet
{
    class ncsInfo
    {
        public void getncsInfo()
        {
            List<ncs_detail> list = new List<ncs_detail>();

            string url = "http://www.ncs.go.kr/api/openapi3.do?serviceKey=SfRfqLWT2LlZAqs5Ug3g9ro6HYeA3Xznw8tH%2Bs%2FGzE3exHM46aR%2BFlJgYMcov6dYn3csiT5rG16%2BLVi8IQbYtw%3D%3D&pageNo=1&numOfRows=10&returnType=xml"; // URL

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            int totalCount = 0;
            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(results);
                Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

                foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
                {
                    int keyInt = 0;
                    string keyName = element.Name.LocalName;

                    if (keyName == "totCnt")
                    {
                        totalCount = int.Parse(element.Value);

                        break;
                    }
                }
            }

            int forCount = (totalCount / 323) + 1;
            for (int i = 1; i < forCount; i++)
            {
                string param = "http://www.ncs.go.kr/api/openapi3.do?serviceKey=SfRfqLWT2LlZAqs5Ug3g9ro6HYeA3Xznw8tH%2Bs%2FGzE3exHM46aR%2BFlJgYMcov6dYn3csiT5rG16%2BLVi8IQbYtw%3D%3D"; // Service Key
                param += "&pageNo=" + i;
                param += "&numOfRows=323";
                param += "&returnType=xml";

                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(param);
                request.Method = "GET";

                string results2 = "";
                using (HttpWebResponse response = request2.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    results2 = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(results2);

                    var itemList = from r in doc.Descendants("row")
                                   select new ncs_detail
                                   {
                                       dutyCd = r.Element("dutyCd") == null ? "" : r.Element("dutyCd").Value,
                                       dutySvcNo = r.Element("dutySvcNo") == null ? "" : r.Element("dutySvcNo").Value,
                                       ncsClCd = r.Element("ncsClCd") == null ? "" : r.Element("ncsClCd").Value,
                                       compUnitCd = r.Element("compUnitCd") == null ? "" : r.Element("compUnitCd").Value,
                                       compUnitName = r.Element("compUnitName") == null ? "" : r.Element("compUnitName").Value,
                                       compUnitDef = r.Element("compUnitDef") == null ? "" : r.Element("compUnitDef").Value,
                                       compUnitLevel = r.Element("compUnitLevel") == null ? "" : r.Element("compUnitLevel").Value
                                   };
                    list = itemList.ToList();

                    for (int j = 0; j < list.Count; j++)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into ncs_detail values(");
                        sb.Append(" '" + list[j].dutyCd.Replace("'", "") + "',");
                        sb.Append(" '" + list[j].dutySvcNo.Replace("'", "") + "',");
                        sb.Append(" '" + list[j].ncsClCd.Replace("'", "") + "',");
                        sb.Append(" '" + list[j].compUnitCd.Replace("'", "") + "',");
                        sb.Append(" '" + list[j].compUnitName.Replace("'", "") + "',");
                        sb.Append(" '" + list[j].compUnitDef.Replace("'", "") + "',");
                        sb.Append(" '" + list[j].compUnitLevel.Replace("'", "") + "')");

                        Program.insert(sb.ToString());
                    }
                }
            }
        }
    }

    class ncs_detail
    {
        public string dutyCd { get; set; }
        public string dutySvcNo { get; set; }
        public string ncsClCd { get; set; }
        public string compUnitCd { get; set; }
        public string compUnitName { get; set; }
        public string compUnitDef { get; set; }
        public string compUnitLevel { get; set; }
    }
}
