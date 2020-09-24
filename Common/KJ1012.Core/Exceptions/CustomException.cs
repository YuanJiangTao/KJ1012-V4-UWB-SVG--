namespace KJ1012.Core.Exceptions
{
    public class CustomException : System.ApplicationException
    {
        public CustomException():base()
        {

        }

        public CustomException(string message):base(message)
        {

        }

        public CustomException(string message, System.Exception exception) : base(message, exception)
        {

        }
    }
}
