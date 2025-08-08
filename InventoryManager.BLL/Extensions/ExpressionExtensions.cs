namespace InventoryManager.Extension
{
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Linq.Expressions;

    public static partial class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }
        public static Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> And<T>(this Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> expr1, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);
            return Expression.Lambda<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>(
                Expression.AndAlso(left, right), parameter);
        }
    }
}
