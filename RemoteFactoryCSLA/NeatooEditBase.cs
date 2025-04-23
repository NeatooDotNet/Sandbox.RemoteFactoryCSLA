using Neatoo;
using Neatoo.RemoteFactory;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace RemoteFactoryCSLA
{
    public partial class NeatooEditBase : EditBase<NeatooEditBase>
    {
        public static uint TotalCount = 0;

        public NeatooEditBase(IEditBaseServices<NeatooEditBase> neatooEditBase) : base(neatooEditBase)
        {
            TotalCount++;
        }

        [Neatoo.RemoteFactory.Create]
        public void Create([Service] INeatooEditBaseFactory factory)
        {
            this.Id = 1;
            this.Description = Guid.NewGuid().ToString();
            this.ChildA = factory.Create(2);
            this.ChildB = factory.Create(2);
        }

        [Neatoo.RemoteFactory.Create]
        public void Create(int id, [Service] INeatooEditBaseFactory factory)
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
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        public NeatooEditBase? ChildA { get; set; }
        public NeatooEditBase? ChildB { get; set; }
    }

    internal class NeatooEditBaseAuth
    {
        private readonly IPrincipal principal;

        public NeatooEditBaseAuth(IPrincipal principal)
        {
            this.principal = principal;
        }

        [Authorize(AuthorizeOperation.Create)]
        public bool CanCreate()
        {
            return principal.IsInRole("Admin");
        }
    }
}
