using BobbaServer.Core.Logger;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace BobbaServer.Database.Redis
{
    class RedisManager
    {
        public static readonly ILogger Logger = LoggerManager.GetCurrentClassLogger(typeof(RedisManager));
        public RedisManager()
        {
            try
            {
                Logger.Debug("Loading Redis...");
                if (BobbaEnvironment.GetConfig().GetBool("redis.start.auto"))
                {
                    Process.Start("cmd.exe", "/C redis-server");
                }

                ConfigurationOptions Config = new ConfigurationOptions()
                {
                    AbortOnConnectFail = true,
                    ConnectTimeout = 3000,
                    ClientName = "BobbaServer",
                    EndPoints =
                        {
                            { BobbaEnvironment.GetConfig().GetString("redis.hostname"), BobbaEnvironment.GetConfig().GetInt("redis.port") }
                        },
                    Password = BobbaEnvironment.GetConfig().GetString("redis.password"),
                    SyncTimeout = BobbaEnvironment.GetConfig().GetInt("redis.sync.timeout"),
                };

                lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(Config);
                });
                Logger.Info("Redis Ready.");
            }
            catch (RedisConnectionException e)
            {
                Logger.Critical("Redis isn't connected to Redis Server.", e);
            }
            catch (Exception e)
            {
                Logger.Critical(e);
                throw;
            }
        }

        private readonly Lazy<ConnectionMultiplexer> lazyConnection;

        private ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public Redis GetConnection()
        {
            return new Redis(Connection.GetDatabase());
        }
    }
}
