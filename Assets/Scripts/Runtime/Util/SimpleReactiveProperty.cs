using System;

namespace AS.Runtime.Util
{
    public class SimpleReactiveProperty<T>
    {
        public T Value 
        {
            get => _value;
            set 
            {
                _value = value;
                ChangedEvent?.Invoke(_value);
            }
        }
        
        private T _value;
        
        public event Action<T> ChangedEvent;
    }
}
