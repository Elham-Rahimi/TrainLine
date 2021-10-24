using System.Net;

namespace TrainLine.CurrencyConverter.Exceptions
{
    public class NullResponseException : ApplicationException
    {
        public NullResponseException() :
            base((int)HttpStatusCode.BadRequest, "service doesn't respond properly")
        {
        }
    }
}
