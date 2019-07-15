namespace ThreadSafeCollections.PasswordProviders
{
    using System.Collections.Generic;

    public class NumberAtTheEndPasswordProvider : IPasswordProvider
    {
        private readonly string[] _dictionary;
        private readonly int _maxNumberToAdd;

        public NumberAtTheEndPasswordProvider(string[] dictionary, int maxNumberToAdd)
        {
            _dictionary = dictionary;
            _maxNumberToAdd = maxNumberToAdd;
        }

        public IEnumerable<string> Produce()
        {
            foreach (var word in _dictionary)
            {
                for (var i = 0; i <= _maxNumberToAdd; i++)
                {
                    var password = word + i;
                    yield return password;
                }
            }
        }
    }
}
