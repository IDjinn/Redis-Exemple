using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BobbaServer.Database.Redis
{
    public class Redis : IDisposable
    {
        BinaryFormatter BinaryFormatter = new BinaryFormatter();
        MemoryStream MemoryStream;
        IDatabase Database;
        public Redis(IDatabase Database)
        {
            this.Database = Database;
        }
        public T Get<T>(object Class, object Key)
        {
            return Deserialize<T>(Database.StringGet($"{Class.GetType().Namespace}.{Key.ToString()}"));
        }
        public T Get<T>(object Key)
        {
            return Deserialize<T>(Database.StringGet($"{typeof(T).Namespace}.{Key.ToString()}"));
        }
        public async Task<T> GetAsync<T>(object Class, object Key)
        {
            return Deserialize<T>(await Database.StringGetAsync($"{Class.GetType().Namespace}.{Key.ToString()}"));
        }
        public async Task<object> GetAsync(object Class, object Key)
        {
            return Deserialize<object>(await Database.StringGetAsync($"{Class.GetType().Namespace}.{Key.ToString()}"));
        }
        public async Task<bool> SetAsync(object Class, object Key, object Value)
        {
            return await Database.StringSetAsync($"{Class.GetType().Namespace}.{Key.ToString()}", Serialize(Value));
        }
        public async Task<bool> SetAsync(Type Class, object Key, object Value)
        {
            return await Database.StringSetAsync($"{Class.Namespace}.{Key.ToString()}", Serialize(Value));
        }
        public void Set(object Class, object Key, object Value)
        {
            Database.StringSet($"{Class.GetType().Namespace}.{Key.ToString()}", Serialize(Value));
        }
        public bool KeyExistis(object Class, object Key)
        {
            return Database.KeyExists($"{Class.GetType().Namespace}.{Key.ToString()}");
        }
        public bool KeyExistis(Type Class, object Key)
        {
            return Database.KeyExists($"{Class.Namespace}.{Key.ToString()}");
        }
        public async Task<bool> KeyExistisAsync(object Class, object Key)
        {
            return await Database.KeyExistsAsync($"{Class.GetType().Namespace}.{Key.ToString()}");
        }

        public byte[] Serialize(object o)
        {
            if (o != null)
            {
                using (MemoryStream = new MemoryStream())
                {
                    BinaryFormatter.Serialize(MemoryStream, o);
                    return MemoryStream.ToArray();
                }
            }

            return null;
        }

        public T Deserialize<T>(byte[] stream)
        {
            if (stream != null)
            {
                using (MemoryStream = new MemoryStream(stream))
                {
                    return (T)BinaryFormatter.Deserialize(MemoryStream);
                }
            }

            return default;
        }

        public void Dispose()
        {
            BinaryFormatter = null;
            if (MemoryStream != null)
            {
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            Database = null;
            GC.SuppressFinalize(this);
        }
    }
}
