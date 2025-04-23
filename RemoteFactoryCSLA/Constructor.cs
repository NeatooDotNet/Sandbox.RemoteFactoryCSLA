using Neatoo;
using Neatoo.RemoteFactory;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace RemoteFactoryCSLA
{
    public partial class Constructor
    {
        public static uint TotalCount = 0;
        private readonly IPrincipal principal;

        public Constructor(IPrincipal principal) : base()
        {
            TotalCount++;
            this.principal = principal;
            if (principal.IsInRole("Admin"))
            {
                this.Id = 1;
                this.Description = Guid.NewGuid().ToString();
                this.ChildA = new Constructor(principal, 2);
                this.ChildB = new Constructor(principal, 2);
            }
        }

        public Constructor(IPrincipal principal, int id)
        {
            TotalCount++;
            if (principal.IsInRole("Admin"))
            {
                this.Id = id;
                this.Description = Guid.NewGuid().ToString();
                if (id < 15)
                {
                    this.ChildA = new Constructor(principal, id + 1);
                    this.ChildB = new Constructor(principal, id + 1);
                }
            }
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        public Constructor? ChildA { get; set; }
        public Constructor? ChildB { get; set; }
    }
}
