using System;

namespace Xochipilli.WebAPI.Classes
{
    public class ExceptionManager
    {
        public static string GetExceptionText(Exception ex)
        {
            if (ex == null)
            {
                return "";
            }
            else if (ex.InnerException == null)
            {
                return ex.ToString();
            }
            else
            {
                return ex.ToString() + "\n----- Inner Exception -----\n" + GetExceptionText(ex.InnerException);
            }
        }
    }
}