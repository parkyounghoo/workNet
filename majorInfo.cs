using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace workNet
{
    class majorInfo
    {
        public void getmajorInfo()
        {

            List<major_Info> list = new List<major_Info>();
            string url = "http://openapi.work.go.kr/opi/majorInfo/majorSrch.do?authKey=WNKH9WSDCFEAZSE8TVPO72VR1HJ&returnType=XML&target=MAJORCD&srchType=A"; // URL

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                XDocument doc = XDocument.Parse(results);

                var itemList = from r in doc.Descendants("majorList")
                    select new major_Info
                    {
                        majorGb = r.Element("majorGb") == null ? "" : r.Element("majorGb").Value,
                        knowDtlSchDptNm = r.Element("knowDtlSchDptNm") == null ? "" : r.Element("knowDtlSchDptNm").Value,
                        knowSchDptNm = r.Element("knowSchDptNm") == null ? "" : r.Element("knowSchDptNm").Value,
                        empCurtState1Id = r.Element("empCurtState1Id") == null ? "" : r.Element("empCurtState1Id").Value,
                        empCurtState2Id = r.Element("empCurtState2Id") == null ? "" : r.Element("empCurtState2Id").Value
                    };

                list = itemList.ToList();
            }

            for (int i = 0; i < list.Count; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" insert into major_Info values(");
                sb.Append(" '" + list[i].majorGb.Replace("'", "") + "',");
                sb.Append(" '" + list[i].knowDtlSchDptNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].knowSchDptNm.Replace("'", "") + "',");
                sb.Append(" '" + list[i].empCurtState1Id.Replace("'", "") + "',");
                sb.Append(" '" + list[i].empCurtState2Id.Replace("'", "") + "')");


                Program.insert(sb.ToString());
            }
        }

        public void getmajorInfoDetail()
        {
            DataSet ds = Program.selectDS("select * from major_Info");

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                DataRow dr = ds.Tables[0].Rows[j];
                List<major_Info_detail> list = new List<major_Info_detail>();
                string url = "http://openapi.work.go.kr/opi/majorInfo/majorSrch.do?authKey=WNKH9WSDCFEAZSE8TVPO72VR1HJ&returnType=XML&target=MAJORDTL&majorGb=1" +
                    "&empCurtState1Id=" + dr["empCurtState1Id"] + 
                    "&empCurtState2Id=" + dr["empCurtState2Id"]; // URL

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                string results = "";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    results = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(results);

                    var itemList = from r in doc.Descendants("majorSum")
                        select new major_Info_detail
                        {
                            knowDptNm = r.Element("knowDptNm") == null ? "" : r.Element("knowDptNm").Value,
                            knowSchDptNm = r.Element("knowSchDptNm") == null ? "" : r.Element("knowSchDptNm").Value,
                            knowDptId = r.Element("knowDptId") == null ? "" : r.Element("knowDptId").Value,
                            knowSchDptId = r.Element("knowSchDptId") == null ? "" : r.Element("knowSchDptId").Value,
                            schDptIntroSum = r.Element("schDptIntroSum") == null ? "" : r.Element("schDptIntroSum").Value,
                            aptdIntrstCont = r.Element("aptdIntrstCont") == null ? "" : r.Element("aptdIntrstCont").Value,
                            relSchDptList = Program.getStringList(doc, "relSchDptList"),
                            mainSubjectList = Program.getStringList(doc, "mainSubjectList"),
                            licList = Program.getStringList(doc, "licList"),
                            schDptList = getschDptList(doc),
                            advncFieldCont = r.Element("advncFieldCont") == null ? "" : r.Element("advncFieldCont").Value,
                            relAdvanJobsList = getrelAdvanJobsList(doc),
                            stateList = getstateList(doc)
                        };

                    list = itemList.ToList();
                }

                for (int i = 0; i < list.Count; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" insert into major_Info_detail values(");
                    sb.Append(" '" + list[i].knowDptNm.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].knowSchDptNm.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].knowDptId.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].knowSchDptId.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].schDptIntroSum.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].aptdIntrstCont.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].relSchDptList.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].mainSubjectList.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].licList.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].schDptList.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].advncFieldCont.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].relAdvanJobsList.Replace("'", "") + "',");
                    sb.Append(" '" + list[i].stateList.Replace("'", "") + "')");

                    Program.insert(sb.ToString());
                }
            }
        }
        private string getschDptList(XDocument doc)
        {
            var itemList = from r in doc.Descendants("schDptList")
                select new schDpt
                {
                    schDptNm = r.Element("schDptNm") == null ? "" : r.Element("schDptNm").Value,
                    univNm = r.Element("univNm") == null ? "" : r.Element("univNm").Value,
                    univUrl = r.Element("univUrl") == null ? "" : r.Element("univUrl").Value
                };

            string val = "";
            for (int i = 0; i < itemList.ToList().Count(); i++)
            {
                val += itemList.ToList()[i].schDptNm + "|" + itemList.ToList()[i].univNm + "|" + itemList.ToList()[i].univUrl + "||";
            }

            return val;
        }
        
        private string getrelAdvanJobsList(XDocument doc)
        {
            var itemList = from r in doc.Descendants("relAdvanJobsList")
                           select new relAdvanJobs
                           {
                               knowJobCd = r.Element("knowJobCd") == null ? "" : r.Element("knowJobCd").Value,
                               knowJobNm = r.Element("knowJobNm") == null ? "" : r.Element("knowJobNm").Value
                           };

            string val = "";
            for (int i = 0; i < itemList.ToList().Count(); i++)
            {
                val += itemList.ToList()[i].knowJobCd + "|" + itemList.ToList()[i].knowJobNm + "||";
            }

            return val;
        }
        //

        private string getstateList(XDocument doc)
        {
            var itemList = from r in doc.Descendants("stateList")
                           select new stateModel
                           {
                               apprPsnCnt = r.Element("apprPsnCnt") == null ? "" : r.Element("apprPsnCnt").Value,
                               empRatio = r.Element("empRatio") == null ? "" : r.Element("empRatio").Value,
                               emprCnt = r.Element("emprCnt") == null ? "" : r.Element("emprCnt").Value,
                               graduPsnCnt = r.Element("graduPsnCnt") == null ? "" : r.Element("graduPsnCnt").Value,
                               majorNm = r.Element("majorNm") == null ? "" : r.Element("majorNm").Value,
                               mtclPsnCnt = r.Element("mtclPsnCnt") == null ? "" : r.Element("mtclPsnCnt").Value,
                               univGbnNm = r.Element("univGbnNm") == null ? "" : r.Element("univGbnNm").Value
                           };

            string val = "";
            for (int i = 0; i < itemList.ToList().Count(); i++)
            {
                val += "|" + itemList.ToList()[i].apprPsnCnt
                    + "|" + itemList.ToList()[i].empRatio
                    + "|" + itemList.ToList()[i].emprCnt
                    + "|" + itemList.ToList()[i].graduPsnCnt
                    + "|" + itemList.ToList()[i].majorNm
                    + "|" + itemList.ToList()[i].mtclPsnCnt
                    + "|" + itemList.ToList()[i].univGbnNm + "||";
            }

            return val;
        }
    }

    internal class major_Info
    {
        public string majorGb { get; set; }
	    public string knowDtlSchDptNm { get; set; }
	    public string knowSchDptNm { get; set; }
	    public string empCurtState1Id { get; set; }
	    public string empCurtState2Id { get; set; }
    }

    internal class major_Info_detail
    {
        public string knowDptNm { get; set; }
        public string knowSchDptNm { get; set; }
        public string knowDptId { get; set; }
        public string knowSchDptId { get; set; }
        public string schDptIntroSum { get; set; }
        public string aptdIntrstCont { get; set; }
        public string relSchDptList { get; set; }
        public string mainSubjectList { get; set; }
        public string licList { get; set; }
        public string schDptList { get; set; }
        public string advncFieldCont { get; set; }
        public string relAdvanJobsList { get; set; }
        public string stateList { get; set; }
    }

    internal class schDpt
    {
        public string schDptNm { get; set; }
        public string univNm { get; set; }
        public string univUrl { get; set; }
    }

    internal class relAdvanJobs
    {
        public string knowJobCd { get; set; }
        public string knowJobNm { get; set; }
    }

    internal class stateModel
    { 
        public string apprPsnCnt { get; set; }
        public string empRatio { get; set; }
        public string emprCnt { get; set; }
        public string graduPsnCnt { get; set; }
        public string majorNm { get; set; }
        public string mtclPsnCnt { get; set; }
        public string univGbnNm { get; set; }
    }
}
