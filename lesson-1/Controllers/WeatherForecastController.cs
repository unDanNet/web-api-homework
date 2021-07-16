using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace lesson_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[] {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private WeatherStore _weatherStore;
        
        private List<WeatherForecast> _weather = new List<WeatherForecast>();

        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherStore weatherStore)
        {
            _logger = logger;
            _weatherStore = weatherStore;
            
            var rng = new Random();
            
            _weather = Enumerable.Range(0, 29).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList();
        }

        
        private string GetFailureMessage(string reason) => $"Can not get the forecasts. Reason: {reason}.";
        
        private string SaveFailureMessage(string reason) => $"Can not save the forecast. Reason: {reason}.";
        
        private string DeleteFailureMessage(string reason) => $"Can not delete the forecasts. Reason: {reason}.";
        
        private string EditFailureMessage(string reason) => $"Can not edit the forecast. Reason: {reason}.";

        
        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return Content(JsonSerializer.Serialize(_weather));
        }
        
        
        [HttpGet]
        [Route("from/{startTime}/days/{daysAmount}")]
        public IActionResult Get([FromRoute] string startTime, [FromRoute] int daysAmount)
        {
            DateTime startDate;

            if (daysAmount < 0)
                return BadRequest(GetFailureMessage("invalid amount of days"));
            
            try
            {
                startDate = Convert.ToDateTime(startTime);
            }
            catch (Exception e)
            {
                return BadRequest(GetFailureMessage("the wrong date format"));
            }

            return Content(JsonSerializer.Serialize(
                _weather.Where(
                    w => w.Date >= startDate && w.Date <= startDate.AddDays(daysAmount)
                )
            ));
        }

        
        [HttpPost]
        [Route("save-for-date")]
        public IActionResult Save([FromQuery] string date)
        {
            DateTime dateToSave;
            
            try
            {
                dateToSave = Convert.ToDateTime(date);
            }
            catch (Exception e)
            {
                return BadRequest(SaveFailureMessage("the wrong date format"));
            }

            var weatherToSave = _weather.Find(
                w => w.Date.Day == dateToSave.Day && w.Date.Month == dateToSave.Month && w.Date.Year == dateToSave.Year
            );

            if (weatherToSave is null)
            {
                return BadRequest(SaveFailureMessage("forecast for the specified date not found"));
            }
            
            _weatherStore.data.Add(weatherToSave.Date, weatherToSave);
            
            return Ok($"Successfully saved forecast for date {dateToSave.ToShortDateString()}.");
        }


        [HttpGet]
        [Route("saved")]
        public IActionResult GetSaved()
        {
            if (_weatherStore.data.Count == 0)
                return Content("There are no saved forecasts yet.");
            
            return Content(JsonSerializer.Serialize(_weatherStore.data.Values));
        }


        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete([FromQuery] string startTime, [FromQuery] int daysAmount)
        {
            DateTime startDate;

            if (daysAmount < 0)
                return BadRequest(DeleteFailureMessage("invalid amount of days"));
            
            try
            {
                startDate = Convert.ToDateTime(startTime);
            }
            catch (Exception e)
            {
                return BadRequest(DeleteFailureMessage("the wrong date format"));
            }

            foreach (var key in _weatherStore.data.Keys)
            {
                if (key >= startDate && key <= startDate.AddDays(daysAmount))
                    _weatherStore.data.Remove(key);
            }

            return Ok(
                $"Successfully deleted all forecasts from {startDate.ToShortDateString()} " +
                $"to {startDate.AddDays(daysAmount).ToShortDateString()}"
            );
        }


        [HttpPut]
        [Route("edit")]
        public IActionResult Edit([FromQuery] string date, [FromQuery] int temperatureC, [FromQuery] string summary)
        {
            DateTime dateToEdit;
            
            try
            {
                dateToEdit = Convert.ToDateTime(date);
            }
            catch (Exception e)
            {
                return BadRequest(EditFailureMessage("the wrong date format"));
            }
            
            var predicate = new Func<KeyValuePair<DateTime, WeatherForecast>, bool>(
                w => w.Key.Day == dateToEdit.Day && w.Key.Month == dateToEdit.Month && w.Key.Year == dateToEdit.Year
            );

            var weatherToEdit = _weatherStore.data.FirstOrDefault(predicate).Value;

            if (weatherToEdit is null)
            {
                return BadRequest(EditFailureMessage("forecast for the specified date not found"));
            }
            
            weatherToEdit.TemperatureC = temperatureC;

            if (!string.IsNullOrEmpty(summary))
                weatherToEdit.Summary = summary;

            _weatherStore.data[_weatherStore.data.FirstOrDefault(predicate).Key] = weatherToEdit;

            return Ok($"Successfully edited forecast for date {dateToEdit.ToShortDateString()}.");
        }
    }
}