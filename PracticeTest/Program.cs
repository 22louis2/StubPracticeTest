using System;
using System.Collections.Generic;
using System.Linq;
namespace Viagogo
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class Solution
    {
        static Dictionary<string, int> _cachedDistanceStore = new Dictionary<string, int>();
        static Dictionary<string, List<Event>> _cachedClosestCityStore = new Dictionary<string, List<Event>>();

        static void Main(string[] args)
        {
            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
            };

            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            /* 
             * QUESTION 1. find out all events that are in cities of customer
             * then add to email.
             * we want you to send an email to this customer with all events in their city
             * Just call AddToEmail(customer, event) for each event you think they should get
             */
            events.Where(x => x.City.Contains(customer.City))?.ToList()?.ForEach(x => AddToEmail(customer, x));

            /*
             * QUESTION 2. find out the top 5 closest cities to the customer
             * and send an email to the customer(s)
             */
            events.OrderBy(x => GetDistance(customer.City, x.City))?.Take(5)?.ToList()?.ForEach(x => AddToEmail(customer, x));

            /*
             * QUESTION 3. to solve the problem of the GetDistance method, if it is an API call and could fail or is too expensive
             * how would you improve the code in question 2. Best possible way is to cache the closest cities result and first of all, 
             * search if the data, exists in the cached storage, before making the request to the GetDistance method
             * then send the email, to the customer(s)
             */
            List<Event> cachedClosestCityEvent = new List<Event>();

            if (_cachedClosestCityStore.ContainsKey(customer.City))
                cachedClosestCityEvent = _cachedClosestCityStore?.FirstOrDefault(x => x.Key == customer.City).Value;
            else
                _cachedClosestCityStore.Add(customer.City, cachedClosestCityEvent = events.OrderBy(x => GetDistance(customer.City, x.City))?.Take(5)?.ToList());

            cachedClosestCityEvent.ForEach(x => AddToEmail(customer, x));

            /*
             * QUESTION 4. to solve the problem of the GetDistance method, failing, and for which we don't want the process to fail.
             * we can cache the data in a store, by using the from and to city as key, then before making a request to the 
             * GetDistance method, search if it exists as a key in the cache store.
             */
            int getDistance = 0;

            events.ForEach(x => getDistance
            = _cachedDistanceStore.ContainsKey(x.City + customer?.City)
            ? _cachedDistanceStore.FirstOrDefault(y => y.Key == x.City + customer?.City).Value
            : GetDistance(customer.City, x.City)
            );

            /*
             * QUESTION 5. Tackling this in respect to sorting, I would order it by price, which would help determine
             * the ones to be sent to the customer, from my cached closest city event list
             */
            cachedClosestCityEvent.OrderBy(x => GetPrice(x));
        }

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }

        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }

        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }

        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
/*
var customers = new List<Customer>{
    new Customer{ Name = "Nathan", City = "New York"},
    new Customer{ Name = "Bob", City = "Boston"},
    new Customer{ Name = "Cindy", City = "Chicago"},
    new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/