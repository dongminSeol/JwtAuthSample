using JwtAuthenticationSample.JwtHelpers;
using JwtAuthenticationSample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JwtAuthenticationSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
         
        public IConfiguration Configuration { get; }

        // 이 방법은 런타임에 의해 호출된다. 컨테이너에 서비스를 추가하려면 이 방법을 사용.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // App 설정 값
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // 테스트 용 사용자 정보 .
            services.AddScoped<IUserData, UserData>();
        }

        // 이 방법은 런타임에 의해 호출된다. 이 방법을 사용하여 HTTP 요청 파이프라인을 구성.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //SSL 주석
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //파이프라인 Jwt 미들웨어 Class 추가.
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
