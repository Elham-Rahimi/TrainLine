using System.Net;

namespace TrainLine.CurrencyConverter.Exceptions
{
    public class CurrencyNotSupportedException : ApplicationException
    {
        public CurrencyNotSupportedException() :
            base((int)HttpStatusCode.NotFound, "the currency is not supported")
        {
        }
    }
}
