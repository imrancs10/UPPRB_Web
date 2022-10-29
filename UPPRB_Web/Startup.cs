using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;

[assembly: OwinStartupAttribute(typeof(UPPRB_Web.Startup))]
namespace UPPRB_Web
{
    public partial class Startup
    {
        public void Configuration(AppBuilder app)
        {
        }
    }
}
