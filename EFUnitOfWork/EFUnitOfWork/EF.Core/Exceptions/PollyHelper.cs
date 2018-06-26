using Polly;
using System;

namespace EF.Core.Exceptions
{
    public static class PollyHelper
    {
        public static void WaitAndRetry<T>(Action execution, int maxRetryAttempts = 3) where T : Exception
        {
            var pauseBetweenFailures = TimeSpan.FromSeconds(2);

            var retryPolicy =
                Policy.Handle<T>()
                .WaitAndRetry(maxRetryAttempts, i => pauseBetweenFailures, (ex, t) =>
                 {
                     execution();
                 });

            retryPolicy.Execute(() =>
                {
                    execution();
                });
        }
    }
}
