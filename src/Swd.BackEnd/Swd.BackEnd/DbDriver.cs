using MongoDB.Driver;
using Swd.BackEnd;
using Swd.BackEnd.Entities;

namespace Swd.Backend
{
    public class DbDriver
    {
        private string ConnectionString { get; set; }
        private MongoClient Client { get; set; }
        private MongoServer Server { get; set; }
        public MongoDatabase Database { get; set; }
        public MongoCollection UniversitiesCollection { get; set; }

        public DbDriver()
        {
            ConnectionString = "mongodb://localhost";
            Client = new MongoClient(ConnectionString);
            Server = Client.GetServer();
            Database = Server.GetDatabase("test");
            UniversitiesCollection = Database.GetCollection<University>("Universities");
        }
    }
}