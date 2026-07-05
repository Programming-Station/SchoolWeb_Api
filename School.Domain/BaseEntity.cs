namespace School.Domain
{
    public class BaseEntity
    {
        public interface IDeleteEntity
        {
            bool IsDeleted { get; set; }
        }

        public interface IDeleteEntity<TKey> : IDeleteEntity
        {
        }

        public interface ITenantEntity
        {
            int SchoolRegistrationId { get; set; }
        }

        public interface IAuditEntity
        {
            DateTime? CreatedDate { get; set; }
            string? CreatedBy { get; set; }
            DateTime? UpdatedDate { get; set; }
            string? UpdatedBy { get; set; }
        }

        public interface IAuditEntity<TKey> : IAuditEntity, IDeleteEntity<TKey>
        {
        }

        public abstract class DeleteEntity<TKey> : IDeleteEntity<TKey>
        {
            public bool IsDeleted { get; set; } = false;
        }

        public abstract class AuditEntity<TKey> : DeleteEntity<TKey>, IAuditEntity<TKey>
        {
            public DateTime? CreatedDate { get; set; }
            public string? CreatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string? UpdatedBy { get; set; }
        }
    }
}

