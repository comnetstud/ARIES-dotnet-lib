using System;
using System.IO;
using System.Net;
using System.Windows;
using Aries.Test;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace Aries.MockServer.Test
{
    public class AriesWebController : WebApiController
    {
        private static readonly string SIMULATION = @"\Resources\TestsForMockServerAriesHelper\simulation.json";
        private static readonly string AGENT = @"\Resources\TestsForMockServerAriesHelper\agents.json";
        private static readonly string SIMULATIONSTEP = @"\Resources\TestsForMockServerAriesHelper\simulationstep.json";

        [WebApiHandler(HttpVerbs.Get, "/api/aries/simulation/active")]
        public bool GetSimulation(WebServer server, HttpListenerContext context)
        {
            try
            {
                var bytes = File.ReadAllBytes(TestHelper.GetBinPath(SIMULATION));
                WriteDataToResponse(bytes, context);
                return true;
            }
            catch (Exception ex)
            {
                return context.JsonExceptionResponse(ex);
            }
        }

        [WebApiHandler(HttpVerbs.Get, "/api/aries/simulationstep/active")]
        public bool GetSimulationStep(WebServer server, HttpListenerContext context)
        {
            try
            {
                var bytes = File.ReadAllBytes(TestHelper.GetBinPath(SIMULATIONSTEP));
                WriteDataToResponse(bytes, context);
                return true;
            }
            catch (Exception ex)
            {
                return context.JsonExceptionResponse(ex);
            }
        }

        [WebApiHandler(HttpVerbs.Get, "/api/aries/agent/simulationstep/{simulationStepId}")]
        public bool GetAgents(WebServer server, HttpListenerContext context, string simulationStepId)
        {
            try
            {
                var bytes = File.ReadAllBytes(TestHelper.GetBinPath(AGENT));
                WriteDataToResponse(bytes, context);
                return true;
            }
            catch (Exception ex)
            {
                return context.JsonExceptionResponse(ex);
            }
        }

        [WebApiHandler(HttpVerbs.Post, "/api/aries/state")]
        public bool PostState(WebServer server, HttpListenerContext context)
        {
            try
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string userJson = reader.ReadToEnd();
                TestHelper.WriteResult(userJson, "state");
                return true;
            }
            catch (Exception ex)
            {
                return context.JsonExceptionResponse(ex);
            }
        }

        [WebApiHandler(HttpVerbs.Post, "/api/aries/cluster")]
        public bool PostCluster(WebServer server, HttpListenerContext context)
        {
            try
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string userJson = reader.ReadToEnd();
                TestHelper.WriteResult(userJson, "cluster");
                return true;
            }
            catch (Exception ex)
            {
                return context.JsonExceptionResponse(ex);
            }
        }

        private void WriteDataToResponse(byte[] bytes, HttpListenerContext context)
        {
            HttpListenerResponse Response = context.Response;
            Response.ContentType = "application/json";
            Response.ContentLength64 = bytes.Length;
            Stream OutputStream = Response.OutputStream;
            OutputStream.Write(bytes, 0, bytes.Length);
            OutputStream.Close();
        }

        public override void SetDefaultHeaders(HttpListenerContext context) => context.NoCache();
    }
}