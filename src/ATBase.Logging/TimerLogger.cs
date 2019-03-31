using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATBase.Core;
using ATBase.Logging.Appenders;

namespace ATBase.Logging
{
    internal class TimerLogger : ILogger
    {
        private Dictionary<Int32, ConcurrentQueue<LogEntity>> _messages;
        private ThreadLocal<LocalTraceData> _local;
        private IEnumerable<IAppender> _appenders;
        private ReaderWriterLockSlim _rwLock;

        private IEnumerable<Int32> _logTypes = null;
        private volatile Int32 _dueTime = 1000;
        private Int32 _batchCommitMaxSize = 10;
        private Int32 _idleTimes = 0;

        private Timer _timer;

        public TimerLogger(IEnumerable<LogConfig> configs)
        {
            if (configs == null || configs.Count() == 0)
            {
                throw new ArgumentNullException(nameof(configs));
            }

            var values = Enum.GetValues(typeof(LogType));
            List<Int32> logTypes = new List<Int32>(values.Length);
            foreach (var value in values)
            {
                logTypes.Add((Int32)value);
            }
            _logTypes = logTypes;

            _messages = new Dictionary<Int32, ConcurrentQueue<LogEntity>>(_logTypes.Count());
            _appenders = CreateAppenders(configs);

            _local = new ThreadLocal<LocalTraceData>();
            _rwLock = new ReaderWriterLockSlim();

            InitMessageQueues();
            InitTimer();
            InitLogEnabled(_appenders);
        }

        /*---------------------------------------------------------------------------------------*/

        public void Trace(String traceType, String callResultStatus, String service, String tag, LogPhase phase, String content = null, Object keyData = null)
        {
            if (this.IsTraceEnabled)
            {
                Log(LogType.Trace, traceType, callResultStatus, service, tag, phase, content, keyData);
            }
        }
        public void Debug(String content, Object keyData = null)
        {
            if (this.IsDebugEnabled)
            {
                Log(LogType.Debug, null, null, null, null, LogPhase.ACTION, content, keyData);
            }
        }
        public void Debug(String traceType, String callResultStatus, String service, String tag, LogPhase phase, String content = null, Object keyData = null)
        {
            if (this.IsDebugEnabled)
            {
                Log(LogType.Debug, traceType, callResultStatus, service, tag, phase, content, keyData);
            }
        }
        public void Error(String traceType, String callResultStatus, String service, String tag, String content, Exception exception = null, Object keyData = null)
        {
            Error(traceType, callResultStatus, service, tag, LogPhase.ACTION, content, exception, keyData);
        }
        public void Error(String traceType, String callResultStatus, String service, String tag, LogPhase phase, String content, Exception exception = null, Object keyData = null)
        {
            if (exception != null)
            {
                Log(LogType.Error, traceType, callResultStatus, service, tag, phase, content, keyData, exception);
            }
            else
            {
                Log(LogType.Error, traceType, callResultStatus, service, tag, phase, content, keyData);
            }
        }
        public void Error(Exception ex, String subject, Object keyData = null)
        {
            if (ex != null)
            {
                Log(LogType.Error, null, null, null, null, LogPhase.ACTION, subject, keyData, ex);
            }
            else
            {
                Log(LogType.Error, null, null, null, null, LogPhase.ACTION, subject, keyData);
            }
        }
        public void Error(String message, Object keyData = null)
        {
            this.Error(new SystemException(message), message, keyData);
        }

        public void StartTrace(String appName, String traceId, String ip)
        {
            String m_traceId = traceId;
            String m_appName = appName;

            if (m_appName.IsNullOrWhiteSpace())
            {
                m_appName = "UN_KNOW_APP";
            }

            if (String.IsNullOrWhiteSpace(m_traceId))
            {
                m_traceId = Guid.NewGuid().ToString().ToUpper();
            }

            SetLocalTraceId(new LocalTraceData()
            {
                TraceId = m_traceId,
                AppName = appName
            });

            Log(LogType.Trace, "TRACE", "OK", "$:BEGIN_TRACE", "START_TRACE", LogPhase.BEGIN, ip, null);
        }

        public void StopTrace()
        {
            Log(LogType.Trace, "TRACE", "OK", "$:END_TRACE", "STOP_TRACE", LogPhase.END, $"END_TRACE", null);
            SetLocalTraceId(null);
        }

        public void ContinueTrace(String appName, String traceId)
        {
            if (appName.HasValue() && traceId.HasValue())
            {
                SetLocalTraceId(new LocalTraceData()
                {
                    AppName = appName,
                    TraceId = traceId
                });
            }
        }

        private void SetLocalTraceId(LocalTraceData data)
        {
            try
            {
                _rwLock.EnterWriteLock();
                _local.Value = data;
            }
            catch { }
            finally
            {
                if (_rwLock.IsWriteLockHeld) { _rwLock.ExitWriteLock(); }
            }
        }

        public String CurrentTraceId
        {
            get
            {
                try
                {
                    return _local.Value != null ? _local.Value.TraceId : "TRACE_LOCAL_VALUE_CurrentTraceId_NULL";
                }
                catch
                {
                    return $"EX_CurrentTraceId_{Guid.NewGuid().ToString()}";
                }
            }
        }

        public String CurrentAppName
        {
            get
            {
                try
                {
                    return _local.Value != null ? _local.Value.AppName : "TRACE_LOCAL_VALUE_CurrentAppName_NULL";
                }
                catch
                {
                    return $"EX_CurrentAppName_{Guid.NewGuid().ToString()}";
                }
            }
        }

        private void Log(LogType logType, String traceType, String callResultStatus, String service, String tag, LogPhase phase, String content, Object keyData, params Exception[] exceptions)
        {
            var logEntity = new LogEntity()
            {
                TraceId = CurrentTraceId,
                ThreadId = $"#{Thread.CurrentThread.ManagedThreadId.ToString()}",
                AppName = CurrentAppName,
                TraceType = traceType,
                CallResultStatus = callResultStatus,
                Service = service,
                Tag = tag,
                Phase = phase,
                Exceptions = exceptions,
                LogContent = content,
                KeyData = keyData,
                LogType = logType,
                CreateTime = DateTime.Now
            };

            AppendToQueue(logType, logEntity);
        }

        private Boolean IsTraceEnabled { get; set; } = false;
        private Boolean IsDebugEnabled { get; set; } = false;

        /*---------------------------------------------------------------------------------------*/

        private void AppendToQueue(LogType logType, LogEntity log)
        {
            Int32 key = (Int32)logType;

            if (log != null)
            {
                _messages[key].Enqueue(log);
            }
        }

        private void InitTimer()
        {
            _timer = new Timer(TimerTick, null, Timeout.Infinite, Timeout.Infinite);
            _timer.Change(_dueTime, Timeout.Infinite);
        }

        private IEnumerable<IAppender> CreateAppenders(IEnumerable<LogConfig> logConfigs)
        {
            foreach (var config in logConfigs)
            {
                if (config is FileLogConfig)
                {
                    yield return new FileAppender(config as FileLogConfig);
                }

                if (config is MongoDbLogConfig)
                {
                    yield return new MongoDbAppender(config as MongoDbLogConfig);
                }

                if (config is EmailLogConfig)
                {
                    yield return new EmailAppender(config as EmailLogConfig);
                }

                if (config is ConsoleLogConfig)
                {
                    yield return new ConsoleAppender(config as ConsoleLogConfig);
                }
            }
        }

        private void InitMessageQueues()
        {
            foreach (var logType in _logTypes)
            {
                _messages[logType] = new ConcurrentQueue<LogEntity>();
            }
        }

        private void InitLogEnabled(IEnumerable<IAppender> appenders)
        {
            if (_appenders != null)
            {
                foreach (var appender in _appenders)
                {
                    if (appender.Allow(LogType.Trace))
                    {
                        this.IsTraceEnabled = true;
                    }

                    if (appender.Allow(LogType.Debug))
                    {
                        this.IsDebugEnabled = true;
                    }
                }
            }
        }

        /*---------------------------------------------------------------------------------------*/

        private void TimerTick(Object state)
        {
            foreach (Int32 logType in _logTypes)
            {
                if (_messages.ContainsKey(logType) && _messages[logType].Count > 0)
                {
                    Interlocked.Exchange(ref _idleTimes, 0);

                    List<LogEntity> entities = new List<LogEntity>(_batchCommitMaxSize);

                    while (_messages[logType].Count > 0)
                    {
                        _messages[logType].TryDequeue(out LogEntity entity);

                        if (entity != null)
                        {
                            entities.Add(entity);
                        }

                        if (entities.Count == _batchCommitMaxSize)
                        {
                            WriteToAppenders(logType, entities);
                            entities.Clear();
                        }
                    }

                    if (entities.Count > 0)
                    {
                        WriteToAppenders(logType, entities);
                        entities.Clear();
                    }
                }
            }

            Interlocked.Add(ref _idleTimes, 1);

            if (_idleTimes >= 1 && _idleTimes <= 6)
            {
                _dueTime = _idleTimes * 1000;
            }

            _timer.Change(_dueTime, Timeout.Infinite);
        }

        private void WriteToAppenders(Int32 logType, IEnumerable<LogEntity> entities)
        {
            Parallel.ForEach(_appenders, appender =>
            {
                if (!appender.Allow((LogType)logType)) { return; }

                var writeResult = appender.Write(entities);
                if (!writeResult.Success)
                {
                    var failedLogs = writeResult.Value != null ? entities.Except(writeResult.Value) : entities;
                    foreach (var failedLog in failedLogs)
                    {
                        AppendToQueue((LogType)logType, failedLog);
                    }
                }
            });
        }
    }
}
