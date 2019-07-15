using Boot4ServiceCollection.Test.Interfaces;

namespace Boot4ServiceCollection.Test.Implementation
{
    public sealed class NotAddedTestClass : INotAddedTestClass
    {
        public int Method(int a, int b)
        {
            return a + b;
        }
    }
}
