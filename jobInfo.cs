using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace workNet
{
    class jobInfo
    {
        public void getjobInfo()
        {

            List<jobList> list = new List<jobList>();
            string url = "http://openapi.work.go.kr/opi/opi/opia/jobSrch.do?authKey=WNKGRAG0SIOIC6OPH6AI42VR1HK&returnType=XML&target=JOBCD"; // URL

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                XDocument doc = XDocument.Parse(results);

                var itemList = from r in doc.Descendants("jobList")
                               select new jobList
                               {
                                   jobGb = r.Element("jobGb") == null ? "" : r.Element("jobGb").Value,
                                   jobClcd = r.Element("jobClcd") == null ? "" : r.Element("jobClcd").Value,
                                   jobClcdNM = r.Element("jobClcdNM") == null ? "" : r.Element("jobClcdNM").Value,
                                   jobCd = r.Element("jobCd") == null ? "" : r.Element("jobCd").Value,
                                   jobNm = r.Element("jobNm") == null ? "" : r.Element("jobNm").Value
                               };

                list = itemList.ToList();
            }

            for (int i = 0; i < list.Count; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" insert into jobList values(");
                sb.Append(" '" + list[i].jobGb.Replace("'", "") + "',");
                sb.Append(" '" + list[i].jobClcd.Replace("'", "") + "',");
                sb.Append(" '" + list[i].jobClcdNM.Replace("'", "") + "',");
                sb.Append(" '" + list[i].jobCd.Replace("'", "") + "',");
                sb.Append(" '" + list[i].jobNm.Replace("'", "") + "')");

                Program.insert(sb.ToString());
            }
        }

        public void getjobInfoDetail()
        {
            DataSet ds = Program.selectDS("select * from jobList");

            int time = 0;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                try
                {
                    time++;
                    if (time == 50)
                    {
                        time = 0;
                        Console.WriteLine("대기중 ....");
                        Thread.Sleep(5000);
                    }
                    
                    DataRow dr = ds.Tables[0].Rows[j];
                    List<jobList_detail> list = new List<jobList_detail>();
                    string url = "http://openapi.work.go.kr/opi/opi/opia/jobSrch.do?authKey=WNKGRAG0SIOIC6OPH6AI42VR1HK&returnType=XML&target=JOBDTL&jobGb=1&jobCd=" + dr["jobCd"] + "&dtlGb=1"; // URL

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";

                    string results = "";
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        results = reader.ReadToEnd();

                        XDocument doc = XDocument.Parse(results);

                        var itemList = from r in doc.Descendants("jobSum")
                                       select new jobList_detail
                                       {
                                           jobCd = r.Element("jobCd") == null ? "" : r.Element("jobCd").Value,
                                           jobLrclNm = r.Element("jobLrclNm") == null ? "" : r.Element("jobLrclNm").Value,
                                           jobMdclNm = r.Element("jobMdclNm") == null ? "" : r.Element("jobMdclNm").Value,
                                           jobSmclNm = r.Element("jobSmclNm") == null ? "" : r.Element("jobSmclNm").Value,
                                           jobSum = r.Element("jobSum") == null ? "" : r.Element("jobSum").Value,
                                           way = r.Element("way") == null ? "" : r.Element("way").Value,
                                           relMajorList = getrelMajorList(doc),
                                           relCertList = Program.getStringList(doc, "relCertList"),
                                           sal = r.Element("sal") == null ? "" : r.Element("sal").Value,
                                           jobSatis = r.Element("jobSatis") == null ? "" : r.Element("jobSatis").Value,
                                           jobProspect = r.Element("jobProspect") == null ? "" : r.Element("jobProspect").Value,
                                           jobStatus = r.Element("jobStatus") == null ? "" : r.Element("jobStatus").Value,
                                           jobAbil = r.Element("jobAbil") == null ? "" : r.Element("jobAbil").Value,
                                           knowldg = r.Element("knowldg") == null ? "" : r.Element("knowldg").Value,
                                           jobEnv = r.Element("jobEnv") == null ? "" : r.Element("jobEnv").Value,
                                           jobChr = r.Element("jobChr") == null ? "" : r.Element("jobChr").Value,
                                           jobIntrst = r.Element("jobIntrst") == null ? "" : r.Element("jobIntrst").Value,
                                           jobVals = r.Element("jobVals") == null ? "" : r.Element("jobVals").Value,
                                           jobActvImprtncs = r.Element("jobActvImprtncs") == null ? "" : r.Element("jobActvImprtncs").Value,
                                           jobActvLvls = r.Element("jobActvLvls") == null ? "" : r.Element("jobActvLvls").Value,
                                           relJobList = getrelJobList(doc)
                                       };

                        list = itemList.ToList();
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].jobCd.Replace("'", "") != "")
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" insert into jobList_detail values(");
                            sb.Append(" '" + list[i].jobCd.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobLrclNm.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobMdclNm.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobSmclNm.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobSum.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].way.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].relMajorList.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].relCertList.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].sal.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobSatis.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobProspect.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobStatus.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobAbil.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].knowldg.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobEnv.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobChr.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobIntrst.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobVals.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobActvImprtncs.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].jobActvLvls.Replace("'", "") + "',");
                            sb.Append(" '" + list[i].relJobList.Replace("'", "") + "')");

                            Program.insert(sb.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        private string getrelMajorList(XDocument doc)
        {
            var itemList = from r in doc.Descendants("relMajorList")
                           select new relMajor
                           {
                               majorCd = r.Element("majorCd") == null ? "" : r.Element("majorCd").Value,
                               majorNm = r.Element("majorNm") == null ? "" : r.Element("majorNm").Value
                           };

            string val = "";
            for (int i = 0; i < itemList.ToList().Count(); i++)
            {
                val += itemList.ToList()[i].majorCd + "|" + itemList.ToList()[i].majorNm + "||";
            }

            return val;
        }
        //
        private string getrelJobList(XDocument doc)
        {
            var itemList = from r in doc.Descendants("relJobList")
                           select new relJob
                           {
                               jobCd = r.Element("jobCd") == null ? "" : r.Element("jobCd").Value,
                               jobNm = r.Element("jobNm") == null ? "" : r.Element("jobNm").Value
                           };

            string val = "";
            for (int i = 0; i < itemList.ToList().Count(); i++)
            {
                val += itemList.ToList()[i].jobCd + "|" + itemList.ToList()[i].jobNm + "||";
            }

            return val;
        }
    }

    class jobList
    {
        public string jobGb { get; set; }
        public string jobClcd { get; set; }
        public string jobClcdNM { get; set; }
        public string jobCd { get; set; }
        public string jobNm { get; set; }
    }
    class jobList_detail
    {
        public string jobCd { get; set; }
        public string jobLrclNm { get; set; }
        public string jobMdclNm { get; set; }
        public string jobSmclNm { get; set; }
        public string jobSum { get; set; }
        public string way { get; set; }
        public string relMajorList { get; set; }//list
        public string relCertList { get; set; }//list
        public string sal { get; set; }
        public string jobSatis { get; set; }
        public string jobProspect { get; set; }
        public string jobStatus { get; set; }
        public string jobAbil { get; set; }
        public string knowldg { get; set; }
        public string jobEnv { get; set; }
        public string jobChr { get; set; }
        public string jobIntrst { get; set; }
        public string jobVals { get; set; }
        public string jobActvImprtncs { get; set; }
        public string jobActvLvls { get; set; }
        public string relJobList { get; set; }//list
    }

    class relMajor
    {
        public string majorCd { get; set; }
        public string majorNm { get; set; }
    }

    class relJob
    {
        public string jobCd { get; set; }
        public string jobNm { get; set; }
    }
}
