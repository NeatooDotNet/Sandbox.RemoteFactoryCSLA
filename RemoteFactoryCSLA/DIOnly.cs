using Neatoo;
using Neatoo.RemoteFactory;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace RemoteFactoryCSLA
{
    public interface IDIOnly
    {
        int Id { get; set; }
        string? Description { get; set; }
        IDIOnly? ChildA { get; set; }
        IDIOnly? ChildB { get; set; }
        void Create();
        void Create(int id);
    }

    public partial class DIOnly : IDIOnly
    {
        public static uint TotalCount = 0;
        private readonly IPrincipal principal;
        private readonly Func<int, IDIOnly> create;

        public DIOnly(IPrincipal principal, Func<int, IDIOnly> create) : base()
        {
            TotalCount++;
            this.principal = principal;
            this.create = create;
        }

        public void Create()
        {
            if (principal.IsInRole("Admin"))
            {
                this.Id = 1;
                this.Description = Guid.NewGuid().ToString();
                this.ChildA = create(2);
                this.ChildB = create(2);
            }
        }

        public void Create(int id)
        {
            if (principal.IsInRole("Admin"))
            {
                this.Id = id;
                this.Description = Guid.NewGuid().ToString();
                if (id < 15)
                {
                    this.ChildA = create(id + 1);
                    this.ChildB = create(id + 1);
                }
            }
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        public IDIOnly? ChildA { get; set; }
        public IDIOnly? ChildB { get; set; }
    }
}
