using Newtonsoft.Json;

namespace KJ1012.Domain
{
    public class ResultEx
    {
        public bool Flag { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }

        public static ResultEx Init(bool flag = true, string msg = "", object data = null)
        {
            return new ResultEx(flag, msg, data);
        }
        public ResultEx()
        {
            Flag = true;
        }
        public ResultEx(string msg)
        {
            Flag = false;
            Msg = msg;
        }
        public ResultEx(bool flag)
        {
            Flag = flag;
        }
        public ResultEx(bool flag, string msg)
        {
            Flag = flag;
            Msg = msg;
        }
        public ResultEx(bool flag, string msg, object data)
        {
            Flag = flag;
            Msg = msg;
            Data = data;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
