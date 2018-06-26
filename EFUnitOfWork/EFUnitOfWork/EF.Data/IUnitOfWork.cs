namespace EF.Data
{
    public interface IUnitOfWork
    {
        void Commit();
        void RollBack();
    }
}
