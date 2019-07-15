namespace ThreadSafeCollections.PasswordProviders
{
    using System.Collections.Generic;

    public interface IPasswordProvider
    {
        IEnumerable<string> Produce();
    }
}
