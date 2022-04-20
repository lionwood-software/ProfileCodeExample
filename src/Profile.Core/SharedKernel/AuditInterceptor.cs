using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Profile.Core.SharedKernel
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IProfileUserContext profileUserContext;

        public AuditInterceptor(IProfileUserContext profileUserContext) { this.profileUserContext = profileUserContext; }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ProcessAuditableEntries(eventData.Context.ChangeTracker.Entries());
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ProcessAuditableEntries(eventData.Context.ChangeTracker.Entries());
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ProcessAuditableEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries.Where(x => x.Entity is IAuditable))
            {
                var auditableEntity = (IAuditable)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            FillModifiedAuditData(auditableEntity);
                            break;
                        }
                    case EntityState.Added:
                        {
                            FillAddedAuditData(auditableEntity);
                            break;
                        }
                }
            }
        }

        private void FillModifiedAuditData(IAuditable entity)
        {
            entity.ModifiedBy = profileUserContext.Email;
            entity.ModifiedDate = DateTime.UtcNow;
        }

        private void FillAddedAuditData(IAuditable entity)
        {
            entity.CreatedBy = profileUserContext.Email;
            entity.CreatedDate = DateTime.UtcNow;
            FillModifiedAuditData(entity);
        }
    }
}
