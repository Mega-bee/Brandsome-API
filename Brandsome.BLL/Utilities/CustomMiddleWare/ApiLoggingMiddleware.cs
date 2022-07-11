using Brandsome.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities.CustomMiddleWare
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly BrandsomeDbContext _context;
        //private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;


        public ApiLoggingMiddleware(RequestDelegate next/*, RecyclableMemoryStreamManager recyclableMemoryStreamManager*/)
        {
            _next = next;
            //_recyclableMemoryStreamManager = recyclableMemoryStreamManager;

        }

        public async Task Invoke(HttpContext context,BrandsomeDbContext db, RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            try
            {
                ApiDataLogging ApiLoggingModel = new ApiDataLogging();
                //First, get the incoming request
                //var request = await FormatRequest(context.Request);
                await LogRequest(context, ApiLoggingModel,recyclableMemoryStreamManager);
                //await using var requestStream = _recyclableMemoryStreamManager.GetStream();
                //await context.Request.Body.CopyToAsync(requestStream);
                //context.Request.Body.Position = 0;
                //Copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                //Create a new memory stream...
                using var responseBody = new MemoryStream();
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                var watch = new Stopwatch();
                watch.Start();
                //context.Response.OnStarting(() => {
                //    // Stop the timer information and calculate the time   
                //    watch.Stop();
                //    var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
                //    // Add the Response time information in the Response headers.   
                   
                //    return Task.CompletedTask;
                //});
                await _next(context);

                //Format the response from the server
                var response = await FormatResponse(context.Response);

                //TODO: Save log to chosen datastore
                ApiLoggingModel.CreatedDate = DateTime.UtcNow;
                ApiLoggingModel.JsonResponse = response;
                ApiLoggingModel.ProfileId = context.User.Claims.Where(x => x.Type == "UID").Select(x => x.Value).FirstOrDefault();
                await db.ApiDataLoggings.AddAsync(ApiLoggingModel);
                await db.SaveChangesAsync();
                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
                //ApiLoggingModel.ProfileId = context.User.Claims.Where(x => x.Type == "UID").Select(x => x.Value).FirstOrDefault();
                //await _next(context);
                //await LogRequest(context);
                //await LogResponse(context);

                //await _context.ApiDataLoggings.AddAsync(ApiLoggingModel);
                //await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            // reset the stream position to 0, which is allowed because of EnableBuffering()
            request.Body.Seek(0, SeekOrigin.Begin);

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $" {text}";
        }

        private async Task LogRequest(HttpContext context, ApiDataLogging ApiLoggingModel , RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            context.Request.EnableBuffering();
            //string request = context.Request.Form.ToString();
            Dictionary<string, string> param = new Dictionary<string, string>();
            
            if(context.Request.HasFormContentType && context.Request.Form != null && context.Request.Form.Keys.Count > 0)
            {
                    foreach (var key in context.Request.Form.Keys)
                    {
                        param.Add(key, context.Request.Form[key]);
                    }
            }

            //await using var requestStream = recyclableMemoryStreamManager.GetStream();
            //await context.Request.Body.CopyToAsync(requestStream);
            //_logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
            //                       $"Schema:{context.Request.Scheme} " +
            //                       $"Host: {context.Request.Host} " +
            //                       $"Path: {context.Request.Path} " +
            //                       $"QueryString: {context.Request.QueryString} " +
            //                       $"Request Body: {ReadStreamInChunks(requestStream)}");
            ApiLoggingModel.ApiName = context.Request.Path;
            
            //ApiLoggingModel.ApiParameters = ReadStreamInChunks(requestStream);
            ApiLoggingModel.ApiParameters = param.Count > 0 ? JsonConvert.SerializeObject(param) : "";
            ApiLoggingModel.QueryParameters = context.Request.QueryString.Value;
            ApiLoggingModel.Imei = context.Request.Headers["imei"];
            ApiLoggingModel.Method = context.Request.Method;
            ApiLoggingModel.CreatedDate = DateTime.UtcNow;
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

    }
}
