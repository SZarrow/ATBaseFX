using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LockProvider
    {
        private readonly Dictionary<Int32, Int32> _lockDic;
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public LockProvider(Int32 capacity = 1000)
        {
            if (capacity <= 0)
            {
                capacity = 1000;
            }

            _lockDic = new Dictionary<Int32, Int32>(capacity);
            _rwLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean Exists(Int32 key)
        {
            try
            {
                _rwLock.EnterReadLock();
                return _lockDic.TryGetValue(key, out _);
            }
            finally
            {
                if (_rwLock.IsReadLockHeld) { _rwLock.ExitReadLock(); }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public Boolean Lock(Int32 key)
        {
            try
            {
                _rwLock.EnterWriteLock();

                if (_lockDic.TryGetValue(key, out Int32 value))
                {
                    return false;
                }

                _lockDic[key] = 1;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (_rwLock.IsWriteLockHeld)
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void UnLock(Int32 key)
        {
            try
            {
                _rwLock.EnterWriteLock();
                _lockDic.Remove(key);
            }
            catch { }
            finally
            {
                if (_rwLock.IsWriteLockHeld)
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }
    }
}
