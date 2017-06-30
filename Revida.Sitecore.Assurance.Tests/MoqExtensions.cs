using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq.Language.Flow;

namespace Revida.Sitecore.Assurance.Tests
{
    
    [ExcludeFromCodeCoverage]
    public static class MoqExtensions
    {
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
          params TResult[] results) where T : class
        {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }
    }
}
