using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace NothingKafka.UnitTests.DbContextMock;

public class MockAsyncEnumerable<TEntity>
    : MockQueryProvider<TEntity>, IAsyncEnumerable<TEntity>, IAsyncQueryProvider
{
    public MockAsyncEnumerable(Expression expression) : base(expression)
    {
    }

    public MockAsyncEnumerable(IEnumerable<TEntity> enumerable) : base(enumerable)
    {
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        var expectedResultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
            .GetMethods()
            .First(method => method is {Name: nameof(IQueryProvider.Execute), IsGenericMethod: true})
            .MakeGenericMethod(expectedResultType)
            .Invoke(this, [expression]);

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
            ?.MakeGenericMethod(expectedResultType)
            .Invoke(null, [executionResult])!;
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new MockAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
    }
}