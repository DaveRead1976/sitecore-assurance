using System.Collections.Generic;
using Moq.Language.Flow;

namespace Revida.Sitecore.Assurance.Tests
{
    public static class MoqExtensions
    {
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
          params TResult[] results) where T : class
        {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }
    }
}
