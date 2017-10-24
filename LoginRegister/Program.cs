using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace LoginRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            Database robomarktDB = new Database();

            Console.Clear();

            robomarktDB.Register("Natsu", "FairyTail", "dragneelft@panda.com");
            robomarktDB.Register("Happy", "FairyTail", "happyft@panda.com");
            robomarktDB.Register("Gosu", "Poep", "gosu@bronzev.noob");
            robomarktDB.Register("Gosu", "Poep", "gosu@bronzev.noob");

            robomarktDB.ViewData();

            robomarktDB.Login("Natsu", "FairyTail");
            robomarktDB.Login("Happy", "FairyTail");
            robomarktDB.Login("Gosu", "Good");
            robomarktDB.Login("Gosu", "Poep");
            robomarktDB.Login("Uzi", "RNG");
        }
    }

    class Database
    {
        private List<string> usernames = new List<string>();
        private List<string> passwords = new List<string>();
        private List<string> salts = new List<string>();
        private List<string> emails = new List<string>();

        static string GetSalt()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 5)
                                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        static string GetSHA512Hash(string input, string salt = "")
        {
            SHA512 shaM = SHA512.Create();
            StringBuilder sBuilder = new StringBuilder();

            byte[] data = shaM.ComputeHash(Encoding.ASCII.GetBytes(input + salt));

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public void Login(string username, string password)
        {
            try
            {
                int index = this.usernames.IndexOf(username);

                if (this.passwords[index] == GetSHA512Hash(password, this.salts[index]))
                {
                    Console.WriteLine("You have succesfully logged in.");
                }
                else
                {
                    Console.WriteLine("Wrong password, Try again.");
                }
            }
            catch
            {
                Console.WriteLine("Couldn't find your account.");
            }
        }

        public void Register(string username, string password, string email)
        {
            if (this.usernames.Contains(username))
            {
                Console.WriteLine("That username is taken. Try another.");
            }
            else
            {
                string salt = GetSalt();

                this.usernames.Add(username);
                this.passwords.Add(GetSHA512Hash(password, salt));
                this.salts.Add(salt);
                this.emails.Add(email);

                Console.WriteLine("You have succesfully registered.");
            }
        }

        public void ViewData()
        {
            if (this.usernames.Count > 0)
            {
                Console.WriteLine();
                for (int i = 0; i < this.usernames.Count; i++)
                {
                    Console.WriteLine("{0} - {1}, {2}, {3}, {4}", i, this.usernames[i], this.passwords[i], this.salts[i], this.emails[i]);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Database is empty.");
            }
        }
    }
}

// This code produces the following output:
// 
// You have succesfully registered.
// You have succesfully registered.
// You have succesfully registered.
// That username is taken. Try another.
// 
// 0 - Natsu, 481c57a8a5246b51a3f0a8b765d8021a2bfe329e94bb5a88eb0fd8c4cd443f4b702bcc2d2883b4c803e7e942bb09af384c285df5942af873160474abc80e5650, g8a7i, dragneelft@panda.com
// 1 - Happy, cf56d3374d228a9f3be996ae72a4e96208690a25c0bae3af648ee2dcc90110ad95afce77d585c2778945619e0294540fe205bb47de2025393691ff8bd70c685d, QsUf1, happyft@panda.com
// 2 - Gosu, 090d0640cc9c390f75e49d1511dd088b4da46092ec9efaf403b3066b50d847fba01274a7c7ed6e5b740fe4522809e029e2546561ef6c75bb52fbec350454bfe9, zGtZK, gosu@bronzev.noob
// 
// You have succesfully logged in.
// You have succesfully logged in.
// Wrong password, Try again.
// You have succesfully logged in.
// Couldn't find your account.