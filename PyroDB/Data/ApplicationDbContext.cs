using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PyroDB.Models.Database;
using PyroDB.Services;
using System.Runtime.CompilerServices;

namespace PyroDB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Chemical> Chemicals { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<ChangeTrackerItem> Changes { get; set; }







        //public override int SaveChanges()
        //{
        //    SaveChangesHandler().RunSynchronously();
        //    return base.SaveChanges();
        //}
        //
        //public override int SaveChanges(bool acceptAllChangesOnSuccess)
        //{
        //    SaveChangesHandler().RunSynchronously();
        //    return base.SaveChanges(acceptAllChangesOnSuccess);
        //}
        //
        //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    await SaveChangesHandler();
        //    return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}
        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    await SaveChangesHandler();
        //    return await base.SaveChangesAsync(cancellationToken);
        //}
        //
        //
        //private async Task SaveChangesHandler()
        //{
        //    var v = _userService.GetCurrentUser();
        //
        //    foreach (var e in this.ChangeTracker.Entries<ITrackChanges>())
        //    {
        //        if(e.State == EntityState.Added)
        //        {
        //            e.Entity.Changes.Add(new ChangeTrackerItem(DataSources.Unknown, ChangeTrackerTypes.Added));
        //        }
        //        if(e.State == EntityState.Modified)
        //        {
        //            e.Entity.Changes.Add(new ChangeTrackerItem(DataSources.Unknown, ChangeTrackerTypes.Modified));
        //        }
        //    }
        //}

    }
}
