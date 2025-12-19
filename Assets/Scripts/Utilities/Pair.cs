using System;
using System.Collections.Generic;

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
    
    public static class DisposerExt {
        public static void AddDisposer(this List<IDisposable> disposables, Action handler) {
            disposables.Add(new Disposer(handler));
        }
        
        public static void DisposeAll(this List<IDisposable> disposables) {
            foreach (IDisposable disposer in disposables) {
                disposer.Dispose();
            }
        }
    }
}