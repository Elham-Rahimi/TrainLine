using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainLine.CurrencyConverter.Exceptions
{
    public interface IApplicationException
    {
        int GetCode();
        string GetMessage();
    }
}
