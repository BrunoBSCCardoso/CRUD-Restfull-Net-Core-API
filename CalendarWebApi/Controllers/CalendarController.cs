using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalendarWebApi.DataAccess;
using CalendarWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CalendarWebApi.DTO;
using Newtonsoft.Json;
using Model = CalendarWebApi.Models;
using AutoMapper;

namespace CalendarWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Calendar : ControllerBase
    {
        private Repository _repository;
        private IMapper _mapper;

        public Calendar(CalendarContext context, IMapper mapper)
        {
            _repository = new Repository(context);
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateEvent")]
        public IActionResult CreateEvent([FromForm] CreateForm objectInput)
        {
            Model.Calendar newEvent = new Model.Calendar();

            var result = _repository.AddEvent(_mapper.Map<Model.Calendar>(objectInput)).ConfigureAwait(false);

            newEvent = result.GetAwaiter().GetResult();

            return Created(Uri.UriSchemeHttps, newEvent);
        }

        [HttpGet]
        [Route("GetAllEvents")]
        public IActionResult GetAllEvents()
        {
            List<Model.Calendar> calendarCollection = new List<Model.Calendar>();

            var result =  _repository.GetCalendar().ConfigureAwait(false);

            calendarCollection = result.GetAwaiter().GetResult();

            return Ok(calendarCollection);
        }

        [HttpGet]
        [Route("GetSortedCalendar")]
        public IActionResult GetSortedCalendar()
        {
            List<Model.Calendar> calendarCollection = new List<Model.Calendar>();

            var result = _repository.GetEventsSorted().ConfigureAwait(false);

            calendarCollection = result.GetAwaiter().GetResult();

            return Ok(calendarCollection);
        }

        [HttpGet]
        [Route("GetEventByID")]
        public IActionResult GetEventByID([FromQuery]int id)
        {
            Model.Calendar eventFound = new Model.Calendar();

            var result = _repository.GetCalendarByID(id).ConfigureAwait(false);

            eventFound = result.GetAwaiter().GetResult();

            if(eventFound != null)
            {
                return Ok(eventFound);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("GetEventsByLocation")]
        public IActionResult GetEventsByLocation([FromQuery] string location)
        {
            List<Model.Calendar> events = new List<Model.Calendar>();

            var result = _repository.GetCalendar(new EventQueryModel { Location = location }).ConfigureAwait(false);

            events = result.GetAwaiter().GetResult();

            if (events.Count > 0)
            {
                return Ok(events);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("GetEventsByEventOrganizer")]
        public IActionResult GetEventsByEventOrganizer([FromQuery] string eventOrganizer)
        {
            List<Model.Calendar> events = new List<Model.Calendar>();

            var result = _repository.GetCalendar(new EventQueryModel { EventOrganizer = eventOrganizer }).ConfigureAwait(false);

            events = result.GetAwaiter().GetResult();

            if (events.Count > 0)
            {
                return Ok(events);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("GetEventsByName")]
        public IActionResult GetEventsByName([FromQuery] string name)
        {
            List<Model.Calendar> events = new List<Model.Calendar>();

            var result = _repository.GetCalendar(new EventQueryModel { Name = name }).ConfigureAwait(false);

            events = result.GetAwaiter().GetResult();

            if (events.Count > 0)
            {
                return Ok(events);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPut]
        [Route("UpdateEvent")]
        public IActionResult UpdateEvent([FromQuery]int id, [FromBody] EventQueryModel objectInput)
        {
            Model.Calendar calendar = new Model.Calendar();

            if (id == 0)
                return BadRequest("The parameter ID cannot be null");

            var result = _repository.GetCalendarByID(id).ConfigureAwait(false);

            calendar = result.GetAwaiter().GetResult();

            if(calendar != null)
            {
                if(objectInput.Name != null)
                {
                    calendar.Name = objectInput.Name;
                }
                if (objectInput.Location != null)
                {
                    calendar.Location = objectInput.Location;
                }
                if (objectInput.EventOrganizer != null)
                {
                    calendar.EventOrganizer = objectInput.EventOrganizer;
                }

                var taskCalendarUpdated = _repository.UpdateEvent(calendar);

                var calendarUpdated = taskCalendarUpdated.GetAwaiter().GetResult();

                return NoContent();

            }

            return StatusCode(410, "No data was found");
        }

        [HttpDelete]
        [Route("DeleteEvent")]
        public IActionResult DeleteEvent([FromQuery]int id)
        {
            Model.Calendar foundEvent = new Model.Calendar();

            if (id == 0)
                return BadRequest("The parameter ID cannot be null");

            var searchEvent = _repository.GetCalendarByID(id).ConfigureAwait(false);

            foundEvent = searchEvent.GetAwaiter().GetResult();

            if(foundEvent != null)
            {
                _repository.DeleteEvent(foundEvent).ConfigureAwait(false);
                return NoContent();
            }

            return StatusCode(410,"No data was found");
        }
    }

 
}
