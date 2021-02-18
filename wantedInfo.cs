using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace workNet
{
    internal class wantedInfo
    {
        public void getwantedInfo()
        {
            List<major_Info> list = new List<major_Info>();
            string url = "http://openapi.work.go.kr/opi/opi/opia/wantedApi.do?authKey=WNKHWPRM1OUVIE2B0DDHS2VR1HK&callTp=L&returnType=XML&startPage=1&display=10"; // URL

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

                    if (keyName == "total")
                    {
                        totalCount = int.Parse(element.Value);

                        break;
                    }
                }
            }

            int forCount = (totalCount / 100) + 1;
            int time = 0;

            results = "";
            for (int i = 1; i < forCount; i++)
            {
                string url2 = "http://openapi.work.go.kr/opi/opi/opia/wantedApi.do?authKey=WNKHWPRM1OUVIE2B0DDHS2VR1HK&callTp=L&returnType=XML" +
                    "&startPage=" + i +
                    "&display=100";

                time++;
                if (time == 50)
                {
                    time = 0;
                    Console.WriteLine("대기중 ....");
                    Thread.Sleep(5000);
                }

                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url2);
                request2.Method = "GET";

                using (HttpWebResponse response = request2.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    results = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(results);

                    var itemList = from r in doc.Descendants("wanted")
                                   select new wanted_Info
                                   {
                                       wantedAuthNo = r.Element("wantedAuthNo") == null ? "" : r.Element("wantedAuthNo").Value,
                                       company = r.Element("company") == null ? "" : r.Element("company").Value,
                                       title = r.Element("title") == null ? "" : r.Element("title").Value,
                                       salTpNm = r.Element("salTpNm") == null ? "" : r.Element("salTpNm").Value,
                                       sal = r.Element("sal") == null ? "" : r.Element("sal").Value,
                                       minSal = r.Element("minSal") == null ? "" : r.Element("minSal").Value,
                                       maxSal = r.Element("maxSal") == null ? "" : r.Element("maxSal").Value,
                                       region = r.Element("region") == null ? "" : r.Element("region").Value,
                                       holidayTpNm = r.Element("holidayTpNm") == null ? "" : r.Element("holidayTpNm").Value,
                                       minEdubg = r.Element("minEdubg") == null ? "" : r.Element("minEdubg").Value,
                                       maxEdubg = r.Element("maxEdubg") == null ? "" : r.Element("maxEdubg").Value,
                                       career = r.Element("career") == null ? "" : r.Element("career").Value,
                                       regDt = r.Element("regDt") == null ? "" : r.Element("regDt").Value,
                                       closeDt = r.Element("closeDt") == null ? "" : r.Element("closeDt").Value,
                                       infoSvc = r.Element("infoSvc") == null ? "" : r.Element("infoSvc").Value,
                                       wantedInfoUrl = r.Element("wantedInfoUrl") == null ? "" : r.Element("wantedInfoUrl").Value,
                                       wantedMobileInfoUrl = r.Element("wantedMobileInfoUrl") == null ? "" : r.Element("wantedMobileInfoUrl").Value,
                                       smodifyDtm = r.Element("smodifyDtm") == null ? "" : r.Element("smodifyDtm").Value,
                                       zipCd = r.Element("zipCd") == null ? "" : r.Element("zipCd").Value,
                                       strtnmCd = r.Element("strtnmCd") == null ? "" : r.Element("strtnmCd").Value,
                                       basicAddr = r.Element("basicAddr") == null ? "" : r.Element("basicAddr").Value,
                                       detailAddr = r.Element("detailAddr") == null ? "" : r.Element("detailAddr").Value,
                                       empTpCd = r.Element("empTpCd") == null ? "" : r.Element("empTpCd").Value,
                                       jobsCd = r.Element("jobsCd") == null ? "" : r.Element("jobsCd").Value,
                                       prefCd = r.Element("prefCd") == null ? "" : r.Element("prefCd").Value
                                   };

                    for (int j = 0; j < itemList.ToList().Count; j++)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into wanted_Info values(");
                        sb.Append(" '" + itemList.ToList()[j].wantedAuthNo.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].company.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].title.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].salTpNm.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].sal.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].minSal.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].maxSal.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].region.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].holidayTpNm.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].minEdubg.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].maxEdubg.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].career.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].regDt.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].closeDt.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].infoSvc.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].wantedInfoUrl.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].wantedMobileInfoUrl.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].smodifyDtm.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].zipCd.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].strtnmCd.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].basicAddr.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].detailAddr.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].empTpCd.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].jobsCd.Replace("'", "") + "',");
                        sb.Append(" '" + itemList.ToList()[j].prefCd.Replace("'", "") + "')");

                        Program.insert(sb.ToString());
                    }
                }
            }
        }

        public void getwantedInfoDetail()
        {
            DataSet ds = Program.selectDS("select * from wanted_Info");

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                DataRow dr = ds.Tables[0].Rows[j];
                List<major_Info_detail> list = new List<major_Info_detail>();
                string url = "http://openapi.work.go.kr/opi/opi/opia/wantedApi.do?authKey=WNKHWPRM1OUVIE2B0DDHS2VR1HK&callTp=D&returnType=XML" +
                    "&wantedAuthNo=" + dr["wantedAuthNo"] +
                    "&infoSvc=VALIDATION";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                string results = "";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    results = reader.ReadToEnd();
                    XDocument doc = null;
                    try
                    {
                        doc = XDocument.Parse(results);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    var corp_Info_List = from r in doc.Descendants("corpInfo")
                                         select new corp_Info
                                         {
                                             corpNm = r.Element("corpNm") == null ? "" : r.Element("corpNm").Value,
                                             reperNm = r.Element("reperNm") == null ? "" : r.Element("reperNm").Value,
                                             totPsncnt = r.Element("totPsncnt") == null ? "" : r.Element("totPsncnt").Value,
                                             capitalAmt = r.Element("capitalAmt") == null ? "" : r.Element("capitalAmt").Value,
                                             yrSalesAmt = r.Element("yrSalesAmt") == null ? "" : r.Element("yrSalesAmt").Value,
                                             indTpCdNm = r.Element("indTpCdNm") == null ? "" : r.Element("indTpCdNm").Value,
                                             busiCont = r.Element("busiCont") == null ? "" : r.Element("busiCont").Value,
                                             corpAddr = r.Element("corpAddr") == null ? "" : r.Element("corpAddr").Value,
                                             homePg = r.Element("homePg") == null ? "" : r.Element("homePg").Value,
                                             busiSize = r.Element("busiSize") == null ? "" : r.Element("busiSize").Value
                                         };

                    var wanted_Info_Detail_List = from r in doc.Descendants("wantedInfo")
                                                  select new wanted_Info_Detail
                                                  {
                                                      jobsNm = r.Element("jobsNm") == null ? "" : r.Element("jobsNm").Value,
                                                      wantedTitle = r.Element("wantedTitle") == null ? "" : r.Element("wantedTitle").Value,
                                                      relJobsNm = r.Element("relJobsNm") == null ? "" : r.Element("relJobsNm").Value,
                                                      jobCont = r.Element("jobCont") == null ? "" : r.Element("jobCont").Value,
                                                      receiptCloseDt = r.Element("receiptCloseDt") == null ? "" : r.Element("receiptCloseDt").Value,
                                                      empTpNm = r.Element("empTpNm") == null ? "" : r.Element("empTpNm").Value,
                                                      collectPsncnt = r.Element("collectPsncnt") == null ? "" : r.Element("collectPsncnt").Value,
                                                      salTpNm = r.Element("salTpNm") == null ? "" : r.Element("salTpNm").Value,
                                                      enterTpNm = r.Element("enterTpNm") == null ? "" : r.Element("enterTpNm").Value,
                                                      eduNm = r.Element("eduNm") == null ? "" : r.Element("eduNm").Value,
                                                      forLang = r.Element("forLang") == null ? "" : r.Element("forLang").Value,
                                                      major = r.Element("major") == null ? "" : r.Element("major").Value,
                                                      certificate = r.Element("certificate") == null ? "" : r.Element("certificate").Value,
                                                      mltsvcExcHope = r.Element("mltsvcExcHope") == null ? "" : r.Element("mltsvcExcHope").Value,
                                                      compAbl = r.Element("compAbl") == null ? "" : r.Element("compAbl").Value,
                                                      pfCond = r.Element("pfCond") == null ? "" : r.Element("pfCond").Value,
                                                      etcPfCond = r.Element("etcPfCond") == null ? "" : r.Element("etcPfCond").Value,
                                                      selMthd = r.Element("selMthd") == null ? "" : r.Element("selMthd").Value,
                                                      rcptMthd = r.Element("rcptMthd") == null ? "" : r.Element("rcptMthd").Value,
                                                      submitDoc = r.Element("submitDoc") == null ? "" : r.Element("submitDoc").Value,
                                                      etcHopeCont = r.Element("etcHopeCont") == null ? "" : r.Element("etcHopeCont").Value,
                                                      workRegion = r.Element("workRegion") == null ? "" : r.Element("workRegion").Value,
                                                      indArea = r.Element("indArea") == null ? "" : r.Element("indArea").Value,
                                                      nearLine = r.Element("nearLine") == null ? "" : r.Element("nearLine").Value,
                                                      workdayWorkhrCont = r.Element("workdayWorkhrCont") == null ? "" : r.Element("workdayWorkhrCont").Value,
                                                      fourIns = r.Element("fourIns") == null ? "" : r.Element("fourIns").Value,
                                                      retirepay = r.Element("retirepay") == null ? "" : r.Element("retirepay").Value,
                                                      etcWelfare = r.Element("etcWelfare") == null ? "" : r.Element("etcWelfare").Value,
                                                      disableCvntl = r.Element("disableCvntl") == null ? "" : r.Element("disableCvntl").Value,
                                                      srchKeywordNm = Program.getStringList(doc, "keywordList"),
                                                      jobsCd = r.Element("jobsCd") == null ? "" : r.Element("jobsCd").Value,
                                                      minEdubgIcd = r.Element("minEdubgIcd") == null ? "" : r.Element("minEdubgIcd").Value,
                                                      maxEdubgIcd = r.Element("maxEdubgIcd") == null ? "" : r.Element("maxEdubgIcd").Value,
                                                      regionCd = r.Element("regionCd") == null ? "" : r.Element("regionCd").Value,
                                                      empTpCd = r.Element("empTpCd") == null ? "" : r.Element("empTpCd").Value,
                                                      enterTpCd = r.Element("enterTpCd") == null ? "" : r.Element("enterTpCd").Value,
                                                      salTpCd = r.Element("salTpCd") == null ? "" : r.Element("salTpCd").Value,
                                                      staAreaRegionCd = r.Element("staAreaRegionCd") == null ? "" : r.Element("staAreaRegionCd").Value,
                                                      lineCd = r.Element("lineCd") == null ? "" : r.Element("lineCd").Value,
                                                      staNmCd = r.Element("staNmCd") == null ? "" : r.Element("staNmCd").Value,
                                                      exitNoCd = r.Element("exitNoCd") == null ? "" : r.Element("exitNoCd").Value,
                                                      walkDistCd = r.Element("walkDistCd") == null ? "" : r.Element("walkDistCd").Value
                                                  };

                    for (int i = 0; i < corp_Info_List.ToList().Count; i++)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into corp_Info values(");
                        sb.Append(" '" + dr["wantedAuthNo"] + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].corpNm.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].reperNm.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].totPsncnt.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].capitalAmt.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].yrSalesAmt.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].indTpCdNm.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].busiCont.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].corpAddr.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].homePg.Replace("'", "") + "',");
                        sb.Append(" '" + corp_Info_List.ToList()[i].busiSize.Replace("'", "") + "')");

                        Program.insert(sb.ToString());
                    }

                    for (int i = 0; i < wanted_Info_Detail_List.ToList().Count; i++)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into wanted_Info_Detail values(");
                        sb.Append(" '" + dr["wantedAuthNo"] + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].jobsNm.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].wantedTitle.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].relJobsNm.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].jobCont.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].receiptCloseDt.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].empTpNm.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].collectPsncnt.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].salTpNm.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].enterTpNm.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].eduNm.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].forLang.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].major.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].certificate.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].mltsvcExcHope.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].compAbl.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].pfCond.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].etcPfCond.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].selMthd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].rcptMthd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].submitDoc.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].etcHopeCont.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].workRegion.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].indArea.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].nearLine.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].workdayWorkhrCont.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].fourIns.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].retirepay.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].etcWelfare.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].disableCvntl.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].srchKeywordNm.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].jobsCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].minEdubgIcd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].maxEdubgIcd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].regionCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].empTpCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].enterTpCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].salTpCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].staAreaRegionCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].lineCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].staNmCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].exitNoCd.Replace("'", "") + "',");
                        sb.Append(" '" + wanted_Info_Detail_List.ToList()[i].walkDistCd.Replace("'", "") + "')");

                        Program.insert(sb.ToString());
                    }
                }
            }
        }
    }

    internal class wanted_Info
    {
        public string wantedAuthNo { get; set; }
        public string company { get; set; }
        public string title { get; set; }
        public string salTpNm { get; set; }
        public string sal { get; set; }
        public string minSal { get; set; }
        public string maxSal { get; set; }
        public string region { get; set; }
        public string holidayTpNm { get; set; }
        public string minEdubg { get; set; }
        public string maxEdubg { get; set; }
        public string career { get; set; }
        public string regDt { get; set; }
        public string closeDt { get; set; }
        public string infoSvc { get; set; }
        public string wantedInfoUrl { get; set; }
        public string wantedMobileInfoUrl { get; set; }
        public string smodifyDtm { get; set; }
        public string zipCd { get; set; }
        public string strtnmCd { get; set; }
        public string basicAddr { get; set; }
        public string detailAddr { get; set; }
        public string empTpCd { get; set; }
        public string jobsCd { get; set; }
        public string prefCd { get; set; }
    }

    internal class wanted_Info_Detail
    {
        public string wantedAuthNo { get; set; }
        public string jobsNm { get; set; }
        public string wantedTitle { get; set; }
        public string relJobsNm { get; set; }
        public string jobCont { get; set; }
        public string receiptCloseDt { get; set; }
        public string empTpNm { get; set; }
        public string collectPsncnt { get; set; }
        public string salTpNm { get; set; }
        public string enterTpNm { get; set; }
        public string eduNm { get; set; }
        public string forLang { get; set; }
        public string major { get; set; }
        public string certificate { get; set; }
        public string mltsvcExcHope { get; set; }
        public string compAbl { get; set; }
        public string pfCond { get; set; }
        public string etcPfCond { get; set; }
        public string selMthd { get; set; }
        public string rcptMthd { get; set; }
        public string submitDoc { get; set; }
        public string etcHopeCont { get; set; }
        public string workRegion { get; set; }
        public string indArea { get; set; }
        public string nearLine { get; set; }
        public string workdayWorkhrCont { get; set; }
        public string fourIns { get; set; }
        public string retirepay { get; set; }
        public string etcWelfare { get; set; }
        public string disableCvntl { get; set; }
        public string srchKeywordNm { get; set; }
        public string jobsCd { get; set; }
        public string minEdubgIcd { get; set; }
        public string maxEdubgIcd { get; set; }
        public string regionCd { get; set; }
        public string empTpCd { get; set; }
        public string enterTpCd { get; set; }
        public string salTpCd { get; set; }
        public string staAreaRegionCd { get; set; }
        public string lineCd { get; set; }
        public string staNmCd { get; set; }
        public string exitNoCd { get; set; }
        public string walkDistCd { get; set; }
    }

    internal class corp_Info
    {
        public string wantedAuthNo { get; set; }
        public string corpNm { get; set; }
        public string reperNm { get; set; }
        public string totPsncnt { get; set; }
        public string capitalAmt { get; set; }
        public string yrSalesAmt { get; set; }
        public string indTpCdNm { get; set; }
        public string busiCont { get; set; }
        public string corpAddr { get; set; }
        public string homePg { get; set; }
        public string busiSize { get; set; }
    }
}