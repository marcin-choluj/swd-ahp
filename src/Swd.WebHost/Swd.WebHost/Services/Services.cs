using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using ServiceStack;
using Swd.WebHost.Dtos;
using Swd.WebHost.Entities;

namespace Swd.WebHost.Services
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
            var tmp = request.ToUniversity();
            tmp.AddedBy = SessionAs<AuthUserSession>().Id;
            new DbDriver().UniversitiesCollection.Insert(tmp);

            return new AddUniversityReqResponse() { Result = "OK" };
        }

        public object Post(CalculateDecisionReq request)
        {
            var alg = new Algorithm(request.Preferences);
            return new CalculateDecisionReqResponse() { Result = alg.ResultUniversity.Name };

        }

        public object Get(MyUniversitiesReq request)
        {
            var session = SessionAs<AuthUserSession>();
            var universities = from u in new DbDriver().UniversitiesCollection.AsQueryable<University>() where u.AddedBy==session.UserAuthId select u;
            return new UniversitiesReqResponse() { Result = universities.ToList<University>() };
        }

        public object Get(DeleteMyUniversitiesReq request)
        {
            new DbDriver().UniversitiesCollection.Remove(Query.EQ("_id", new ObjectId(request.Id)));
            return new AddUniversityReqResponse() { Result = "OK" };
        }
    }

    [Route("/universities/myratings")]
    [Authenticate]
    public class MyUniversitiesReq
    {
    }

    [Route("/universities/myratings/delete/{id}")]
    [Authenticate]
    public class DeleteMyUniversitiesReq
    {
        public string Id { get; set; }
    }

    [Route("/authinfo")]
    [Authenticate]
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string TwitterUserId { get; set; }
        public string TwitterScreenName { get; set; }
        public string TwitterName { get; set; }
        public string FacebookName { get; set; }
        public string FacebookFirstName { get; set; }
        public string FacebookLastName { get; set; }
        public string FacebookUserId { get; set; }
        public string FacebookUserName { get; set; }
        public string FacebookEmail { get; set; }
        public string GravatarImageUrl64 { get; set; }
    }

    public class UserProfileDtoResponse
    {
        public UserProfileDto Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class UserProfileService:Service
    {
        public object Get(UserProfileDto request)
        {
            var session = SessionAs<AuthUserSession>();

            var userProfile = new UserProfileDto();
            userProfile.DisplayName = session.DisplayName;
            //userProfile.Id = int.Parse(session.UserAuthId);

            //var user = base.TryResolve<DbDriver>().UsersCollection.FindOneAs<User>(Query.EQ("_id", userProfile.Id));
            //userProfile.PopulateWith(user);
            return new UserProfileDtoResponse
            {
                Result = userProfile
            };
        }
    }
}
