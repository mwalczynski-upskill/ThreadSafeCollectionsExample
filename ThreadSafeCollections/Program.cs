namespace ThreadSafeCollections
{
    using System;
    using Utils;

    public class Program
    {
        static void Main()
        {
            var streamHelper = new StreamHelper();
            var passwordsToCrack = streamHelper.GetDataFromFile("..//..//..//Files//Passwords.txt");
            var words = streamHelper.GetDataFromFile("..//..//..//Files//Words.txt");

            var passwordCrackerManager = new PasswordCrackerManager(passwordsToCrack, words);
            passwordCrackerManager.CrackPasswords();

            Console.WriteLine("The end of processing");
            Console.ReadKey();
        }
    }
}
