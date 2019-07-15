namespace ThreadSafeCollections.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Models;
    using Utils;

    public class Consumer : IConsumer
    {
        private readonly Dictionary<string, bool> _passwordsToCrack;
        private readonly SyncQueue<HashedPassword> _crackedPasswords;

        private readonly Mutex _mutex;

        public Consumer(Dictionary<string, bool> passwordsToCrack, SyncQueue<HashedPassword> crackedPasswords, Mutex mutex)
        {
            _passwordsToCrack = passwordsToCrack;
            _crackedPasswords = crackedPasswords;
            _mutex = mutex;
        }

        public void Consume()
        {
            var time = 0;
            while (true)
            {
                if (_crackedPasswords.Count() != 0)
                {
                    time = 0;

                    var pass = _crackedPasswords.Dequeue();
                    var hashedPass = pass.HashedPass;

                    Console.WriteLine("Cracked: " + pass);

                    _mutex.WaitOne();
                    _passwordsToCrack[hashedPass] = true;

                    if (_passwordsToCrack.Count(p => p.Value == false) == 0)
                    {
                        _mutex.ReleaseMutex();
                        ConsumptionFinished("All passwords cracked");
                        return;
                    }
                    _mutex.ReleaseMutex();
                }
                else if (_passwordsToCrack.Count(p => p.Value == false) == 0)
                {
                    ConsumptionFinished("All passwords cracked");
                    return;
                }
                else
                {
                    Thread.Sleep(20);
                    if (++time != 500) continue;

                    ConsumptionFinished("No more passwords found for long time");
                    return;
                }
            }
        }

        private void ConsumptionFinished(string reason)
        {
            Console.WriteLine($"Consumer out: {reason}");

        }
    }
}
