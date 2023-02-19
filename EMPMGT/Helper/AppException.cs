using System.Globalization;

namespace EMPMGT.Helper
{
    // RJ - Why I have created this exception class becuase i need to route everything with our project scope way which throwing application specific exceptions. 
    public class AppException: Exception
    {
        public AppException(): base() { }

        public AppException(string message): base(message) { }

        public AppException(string message, params object[] args): base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
