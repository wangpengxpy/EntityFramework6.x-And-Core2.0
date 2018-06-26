using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using EFCore.Model;

namespace EFCoreConcurrency
{
    public static class Concurrency
    {
        public static void NoCheck(
            DbQueryCommit readerWriter1, DbQueryCommit readerWriter2, DbQueryCommit readerWriter3)
        {
            int id = 1;
            Blog blog1 = readerWriter1.Query<Blog>(id);
            Blog blog2 = readerWriter2.Query<Blog>(id);

            readerWriter1.Commit(() => blog1.Name = nameof(readerWriter1));

            readerWriter2.Commit(() => blog2.Name = nameof(readerWriter2));

            Blog category3 = readerWriter3.Query<Blog>(id);
            Console.WriteLine(category3.Name);
        }

        public static void ConcurrencyCheck(DbQueryCommit readerWriter1, DbQueryCommit readerWriter2)
        {
            int id = 1;
            Blog blog1 = readerWriter1.Query<Blog>(id);
            Blog blog2 = readerWriter2.Query<Blog>(id);

            readerWriter1.Commit(() =>
            {
                blog1.Name = nameof(readerWriter1);
                blog1.Count = 2;
            });

            readerWriter2.Commit(() =>
            {
                blog2.Name = nameof(readerWriter2);
                blog2.Count = 2;
            });
        }

        public static void RowVersion(DbQueryCommit readerWriter1, DbQueryCommit readerWriter2)
        {
            int id = 1;
            Blog blog1 = readerWriter1.Query<Blog>(id);
            Console.WriteLine(blog1.RowVersionString);

            Blog blog2 = readerWriter2.Query<Blog>(id);
            Console.WriteLine(blog2.RowVersionString);

            readerWriter1.Commit(() => blog1.Name = nameof(readerWriter1));
            Console.WriteLine(blog1.RowVersionString);

            readerWriter2.Commit(() => readerWriter2.Set<Blog>().Remove(blog2));
        }

        public static void UpdateBlog(
    DbQueryCommit readerWriter1, DbQueryCommit readerWriter2,
    DbQueryCommit readerWriter3,
    Action<EntityEntry> resolveConflict)
        {
            int id = 1;
            Blog blog1 = readerWriter1.Query<Blog>(id);
            Blog blog2 = readerWriter2.Query<Blog>(id);
            Console.WriteLine($"查询行版本：{blog1.RowVersionString}");
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine($"查询行版本：{blog2.RowVersionString}");
            Console.WriteLine("----------------------------------------------------------");

            readerWriter1.Commit(() =>
            {
                blog1.Name = nameof(readerWriter1);
                blog1.Count = 3;
            });

            Console.WriteLine($"更新blog1后行版本：{blog1.RowVersionString}");
            Console.WriteLine("----------------------------------------------------------");

            readerWriter2.Commit(
            change: () =>
            {
                blog2.Name = nameof(readerWriter2);
                blog2.Count = 4;
                blog2.Url = "http://www.cnblogs.com/CreateMyself";
            },
            handleException: exception =>
            {
                EntityEntry tracking = exception.Entries.Single();
                Blog original = (Blog)tracking.OriginalValues.ToObject();
                Blog current = (Blog)tracking.CurrentValues.ToObject();
                Blog database = blog1;

                var origin = $"原始值:({original.Name},{original.Count},{original.Id},{original.RowVersionString})";
                Console.WriteLine(original);
                Console.WriteLine("----------------------------------------------------------");

                var databaseValue = $"数据库中值:({database.Name},{database.Count},{database.Id},{database.RowVersionString})";
                Console.WriteLine(databaseValue);
                Console.WriteLine("----------------------------------------------------------");

                var update = $"更新的值:({current.Name},{current.Count},{current.Id},{current.RowVersionString})";
                Console.WriteLine(update);
                Console.WriteLine("----------------------------------------------------------");

                resolveConflict(tracking);
            });

            Blog resolved = readerWriter3.Query<Blog>(id);

            var resolvedValue = $"查询并发解析后中的值:  ({resolved.Name}, {resolved.Count}, {resolved.Id},{resolved.RowVersionString})";
            Console.WriteLine(resolvedValue);
        }

        public static void DatabaseWins(
    DbQueryCommit readerWriter1, DbQueryCommit readerWriter2, DbQueryCommit readerWriter3) =>
        UpdateBlog(readerWriter1, readerWriter2, readerWriter3, resolveConflict: tracking =>
        {
            Console.WriteLine(tracking.State);
            Console.WriteLine(tracking.Property(nameof(Blog.Count)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Name)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Id)).IsModified);
            Console.WriteLine("----------------------------------------------------------");

            tracking.Reload();

            Console.WriteLine(tracking.State);
            Console.WriteLine(tracking.Property(nameof(Blog.Count)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Name)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Id)).IsModified);
        });


        public static void ClientWins(
    DbQueryCommit readerWriter1, DbQueryCommit readerWriter2, DbQueryCommit readerWriter3) =>
        UpdateBlog(readerWriter1, readerWriter2, readerWriter3, resolveConflict: tracking =>
        {
            PropertyValues databaseValues = tracking.GetDatabaseValues();
            tracking.OriginalValues.SetValues(databaseValues);

            Console.WriteLine(tracking.State);
            Console.WriteLine(tracking.Property(nameof(Blog.Count)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Name)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Id)).IsModified);
        });

        public static void MergeClientAndDatabase(
    DbQueryCommit readerWriter1, DbQueryCommit readerWriter2, DbQueryCommit readerWriter3) =>
        UpdateBlog(readerWriter1, readerWriter2, readerWriter3, resolveConflict: tracking =>
        {
            PropertyValues originalValues = tracking.OriginalValues.Clone();
            PropertyValues databaseValues = tracking.GetDatabaseValues();

            tracking.OriginalValues.SetValues(databaseValues);

#if selfDefine
            databaseValues.PropertyNames
                    .Where(property => !object.Equals(originalValues[property], databaseValues[property]))
                    .ForEach(property => tracking.Property(property).IsModified = false);
#else
            databaseValues.Properties
                .Where(property => !object.Equals(originalValues[property.Name], databaseValues[property.Name]))
                .ToList()
                .ForEach(property => tracking.Property(property.Name).IsModified = false);
#endif

            Console.WriteLine(tracking.State);
            Console.WriteLine(tracking.Property(nameof(Blog.Count)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Name)).IsModified);
            Console.WriteLine(tracking.Property(nameof(Blog.Id)).IsModified);
        });
    }
}
