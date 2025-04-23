using Csla;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RemoteFactoryCSLA
{

    // I can't get IDataPortal<ICSLABusinessBase> to work
    public interface ICSLABusinessBase : Csla.IBusinessBase
    {
        int Id { get; }
        string? Description { get; set; }
        ICSLABusinessBase? ChildA { get; }
        ICSLABusinessBase? ChildB { get; }
    }

    [Serializable]
    [CslaImplementProperties]
    public partial class CSLABusinessBase : Csla.BusinessBase<CSLABusinessBase>, ICSLABusinessBase
    {
        public static uint TotalCount = 0;

        [DynamicDependency(DynamicallyAccessedMemberTypes.NonPublicMethods, typeof(ICSLABusinessBase))]
        public CSLABusinessBase()
        {
            TotalCount++;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [ObjectAuthorizationRules]
        public static void AddObjectAuthorizationRules()
        {
            Csla.Rules.BusinessRules.AddRule(typeof(ICSLABusinessBase),
              new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject, "Admin"));
        }

        // IDataPortal<ICSLABusinessBase> throws an exception

        [Create]

        private void Create([Inject] IDataPortal<CSLABusinessBase> factory)
        {
            this.Id = 1;
            this.Description = Guid.NewGuid().ToString();
            this.ChildA = factory.Create(2);
            this.ChildB = factory.Create(2);
        }

        [Create]
        private void Create(int id, [Inject] IDataPortal<CSLABusinessBase> factory)
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
        public partial ICSLABusinessBase? ChildA { get; set; }
        public partial ICSLABusinessBase? ChildB { get; set; }
    }
}
