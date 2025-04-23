using Csla;
using Csla.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neatoo;
using Neatoo.RemoteFactory;
using RemoteFactoryCSLA;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var principal = CreateDefaultClaimsPrincipal();
var serviceCollection = new ServiceCollection();

serviceCollection.AddCsla(o => o.AddConsoleApp());
serviceCollection.AddTransient<CSLABusinessBase>();
serviceCollection.AddTransient<NoBase>();
serviceCollection.AddTransient<Func<int, NoBase>>(cc => {
    return (id) =>
    {
        var noBase = cc.GetRequiredService<NoBase>();
        noBase.Create(id);
        return noBase;
    };
});

serviceCollection.AddNeatooServices(NeatooFactory.Local, typeof(RemoteFactoryCSLABusinessBase).Assembly);
serviceCollection.AddTransient<RemoteFactoryCSLABusinessBase>();
serviceCollection.AddScoped<RemoteFactoryCSLABusinessBaseAuth>();
serviceCollection.AddTransient<NeatooEditBase>();
serviceCollection.AddScoped<NeatooEditBaseAuth>();
serviceCollection.AddSingleton<IPrincipal>(principal);

var serviceProvider = serviceCollection.BuildServiceProvider();

var stopwatch = new System.Diagnostics.Stopwatch();

var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
applicationContext.User = principal;

Debug.Assert(applicationContext.Principal.IsInRole("Admin"), "User is not in role Admin");

//var factory = serviceProvider.GetRequiredService<IDataPortalFactory>();
//var cslaBBFactory = factory.GetPortal<CSLABusinessBase>();

//stopwatch.Start();
//var cslaBB = cslaBBFactory.Create();
//stopwatch.Stop();

//Debug.Assert(cslaBB.IsValid);
//cslaBB.ChildA.ChildA.ChildA.Description = null;
//Debug.Assert(!cslaBB.IsValid);

//Console.WriteLine($"CSLA Total Count: {CSLABusinessBase.TotalCount}");
//Console.WriteLine($"CSLA Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

//var remoteFactory = serviceProvider.GetRequiredService<IRemoteFactoryCSLABusinessBaseFactory>();
//stopwatch.Reset();
//stopwatch.Start();
//var rfBB = remoteFactory.Create();
//stopwatch.Stop();

//// Small check to ensure CSLA is functional - at least a little
//Debug.Assert(rfBB.IsValid);
//rfBB.ChildA.ChildA.ChildA.Description = null;
//Debug.Assert(!rfBB.IsValid);

//Console.WriteLine($"RemoteFactory Total Count: {RemoteFactoryCSLABusinessBase.TotalCount}");
//Console.WriteLine($"RemoteFactory Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

//var neatooFactory = serviceProvider.GetRequiredService<INeatooEditBaseFactory>();
//stopwatch.Reset();
//stopwatch.Start();
//var neatooEdit = neatooFactory.Create();
//stopwatch.Stop();

//Debug.Assert(neatooEdit.IsValid);
//neatooEdit.ChildA.ChildA.ChildA.Description = null;
//Debug.Assert(!neatooEdit.IsValid);

//Console.WriteLine($"NeatooEditBase Total Count: {NeatooEditBase.TotalCount}");
//Console.WriteLine($"NeatooEditBase Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");


stopwatch.Reset();
stopwatch.Start();
var noBase = serviceProvider.GetRequiredService<NoBase>();
noBase.Create();
stopwatch.Stop();

Console.WriteLine($"noBase Total Count: {NoBase.TotalCount}");
Console.WriteLine($"noBase Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

Console.WriteLine($"Press any key to exit...");
Console.ReadLine();


static ClaimsPrincipal CreateDefaultClaimsPrincipal()
{
    var identity = new ClaimsIdentity(new GenericIdentity("Admin"));

    identity.AddClaim(new Claim("Id", Guid.NewGuid().ToString()));
    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

    return new ClaimsPrincipal(identity);
}