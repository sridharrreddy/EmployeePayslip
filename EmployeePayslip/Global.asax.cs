using Services;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;
using EmployeePayslip.HttpHelpers;

namespace EmployeePayslip
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new Container();
            container.Register<IMemoryStreamHelper, MemoryStreamHelper>(Lifestyle.Singleton);
            container.Register<IPayslipService, PayslipService>(Lifestyle.Singleton);
            container.Register<ITaxCalculatorService, TaxCalculatorService>(Lifestyle.Singleton);
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}