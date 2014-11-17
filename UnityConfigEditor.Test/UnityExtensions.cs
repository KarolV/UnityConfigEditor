using System;

using Microsoft.Practices.Unity;

namespace UnityConfigEditor.Test
{
	public static class UnityExtensions
	{
		public static string GetMappingAsString(
			this ContainerRegistration registration)
		{
			var r = registration.RegisteredType;
			var regType = r.Name + GetGenericArgumentsList(r);

			var m = registration.MappedToType;
			var mapTo = m.Name + GetGenericArgumentsList(m);

			var regName = registration.Name ?? "[default]";

			var lifetime = registration.LifetimeManagerType.Name;
			if (mapTo != regType)
				mapTo = " -> " + mapTo;
			else
				mapTo = string.Empty;

			lifetime = lifetime.Substring(0, lifetime.Length - "LifetimeManager".Length);

			return String.Format("+ {0}{1}  '{2}'  {3}",
			                     regType,
			                     mapTo,
			                     regName,
			                     lifetime);
		}

		private static string GetGenericArgumentsList(Type type)
		{
			if (type.GetGenericArguments().Length == 0) return string.Empty;
			var arglist = string.Empty;
			var first = true;
			foreach (var t in type.GetGenericArguments())
			{
				arglist += first
					? t.Name
					: ", " + t.Name;
				first = false;
				if (t.GetGenericArguments().Length > 0)
					arglist += GetGenericArgumentsList(t);
			}
			return "<" + arglist + ">";
		}
	}
}