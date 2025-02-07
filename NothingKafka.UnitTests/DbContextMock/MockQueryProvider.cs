using System.Collections;
using System.Linq.Expressions;

namespace NothingKafka.UnitTests.DbContextMock;

public class MockQueryProvider<TEntity> : IOrderedQueryable<TEntity>, IQueryProvider
{
    private IEnumerable<TEntity>? _enumerable;

    protected MockQueryProvider(Expression expression)
    {
        Expression = expression;
    }

    protected MockQueryProvider(IEnumerable<TEntity> enumerable)
    {
        var entities = enumerable.ToList();
        _enumerable = entities;
        Expression = entities.AsQueryable().Expression;
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
    {
        _enumerable ??= CompileExpressionItem<IEnumerable<TEntity>>(Expression);
        return _enumerable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        _enumerable ??= CompileExpressionItem<IEnumerable<TEntity>>(Expression);
        return _enumerable.GetEnumerator();
    }

    public Type ElementType => typeof(TEntity);

    public Expression Expression { get; }

    public IQueryProvider Provider => this;

    public IQueryable CreateQuery(Expression expression)
    {
        if (expression is not MethodCallExpression m)
            return CreateQuery<TEntity>(expression);
        var resultType = m.Method.ReturnType;
        var elementType = resultType.GetGenericArguments().First();
        return (IQueryable) CreateInstance(elementType, expression)!;
    }

    public IQueryable<T> CreateQuery<T>(Expression expression)
    {
        return (IQueryable<T>) CreateInstance(typeof(TEntity), expression)!;
    }

    public object Execute(Expression expression)
    {
        return CompileExpressionItem<object>(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return CompileExpressionItem<TResult>(expression);
    }

    private object? CreateInstance(Type tElement, Expression expression)
    {
        var queryType = GetType().GetGenericTypeDefinition().MakeGenericType(tElement);
        return Activator.CreateInstance(queryType, expression);
    }

    private static TResult CompileExpressionItem<TResult>(Expression expression)
    {
        var visitor = new MockExpressionVisitor();
        var body = visitor.Visit(expression);
        var f = Expression.Lambda<Func<TResult>>(body ?? throw new InvalidOperationException($"{nameof(body)} is null"),
            (IEnumerable<ParameterExpression>?) null);
        return f.Compile().Invoke();
    }
}