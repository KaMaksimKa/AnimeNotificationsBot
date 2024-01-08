using AnimeNotificationsBot.Api.Commands.Base;
using System.Reflection;

var assembly = typeof(TelegramCommand).Assembly;

Type baseType = typeof(CallbackCommand);

var derivedTypes = assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t != baseType && !t.IsAbstract);

Console.WriteLine($"Классы, наследующиеся от {baseType.Name} в сборке {assembly.FullName}:");
foreach (Type type in derivedTypes)
{
    Console.WriteLine(type.FullName);
    foreach (ParameterInfo parameter in type.GetConstructors().First().GetParameters())
    {
        Console.WriteLine($"{parameter.ParameterType} {parameter.Name}");
    }
    Console.WriteLine();
}