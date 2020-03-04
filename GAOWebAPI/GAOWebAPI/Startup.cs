using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GAOWebAPI.Models;
using GAOWebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GAOWebAPI
{
    public class Startup
    {
        readonly private IConfiguration _Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this._Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(option => option.AddPolicy("cors", 
                                                            policy => policy.AllowAnyHeader()
                                                            .AllowAnyMethod()
                                                            .AllowCredentials()
                                                            .AllowAnyOrigin()) );

            services.AddSession(config =>
            {
                config.IdleTimeout = TimeSpan.FromSeconds(10);
                config.Cookie.HttpOnly = true;
                config.Cookie.IsEssential = true;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ErrorFilterAttribute));
                options.Filters.Add(typeof(GlobalFilterAttribute));
                options.RespectBrowserAcceptHeader = true;
            });//.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvc();
            

            var jObject = new JsonConfigHelper("BomSetting.json").jObject;
            
            SqlSugarBase.DB_ConnectionString = jObject.SelectToken("主数据库连接").ToString();   //为数据库连接字符串赋值
            SqlSugarBase.DB_ConnectionStringFormatOnDB = jObject.SelectToken("帐套数据库连接格式").ToString();   //为数据库连接字符串赋值
            BomService.BomConflictHandleWay = jObject.SelectToken("货号存在多BOM").ToString() == "取最新" 
                                                    ? EnumBomConflictWay.GetLastest : EnumBomConflictWay.Error;
            //"ERP系统序列号:": "GAD12012",
            //"层级类型": "带点号", //或 纯数字
            //"ERP系统": "T8", //或 "SUNLIKE"
            //LoginService.SYS_NUMBER = jObject.SelectToken("ERP系统序列号").ToString();
            BomService.BomLevelWay = jObject.SelectToken("层级类型").ToString() == "带点号" ? BomLevelWay.Split : BomLevelWay.Number;
            LoginService.SYS_MODEL = jObject.SelectToken("ERP系统").ToString() == "T8" ? SysModel.T8 : SysModel.SUNLIKE;


            BomService.DefaultBomHeaderSection = new Dictionary<string, string>();
            BomService.DefaultBomBodySection = new Dictionary<string, string>();
            BomService.DefaultPrdtSection = new Dictionary<string, string>();

            
            var jToken = jObject.SelectToken("T8默认值");
            if (jToken != null)
            {
                var jTokenBomHeader = jToken.SelectToken("BOM表头");
                if (jTokenBomHeader != null)
                {
                    foreach (JProperty item in jTokenBomHeader.Children())
                    {
                        BomService.DefaultBomHeaderSection.Add(item.Name, item.Value.ToString());
                    }
                }

                var jTokenBomBody = jToken.SelectToken("BOM表身");
                if (jTokenBomBody != null)
                {
                    foreach (JProperty item in jTokenBomBody.Children())
                    {
                        BomService.DefaultBomBodySection.Add(item.Name, item.Value.ToString());
                    }
                }

                var jTokenPrdt = jToken.SelectToken("货号资料");
                if (jTokenPrdt != null)
                {
                    foreach (JProperty item in jTokenPrdt.Children())
                    {
                        BomService.DefaultPrdtSection.Add(item.Name, item.Value.ToString());
                    }
                }
            }

            //// //层级类型
            //// //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //一定要在UseMvc之前
            app.UseCors("cors");
            app.UseSession();
            app.UseMvc();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}"///{id?}
                   // defaults: new { id = RouteParameter.Optional })
                );
            });

        }


    }
}
