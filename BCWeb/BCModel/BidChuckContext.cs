using BCModel.Audit;
using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BCModel
{
    public class BidChuckContext : DbContext
    {
        private string _currentUser = "";
        private string _ipAddress = "";

        public BidChuckContext()
            : base("DefaultConnection")
        {

            var oc = this as IObjectContextAdapter;
            oc.ObjectContext.SavingChanges += new EventHandler(ObjectContext_SavingChanges);
        }

        public BidChuckContext(string currentUser, string ipAddress)
            : base("DefaultConnection")
        {
            _currentUser = currentUser;
            _ipAddress = ipAddress;
            var oc = this as IObjectContextAdapter;
            oc.ObjectContext.SavingChanges += new EventHandler(ObjectContext_SavingChanges);
        }

        public BidChuckContext(string currentUser, string ipAddress, string connection)
            : base(connection)
        {
            _currentUser = currentUser;
            _ipAddress = ipAddress;
            var oc = this as IObjectContextAdapter;
            oc.ObjectContext.SavingChanges += new EventHandler(ObjectContext_SavingChanges);
        }

        protected void ObjectContext_SavingChanges(object sender, EventArgs e)
        {

            var oc = this as IObjectContextAdapter;
            List<ObjectStateEntry> entries = oc.ObjectContext.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added | System.Data.EntityState.Deleted | System.Data.EntityState.Modified).ToList();
            foreach (ObjectStateEntry entry in entries)
            {
                if (!entry.IsRelationship && entry.Entity != null && !(entry.Entity is Audit.DBAudit))
                {
                    string userName = _currentUser;
                    Audit.DBAudit audit = this.AuditTrailFactory(entry, userName, _ipAddress);
                    this.DBAudits.Add(audit);
                }
            }

        }

        private Audit.DBAudit AuditTrailFactory(ObjectStateEntry entry, string userName, string ip)
        {
            var oc = this as IObjectContextAdapter; ;
            oc.ObjectContext.DetectChanges();

            Audit.DBAudit audit = new Audit.DBAudit();
            audit.TimeStamp = DateTime.Now;
            audit.Entity = entry.EntitySet.Name;
            audit.User = userName;
            audit.Address = ip;

            // set action type
            switch (entry.State)
            {
                case System.Data.EntityState.Added:
                    audit.ActionType = ActionTypes.I.ToString();
                    break;
                case System.Data.EntityState.Modified:
                    audit.ActionType = ActionTypes.U.ToString();
                    audit.Columns = getModifiedColumns(entry);
                    break;
                case System.Data.EntityState.Deleted:
                    audit.ActionType = ActionTypes.D.ToString();
                    break;
                default:
                    audit.ActionType = "F"; // fubar
                    break;
            }

            audit.NewValue = serializeEntity(entry);

            return audit;
        }

        private string getModifiedColumns(ObjectStateEntry entry)
        {
            List<string> props = entry.GetModifiedProperties().ToList();
            StringBuilder serialXML = new StringBuilder();
            XmlSerializer ser = new XmlSerializer(props.GetType());
            using (XmlWriter xWriter = XmlWriter.Create(serialXML))
            {
                ser.Serialize(xWriter, props);
                xWriter.Flush();
            }
            return serialXML.ToString(); ;
        }

        private string serializeEntity(ObjectStateEntry entry)
        {
            StringBuilder serialXML = new StringBuilder();
            DataContractSerializer dcSerializer = new DataContractSerializer(entry.Entity.GetType());


            using (XmlWriter xWriter = XmlWriter.Create(serialXML))
            {
                dcSerializer.WriteObject(xWriter, entry.Entity);
                xWriter.Flush();

            }
            return serialXML.ToString();
        }


        public enum ActionTypes
        {
            I, U, D
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserXScope> UserScopes { get; set; }

        public DbSet<Scope> Scopes { get; set; }

        public DbSet<DBAudit> DBAudits { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<County> Counties { get; set; }
        //public DbSet<BusinessType> BusinessTypes { get; set; }

        public DbSet<CompanyProfile> Companies { get; set; }
        public DbSet<CompanyXScope> CompanyScopes { get; set; }

        // projects
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDocument> ProjectDocs { get; set; }
        public DbSet<ProjectXScope> ProjectScopes { get; set; }
        public DbSet<BaseBid> BaseBids { get; set; }
        public DbSet<ComputedBid> ComputedBids { get; set; }

        public DbSet<BidPackage> BidPackages { get; set; }
        public DbSet<BidPackageXScope> BidPackageScopes { get; set; }

        //public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<ConstructionType> ConstructionTypes { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
