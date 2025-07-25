﻿using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace ExHooks
{
    public static class HookHelper
    {
        public static MethodInfo MethodOf(Delegate method) => method.Method;
        public static MethodInfo? EzGetMethod<T>(string name, Type[]? parameters = null) => EzGetMethod(typeof(T), name, parameters);
        public static MethodInfo? EzGetMethod(Type type, string name, Type[]? parameters = null)
        {
            BindingFlags query = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (parameters == null)
            {
                return type.GetMethod(name, query);
            }
            return type.GetMethod(name, query, null, parameters, null);
        }

        public static ConstructorInfo? EzGetConstructor<T>(Type[]? parameters = null) => EzGetConstructor(typeof(T), parameters);
        public static ConstructorInfo? EzGetConstructor(Type type, Type[]? parameters = null)
        {
            BindingFlags query = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (parameters == null)
            {
                return type.GetConstructor(query, Type.EmptyTypes);
            }
            return type.GetConstructor(query, parameters);
        }

        public class DisposableHookCollection
        {
            internal List<ILHook> ilHooks = new List<ILHook>();
            internal List<Hook> mmHooks = new List<Hook>();

            public void Dispose()
            {
                foreach (ILHook hook in ilHooks)
                {
                    hook.Dispose();
                }
                ilHooks.Clear();

                foreach (Hook hook in mmHooks)
                {
                    hook.Dispose();
                }
                mmHooks.Clear();
            }

            public void ILHook<T>(string methodName, ILContext.Manipulator to, Type[]? parameters = null)
                => ilHooks.Add(new ILHook(EzGetMethod<T>(methodName, parameters), to));
            public void ILHook(Type type, string methodName, ILContext.Manipulator to, Type[]? parameters = null)
                => ilHooks.Add(new ILHook(EzGetMethod(type, methodName, parameters), to));
            public void ILHook<T>(ILContext.Manipulator to, Type[]? parameters = null)
                => ilHooks.Add(new ILHook(EzGetConstructor(typeof(T), parameters), to));
            public void ILHook(Type type, ILContext.Manipulator to, Type[]? parameters = null)
                => ilHooks.Add(new ILHook(EzGetConstructor(type, parameters), to));
            public void MMHook<T>(string methodName, Delegate to, Type[]? parameters = null)
                => mmHooks.Add(new Hook(EzGetMethod<T>(methodName, parameters), to));
            public void MMHook(Type type, string methodName, Delegate to, Type[]? parameters = null)
                => mmHooks.Add(new Hook(EzGetMethod(type, methodName, parameters), to));
            public void MMHook<T>(Delegate to, Type[]? parameters = null)
                => mmHooks.Add(new Hook(EzGetConstructor<T>(parameters), to));
            public void MMHook(Type type, Delegate to, Type[]? parameters = null)
                => mmHooks.Add(new Hook(EzGetConstructor(type, parameters), to));
        }
    }
}