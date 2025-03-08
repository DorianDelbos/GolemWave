using System;
using System.Collections.Generic;

namespace StateMachine
{
    public class StateFactory<T>
    {
        public bool valid = false;

        private Dictionary<Type, Func<BaseState<T>>> stateFactories = new Dictionary<Type, Func<BaseState<T>>>();

        public StateFactory(T context)
        {
            Type[] allValidTypes = typeof(T).Assembly.GetTypes();
            foreach (Type type in allValidTypes)
            {
                if (typeof(BaseState<T>).IsAssignableFrom(type) && !type.IsAbstract && type != typeof(BaseState<T>))
                {
                    stateFactories[type] = () => (BaseState<T>)Activator.CreateInstance(type, context, this);
                }
            }
        }

        public BaseState<T> GetState<U>(bool forceStart = false)
        {
            BaseState<T> state = GetState(typeof(U));

            if (forceStart)
                state.ForceStart();

            return state;
        }

        public BaseState<T> GetState(Type stateType)
        {
            if (stateFactories.ContainsKey(stateType))
            {
                valid = true;
                return stateFactories[stateType]();
            }
            else
            {
                valid = false;
                throw new ArgumentException($"{stateType.Name} is not a valid state Type.");
            }
        }
    }
}