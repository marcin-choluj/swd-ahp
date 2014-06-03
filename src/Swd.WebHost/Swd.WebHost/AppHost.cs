using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Authentication.MongoDb;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using Swd.WebHost.Services;

namespace Swd.WebHost
{
    public class AppHost : AppHostBase
    {
        //Tell Service Stack the name of your application and where to find your web services
        public AppHost()
            : base("AppHost", typeof(UniversitiesService).Assembly)
        {
        }

        public override void Configure(Funq.Container container)
        {
            SetConfig(new HostConfig
                {
                    DefaultRedirectPath = "/app/index.html"
                });


            container.Register(new DbDriver());

            var appSettings = new AppSettings();

            Plugins.Add(new AuthFeature(
                            () => new AuthUserSession(), //Use your own typed Custom UserSession type
                            new IAuthProvider[]
                                {
                                    new FacebookAuthProvider(appSettings), //Sign-in with Facebook
                                    new TwitterAuthProvider(appSettings), 
                                }));
            container.Register<IUserAuthRepository>(c =>
               new MongoDbAuthRepository(c.Resolve<DbDriver>().Database,true)); //Use OrmLite DB Connection to persist the UserAuth and AuthProvider info
            //var authRepo = (MongoDBAuthRepository)container.Resolve<IUserAuthRepository>(); //If using and RDBMS to persist UserAuth, we must create required tables
            //authRepo.DropAndReCreateCollections();
            
            //register any dependencies your services use, e.g:
            container.Register<ICacheClient>(new MemoryCacheClient());
        }
    }
}