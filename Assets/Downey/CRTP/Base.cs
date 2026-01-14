using UnityEngine;

namespace Downey.CRTP
{
    abstract class Base<TDerived> where TDerived : Base<TDerived>
    {
        public void DoWork()
        {
            ((TDerived)this).Work();
        }
        
        protected abstract void Work();
    }

    class Worker : Base<Worker>
    {
        protected override void Work()
        {
            Debug.Log("#Worker# is doing work");
        }
    }
}