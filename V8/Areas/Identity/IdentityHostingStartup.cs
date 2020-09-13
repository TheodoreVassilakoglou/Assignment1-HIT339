using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using V8.Models;
using V8.Data;

[assembly: HostingStartup(typeof(V8.Areas.Identity.IdentityHostingStartup))]
namespace V8.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<V8Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("V8ContextConnection")));

                services.AddDefaultIdentity<V8User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<V8Context>();
            });
        }
    }
}