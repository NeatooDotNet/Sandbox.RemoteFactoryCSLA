using Csla;
using Neatoo.RemoteFactory;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace RemoteFactoryCSLA
{
    [Factory]
    [CslaImplementProperties]
    [Authorize<RemoteFactoryCSLABusinessBaseAuth>]
    public partial class RemoteFactoryCSLABusinessBase : Csla.BusinessBase<RemoteFactoryCSLABusinessBase>
    {
        public static uint TotalCount = 0;

        public RemoteFactoryCSLABusinessBase(ApplicationContext applicationContext)
        {
            TotalCount++;
            this.ApplicationContext = applicationContext;
        }

        [Neatoo.RemoteFactory.Create]
        internal void Create([Service] IRemoteFactoryCSLABusinessBaseFactory factory)
        {
            this.Id = 1;
            this.Description = Guid.NewGuid().ToString();
            this.ChildA = factory.Create(2);
            this.ChildB = factory.Create(2);
            MarkNew();
            MarkDirty();
        }

        [Neatoo.RemoteFactory.Create]
        internal void Create(int id, [Service] IRemoteFactoryCSLABusinessBaseFactory factory)
        {
            this.Id = id;
            this.Description = Guid.NewGuid().ToString();
            if (id < 15)
            {
                this.ChildA = factory.Create(id + 1);
                this.ChildB = factory.Create(id + 1);
            }
        }

        [Required]
        public partial int Id { get; set; }
        [Required]
        public partial string? Description { get; set; }
        public partial RemoteFactoryCSLABusinessBase? ChildA { get; set; }
        public partial RemoteFactoryCSLABusinessBase? ChildB { get; set; }
    }

    internal class RemoteFactoryCSLABusinessBaseAuth
    {
        private readonly ApplicationContext applicationContext;

        public RemoteFactoryCSLABusinessBaseAuth(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [Authorize(AuthorizeOperation.Create)]
        public bool CanCreate()
        {
            return applicationContext.User.IsInRole("Admin");
        }
    }
}
