using System;

namespace Boot4ServiceCollection.Attributes
{
    public sealed class AddTransientAttribute : AddToServiceCollectionAttribute
    {
        public AddTransientAttribute(Type interfaceType) : base(interfaceType)
        {

        }
    }
}
