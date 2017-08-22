using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectQuestionManagement.Startup))]
namespace ProjectQuestionManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
