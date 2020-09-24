namespace KJ1012.Core.Data
{
    public class ResponseResult
    {
        public ResponseResult()
        {
        }
        public ResponseResult(string status, string message)
        {
            Status = status;
            Message = message;
        }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
