using Boot4ServiceCollection.Attributes;
using Boot4ServiceCollection.Test.Interfaces;

namespace Boot4ServiceCollection.Test.Implementation
{
    [AddTransient(typeof(ITransientTestClass))]
    public sealed class TransientTestClass : ITransientTestClass
    {
        public int Method(int a, int b)
        {
            return a / b;
        }
    }
}
