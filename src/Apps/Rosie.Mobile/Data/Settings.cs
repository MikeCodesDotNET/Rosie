﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Rosie
{
	public static class Settings
	{
		public static string ApiKey {
			get { return GetString ("ApiKey"); }
			set { SetString (value);}
		}
		public static string ApiUrl {
			get { return GetString (); }
			set { SetString (value); }
		}

		public static Usage CurrentUsage {
			get { return (Usage)GetInt ((int)Usage.Medium);}
			set { SetInt ((int)value); }
		}

		#region Helpers
		public static ISettings AppSettings { get; } = CrossSettings.Current;

		public static string GetString (string defaultValue = "", [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetString (string value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<string> (memberName, value);
		}
		public static int GetInt (int defaultValue = 0, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetInt (int value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<int> (memberName, value);
		}

		public static long GetLong (long defaultValue = 0, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetLong (long value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<long> (memberName, value);
		}

		public static bool GetBool (bool defaultValue = false, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetBool (bool value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<bool> (memberName, value);
		}


		static T Get<T> (T defaultValue, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		static void Set<T> (T value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<T> (memberName, value);
		}
		#endregion
	}
}


