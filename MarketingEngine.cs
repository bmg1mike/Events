namespace AndelaEvent;

public class MarketingEngine
{
    public record City(string Name, int X, int Y);
    private readonly List<Event> events;
    private Dictionary<string, int> cache = new Dictionary<string, int>();

    public MarketingEngine(List<Event> events)
    {
        this.events = events;
    }

    public void SendCustomerNotifications(Customer customer, Event e)
    {
        // send customer notifications for Birthday
        NextEventsCloseToCustomerBirthday(customer,events);
        FiveClosestEvents(customer);

        Console.WriteLine($"{customer.Name} from {customer.City} event {e.Name} at {e.Date}");


    }

    public List<Event> NextEventsCloseToCustomerBirthday(Customer customer, List<Event> allEvents)
    {

        var events = allEvents.Where(x => x.Date.Month == customer.BirthDate.Month && x.Date > DateTime.Now).ToList();


        return events;
    }

    public List<Event> FiveClosestEvents(Customer customer)
    {
        var eventList = new List<Event>();

        var customerCityInfo = Cities.Where(c => c.Key == customer.City).Single().Value;
        if (cache.Count == 0)
        {
            foreach (var e in events)
            {
                var eventCityInfo = Cities.Where(c => c.Key == e.City).Single().Value;

                var distance = Math.Abs(customerCityInfo.X - eventCityInfo.X) + Math.Abs(customerCityInfo.Y - eventCityInfo.Y);
                cache.Add(e.City, distance);
                if (distance <= 1) // all events less than or equal to 1 mile distance
                {
                    eventList.Add(e);
                }
            }
        }
        else
        {
            foreach (var e in events)
            {
                if(cache.ContainsKey(e.City))
                {
                    if(cache[e.City] <= 1)
                    {
                        eventList.Add(e);
                    }
                }
            }
        }



        return eventList.OrderBy(x => x.Date).Take(5).ToList();
    }


    public static readonly IDictionary<string, City> Cities = new Dictionary<string, City>()
    {
        { "New York", new City("New York", 3572, 1455) },
        { "Los Angeles", new City("Los Angeles", 462, 975) },
        { "San Francisco", new City("San Francisco", 183, 1233) },
        { "Boston", new City("Boston", 3778, 1566) },
        { "Chicago", new City("Chicago", 2608, 1525) },
        { "Washington", new City("Washington", 3358, 1320) },
    };


}