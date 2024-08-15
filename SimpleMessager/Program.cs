// See https://aka.ms/new-console-template for more information


using SimpleMessager;

var eventAggregator = EventAggregator.Instance;


Console.WriteLine("Hello, World!");
eventAggregator.Register("fdas",
    (object rec,string message) =>
    {
        Console.WriteLine($"{rec} Message received from {rec.GetType().Name}: {message}");
    });

eventAggregator.Send("hello");