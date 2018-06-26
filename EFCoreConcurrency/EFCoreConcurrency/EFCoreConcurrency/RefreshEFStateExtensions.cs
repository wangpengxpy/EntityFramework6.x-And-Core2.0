using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace EFCoreConcurrency
{
    public static class RefreshEFStateExtensions
    {
        public static EntityEntry Refresh(this EntityEntry tracking, 
            RefreshConflict refreshMode)
        {
            switch (refreshMode)
            {
                case RefreshConflict.StoreWins:
                    {
                        //当实体被删除时，重新加载设置追踪状态为Detached
                        //当实体被更新时，重新加载设置追踪状态为Unchanged
                        tracking.Reload(); 
                        break;
                    }
                case RefreshConflict.ClientWins:
                    {
                        PropertyValues databaseValues = tracking.GetDatabaseValues();
                        if (databaseValues == null)
                        {
                            //当实体被删除时，设置追踪状态为Detached，当然此时客户端无所谓获胜
                            tracking.State = EntityState.Detached;
                        }
                        else
                        {
                            //当实体被更新时，刷新数据库原始值
                            tracking.OriginalValues.SetValues(databaseValues);
                        }
                        break;
                    }
                case RefreshConflict.MergeClientAndStore:
                    {
                        PropertyValues databaseValues = tracking.GetDatabaseValues();
                        if (databaseValues == null)
                        {
                            /*当实体被删除时，设置追踪状态为Detached，当然此时客户端没有合并的数据
                             并设置追踪状态为Detached
                             */
                            tracking.State = EntityState.Detached;
                        }
                        else
                        {
                            //当实体被更新时，刷新数据库原始值
                            PropertyValues originalValues = tracking.OriginalValues.Clone();
                            tracking.OriginalValues.SetValues(databaseValues);
                            //如果数据库中对于属性有不同的值保留数据库中的值
#if SelfDefine
                databaseValues.PropertyNames // Navigation properties are not included.
                    .Where(property => !object.Equals(originalValues[property], databaseValues[property]))
                    .ForEach(property => tracking.Property(property).IsModified = false);
#else
                            databaseValues.Properties
                                    .Where(property => !object.Equals(originalValues[property.Name], 
                                    databaseValues[property.Name]))
                                    .ToList()
                                    .ForEach(property => 
                                    tracking.Property(property.Name).IsModified = false);
#endif
                        }
                        break;
                    }
            }
            return tracking;
        }
    }
}
