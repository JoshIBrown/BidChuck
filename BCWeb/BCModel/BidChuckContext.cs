using BCModel.Audit;
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

        public BidChuckContext()
            : base("DefaultConnection")
        {
            
            var oc = this as IObjectContextAdapter;
            oc.ObjectContext.SavingChanges += new EventHandler(ObjectContext_SavingChanges);
        }

        public BidChuckContext(string currentUser)
            : base("DefaultConnection")
        {
            _currentUser = currentUser;
            var oc = this as IObjectContextAdapter;
            oc.ObjectContext.SavingChanges += new EventHandler(ObjectContext_SavingChanges);
        }

        public BidChuckContext(string currentUser,string connection)
            : base(connection)
        {
            _currentUser = currentUser;
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
                    Audit.DBAudit audit = this.AuditTrailFactory(entry, userName);
                    this.DBAudits.Add(audit);
                }
            }

        }

        private Audit.DBAudit AuditTrailFactory(ObjectStateEntry entry, string userName)
        {
            var oc = this as IObjectContextAdapter; ;
            oc.ObjectContext.DetectChanges();

            Audit.DBAudit audit = new Audit.DBAudit();
            audit.TimeStamp = DateTime.Now;
            audit.Entity = entry.EntitySet.Name;
            audit.User = userName;

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
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<DBAudit> DBAudits { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<County> Counties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
