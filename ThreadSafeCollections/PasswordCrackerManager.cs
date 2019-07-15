namespace ThreadSafeCollections
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Consumers;
    using Cracker;
    using Models;
    using PasswordProviders;
    using Utils;

    public class PasswordCrackerManager
    {
        private readonly Dictionary<string, bool> _passwordsToCrack;
        private readonly string[] _dictionary;

        private readonly SyncQueue<HashedPassword> _crackedPasswords = new SyncQueue<HashedPassword>();

        private readonly Mutex _mutex = new Mutex();

        public PasswordCrackerManager(string[] passwordsToFind, string[] dictionary)
        {
            _passwordsToCrack = passwordsToFind.ToDictionary(p => p, _ => false);
            _dictionary = dictionary;
        }

        public void CrackPasswords()
        {
            var consumer = new Consumer(_passwordsToCrack, _crackedPasswords, _mutex);
            var consumerThread = new Thread(consumer.Consume);

            var passwordCrackers = new List<IPasswordCracker>()
            {
                new PasswordCracker(new SimpleBasePasswordProvider(_dictionary), _passwordsToCrack, _crackedPasswords, _mutex),
                new PasswordCracker(new NumberAtTheBeginningBasePasswordProvider(_dictionary, 1234), _passwordsToCrack, _crackedPasswords, _mutex),
                new PasswordCracker(new NumberAtTheEndPasswordProvider(_dictionary, 1234), _passwordsToCrack, _crackedPasswords, _mutex),
                new PasswordCracker(new FirstBigRestSmallPasswordProvider(_dictionary), _passwordsToCrack, _crackedPasswords, _mutex),
            };            
            var producerThreads = passwordCrackers.Select(p => new Thread(p.CrackPasswords)).ToArray();

            foreach (var thread in producerThreads)
            {
                thread.Start();
            }
            consumerThread.Start();

            foreach (var thread in producerThreads)
            {
                thread.Join();
            }
            consumerThread.Join();
        }
    }
}
