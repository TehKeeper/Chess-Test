using System;

namespace Utilities {
    [System.Serializable]
    public struct Pair<T1, T2> {
        public T1 A;
        public T2 B;

        public Pair(T1 a, T2 b) {
            A = a;
            B = b;
        }
    }

    public class Disposer : IDisposable {
        private Action _handler;

        public Disposer(Action handler) {
            _handler = handler;
        }


        public void Dispose() {
            Action handler = _handler;
            _handler = null;
            handler?.Invoke();
        }
    }
}