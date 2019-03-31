using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ATBase.Core
{
    /// <summary>
    /// 这是一个简易版的依赖注入工具类。
    /// </summary>
    public static class XDI
    {
        private const Int32 DEFAULT_SCOPEID = -1;
        private static readonly ConcurrentQueue<Type> s_types = new ConcurrentQueue<Type>();
        private static readonly IServiceCollection s_services = new ServiceCollection();
        private static IServiceProvider s_defaultServiceProvider = null;
        /// <summary>
        /// 接口类型，实现类型
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Type> _cachedTypes = new ConcurrentDictionary<Type, Type>();
        /// <summary>
        /// 实例对象，对象的ScopeId
        /// </summary>
        private static readonly ConcurrentDictionary<Object, Int32> _objectScopes = new ConcurrentDictionary<Object, Int32>();
        /// <summary>
        /// ScopeId，域下的对象链
        /// </summary>
        private static readonly ConcurrentDictionary<Int32, Stack<Object>> _callChain = new ConcurrentDictionary<Int32, Stack<Object>>();
        /// <summary>
        /// 线程Id，ScopeId链
        /// </summary>
        private static readonly ConcurrentDictionary<Int32, Stack<Int32>> _scopeChain = new ConcurrentDictionary<Int32, Stack<Int32>>();
        /// <summary>
        /// Scoped生命周期的类型集合
        /// </summary>
        private static readonly ConcurrentQueue<Type> _scopedTypes = new ConcurrentQueue<Type>();

        /// <summary>
        /// 
        /// </summary>
        public static void Run()
        {
            LoadAssemblies();
            s_defaultServiceProvider = s_services.BuildServiceProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="scopeId"></param>
        public static Object Resolve(Type interfaceType, Int32 scopeId = DEFAULT_SCOPEID)
        {
            if (interfaceType == null)
            {
                return null;
            }

            if (scopeId == -1)
            {
                scopeId = GetCurrentScopeId();
            }

            Object service = null;

            if (s_defaultServiceProvider != null)
            {
                service = s_defaultServiceProvider.GetService(interfaceType);
                if (service != null)
                {
                    return service;
                }

                try
                {
                    service = s_defaultServiceProvider.GetRequiredService(interfaceType);
                    if (service != null)
                    {
                        return service;
                    }
                }
                catch { }
            }

            service = GetServiceFromAssemblies(interfaceType, scopeId);
            if (service != null)
            {
                return service;
            }

            return null;
        }

        /// <summary>
        /// Sets the instance of specified interfaceType as a scoped lifetime object.
        /// </summary>
        public static void Scope(Type interfaceType)
        {
            if (interfaceType != null)
            {
                _scopedTypes.Enqueue(interfaceType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceTypeFullName"></param>
        public static void Scope(String interfaceTypeFullName)
        {
            if (interfaceTypeFullName.HasValue())
            {
                var interfaceType = (from t0 in s_types
                                     where String.Compare(t0.FullName, interfaceTypeFullName, true) == 0
                                     select t0).FirstOrDefault();

                Scope(interfaceType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public static void AddService(ServiceDescriptor service)
        {
            s_services.Add(service);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(IServiceCollection services)
        {
            foreach (var service in services)
            {
                if (!s_services.Contains(service))
                {
                    s_services.Add(service);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public static Int32 CreateScope(Object instance)
        {
            Int32 scopeId = DEFAULT_SCOPEID;
            if (instance != null)
            {
                scopeId = (instance.GetHashCode() & 0x7fffffff) % Int32.MaxValue;
            }

            _objectScopes[instance] = scopeId;

            if (!_scopeChain.ContainsKey(Thread.CurrentThread.ManagedThreadId))
            {
                _scopeChain[Thread.CurrentThread.ManagedThreadId] = new Stack<Int32>();
            }

            _scopeChain[Thread.CurrentThread.ManagedThreadId].Push(scopeId);

            _callChain[scopeId] = new Stack<Object>();

            return scopeId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public static void Composite(Object instance)
        {
            CompositeCore(instance, GetScopeId(instance));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public static void Release(Object instance)
        {
            Int32 scopeId = GetScopeId(instance);

            if (_callChain.TryGetValue(scopeId, out Stack<Object> scopedObjects))
            {
                try
                {
                    while (scopedObjects.Count > 0)
                    {
                        var obj = scopedObjects.Pop();
                        if (obj is IDisposable)
                        {
                            (obj as IDisposable).Dispose();
                        }
                    }

                    if (instance is IDisposable)
                    {
                        (instance as IDisposable).Dispose();
                    }
                }
                catch (Exception) { }
                finally
                {
                    _callChain.TryRemove(scopeId, out _);
                    _objectScopes.TryRemove(instance, out _);
                    _scopeChain.TryRemove(Thread.CurrentThread.ManagedThreadId, out _);
                }
            }
        }

        private static void CompositeCore(Object instance, Int32 scopeId)
        {
            if (instance == null)
            {
                return;
            }

            var instanceType = instance.GetType();
            var fields = instanceType.GetRuntimeFields();

            foreach (var fieldInfo in fields)
            {
                Object fieldValue = fieldInfo.XGetValue(instance);

                if (fieldValue == null)
                {
                    var value = Resolve(fieldInfo.FieldType, scopeId);
                    if (value != null)
                    {
                        fieldInfo.XSetValue(instance, value);
                    }
                }
            }
        }

        private static Object GetServiceFromAssemblies(Type interfaceType, Int32 scopeId)
        {
            if (_scopedTypes.Contains(interfaceType))
            {
                if (_callChain.TryGetValue(scopeId, out Stack<Object> objects))
                {
                    var find = (from o in objects
                                where o != null && interfaceType.IsAssignableFrom(o.GetType())
                                select o).LastOrDefault();

                    if (find != null) { return find; }
                }
            }

            if (_cachedTypes.TryGetValue(interfaceType, out Type implType))
            {
                var ins = ConstructInstance(implType, scopeId);
                if (ins != null)
                {
                    return ins;
                }
            }

            var findLastImplType = (from t0 in s_types
                                    where interfaceType.IsAssignableFrom(t0)
                                    select t0).LastOrDefault();

            if (findLastImplType != null)
            {
                var ins = ConstructInstance(findLastImplType, scopeId);
                if (ins != null)
                {
                    _cachedTypes[interfaceType] = findLastImplType;
                    return ins;
                }
            }

            return null;
        }

        private static Object ConstructInstance(Type implementationType, Int32 scopeId)
        {
            var defaultConstructor = implementationType.GetConstructor(Type.EmptyTypes);
            if (defaultConstructor != null)
            {
                var ins = Construct(scopeId, defaultConstructor, null);
                if (ins != null)
                {
                    CompositeCore(ins, scopeId);
                    return ins;
                }
            }

            var ctors = implementationType.GetConstructors();
            foreach (var ctor in ctors)
            {
                var ctorParaInfos = ctor.GetParameters();
                List<Object> ctorParas = new List<Object>(ctorParaInfos.Length);

                foreach (var paraInfo in ctorParaInfos)
                {
                    var paraIns = Resolve(paraInfo.ParameterType, scopeId);
                    if (paraIns != null)
                    {
                        ctorParas.Add(paraIns);
                    }
                }

                var ins = Construct(scopeId, ctor, ctorParas.ToArray());
                if (ins != null)
                {
                    CompositeCore(ins, scopeId);
                    return ins;
                }
            }

            return null;
        }

        private static void LoadAssemblies()
        {
            String binPath = AppDomain.CurrentDomain.BaseDirectory;
            var asmFiles = Directory.EnumerateFiles(binPath, "*.dll");

            List<Task> tasks = new List<Task>(60);

            foreach (var asmFile in asmFiles)
            {
                tasks.Add(Task.Run(() =>
                {
                    var asm = Assembly.LoadFrom(asmFile);
                    var types = asm.GetTypes().Where(
                        x => x.IsPublic
                        && !x.IsAbstract
                        && !x.IsArray
                        && !x.IsEnum
                        && !x.IsInterface);
                    foreach (var type in types)
                    {
                        s_types.Enqueue(type);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private static Object Construct(Int32 scopeId, ConstructorInfo ctorInfo, Object[] parameters)
        {
            var instance = ctorInfo.XConstruct(parameters);

            if (scopeId > 0 && _callChain.TryGetValue(scopeId, out Stack<Object> callChain))
            {
                if (instance != null)
                {
                    callChain.Push(instance);
                }
            }

            return instance;
        }

        private static Int32 GetScopeId(Object instance)
        {
            return _objectScopes.TryGetValue(instance, out Int32 scopeId) ? scopeId : DEFAULT_SCOPEID;
        }

        private static Int32 GetCurrentScopeId()
        {
            return _scopeChain.TryGetValue(Thread.CurrentThread.ManagedThreadId, out Stack<Int32> s) ? s.Peek() : DEFAULT_SCOPEID;
        }
    }
}
