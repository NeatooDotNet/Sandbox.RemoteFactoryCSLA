using BenchmarkDotNet.Attributes;
using Csla;
using Csla.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neatoo;
using Neatoo.RemoteFactory;
using RemoteFactoryCSLA;
using System.Security.Claims;
using System.Security.Principal;

namespace RemoteFactoryCSLA;

public class BusinessObjectBenchmarks
{
    public ServiceProvider serviceProvider;
    public ClaimsPrincipal principal;
    public IDataPortal<CSLABusinessBase> cslaBBFactory;
    public INeatooEditBaseFactory neatooFactory;

    public BusinessObjectBenchmarks()
    {
        principal = CreateDefaultClaimsPrincipal();
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddCsla(o => o.AddConsoleApp());
        //serviceCollection.AddTransient<ICSLABusinessBase, CSLABusinessBase>();
        serviceCollection.AddTransient<DIOnly>();
        serviceCollection.AddTransient<Func<int, IDIOnly>>(cc => {
            return (id) =>
            {
                var noBase = cc.GetRequiredService<IDIOnly>();
                noBase.Create(id);
                return noBase;
            };
        });

        serviceCollection.AddNeatooServices(NeatooFactory.Local, typeof(NeatooEditBase).Assembly);
        serviceCollection.AddScoped<NeatooEditBaseAuth>();
        serviceCollection.AddSingleton<IPrincipal>(principal);

        serviceCollection.AddTransient<IDIOnly, DIOnly>();

        serviceProvider = serviceCollection.BuildServiceProvider();

        var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
        applicationContext.User = principal;

        var factory = serviceProvider.GetRequiredService<IDataPortalFactory>();
        cslaBBFactory = factory.GetPortal<CSLABusinessBase>();
        neatooFactory = serviceProvider.GetRequiredService<INeatooEditBaseFactory>();
    }

    [Benchmark]
    public CSLABusinessBase CSLABusinessBase()
    {
        RemoteFactoryCSLA.CSLABusinessBase.TotalCount = 0;
        var cslaBB = cslaBBFactory.Create();
        return cslaBB;
    }

    [Benchmark]
    public INeatooEditBase NeatooEditBase()
    {
        RemoteFactoryCSLA.NeatooEditBase.TotalCount = 0;
        var neatooEdit = neatooFactory.Create();
        return neatooEdit;
    }

    [Benchmark]
    public IDIOnly DIOnly()
    {
        RemoteFactoryCSLA.DIOnly.TotalCount = 0;
        var noBase = serviceProvider.GetRequiredService<IDIOnly>();
        noBase.Create();
        return noBase;
    }

    [Benchmark]
    public Constructor ConstructorOnly()
    {
        Constructor.TotalCount = 0;
        var constructor = new Constructor(principal);
        return constructor;
    }

    [Benchmark]
    public ActivatorCreateInstance ActivatorCreateInstance()
    {
        RemoteFactoryCSLA.ActivatorCreateInstance.TotalCount = 0;
        var activator = (ActivatorCreateInstance)Activator.CreateInstance(typeof(ActivatorCreateInstance), [principal])!;
        return activator;
    }

    static ClaimsPrincipal CreateDefaultClaimsPrincipal()
    {
        var identity = new ClaimsIdentity(new GenericIdentity("Admin"));

        identity.AddClaim(new Claim("Id", Guid.NewGuid().ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

        return new ClaimsPrincipal(identity);
    }
}