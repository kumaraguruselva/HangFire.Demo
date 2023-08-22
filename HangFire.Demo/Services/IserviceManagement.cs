using HangFire.Demo.Models;
using System.Data;

namespace HangFire.Demo.Services
{
    public interface IserviceManagement
    {
        void SendEmail();
        void InsertRecords(Flight flight);

        DataTable SyncData();

        List<Flight> GetAllRecords();
    }
}
