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

        // �� ����� ��Ÿ�ӿ� ���� ȣ��ȴ�. �����̳ʿ� ���񽺸� �߰��Ϸ��� �� ����� ���.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // App ���� ��
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // �׽�Ʈ �� ����� ���� .
            services.AddScoped<IUserData, UserData>();
        }

        // �� ����� ��Ÿ�ӿ� ���� ȣ��ȴ�. �� ����� ����Ͽ� HTTP ��û ������������ ����.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //SSL �ּ�
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //���������� Jwt �̵���� Class �߰�.
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
