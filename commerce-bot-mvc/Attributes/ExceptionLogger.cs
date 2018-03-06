using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;


namespace commerce_bot_mvc.Attributes
{
    public class BotExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context?.Exception != null)
            {
                // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
                var ai = new TelemetryClient();
                ai.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}