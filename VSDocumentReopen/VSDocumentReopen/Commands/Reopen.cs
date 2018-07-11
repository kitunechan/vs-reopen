﻿using System;
using System.ComponentModel.Design;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Documents;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Commands
{
	internal sealed class Reopen
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0101;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("d968b4de-3a69-4eb1-b676-942055da9dfd");

		private readonly AsyncPackage _package;
		private readonly DTE2 _dte;

		private Reopen(AsyncPackage package, OleMenuCommandService commandService, DTE2 dte)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));

			var menuCommandId = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(Execute, menuCommandId);
			commandService.AddCommand(menuItem);
		}

		public static Reopen Instance
		{
			get;
			private set;
		}

		private IAsyncServiceProvider ServiceProvider => _package;

		public static async Task InitializeAsync(AsyncPackage package, DTE2 dte)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new Reopen(package, commandService, dte);
		}

		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var document = DocumentHistory.Instance.GetLastClosed();
			if (document.IsValid())
			{
				_dte.ItemOperations.OpenFile(document.FullName, document.Kind);
			}
		}
	}
}