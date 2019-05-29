using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class SuperMap
    {
        private static readonly ConcurrentDictionary<Type, Object> _cache = new ConcurrentDictionary<Type, Object>();

        /// <summary>
        /// 将键值对映射到一个实体类
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="keyValues">键值对</param>
        public static XResult<T> Map<T>(Dictionary<String, Object> keyValues) where T : new()
        {
            var func = GetSuperFunc<Dictionary<String, Object>, T>();
            if (func != null)
            {
                try
                {
                    var value = func(keyValues);
                    return new XResult<T>(value);
                }
                catch (Exception ex)
                {
                    return new XResult<T>(default(T), ex);
                }
            }

            return new XResult<T>(default(T), new NotSupportedException("不支持的转换类型"));
        }

        /// <summary>
        /// 将键值对映射到一个实体类
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="keyValues">键值对</param>
        public static XResult<T> Map<T>(SortedDictionary<String, Object> keyValues) where T : new()
        {
            var func = GetSuperFunc<SortedDictionary<String, Object>, T>();
            if (func != null)
            {
                try
                {
                    var value = func(keyValues);
                    return new XResult<T>(value);
                }
                catch (Exception ex)
                {
                    return new XResult<T>(default(T), ex);
                }
            }

            return new XResult<T>(default(T), new NotSupportedException("不支持的转换类型"));
        }

        private static Func<TDictionary, TResult> GetSuperFunc<TDictionary, TResult>() where TResult : new()
            where TDictionary : IDictionary<String, Object>
        {
            var entityType = typeof(TResult);

            if (!_cache.TryGetValue(typeof(TResult), out Object value))
            {
                try
                {
                    LabelTarget returnTarget = Expression.Label(entityType);
                    var keyValuesParameter = Expression.Parameter(typeof(TDictionary), "keyValues");

                    var entityVar = Expression.Variable(entityType, "entity");
                    var kvVar = Expression.Variable(typeof(KeyValuePair<String, Object>), "kv");
                    var keyValuesCountPropertyExpr = Expression.Property(keyValuesParameter, "Count");
                    var propertyExpr = Expression.Call(Expression.Call(entityVar, "GetType", Type.EmptyTypes), "GetProperty", Type.EmptyTypes, Expression.Convert(Expression.Property(kvVar, "Key"), typeof(String)));
                    var foreachExpr = ForEach(keyValuesParameter, kvVar, Expression.Block(
                              Expression.IfThen(Expression.NotEqual(propertyExpr, Expression.Constant(null)),
                              Expression.Call(propertyExpr, "SetValue", Type.EmptyTypes, entityVar, Expression.Property(kvVar, "Value")))
                    ));
                    var blockExpr = Expression.Block(
                        new[] { entityVar, kvVar },
                        Expression.Assign(entityVar, Expression.New(entityType)),
                        Expression.IfThen(Expression.Equal(keyValuesParameter, Expression.Constant(null)), Expression.Return(returnTarget, Expression.Constant(null, entityType))),
                        Expression.IfThen(Expression.Equal(keyValuesCountPropertyExpr, Expression.Constant(0)), Expression.Return(returnTarget, entityVar)),
                        foreachExpr,
                        Expression.Return(returnTarget, entityVar),
                        Expression.Label(returnTarget, Expression.Constant(null, entityType))
                      );

                    var expr = Expression.Lambda<Func<TDictionary, TResult>>(blockExpr, new ParameterExpression[] { keyValuesParameter });
                    value = expr.Compile();

                    _cache[typeof(TResult)] = value;
                }
                catch { }
            }

            return value as Func<TDictionary, TResult>;
        }

        private static Expression ForEach(Expression collection, ParameterExpression loopVar, Expression loopContent)
        {
            var elementType = loopVar.Type;
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);

            var enumeratorVar = Expression.Variable(enumeratorType, "enumerator");
            var getEnumeratorCall = Expression.Call(collection, enumerableType.GetMethod("GetEnumerator"));
            var enumeratorAssign = Expression.Assign(enumeratorVar, getEnumeratorCall);

            var moveNextCall = Expression.Call(enumeratorVar, typeof(IEnumerator).GetMethod("MoveNext"));

            var breakLabel = Expression.Label("LoopBreak");

            var loop = Expression.Block(new[] { enumeratorVar },
                enumeratorAssign,
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Equal(moveNextCall, Expression.Constant(true)),
                        Expression.Block(new[] { loopVar },
                            Expression.Assign(loopVar, Expression.Property(enumeratorVar, "Current")),
                            loopContent
                        ),
                        Expression.Break(breakLabel)
                    ),
                breakLabel)
            );

            return loop;
        }
    }
}
