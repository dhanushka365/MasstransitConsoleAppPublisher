using MassTransit;

namespace MasstransitConsoleAppPublisher
{
    public class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                while(true)
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine(value: "Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    await busControl.Publish<ValueEntered>(new
                    {
                        Value = value
                    });
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
            
        }
    }
}

