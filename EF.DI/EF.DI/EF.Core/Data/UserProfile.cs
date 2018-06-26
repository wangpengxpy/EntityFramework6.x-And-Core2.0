namespace EF.Core.Data
{
    public class UserProfile : BaseEntity
    {
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Address { get; set; }      
      public virtual User User { get; set; }
    }
}