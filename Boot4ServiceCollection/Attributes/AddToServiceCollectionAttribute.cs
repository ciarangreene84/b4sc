using System;

namespace Boot4ServiceCollection.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class AddToServiceCollectionAttribute : Attribute
    {
        public Type InterfaceType { get; private set; }

        protected AddToServiceCollectionAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}
