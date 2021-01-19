using chat_server.Entity.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using chat_server.Entity;
using chat_server.DAL;

namespace chat_server
{
    public static class ExtractServices
    {
        public static SessionIndexer AddIndexer(this ISession session)
        {
            return new SessionIndexer(session);
        }
        public static void ExtractChatServices(IServiceCollection services)
        {
            services.AddScoped<IGenericService<UserVM>, DA_User>();
            services.AddScoped<IGenericService<MessageVM>, DA_Chat>();
            services.AddScoped<IGenericService<tblLogedinStatus>, DA_LogInStatus>();
            services.AddScoped<IAuth<UserVM>, DA_User>();
        }
    }
    public class SessionIndexer
    {
        private ISession Session;
        public SessionIndexer(ISession Session)
        {
            this.Session = Session;
        }
        public object this[string key]
        {
            set
            {
                Session.SetString(key, "");
            }
            get
            {
                return Session.GetString(key);
            }
        }
    }
}
