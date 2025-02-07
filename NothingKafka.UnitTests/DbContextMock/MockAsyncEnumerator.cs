namespace NothingKafka.UnitTests.DbContextMock;

internal class MockAsyncEnumerator<TEntity>(IEnumerator<TEntity> enumerator)
    : IAsyncEnumerator<TEntity>
{
    private readonly IEnumerator<TEntity> _enumerator = enumerator;

    public TEntity Current => _enumerator.Current;

    public ValueTask DisposeAsync()
    {
        _enumerator.Dispose();
        return new ValueTask();
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_enumerator.MoveNext());
    }
}