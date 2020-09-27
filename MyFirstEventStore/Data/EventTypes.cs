using System;
using System.Linq;
using MyFirstEventStore.Interfaces;

namespace MyFirstEventStore.Data
{
    public static class EventTypes
    {
        private static readonly Type[] Types;
        
        static EventTypes()
        {
            Types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.ExportedTypes)
                .Where(a => a
                    .GetInterfaces()
                    .Any(b=>b.IsGenericType && b.GetGenericTypeDefinition() == typeof(IEvent<>)))
                .ToArray();
        }

        public static Type FindEventType(string fullname)
        {
            return Types.FirstOrDefault(x => x.FullName == fullname);
        }
    }
}