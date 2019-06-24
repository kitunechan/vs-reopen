﻿using System;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace VSDocumentReopen.Infrastructure.Logging.Logentries
{
	public sealed class LogentriesSerilogLogger : ILogger
	{

		public LogentriesSerilogLogger() {
			_log = new LoggerConfiguration()
			.MinimumLevel.Information()
			.Enrich.FromLogContext()
			.Enrich.WithMachineName()
			.Enrich.WithEnvironmentUserName()
			.Enrich.WithExceptionDetails()
			.Enrich.With<VisualStudioVersionEnricher>()
			.Enrich.With<ExtensionVersionEnricher>()
			.Enrich.With<MemoryInfoEnricher>()
			.WriteTo.Logentries("token", new JsonFormatter(renderMessage: true))
			.CreateLogger();
		}

		private Logger _log;

		public void Trace(string message)
		{
			_log.Debug(message);
		}

		public void Info(string message)
		{
			_log.Information(message);
		}

		public void Warning(string message)
		{
			_log.Warning(message);
		}

		public void Warning(string message, Exception exception)
		{
			_log.Warning(exception, message);
		}

		public void Error(string message)
		{
			_log.Error(message);
		}

		public void Error(string message, Exception exception)
		{
			_log.Error(exception, message);
		}

		public void Critical(string message)
		{
			_log.Fatal(message);
		}

		public void Critical(string message, Exception exception)
		{
			_log.Fatal(exception, message);
		}
	}
}