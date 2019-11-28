using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RedisExemple
{
    public static class Redis
    {
        static Redis()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("localhost");
            });
        }

        private static readonly Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public static IDatabase GetDatabase()
        {
            return Connection.GetDatabase();
        }

        public static T Get<T>(object Key)
        {
            return Deserialize<T>(GetDatabase().StringGet(Key.ToString()));
        }

        public static async Task<T> GetAsync<T>(object Key)
        {
            return Deserialize<T>(await GetDatabase().StringGetAsync(Key.ToString()));
        }

        public static async Task<object> GetAsync(object Key)
        {
            return Deserialize<object>(await GetDatabase().StringGetAsync(Key.ToString()));
        }

        public static async Task<bool> SetAsync(object Key, object Value)
        {
            return await GetDatabase().StringSetAsync(Key.ToString(), Serialize(Value));
        }

        public static void Set(object Key, object Value)
        {
            GetDatabase().StringSet(Key.ToString(), Serialize(Value));
        }

        public static async Task<bool> KeyExistis(object Key)
        {
            return await GetDatabase().KeyExistsAsync(Key.ToString());
        }

        static byte[] Serialize(object o)
        {
            byte[] objectDataAsStream = null;

            if (o != null)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, o);
                    objectDataAsStream = memoryStream.ToArray();
                }
            }

            return objectDataAsStream;
        }

        static T Deserialize<T>(byte[] stream)
        {
            T result = default;

            if (stream != null)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream(stream))
                {
                    result = (T)binaryFormatter.Deserialize(memoryStream);
                }
            }

            return result;
        }
    }
}
