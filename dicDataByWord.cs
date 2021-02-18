using Newtonsoft.Json.Linq;
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
    class dicDataByWord
    {
        public void getdicDataByWord()
        {
            DataSet ds = Program.selectDS("select dutyCd + compUnitCd as code, RIGHT(compUnitName,LEN(compUnitName) - CHARINDEX('.',compUnitName)) as name from [ncs_detail]");

            int time = 0;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                time++;
                if (time == 50)
                {
                    time = 0;
                    Console.WriteLine("대기중 ....");
                    Thread.Sleep(5000);
                }

                DataRow dr = ds.Tables[0].Rows[j];

                string url = "http://openapi.work.go.kr/opi/opi/opia/dicDataByCodeApi.do?authKey=WNKGRAG0SIOIC6OPH6AI42VR1HK" +
                    "&code=" + dr["code"];

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    string json = wc.DownloadString(url);

                    JObject jo = JObject.Parse(json); 
                    var list = jo.SelectToken("result")[dr["name"]];

                    if (list != null)
                    {
                        dicDataByWord_info model = new dicDataByWord_info();
                        model.ablt_def = list.SelectToken("ablt_def") == null ? "" : list["ablt_def"].ToString();
                        model.abltLabel = list.SelectToken("abltLabel") == null ? "" : list["abltLabel"].ToString();
                        model.ablt = list.SelectToken("ablt") == null ? "" : list["ablt"].ToString();
                        model.ablt_unit = list.SelectToken("ablt_unit") == null ? "" : list["ablt_unit"].ToString();
                        model.job_lrcl_cd = list.SelectToken("job_lrcl_cd") == null ? "" : list["job_lrcl_cd"].ToString();
                        model.job_lcfn = list.SelectToken("job_lcfn") == null ? "" : list["job_lcfn"].ToString();
                        model.job_mlsf_cd = list.SelectToken("job_mlsf_cd") == null ? "" : list["job_mlsf_cd"].ToString();
                        model.job_mcn = list.SelectToken("job_mcn") == null ? "" : list["job_mcn"].ToString();
                        model.job_scla_cd = list.SelectToken("job_scla_cd") == null ? "" : list["job_scla_cd"].ToString();
                        model.job_scfn = list.SelectToken("job_scfn") == null ? "" : list["job_scfn"].ToString();
                        model.job_sdvn_cd = list.SelectToken("job_sdvn_cd") == null ? "" : list["job_sdvn_cd"].ToString();
                        model.job_sdvn = list.SelectToken("job_sdvn") == null ? "" : list["job_sdvn"].ToString();
                        model.fctr = list.SelectToken("fctr") == null ? "" : getJsonText(list["fctr"], new string[2] { "ablt_fctr_label", "ablt_fctr" });
                        model.req = list.SelectToken("req") == null ? "" : getJsonText(list["req"], new string[2] { "req_label", "req" });
                        model.hrd = list.SelectToken("hrd") == null ? "" : getJsonText(list["hrd"], new string[2] { "job_hrd_label", "job_hrd" });
                        model.occp = list.SelectToken("occp") == null ? "" : getJsonText(list["occp"], new string[2] { "job_occp_label", "job_occp" });
                        model.sbjt = list.SelectToken("sbjt") == null ? "" : getJsonText(list["sbjt"], new string[2] { "sbjt_label", "sbjt" });

                        StringBuilder sb = new StringBuilder();
                        sb.Append(" insert into dicDataByWord_info values(");
                        sb.Append(" '" + model.ablt_def.Replace("'", "") + "',");
                        sb.Append(" '" + model.abltLabel.Replace("'", "") + "',");
                        sb.Append(" '" + model.ablt.Replace("'", "") + "',");
                        sb.Append(" '" + model.ablt_unit.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_lrcl_cd.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_lcfn.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_mlsf_cd.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_mcn.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_scla_cd.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_scfn.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_sdvn_cd.Replace("'", "") + "',");
                        sb.Append(" '" + model.job_sdvn.Replace("'", "") + "',");
                        sb.Append(" '" + model.fctr.Replace("'", "") + "',");
                        sb.Append(" '" + model.req.Replace("'", "") + "',");
                        sb.Append(" '" + model.hrd.Replace("'", "") + "',");
                        sb.Append(" '" + model.occp.Replace("'", "") + "',");
                        sb.Append(" '" + model.sbjt.Replace("'", "") + "')");

                        Program.insert(sb.ToString());
                    }
                }
            }
        }

        public string getJsonText(JToken token, string[] name)
        {
            string val = "";
            for (int i = 0; i < token.Count(); i++)
            {
                val += token[i][name[0]] + "|" + token[i][name[1]] + "||";
            }
            return val;
        }
    }

    internal class dicDataByWord_info
    {
        public string ablt_def { get; set; }
        public string abltLabel { get; set; }
        public string ablt { get; set; }
        public string ablt_unit { get; set; }
        public string job_lrcl_cd { get; set; }
        public string job_lcfn { get; set; }
        public string job_mlsf_cd { get; set; }
        public string job_mcn { get; set; }
        public string job_scla_cd { get; set; }
        public string job_scfn { get; set; }
        public string job_sdvn_cd { get; set; }
        public string job_sdvn { get; set; }
        public string fctr { get; set; }
        public string req { get; set; }
        public string hrd { get; set; }
        public string occp { get; set; }
        public string sbjt { get; set; }
    }
}
