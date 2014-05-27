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
    [EnableCors(allowedMethods: "GET,POST")]
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
                calculated = Algorithm.CalculateAverages(ctx.Universities.GroupBy(e => e.Name));
            }
            return new UniversitiesAverageReqResponse() { Result = calculated };
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

        public object Post(CalculateDecisionReq request)
        {
            var alg = new Algorithm(request.Preferences);
            return new CalculateDecisionReqResponse() { Result = alg.ResultUniversity.Name };

        }
    }
}
