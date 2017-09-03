namespace JanHafner.Smartbar.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Markup;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    public static class Extensions
    {
        public static T FromXaml<T>(this Byte[] xaml)
        {
            using (var memoryStream = new MemoryStream(xaml))
            {
                return (T)XamlReader.Load(memoryStream);
            }
        }

        public static Byte[] ToXaml(this Object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                XamlWriter.Save(instance, memoryStream);

                return memoryStream.ToArray();
            }
        }

        [NotNull]
        public static IEnumerable<T> Overridden<T>([InstantHandle] [NotNull] this IEnumerable<T> handlers,
         [NotNull] T ignoredHandler)
        {
            if (handlers == null)
            {
                throw new ArgumentNullException(nameof(handlers));
            }

            if (ignoredHandler == null)
            {
                throw new ArgumentNullException(nameof(ignoredHandler));
            }

            return handlers.Where(handler => !handler.Equals(ignoredHandler)).Overridden();
        }

        [NotNull]
        public static IEnumerable<T> Overridden<T>([NotNull, InstantHandle] this IEnumerable<T> handlers)
        {
            if (handlers == null)
            {
                throw new ArgumentNullException(nameof(handlers));
            }

            var handlerList = handlers.ToList();
            var handlerTypes = handlerList.Select(handler => handler.GetType()).ToList();
            var handlerWithOverrideRequest =
              handlerTypes.Select(
                  handlerType => new
                  {
                      HandlerType = handlerType,
                      HandlerOverrideAttribute = handlerType.GetCustomAttribute<HandlerOverrideAttribute>()
                  }).Where(_ => _.HandlerOverrideAttribute != null);

            foreach (var overrideRequest in handlerWithOverrideRequest.ToList())
            {
                var applicationHandlerToOverride = handlerTypes.SingleOrDefault(
                    handlerType =>
                        handlerType.FullName.Equals(
                                overrideRequest.HandlerOverrideAttribute.FullHandlerType,
                                StringComparison.CurrentCultureIgnoreCase));
                if (applicationHandlerToOverride == null)
                {
                    continue;
                }

                handlerTypes.Remove(applicationHandlerToOverride);
            }

            return handlerTypes.Select(handlerType => handlerList.Single(handler => handler.GetType() == handlerType)).ToList();
        }

        public static async Task DispatchAsync([NotNull] this ICommandDispatcher commandDispatcher, params ICommand[] commands)
        {
            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            await commandDispatcher.DispatchAsync(commands.AsEnumerable());
        }
    }
}
