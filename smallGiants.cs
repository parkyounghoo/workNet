using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace workNet
{
    class smallGiants
    {
        public void getsmallGiants()
        {
            List<Covid19SidoInfState> list = new List<Covid19SidoInfState>();
            string url = "http://openapi.work.go.kr/opi/smallGiants/smallGiants.do?authKey=WNKH9WSDCFEAZSE8TVPO72VR1HJ&returnType=XML"; // URL

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                XDocument doc = XDocument.Parse(results);

                var itemList = from r in doc.Descendants("smallGiant")
                    select new Covid19SidoInfState
                    {
                        selYear = r.Element("selYear") == null ? "" : r.Element("selYear").Value,
                        sgBrandNm = r.Element("sgBrandNm") == null ? "" : r.Element("sgBrandNm").Value,
                        coNm = r.Element("coNm") == null ? "" : r.Element("coNm").Value,
                        busiNo = r.Element("busiNo") == null ? "" : r.Element("busiNo").Value,
                        reperNm = r.Element("reperNm") == null ? "" : r.Element("reperNm").Value,
                        superIndTpCd = r.Element("superIndTpCd") == null ? "" : r.Element("superIndTpCd").Value,
                        superIndTpNm = r.Element("superIndTpNm") == null ? "" : r.Element("superIndTpNm").Value,
                        indTpCd = r.Element("indTpCd") == null ? "" : r.Element("indTpCd").Value,
                        indTpNm = r.Element("indTpNm") == null ? "" : r.Element("indTpNm").Value,
                        coTelNo = r.Element("coTelNo") == null ? "" : r.Element("coTelNo").Value,
                        regionCd = r.Element("regionCd") == null ? "" : r.Element("regionCd").Value,
                        regionNm = r.Element("regionNm") == null ? "" : r.Element("regionNm").Value,
                        coAddr = r.Element("coAddr") == null ? "" : r.Element("coAddr").Value,
                        coContent = r.Element("coContent") == null ? "" : r.Element("coContent").Value,
                        coMainProd = r.Element("coMainProd") == null ? "" : r.Element("coMainProd").Value,
                        coGdpnt = r.Element("coGdpnt") == null ? "" : r.Element("coGdpnt").Value,
                        coHomePage = r.Element("coHomePage") == null ? "" : r.Element("coHomePage").Value,
                        alwaysWorkerCnt = r.Element("alwaysWorkerCnt") == null ? "" : r.Element("alwaysWorkerCnt").Value,
                    };
                list = itemList.ToList();
            }

            Console.WriteLine(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list.Count + ": " + i + "진행중....");
                StringBuilder sb = new StringBuilder();
                sb.Append(" insert into worknet_smallGiants values(");
                sb.Append(" '" + list[i].selYear.Replace("'","") + "',");
                sb.Append(" '" + list[i].sgBrandNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].coNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].busiNo.Replace("'", "") + "',");
                sb.Append(" '" + list[i].reperNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].superIndTpCd.Replace("'", "") + "',");
                sb.Append(" '" + list[i].superIndTpNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].indTpCd.Replace("'", "") + "',");
                sb.Append(" '" + list[i].indTpNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].coTelNo.Replace("'", "") + "',");
                sb.Append(" '" + list[i].regionCd.Replace("'", "") + "',");
                sb.Append(" '" + list[i].regionNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].coAddr.Replace("'", "") + "',");
                sb.Append(" '" + list[i].coContent.Replace("'", "") + "',");
                sb.Append(" '" + list[i].coMainProd.Replace("'", "") + "',");
                sb.Append(" '" + list[i].coGdpnt.Replace("'", "") + "',");
                sb.Append(" '" + list[i].coHomePage.Replace("'", "") + "',");
                sb.Append(" '" + list[i].alwaysWorkerCnt.Replace("'", "") + "',");
                sb.Append(" getdate() )");

                Program.insert(sb.ToString());
            }
        }
    }

    internal class Covid19SidoInfState
    {
        public string selYear { get; set; }
	    public string sgBrandNm { get; set; }
	    public string coNm { get; set; }
	    public string busiNo { get; set; }
	    public string reperNm { get; set; }
	    public string superIndTpCd { get; set; }
	    public string superIndTpNm { get; set; }
	    public string indTpCd { get; set; }
	    public string indTpNm { get; set; }
	    public string coTelNo { get; set; }
	    public string regionCd { get; set; }
	    public string regionNm { get; set; }
	    public string coAddr { get; set; }
	    public string coContent { get; set; }
	    public string coMainProd { get; set; }
	    public string coGdpnt { get; set; }
	    public string coHomePage { get; set; }
	    public string alwaysWorkerCnt { get; set; }
    }
}
