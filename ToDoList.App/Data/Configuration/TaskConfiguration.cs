using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.App.Models;

namespace ToDoList.App.Data.Configuration
{
	public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
	{
		public void Configure(EntityTypeBuilder<TaskModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).HasColumnType("integer").ValueGeneratedOnAdd();
			builder.Property(x => x.Title).HasColumnType("varchar(20)").IsRequired();
			builder.Property(x => x.Description).HasColumnType("varchar(100)").IsRequired();
			builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
			builder.Property(x => x.CompletedAt).HasDefaultValue(null);
		}
	}
}
