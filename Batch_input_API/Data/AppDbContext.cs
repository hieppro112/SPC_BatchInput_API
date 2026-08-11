using Batch_input_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Batch_input_API.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //BatchInput history
        public DbSet<BatchInputHistory> BatchInputHistories { get; set; }
        //BatchInput List PO
        public DbSet<ListPO> ListPOs{get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Mapping Tên bảng
            modelBuilder.Entity<BatchInputHistory>()
           .ToTable("F2_BatchInput_History");


            //Mapping list PO
            modelBuilder.Entity<ListPO>()
            .ToTable("F2_BatchInput_ListPO");

            //qh 1-n
            modelBuilder.Entity<BatchInputHistory>()
                .HasMany(x => x.ListPOs)
                .WithOne(x => x.BatchInputHistory)
                .HasForeignKey(x => x.IDGroup)
                .OnDelete(DeleteBehavior.Cascade);//cha bị xdiróa thì cha cũng bị xóa 
        }
    }
}
