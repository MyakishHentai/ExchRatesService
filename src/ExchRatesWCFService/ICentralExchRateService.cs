using ExchRatesWCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ExchRatesWCFService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "ICentralExchRateService" в коде и файле конфигурации.
    [ServiceContract]
    public interface ICentralExchRateService
    {
        [OperationContract]
        void Update();

        [OperationContract]
        QuoteDesc GetCurrencyQuotes(DateTime date);

        [OperationContract]
        CodesDesc GetCurrencyCodes();
    }
}
