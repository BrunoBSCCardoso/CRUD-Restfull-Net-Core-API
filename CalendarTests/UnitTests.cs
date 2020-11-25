using System;
using Xunit;
using CalendarWebApi.DataAccess;
using CalendarWebApi.DTO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using System.Text;
using FluentAssertions;

namespace CalendarTests
{
    public class UnitTests: IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public UnitTests()
        {
            SetUpClient();
        }

        public void Dispose()
        {

        }

        public async Task SeedData()
        {
            
            // Create entry with id 1 
            var createForm0 = GenerateCreateForm("Event1", 1576436425, "Miami", "John", "Any,Jay");
            var response0 = await Client.PostAsync("/Calendar/CreateEvent", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));

            // Create entry with id 2 
            var createForm1 = GenerateCreateForm("Event2", 1576436425, "Florida", "Sam", "Eric,Lupita");
            var response1 = await Client.PostAsync("/Calendar/CreateEvent", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));

            // Create entry with id 3 
            var createForm2 = GenerateCreateForm("Event3", 1576436606, "California", "Any", "John,Jay,Eric");
            var response2 = await Client.PostAsync("/Calendar/CreateEvent", new StringContent(JsonConvert.SerializeObject(createForm2), Encoding.UTF8, "application/json"));

            // Create entry with id 4 
            var createForm3 = GenerateCreateForm("Event4", 1576435425, "Seattle", "Maria", "Lupita,Amsy,Nick");
            var response3 = await Client.PostAsync("/Calendar/CreateEvent", new StringContent(JsonConvert.SerializeObject(createForm3), Encoding.UTF8, "application/json"));

            // Create entry with id 5
            var createForm4 = GenerateCreateForm("Event5", 1576535425, "Florida", "Nick", "Gene,Presa");
            var response4 = await Client.PostAsync("/Calendar/CreateEvent", new StringContent(JsonConvert.SerializeObject(createForm4), Encoding.UTF8, "application/json"));

        }

        //// TEST NAME - CreateCalendar
        //// TEST DESCRIPTION - A new event should be created
        [Fact]
        public async Task TestCase0()
        {
            await SeedData();

            // Create entry with id 6
            var createForm0 = GenerateCreateForm("Event6", 1577535425, "Miami", "Siby", "Jason,Brian,Dave");
            var response0 = await Client.PostAsync("/Calendar/CreateEvent", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));
            response0.StatusCode.Should().BeEquivalentTo(201);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("{\"id\":6,\"name\":\"Event6\",\"location\":\"Miami\",\"time\":1577535425,\"eventOrganizer\":\"Siby\",\"members\":\"Jason,Brian,Dave\"}");
            realData0.Should().BeEquivalentTo(expectedData0);

            // Create entry with id 7
            var createForm1 = GenerateCreateForm("Event7", 1573535425, "Redmond", "Albert", "Samuele,Matt");
            var response1 = await Client.PostAsync("/Calendar/CreateEvent", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));
            response1.StatusCode.Should().BeEquivalentTo(201);


            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            var expectedData1 = JsonConvert.DeserializeObject("{\"id\":7,\"name\":\"Event7\",\"location\":\"Redmond\",\"time\":1573535425,\"eventOrganizer\":\"Albert\",\"members\":\"Samuele,Matt\"}");
            realData1.Should().BeEquivalentTo(expectedData1);
        }

        // TEST NAME - GetCalendar
        // TEST DESCRIPTION - It finds all the events in a calendar
        [Fact]
        public async Task TestCase1()
        {
            await SeedData();
            
            // Get All events 
            var response0 = await Client.GetAsync("/Calendar/GetAllEvents");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"id\":1,\"name\":\"Event1\",\"location\":\"Miami\",\"time\":1576436425,\"eventOrganizer\":\"John\",\"members\":\"Any,Jay\"},{\"id\":2,\"name\":\"Event2\",\"location\":\"Florida\",\"time\":1576436425,\"eventOrganizer\":\"Sam\",\"members\":\"Eric,Lupita\"},{\"id\":3,\"name\":\"Event3\",\"location\":\"California\",\"time\":1576436606,\"eventOrganizer\":\"Any\",\"members\":\"John,Jay,Eric\"},{\"id\":4,\"name\":\"Event4\",\"location\":\"Seattle\",\"time\":1576435425,\"eventOrganizer\":\"Maria\",\"members\":\"Lupita,Amsy,Nick\"},{\"id\":5,\"name\":\"Event5\",\"location\":\"Florida\",\"time\":1576535425,\"eventOrganizer\":\"Nick\",\"members\":\"Gene,Presa\"}]");
            realData0.Should().BeEquivalentTo(expectedData0);

        }

        //// TEST NAME - getSingleEntryById
        //// TEST DESCRIPTION - It finds single event by ID
        [Fact]
        public async Task TestCase2()
        {
            await SeedData();

            // Get Single event By ID 
            var response0 = await Client.GetAsync("/Calendar/GetEventByID?id=5");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"id\":5,\"name\":\"Event5\",\"location\":\"Florida\",\"time\":1576535425,\"eventOrganizer\":\"Nick\",\"members\":\"Gene,Presa\"}]");
            realData0.Should().Equals(expectedData0);

            // Get Single event By ID which does not exist should return empty array
            var response1 = await Client.GetAsync("/Calendar/GetEventByID?id=9");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        //// TEST NAME - getEventsByLocation
        //// TEST DESCRIPTION - It finds events by location
        [Fact]
        public async Task TestCase3()
        {
            await SeedData();

            // Get Single event By ID 
            var response0 = await Client.GetAsync("Calendar/GetEventsByLocation?location=Miami");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"id\":1,\"name\":\"Event1\",\"location\":\"Miami\",\"time\":1576436425,\"eventOrganizer\":\"John\",\"members\":\"Any,Jay\"}]");
            realData0.Should().Equals(expectedData0);

            // Get Single event By ID which does not exist should return empty array
            var response1 = await Client.GetAsync("Calendar/GetEventsByLocation?location=abc");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        //// TEST NAME - getEventsByEventOrganizer
        //// TEST DESCRIPTION - It finds events by Organizer
        [Fact]
        public async Task TestCase4()
        {
            await SeedData();

            // Get Single event By Organizer 
            var response0 = await Client.GetAsync("Calendar/GetEventsByEventOrganizer?eventOrganizer=Sam");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"id\":2,\"name\":\"Event2\",\"location\":\"Florida\",\"time\":1576436425,\"eventOrganizer\":\"Sam\",\"members\":\"Eric,Lupita\"}]");
            realData0.Should().Equals(expectedData0);

            // Get Single event By ID which does not exist should return empty array
            var response1 = await Client.GetAsync("Calendar/GetEventsByEventOrganizer?eventOrganizer=Rake");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        //// TEST NAME - getEventsByName
        //// TEST DESCRIPTION - It finds events by name
        [Fact]
        public async Task TestCase5()
        {
            await SeedData();

            // Get Single event By ID 
            var response0 = await Client.GetAsync("Calendar/GetEventsByName?name=Event1");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"id\":1,\"name\":\"Event1\",\"location\":\"Miami\",\"time\":1576436425,\"eventOrganizer\":\"John\",\"members\":\"Any,Jay\"}]");
            realData0.Should().Equals(expectedData0);

            // Get Single event By ID which does not exist should return empty array
            var response1 = await Client.GetAsync("Calendar/GetEventsByName?name=abcd");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        //// TEST NAME - checkNonExistentApi
        //// TEST DESCRIPTION - It should check if an API exists
        [Fact]
        public async Task TestCase6()
        {
            await SeedData();

            // Return with 404 if no API path exists 
            var response0 = await Client.GetAsync("/Calendar/GetEventByTime?time=123");
            response0.StatusCode.Should().BeEquivalentTo(404);

            // Return with 405 if API path exists but called with different method
            var response1 = await Client.GetAsync("/Calendar/CreateEvent");
            response1.StatusCode.Should().BeEquivalentTo(405);
        }

        // TEST NAME - getSortedCalendar
        // TEST DESCRIPTION - It finds the sorted calendar in descending order
        [Fact]
        public async Task TestCase7()
        {
            await SeedData();

            // Get events sorted
            var response0 = await Client.GetAsync("/Calendar/GetSortedCalendar");
            response0.StatusCode.Should().BeEquivalentTo(200);
            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"id\":5,\"name\":\"Event5\",\"location\":\"Florida\",\"time\":1576535425,\"eventOrganizer\":\"Nick\",\"members\":\"Gene,Presa\"},{\"id\":3,\"name\":\"Event3\",\"location\":\"California\",\"time\":1576436606,\"eventOrganizer\":\"Any\",\"members\":\"John,Jay,Eric\"},{\"id\":1,\"name\":\"Event1\",\"location\":\"Miami\",\"time\":1576436425,\"eventOrganizer\":\"John\",\"members\":\"Any,Jay\"},{\"id\":2,\"name\":\"Event2\",\"location\":\"Florida\",\"time\":1576436425,\"eventOrganizer\":\"Sam\",\"members\":\"Eric,Lupita\"},{\"id\":4,\"name\":\"Event4\",\"location\":\"Seattle\",\"time\":1576435425,\"eventOrganizer\":\"Maria\",\"members\":\"Lupita,Amsy,Nick\"}]");
            realData0.Should().BeEquivalentTo(expectedData0);
        }

        // TEST NAME - updateEvent
        // TEST DESCRIPTION - Update events details
        [Fact]
        public async Task TestCase8()
        {
            await SeedData();

            // Return with 204 if event is updated 
            var body0 = JsonConvert.DeserializeObject("{\"name\":\"ChangedEvent3\",\"location\":\"Alaska\"}");
            var response0 = await Client.PutAsync("/Calendar/UpdateEvent?id=3", new StringContent(JsonConvert.SerializeObject(body0), Encoding.UTF8, "application/json"));
            response0.StatusCode.Should().Be(204);

            //Check if the event is updated
            var response1 = await Client.GetAsync("/Calendar/GetEventByID?id=3");
            response1.StatusCode.Should().BeEquivalentTo(200);

            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            var expectedData1 = JsonConvert.DeserializeObject("[{\"id\":3,\"name\":\"ChangedEvent3\",\"location\":\"Alaska\",\"time\":1576436606,\"eventOrganizer\":\"Any\",\"members\":\"John,Jay,Eric\"}]");
            realData1.Should().Equals(expectedData1);
        }

        // TEST NAME - deleteEvent
        // TEST DESCRIPTION - Delete an event by id
        [Fact]
        public async Task TestCase9()
        {
            await SeedData();

            // Return with 204 if event is deleted
            var response0 = await Client.DeleteAsync("/Calendar/DeleteEvent?id=3");
            response0.StatusCode.Should().Be(204);

            // Check if the event does not exist
            var response2 = await Client.GetAsync("/Calendar/GetEventByID?id=3");
            response2.StatusCode.Should().BeEquivalentTo(200);
            var realData2 = JsonConvert.DeserializeObject(response2.Content.ReadAsStringAsync().Result);
            realData2.Should().Equals("[]");
        }

        private CreateForm GenerateCreateForm(string name, long time, string location, string eventOrganizer, string members)
        {
            return new CreateForm()
            {
                Location = location,
                Name = name,
                EventOrganizer = eventOrganizer,
                Members = members,
                Time = time
            };
        }

        private void SetUpClient()
        {

            var builder = new WebHostBuilder()
                .UseStartup<CalendarWebApi.Startup>()
                .ConfigureServices(services =>
                {
                    var context = new CalendarContext(new DbContextOptionsBuilder<CalendarContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(CalendarContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);
            
            Client = _server.CreateClient();
        }
    }
}
