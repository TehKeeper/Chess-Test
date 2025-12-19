using System;

namespace Utilities {
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