using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Swd.WebHost.Entities
{
    public class University
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public double Easyness { get; set; }
        public double Job { get; set; }
        public double Prestige { get; set; }
        public double Financies { get; set; }
        public double Fun { get; set; }
        public string AddedBy { get; set; }
    }
}