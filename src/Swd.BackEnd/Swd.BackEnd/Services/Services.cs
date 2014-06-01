using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Extensions;
using ServiceStack.Html;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Cors;
using Swd.Backend;
using Swd.BackEnd.Dtos;
using Swd.BackEnd.Entities;

namespace Swd.BackEnd.Services
{
    [EnableCors(allowedMethods: "GET,POST")]
    public class UniversitiesService : Service
    {
        public object Get(UniversitiesReq request)
        {
            var universities = from u in new DbDriver().UniversitiesCollection.AsQueryable<University>() select u;
            return new UniversitiesReqResponse() { Result = universities.ToList<University>() };
        }

        public object Get(UniversitiesAverageReq request)
        {
            var query = from universities in new DbDriver().UniversitiesCollection.AsQueryable<University>()
                        select universities;

            var calculated = Algorithm.CalculateAverages(query.ToList().GroupBy(e => e.Name));
            return new UniversitiesAverageReqResponse() { Result = calculated };
        }

        public object Post(AddUniversityReq request)
        {

            new DbDriver().UniversitiesCollection.Insert(request.ToUniversity());

            return new AddUniversityReqResponse() { Result = "OK" };
        }

        public object Post(CalculateDecisionReq request)
        {
            var alg = new Algorithm(request.Preferences);
            return new CalculateDecisionReqResponse() { Result = alg.ResultUniversity.Name };

        }
    }
}
