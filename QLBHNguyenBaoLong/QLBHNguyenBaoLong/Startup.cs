using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLBHNguyenBaoLong.Startup))]
namespace QLBHNguyenBaoLong
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
