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
    public IRemoteFactoryCSLABusinessBaseFactory remoteFactory;
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

        serviceCollection.AddNeatooServices(NeatooFactory.Local, typeof(RemoteFactoryCSLABusinessBase).Assembly);
        serviceCollection.AddTransient<RemoteFactoryCSLABusinessBase>();
        serviceCollection.AddScoped<RemoteFactoryCSLABusinessBaseAuth>();
        serviceCollection.AddScoped<NeatooEditBaseAuth>();
        serviceCollection.AddSingleton<IPrincipal>(principal);

        serviceCollection.AddTransient<IDIOnly, DIOnly>();

        serviceProvider = serviceCollection.BuildServiceProvider();

        var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
        applicationContext.User = principal;

        var factory = serviceProvider.GetRequiredService<IDataPortalFactory>();
        cslaBBFactory = factory.GetPortal<CSLABusinessBase>();
        remoteFactory = serviceProvider.GetRequiredService<IRemoteFactoryCSLABusinessBaseFactory>();
        neatooFactory = serviceProvider.GetRequiredService<INeatooEditBaseFactory>();

    }

    [Benchmark]
    public uint CSLABusinessBase()
    {
        RemoteFactoryCSLA.CSLABusinessBase.TotalCount = 0;
        var cslaBB = cslaBBFactory.Create();
        return RemoteFactoryCSLA.CSLABusinessBase.TotalCount;
    }

    [Benchmark]
    public uint RemoteFactoryCSLABusinessBase()
    {
        RemoteFactoryCSLA.RemoteFactoryCSLABusinessBase.TotalCount = 0;
        var rfBB = remoteFactory.Create();
        return RemoteFactoryCSLA.RemoteFactoryCSLABusinessBase.TotalCount;
    }

    [Benchmark]
    public uint NeatooEditBase()
    {
        RemoteFactoryCSLA.NeatooEditBase.TotalCount = 0;
        var neatooEdit = neatooFactory.Create();
        return RemoteFactoryCSLA.NeatooEditBase.TotalCount;
    }

    [Benchmark]
    public uint DIOnly()
    {
        RemoteFactoryCSLA.DIOnly.TotalCount = 0;
        var noBase = serviceProvider.GetRequiredService<IDIOnly>();
        noBase.Create();
        return RemoteFactoryCSLA.DIOnly.TotalCount;
    }

    [Benchmark]
    public uint ConstructorOnly()
    {
        Constructor.TotalCount = 0;
        var constructor = new Constructor(principal);
        return Constructor.TotalCount;
    }

    [Benchmark]
    public uint ActivatorCreateInstance()
    {
        RemoteFactoryCSLA.ActivatorCreateInstance.TotalCount = 0;
        var activator = (ActivatorCreateInstance)Activator.CreateInstance(typeof(ActivatorCreateInstance), [principal])!;
        return RemoteFactoryCSLA.ActivatorCreateInstance.TotalCount;
    }

    static ClaimsPrincipal CreateDefaultClaimsPrincipal()
    {
        var identity = new ClaimsIdentity(new GenericIdentity("Admin"));

        identity.AddClaim(new Claim("Id", Guid.NewGuid().ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

        return new ClaimsPrincipal(identity);
    }
}