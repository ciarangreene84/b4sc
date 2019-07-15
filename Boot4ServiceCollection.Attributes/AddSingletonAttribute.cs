using System;

namespace Boot4ServiceCollection.Attributes
{
    public sealed class AddSingletonAttribute : AddToServiceCollectionAttribute
    {
        public AddSingletonAttribute(Type interfaceType) : base(interfaceType)
        {

        }
    }
}
