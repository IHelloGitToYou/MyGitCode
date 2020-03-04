using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gao.Models.Users;
using Gao.Services.Users;
using GaoCore;
using GaoCore.Extjs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ground
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(option => option.AddPolicy("cors",
                                                          policy => policy.AllowAnyHeader()
                                                          .AllowAnyMethod()
                                                          .AllowCredentials()
                                                          .AllowAnyOrigin()));

            services.AddSession(config =>
            {
                config.IdleTimeout = TimeSpan.FromSeconds(10);
                config.Cookie.HttpOnly = true;
               // config.Cookie.Expiration = TimeSpan.FromSeconds(10);
            });

            services.AddMvc();//.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Json格式化, 保持属性名大小写一致
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(ErrorFilterAttribute));
                    options.Filters.Add(typeof(GlobalFilterAttribute));
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddJsonOptions(o =>
                    {
                        o.SerializerSettings.ContractResolver
                        = new Newtonsoft.Json.Serialization.DefaultContractResolver();

                        o.SerializerSettings.NullValueHandling
                            = Newtonsoft.Json.NullValueHandling.Ignore;

                    });

            LoadDLL();
            LoadSetting();
            ExtjsBuilder.Run();
        }

        // This method gets called by the runtime. 
        //  Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession(new SessionOptions() {  });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //一定要在UseMvc之前
            app.UseCors("cors");
            app.UseMvc();
            
        }


        public void LoadDLL()
        {
            RunningControllers.Register((new Gao.Main()).GetType().Assembly);
            RunningControllers.Register((new GAOSelectBom.Main()).GetType().Assembly);
        }

        public void LoadSetting()
        {
            var jObject = new JsonConfigHelper("GaoSettings.json").jObject;
            UserService.ErpType = jObject.SelectToken("ERP系统").ToString() == "T8" ? ErpTypes.T8 : ErpTypes.SUNLIKE;
            UserService.DB_ConnectionStringFormatOnDB = jObject.SelectToken("帐套数据库连接格式").ToString();   //为数据库连接字符串赋值


            //后门
            SqlSugarBase.Register("DEMO", UserService.DB_ConnectionStringFormatOnDB.FormatOrg("DEMO"));

        }
    }
}
