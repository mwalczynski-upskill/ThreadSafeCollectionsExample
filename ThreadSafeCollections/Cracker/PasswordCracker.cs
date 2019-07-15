namespace ThreadSafeCollections.Cracker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Models;
    using PasswordProviders;
    using Utils;

    public class PasswordCracker : IPasswordCracker
    {
        private readonly IPasswordProvider _passwordProvider;
        private readonly Dictionary<string, bool> _passwordsToCrack;
        private readonly SyncQueue<HashedPassword> _crackedPasswords;
        private readonly Mutex _mutex;

        public PasswordCracker(IPasswordProvider passwordProvider, Dictionary<string, bool> passwordsToCrack, SyncQueue<HashedPassword> crackedPasswords, Mutex mutex)
        {
            _passwordProvider = passwordProvider;
            _passwordsToCrack = passwordsToCrack;
            _crackedPasswords = crackedPasswords;
            _mutex = mutex;
        }

        public void CrackPasswords()
        {
            foreach (var password in _passwordProvider.Produce())
            {
                var morePasswordsToCrack = CrackPassword(password);
                if (morePasswordsToCrack) continue;

                ProductionFinished("No more passwords to crack");
                return;
            }
            ProductionFinished("No more combinations to produce");
        }

        private bool CrackPassword(string password)
        {
            _mutex.WaitOne();
            if (_passwordsToCrack.All(p => p.Value == true))
            {
                _mutex.ReleaseMutex();
                return false;
            }

            var pass = new HashedPassword(password);
            var hashedPass = pass.HashedPass;

            if (_passwordsToCrack.ContainsKey(hashedPass) && _passwordsToCrack[hashedPass] == false)
            {
                _crackedPasswords.Enqueue(pass);
            }

            _mutex.ReleaseMutex();
            return true;
        }

        private void ProductionFinished(string reason)
        {
            Console.WriteLine($"Password cracker with {_passwordProvider.GetType().Name} out: {reason}");
        }
    }
}
