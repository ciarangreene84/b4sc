using Boot4ServiceCollection.Attributes;
using Boot4ServiceCollection.Test.Interfaces;

namespace Boot4ServiceCollection.Test.Implementation
{
    [AddScoped(typeof(IScopedTestClass))]
    public sealed class ScopedTestClass : IScopedTestClass
    {
        public int Method(int a, int b)
        {
            return a + b;
        }
    }
}
