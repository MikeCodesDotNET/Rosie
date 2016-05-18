﻿// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rosie.Server
{
	public class LocalServer
	{
		public bool DebugMode { get; set; }
		public static LocalServer Shared { get; set; } = new LocalServer ();
		private readonly HttpListener _listener = new HttpListener();
		public const int DefaultWebServerPort = 8081;
		public LocalServer(int webServerPort = DefaultWebServerPort)
		{
			if (!HttpListener.IsSupported)
				throw new NotSupportedException("Http Listener is not supported");

			var prefixes = new [] {
				$"http://*:{webServerPort}/",
			};

			foreach (string s in prefixes) {
				Log ($"Listening on: {s}");
				_listener.Prefixes.Add (s);
			}
			init ();
			_listener.Start();
		}
		void init ()
		{
			RegisterRoutes ();
		}
		public virtual void RegisterRoutes()
		{
			
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		public void Start(int concurrentConnections = 4)
		{
			_listener.Start();
			Task.Run (() => {
				var sem = new Semaphore (concurrentConnections, concurrentConnections);
				while (_listener.IsListening) {
					sem.WaitOne ();
					_listener.GetContextAsync ().ContinueWith (async (t) => {
						HttpListenerContext ctx = null;
						try {

							sem.Release ();
							ctx = await t;
							await ProcessReponse (ctx);

						} catch (Exception ex) {
							Console.WriteLine (ex);
							if (ctx == null)
								return;
							ctx.Response.StatusCode = 500;
						} finally {
							ctx?.Response.OutputStream.Close ();
						}
					});
				}
			});
		}

		public void Stop ()
		{
			if (_listener?.IsListening ?? false)
				return;
			_listener.Stop ();
		}

		async Task ProcessReponse(HttpListenerContext context)
		{
			var request = context.Request;
			try{
				var path = request?.Url?.LocalPath;

				path = path?.TrimStart ('/');
				Log ($"Request from: {request.RemoteEndPoint.Address} Path: {path}");
				var route = Router.GetRoute(path);
				if(!route.SupportsMethod(context.Request.HttpMethod)) {
					context.Response.StatusCode = 405;
					return;
				}

				context.Response.ContentType = route.ContentType;
				await route.ProcessReponse (context);
			}
			catch(Exception ex) {
				Console.WriteLine (request.RawUrl);
				Console.WriteLine (ex);
				context.Response.StatusCode = 404;
				return;
			}
		}

		void Log (string message)
		{
			if (DebugMode)
				Console.WriteLine (message);
		}

	}
}