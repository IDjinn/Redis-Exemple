using RedisExemple;
using System;

namespace RedisExemple
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("This is a simple example of how to use Redis in C#. Let's go try set one key: ");
            string Key = Console.ReadLine();
            Console.WriteLine($"Okay, now set the value of key {Key}: ");
            string Value = Console.ReadLine();
            Redis.Set(Key, Value);
            Console.WriteLine($"Now... The key '{Key}' has been set with value '{Value}'.");
            Console.WriteLine($"Trying to get the value of key '{Key}'...");
            Value = Redis.Get<string>(Key);
            Console.WriteLine($"The value of the key '{Key}' is '{Value}'\n\n\n");

            Console.WriteLine($"Now, we'll try set the complex object...");
            Redis.Set("complex-object", new MyObject(true));
            Console.WriteLine("The complex object has been set in the redis. Let's go try to get them.");
            MyObject MyObj = Redis.Get<MyObject>("complex-object");
            Console.WriteLine($"Sucess! Int32 max value is: {MyObj.Int32.ToString()}");
            Console.ReadKey();
        }
    }
}
