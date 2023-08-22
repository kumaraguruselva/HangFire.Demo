using Hangfire.Logging;
using HangFire.Demo.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HangFire.Demo.Services
{
    public class SeviceManagement : IserviceManagement
    {
        private readonly IConfiguration _configuration;
        public SeviceManagement(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail()
        {
            Console.WriteLine($"SendEmail :Sending email is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public DataTable SyncData()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            var query = "SELECT * FROM Flight";
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            adapter.Fill(ds);

            Console.WriteLine($"SyncData :sync is going on..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return ds.Tables[0];
        }

        public void InsertRecords(Flight flight)
        {
            try
            {
                string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
                var query = "INSERT INTO Flight (Name,seatcount) VALUES(@Name,@seatcount)";
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("Name", flight.Name);
                    cmd.Parameters.AddWithValue("seatcount", flight.seatcount);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"UpdatedDatabase :Updating the database is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public List<Flight> GetAllRecords()
        {
            DataTable flights = SyncData();
            return (from DataRow dr in flights.Rows
                    select new Flight()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        seatcount = Convert.ToInt32(dr["seatcount"]),
                    }).ToList();


        }
    }
}
