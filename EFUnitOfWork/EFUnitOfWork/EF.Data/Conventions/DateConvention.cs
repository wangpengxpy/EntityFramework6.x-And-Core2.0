using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EF.Data.Conventions
{
    public class DateConvention : Convention
    {
        public DateConvention()
        {
            Properties<DateTime>()
                .Configure(c => c.HasColumnType("DATE"));
        }
    }
}
