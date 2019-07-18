using Boot4ServiceCollection.Attributes;
using Boot4ServiceCollection.Test.Interfaces;

namespace Boot4ServiceCollection.Test.Implementation
{
    [AddSingleton(typeof(ISingletonTestClass))]
    public sealed class SingletonTestClass : ISingletonTestClass
    {
        public int Method(int a, int b)
        {
            return a * b;
        }
    }
}
