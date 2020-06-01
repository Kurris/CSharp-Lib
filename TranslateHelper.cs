using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpLib
{
    /// <summary>
    /// 百度翻译Api帮助
    /// </summary>
    public class TranslateHelper
    {
        private readonly string _msbaiduAppUserId = "";
        private readonly string _msbaiduAppPwd = "";

        private string _mstranType = "en";

        private HttpClient _mhttpClient = new HttpClient();

        private Regex _mregChina = new Regex("^[^\x00-\xFF]");
        private Regex _mregEnglish = new Regex("^[a-zA-Z]");


        /// <summary>
        /// 获取翻译结果
        /// </summary>
        /// <param name="Content">需要翻译的内容</param>
        /// <returns>翻译结果</returns>
        public async Task<string> GetResult(string Content)
        {
            if (string.IsNullOrEmpty(Content)) return string.Empty;

            if (_mregEnglish.IsMatch(Content))
                _mstranType = "zh";
            else
                _mstranType = "en";

            string surl = $"http://api.fanyi.baidu.com/api/trans/vip/translate?" +
                        $"q={Content}&" +
                        $"from=auto&" +
                        $"to={_mstranType}&" +
                        $"appid={_msbaiduAppUserId}&" +
                        $"salt=1435660288&" +
                        $"sign={MD5Helper.GetStringMD5(string.Concat(_msbaiduAppUserId, Content, "1435660288", _msbaiduAppPwd))}&" +
                        $"tts=1&" +
                        $"dict=1";

            var httpResponse = await _mhttpClient.GetAsync(surl);

            string sResult = string.Empty;

            if (httpResponse.IsSuccessStatusCode)
            {
                string sresult = await httpResponse.Content.ReadAsStringAsync();
                if (sresult.Contains("error_code"))
                {
                    sResult = JObject.Parse(sresult)["error_msg"].Value<string>();
                }
                JToken jToken = JObject.Parse(sresult)["trans_result"];
                sResult = jToken[0]["dst"].Value<string>();

                return sResult;
            }
            return string.Empty;
        }
    }
}
