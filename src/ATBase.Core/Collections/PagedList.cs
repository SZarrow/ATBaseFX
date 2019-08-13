using System;
using System.Collections.Generic;
using System.Linq;

namespace ATBase.Core.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PagedList<T> : IEnumerable<T> where T : class
    {
        private readonly IEnumerable<T> _container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumberShowSize"></param>
        public PagedList(IOrderedQueryable<T> dataSource, Int32 pageIndex, Int32 pageSize, Int32 pageNumberShowSize = 5)
        {
            if (dataSource == null)
            {
                return;
            }

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 1;
            }

            if (pageSize > 0)
            {
                try
                {
                    Int32 totalCount = dataSource.Count();
                    _container = dataSource.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    this.PageInfo = new PageInfo(pageIndex, pageSize, totalCount, pageNumberShowSize);
                }
                catch (Exception ex)
                {
                    this.Exception = ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumberShowSize"></param>
        public PagedList(IEnumerable<T> dataSource, Int32 pageIndex, Int32 pageSize, Int32 pageNumberShowSize = 5)
        {
            if (dataSource == null)
            {
                return;
            }

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 1;
            }

            if (pageSize > 0)
            {
                try
                {
                    Int32 totalCount = dataSource.Count();
                    _container = dataSource.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    this.PageInfo = new PageInfo(pageIndex, pageSize, totalCount, pageNumberShowSize);
                }
                catch (Exception ex)
                {
                    this.Exception = ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PageInfo PageInfo { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _container != null ? _container.GetEnumerator() as IEnumerator<T> : null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
