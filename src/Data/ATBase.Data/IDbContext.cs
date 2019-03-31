using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using ATBase.Core;

namespace ATBase.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        XResult<Boolean> Add<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        XResult<Boolean> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, Boolean>> filter) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, Boolean>> filter, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        XResult<Boolean> Remove<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        XResult<Boolean> RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        XResult<Boolean> Update<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        XResult<Boolean> UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="useTransaction"></param>
        XResult<Int32> SaveChanges(Boolean useTransaction = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="useTransaction"></param>
        Task<XResult<Int32>> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken), Boolean useTransaction = false);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        IQueryable<TEntity> AsQueryable<TEntity>();
    }
}
