using System;

namespace ExchRatesWCFService.Services.Interfaces
{
    public interface IInfoService
    {
        T GetCodesInfoXML<T>() where T : class;
        T GetDailyInfoXML<T>(DateTime date) where T : class;
    }
}
