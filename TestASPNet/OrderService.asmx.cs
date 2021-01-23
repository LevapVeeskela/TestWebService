using Microsoft.Extensions.Caching.Memory;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Services;
using Serilog.Core;
using Serilog.Events;
using TestASPNet.Enums;
using TestASPNet.Models.DB;
using static TestASPNet.Constants.Constants;
using TestASPNet.Extensions;

namespace TestASPNet
{
    /// <summary>
    /// OrderModelDBService for test task
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class OrderService : System.Web.Services.WebService
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings[Defaults.ConnectionString].ConnectionString;
        private readonly Logger _logger;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public OrderService(IMemoryCache memoryCache, Logger logger)
        {
            _logger = logger; // need implementation, that demonstration only
            _cache = memoryCache;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(100)
                .SetSlidingExpiration(TimeSpan.FromSeconds(60));
        }

        [WebMethod]
        public async Task<OrderModelDb> GetOrderInfo(string orderCode) // change code - is bad practice give away DB model(need view model), but it is for example only
        {
            const int firstIndex = 0;
            const int secondIndex = 1;
            const int thordIndex = 2;
            const int fourthIndex = 3;

            var stopWatch = Stopwatch.StartNew(); // change code

            try
            {
                Debug.Assert(!string.IsNullOrEmpty(orderCode)); // change code

                var orderFromCache = _cache.Get<string, OrderModelDb>(orderCode); // change code
                if (orderFromCache != null)
                {
                    stopWatch.Stop();
                    _logger.Write(LogEventLevel.Information, CommonTemplates.Elapsed, stopWatch.Elapsed); // change code

                    return orderFromCache;
                }

                using (SqlConnection connection = new SqlConnection(_connStr)) // change code - that must happened in data layer but I am lazy, so I don't divide by layers(business and data)
                {
                    await connection.OpenAsync(); // change code

                    var query = QueryTemplates.GetOrderInfo.Query(orderCode); // change code
                    var command = new SqlCommand(query, connection); // change code

                    using (SqlDataReader reader = await command.ExecuteReaderAsync()
                    ) // change code(but I don't understand why we use SqlDataReader, that is bad for deserialize!? but ok)
                    {
                        if (reader.HasRows) // change code
                        {
                            var order = new OrderModelDb((int)reader.GetValue(firstIndex), (int)reader.GetValue(secondIndex),
                                 (float)reader.GetValue(thordIndex), (DateTime)reader.GetValue(fourthIndex));

                            _cache.Set<OrderModelDb>(orderCode, order, _cacheEntryOptions);

                            stopWatch.Stop();
                            _logger.Write(LogEventLevel.Information, CommonTemplates.Elapsed,
                                stopWatch.Elapsed); // changed code

                            return order;
                        }
                    }
                }

                stopWatch.Stop();
                _logger.Write(LogEventLevel.Information, CommonTemplates.Elapsed, stopWatch.Elapsed); // changed code

                return null;
            }
            catch (SqlException ex)
            {
                stopWatch.Stop();
                _logger.Write(LogEventLevel.Information, CommonTemplates.Elapsed, stopWatch.Elapsed); // changed code
                _logger.Write(LogEventLevel.Error, CommonTemplates.Exception, ex.Message, ex.StackTrace); // changed code
                throw new ApplicationException(LogType.Error.ToString()); // changed code
            }
        }
    }
}
