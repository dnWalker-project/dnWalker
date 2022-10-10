using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers
{
    internal class ReflectionNativePeerCache<T> : INativePeerCache<T> where T : class
    {
        //private readonly Dictionary<string, Dictionary<string, Type>> _typeLookup = new Dictionary<string, Dictionary<string, Type>>();
        //private readonly Dictionary<Type, T> _cache = new Dictionary<Type, T>();
        
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Type>> _typeLookup = new ConcurrentDictionary<string, ConcurrentDictionary<string, Type>>();
        private readonly ConcurrentDictionary<Type, T> _cache = new ConcurrentDictionary<Type, T>();

        public ReflectionNativePeerCache()
        {
            foreach (Type methodCallNativePeer in typeof(MethodCallNativePeers).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(T))))
            {
                NativePeerAttribute att = methodCallNativePeer.GetCustomAttribute<NativePeerAttribute>();
                if (att == null) continue;

                string typeName = att.TypeName;
                IEnumerable<string> methodNames = att.MatchMethods ? ReflectionHelpers.GetHandlerMethodNames(methodCallNativePeer) : att.Methods;
                UpdateTypeLookup(typeName, methodNames, methodCallNativePeer);
            }
        }

        private void UpdateTypeLookup(string typeName, IEnumerable<string> methodNames, Type methodCallNativePeer)
        {
            //if (!_typeLookup.TryGetValue(typeName, out Dictionary<string, Type> methodMapper))
            //{
            //    methodMapper = new Dictionary<string, Type>();
            //    _typeLookup.Add(typeName, methodMapper);
            //}

            ConcurrentDictionary<string, Type> methodMapper = _typeLookup.GetOrAdd(typeName, _ => new ConcurrentDictionary<string, Type>());

            foreach (string methodName in methodNames)
            {
                if (!methodMapper.TryAdd(methodName, methodCallNativePeer))
                {
                    Debug.WriteLine($"Duplicate native peers for {typeName}::{methodName}: {methodMapper[methodName]} and {methodCallNativePeer}");
                }
            }
        }

        private bool TryGetHandlerType(MethodDef method, out Type handlerType)
        {
            if (_typeLookup.TryGetValue(method.DeclaringType.FullName, out ConcurrentDictionary<string, Type> methodMapper) &&
                methodMapper.TryGetValue(method.Name, out handlerType))
            {
                return true;
            }

            handlerType = null;
            return false;
        }

        private static T BuildHandler(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public bool TryGetNativePeer(MethodDef method, out T nativePeer)
        {
            if (TryGetHandlerType(method, out Type handlerType))
            {
                nativePeer = _cache.GetOrAdd(handlerType, t => BuildHandler(t));

                //if (!_cache.TryGetValue(handlerType, out nativePeer))
                //{
                //    nativePeer = BuildHandler(handlerType);
                //    _cache.Add(handlerType, nativePeer);
                //}
                return true;
            }
            nativePeer = null;
            return false;
        }
    }
}
