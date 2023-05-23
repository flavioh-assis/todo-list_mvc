using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Models;

namespace ToDoList.Data.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasColumnType("varchar(20)");
            builder.Property(x => x.Description).HasColumnType("varchar(150)");
            builder.Property(x => x.Completed).HasDefaultValue(false);
            builder.Property(x => x.CreationDate).HasDefaultValue(DateTime.Now);
        }
    }
}
