using System;

namespace Boot4ServiceCollection.Attributes
{
    public sealed class AddScopedAttribute : AddToServiceCollectionAttribute
    {
        public AddScopedAttribute(Type interfaceType) : base(interfaceType)
        {
            
        }
    }
}
