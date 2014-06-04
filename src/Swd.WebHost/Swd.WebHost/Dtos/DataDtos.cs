using System.Collections.Generic;
using ServiceStack;
using Swd.WebHost.Entities;

namespace Swd.WebHost.Dtos
{
    [Route("/universities/")]
    public class UniversitiesReq
    {
    }

    public class UniversitiesReqResponse
    {
        public List<University> Result { get; set; }
    }

    [Route("/universities/average/")]
    public class UniversitiesAverageReq
    {
    }

    public class UniversitiesAverageReqResponse
    {
        public List<University> Result { get; set; }
    }

    [Route("/universities/add/")]
    [Authenticate]
    public class AddUniversityReq
    {
        public string Name { get; set; }
        public int Easyness { get; set; }
        public int Job { get; set; }
        public int Financies { get; set; }
        public int Fun { get; set; }
        public int Prestige { get; set; }

        public University ToUniversity()
        {
            return new University { Name = Name, Easyness = Easyness, Financies = Financies, Fun = Fun, Job = Job, Prestige = Prestige };
        }
    }

    public class AddUniversityReqResponse
    {
        public string Result { get; set; }
    }

    [Route("/calculatedecision/")]
    public class CalculateDecisionReq
    {
        public string Preferences { get; set; }
    }

    public class CalculateDecisionReqResponse
    {
        public string Result { get; set; }
    }
}
