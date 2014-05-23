using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Html;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Cors;
using Swd.BackEnd.Dtos;

namespace Swd.BackEnd.Services
{
    [EnableCors(allowedMethods: "GET")]
    public class UniversitiesService : Service
    {
        public object Get(UniversitiesReq request)
        {
            List<University> universities;
            using (var ctx = new DatabaseEntities())
            {
                universities = ctx.Universities.ToList();
            }
            return new UniversitiesReqResponse() { Result = universities };
        }

        public object Get(UniversitiesAverageReq request)
        {
            List<University> calculated;
            using (var ctx = new DatabaseEntities())
            {
                calculated = CalculateAverages(ctx.Universities.GroupBy(e => e.Name));
            }
            return new UniversitiesAverageReqResponse() { Result = calculated };
        }

        private List<University> CalculateAverages(IEnumerable<IGrouping<string, University>> data)
        {
            var ret = new List<University>();
            foreach (var group in data)
            {
                var calculatedUniversity = new University();
                foreach (var university in group)
                {
                    calculatedUniversity.Easyness += university.Easyness;
                    calculatedUniversity.Job += university.Job;
                    calculatedUniversity.Financies += university.Financies;
                    calculatedUniversity.Fun += university.Fun;
                    calculatedUniversity.Prestige += university.Prestige;
                }
                calculatedUniversity.Easyness /= group.Count();
                calculatedUniversity.Job /= group.Count();
                calculatedUniversity.Financies /= group.Count();
                calculatedUniversity.Fun /= group.Count();
                calculatedUniversity.Prestige /= group.Count();
                ret.Add(calculatedUniversity);
            }
            return ret;
        }

        public object Post(AddUniversityReq request)
        {
            using (var ctx = new DatabaseEntities())
            {
                ctx.Universities.Add(request.ToUniversity());
                ctx.SaveChanges();
            }
            return new AddUniversityReqResponse() { Result = "OK" };
        }
    }
}
