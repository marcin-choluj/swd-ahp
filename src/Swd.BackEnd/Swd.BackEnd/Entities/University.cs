using MongoDB.Bson;

namespace Swd.BackEnd.Entities
{
    public class University
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public double Easyness { get; set; }
        public double Job { get; set; }
        public double Prestige { get; set; }
        public double Financies { get; set; }
        public double Fun { get; set; }
    }
}