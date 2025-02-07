using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NothingKafka.UnitTests.Extensions;
using NothingServices.Abstractions.Extensions;

namespace NothingKafka.UnitTests.DbContextMock;

internal class MockDbContextBuilder<TDbContext> where TDbContext : DbContext
{
    private readonly Mock<TDbContext> _dbContextMock;

    internal MockDbContextBuilder()
    {
        _dbContextMock = new Mock<TDbContext>(new DbContextOptions<TDbContext>());
        var databaseMock = new Mock<DatabaseFacade>(_dbContextMock.Object);
        databaseMock.SetupGet(x => x.ProviderName).Returns(string.Empty);
        _dbContextMock.SetupGet(x => x.Database).Returns(databaseMock.Object);
    }

    internal Mock<TDbContext> Build() => _dbContextMock;

    public void AddDbSet<TEntity>(
        Expression<Func<TDbContext, DbSet<TEntity>>> expression,
        List<TEntity> entities,
        IEqualityComparer<TEntity>? comparer = null)
        where TEntity : class
    {
        var dbSetMock = entities.AsDbSetMock();
        dbSetMock.Setup(x=> x.Add(It.IsAny<TEntity>()))
            .Callback(entities.Add);
        dbSetMock.Setup(x=> x.AddAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
            .Callback<TEntity, CancellationToken>((item, _) => entities.Add(item));
        dbSetMock.Setup(x=> x.AddRange(It.IsAny<IEnumerable<TEntity>>()))
            .Callback(entities.AddRange);
        dbSetMock.Setup(x=> x.AddRangeAsync(It.IsAny<IEnumerable<TEntity>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<TEntity>, CancellationToken>((items, _) => entities.AddRange(items));
        dbSetMock.Setup(x=> x.Remove(It.IsAny<TEntity>()))
            .Callback<TEntity>(item => entities.Remove(item));
        dbSetMock.Setup(x=> x.RemoveRange(It.IsAny<IEnumerable<TEntity>>()))
            .Callback<IEnumerable<TEntity>>(items => items.ForEach(item => entities.Remove(item)));
        dbSetMock.Setup(x=> x.Update(It.IsAny<TEntity>()))
            .Callback<TEntity>(item =>
            {
                var existItem = entities.Single(entity => comparer?.Equals(item, entity) ?? item.Equals(entity));
                entities.Remove(existItem);
                entities.Add(item);
            });
        dbSetMock.Setup(x=> x.UpdateRange(It.IsAny<IEnumerable<TEntity>>()))
            .Callback<IEnumerable<TEntity>>(items =>
            {
                var enumerable = items.ToList();
                enumerable.ForEach(item => entities.Remove(item));
                entities.AddRange(enumerable);
            });
       _dbContextMock
            .SetupGet(expression)
            .Returns(dbSetMock.Object);
    }
}