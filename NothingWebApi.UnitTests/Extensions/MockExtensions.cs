using Microsoft.EntityFrameworkCore;
using NothingWebApi.UnitTests.DbContextMock;

namespace NothingWebApi.UnitTests.Extensions;

internal static class MockExtensions
{
    internal static Mock<DbSet<TEntity>> AsDbSetMock<TEntity>(this IEnumerable<TEntity> data)
        where TEntity : class
    {
        var queryable = new MockAsyncEnumerable<TEntity>(data);
        return queryable.AsDbSetMock();
    }

    private static Mock<DbSet<TEntity>> AsDbSetMock<TEntity>(this IQueryable<TEntity> data)
        where TEntity : class
    {
        var mock = new Mock<DbSet<TEntity>>();
        var enumerable = new MockAsyncEnumerable<TEntity>(data);
        mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
        mock.As<IQueryable<TEntity>>().ConfigureQueryableCalls(enumerable, data);
        mock.ConfigureDbSetCalls(data);
        mock.As<IAsyncEnumerable<TEntity>>()
            .Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(enumerable.GetAsyncEnumerator);

        return mock;
    }

    private static void ConfigureAsyncEnumerableCalls<TEntity>(
        this Mock<IAsyncEnumerable<TEntity>> mock,
        IAsyncEnumerable<TEntity> enumerable)
        where TEntity : class
    {
        mock.Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(enumerable.GetAsyncEnumerator);
    }

    private static void ConfigureQueryableCalls<TEntity>(
        this Mock<IQueryable<TEntity>> mock,
        IQueryProvider queryProvider,
        IQueryable<TEntity> data)
        where TEntity : class
    {
        mock.SetupGet(x => x.Provider).Returns(queryProvider);
        mock.SetupGet(x => x.Expression).Returns(data.Expression);
        mock.SetupGet(x => x.ElementType).Returns(data.ElementType);
        mock.Setup(x => x.GetEnumerator())
            .Returns(data.GetEnumerator);
    }

    private static void ConfigureDbSetCalls<TEntity>(
        this Mock<DbSet<TEntity>> mock,
        IQueryable<TEntity> data)
        where TEntity : class
    {
        mock.Setup(x => x.AsQueryable()).Returns(data);
        mock.Setup(x => x.AsAsyncEnumerable()).Returns(data.CreateAsyncMock);
    }

    private static async IAsyncEnumerable<TEntity> CreateAsyncMock<TEntity>(this IEnumerable<TEntity> data)
        where TEntity : class
    {
        foreach (var entity in data)
            yield return entity;

        await Task.CompletedTask;
    }
}