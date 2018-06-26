using Polly;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFrameworkTransactionScope.Data
{
    public class HotelFlightConfiguration : DbConfiguration
    {
        public Policy _policy;
        public HotelFlightConfiguration()
        {
            _policy = Policy.Handle<Exception>().CircuitBreaker(3, TimeSpan.FromSeconds(60));

            SetDatabaseInitializer(new DropCreateDatabaseIfModelChanges<HotelDBContext>());
            SetDatabaseInitializer(new DropCreateDatabaseIfModelChanges<FlightDBContext>());

            SetExecutionStrategy("System.Data.SqlClient", () => new CirtuitBreakerExecutionStrategy(_policy));
        }
    }

    public class CirtuitBreakerExecutionStrategy : IDbExecutionStrategy
    {
        private Policy _policy;

        public CirtuitBreakerExecutionStrategy(Policy policy)
        {
            _policy = policy;
        }

        public void Execute(Action operation)
        {

            _policy.Execute(() =>
            {
                operation.Invoke();
            });
        }

        public TResult Execute<TResult>(Func<TResult> operation)
        {
            return _policy.Execute(() =>
            {
                return operation.Invoke();
            });
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken)
        {
            await _policy.ExecuteAsync(() =>
            {
                return operation.Invoke();
            });
        }

        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken)
        {
            return await _policy.ExecuteAsync(() =>
            {
                return operation.Invoke();
            });
        }

        public bool RetriesOnFailure { get { return true; } }
    }
}
