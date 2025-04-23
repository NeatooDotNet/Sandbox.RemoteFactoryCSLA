using Csla;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RemoteFactoryCSLA
{
    [Serializable]
    [CslaImplementProperties]
    internal partial class CSLABusinessBase : Csla.BusinessBase<CSLABusinessBase>
    {
        public static uint TotalCount = 0;

        [DynamicDependency(DynamicallyAccessedMemberTypes.NonPublicMethods, typeof(CSLABusinessBase))]
        public CSLABusinessBase()
        {
            TotalCount++;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [ObjectAuthorizationRules]
        public static void AddObjectAuthorizationRules()
        {
            Csla.Rules.BusinessRules.AddRule(typeof(CSLABusinessBase),
              new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject, "Admin"));
        }

        [Create]

        internal void Create([Inject] IDataPortal<CSLABusinessBase> factory)
        {
            this.Id = 1;
            this.Description = Guid.NewGuid().ToString();
            this.ChildA = factory.Create(2);
            this.ChildB = factory.Create(2);
        }

        [Create]
        internal void Create(int id, [Inject] IDataPortal<CSLABusinessBase> factory)
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
        public partial CSLABusinessBase? ChildA { get; set; }
        public partial CSLABusinessBase? ChildB { get; set; }
    }
}
