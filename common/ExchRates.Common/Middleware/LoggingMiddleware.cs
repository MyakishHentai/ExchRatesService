using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.Common.Middleware
{
    public class LoggingMiddleware : IMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await LogRequestAsync(context.Request);
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{DateTime.Now}]:Возникла ошибка при " +
                                     $"выполнения действия {context.Request.Path}");
                throw;
            }
            finally
            {
                await LogResponseAsync(context.Response);
            }
        }

        public async Task LogRequestAsync(HttpRequest request)
        {
            try
            {
                await Task.Run(() =>
                {
                    _logger.LogInformation($"[{DateTime.Now}]:HttpRequest(Запрос):\r\n" +
                                           $"Method(Метод): {request.Method}\r\n" +
                                           $"Host(Хост): {request.Host}\r\n" +
                                           $"Path(Путь): {request.Path}\r\n" +
                                           $"Query(Переменные): {request.QueryString}\r\n");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось записать данные запроса в лог.");
            }
        }

        public async Task LogResponseAsync(HttpResponse response)
        {
            try
            {
                await Task.Run(() =>
                {
                    _logger.LogInformation($"[{DateTime.Now}]:HttpResponse(Ответ):\r\n" +
                                           $"Status(Код ответа): {response.StatusCode}\r\n" +
                                           $"ContentType(Тип содежимого): {response.ContentType}\r\n");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось записать данные ответа в лог.");
            }
        }
    }
}
