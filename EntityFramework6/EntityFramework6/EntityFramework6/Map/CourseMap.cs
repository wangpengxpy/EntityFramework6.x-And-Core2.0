using EntityFramework6.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFramework6.Map
{
    public class CourseMap : EntityTypeConfiguration<Course>
    {
        public CourseMap()
        {
            //table
            ToTable("Courses");

            //key
            HasKey(k => k.Id);
        }
    }
}
