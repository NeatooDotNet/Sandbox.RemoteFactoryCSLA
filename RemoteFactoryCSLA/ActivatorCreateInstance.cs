using Neatoo;
using Neatoo.RemoteFactory;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace RemoteFactoryCSLA
{
    public partial class ActivatorCreateInstance
    {
        public static uint TotalCount = 0;
        private readonly IPrincipal principal;
        private static Type ActivateCreateInstanceType = typeof(ActivatorCreateInstance);

        public ActivatorCreateInstance(IPrincipal principal) : base()
        {
            TotalCount = 1;
            this.principal = principal;
            if (principal.IsInRole("Admin"))
            {
                this.Id = 1;
                this.Description = Guid.NewGuid().ToString();
                this.ChildA = (ActivatorCreateInstance)Activator.CreateInstance(ActivateCreateInstanceType, [principal, 2])!;
                this.ChildB = (ActivatorCreateInstance)Activator.CreateInstance(ActivateCreateInstanceType, [principal, 2])!;
            }
        }

        public ActivatorCreateInstance(IPrincipal principal, int id)
        {
            TotalCount++;
            if (principal.IsInRole("Admin"))
            {
                this.Id = id;
                this.Description = Guid.NewGuid().ToString();
                if (id < 15)
                {
                    this.ChildA = (ActivatorCreateInstance)Activator.CreateInstance(ActivateCreateInstanceType, [principal, id + 1])!;
                    this.ChildB = (ActivatorCreateInstance)Activator.CreateInstance(ActivateCreateInstanceType, [principal, id + 1])!;
                }
            }
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        public ActivatorCreateInstance? ChildA { get; set; }
        public ActivatorCreateInstance? ChildB { get; set; }
    }
}
