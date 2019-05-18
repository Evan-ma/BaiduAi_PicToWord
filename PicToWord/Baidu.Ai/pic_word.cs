using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;

namespace Baidu.Ai.Properties
{
    public class  Bdai
    {
        [Obsolete]
        private static string APP_ID = ConfigurationSettings.AppSettings["APP_ID"];
        [Obsolete]
        private static string API_KEY = ConfigurationSettings.AppSettings["API_KEY"];
        [Obsolete]
        private static string SECRET_KEY = ConfigurationSettings.AppSettings["SECRET_KEY"];
        public string getwordandfix(string path)
        {
            string word = "";
            Console.WriteLine("加载图片");
            var image = File.ReadAllBytes(path);
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                 {"language_type", "CHN_ENG"},
                 {"detect_direction", "true"},
                 {"detect_language", "true"},
                 {"probability", "true"}
            };
            Console.WriteLine("识别图片");
            try
            {
                object result1 = client.GeneralBasic(image, options);

                JavaScriptSerializer js = new JavaScriptSerializer();
                getword getwd = js.Deserialize<getword>(result1.ToString());
                for (int i = 0; i < getwd.words_result_num; i++)
                {
                    word += getwd.words_result[i].words + "\r\n";
                }
                Console.WriteLine("文字处理完成");
            }
            catch
            {
                return "对不起，图片质量差无法识别";
            }
            return word;
        }

        public string onlygetword(string path)
        {

            string word = "";
            var image = File.ReadAllBytes(path);
            Console.WriteLine("加载图片1");
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间    
            Console.WriteLine("识别图片2");
            try
            {
                object result = client.GeneralBasic(image);
                // 带参数调用通用文字识别, 图片参数为本地图片
                JavaScriptSerializer js = new JavaScriptSerializer();
                getword2 getwd = js.Deserialize<getword2>(result.ToString());
                for (int i = 0; i < getwd.words_result_num; i++)
                {
                    word += getwd.words_result[i].words + "\r\n";
                }
                Console.WriteLine("识别文字完成");

            }
            catch
            {
                return "对不起，图片质量差无法识别";
            }
            return word ;
        }
       
        public class getword
        {
            public long log_id { get; set; }
            public int direction { get; set; }
            public int words_result_num { get; set; }
            public List<words_result> words_result { get; set; }
            public int language { get; set; }
        }
        public class words_result
        {
            public string words { get; set; }
            public probability probability { get; set; }
        }
        public class probability
        {
            public float variance { get; set; }
            public float average { get; set; }
            public float min { get; set; }
        }
        public class getword2
        {
            public long log_id { get; set; }
            public int words_result_num { get; set; }
            public List<words_result2> words_result { get; set; }
        }
        public class words_result2
        {
            public string words { get; set; }
        }
    }
}