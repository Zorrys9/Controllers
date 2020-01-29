using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Controllers.Models;
using Controllers.Util;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Controllers.Controllers
{
    public class HomeController : HelloBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void Index()
        {
            string table = "";

            foreach(var header in Request.Headers)
            {
                table += $"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>";
            }
            Response.WriteAsync($"<table>{table}</table>");
        }
        //public IActionResult Index()
        //{
        //   return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        public string Area(Geometry geometry)
        {
            return $"{geometry.Altitude} * {geometry.Height} / 2 = {geometry.GetArea()}";
        }
        [HttpPost]
        public IActionResult Area()
        {
            int Altitude = int.Parse(Request.Form.FirstOrDefault(p => p.Key == "altitude").Value);
            int Height = int.Parse(Request.Form.FirstOrDefault(p => p.Key == "height").Value);

            double result = Altitude * Height / 2;
            return Content($"result: {result}");
        }

        public JsonResult GetPerson()
        {
            Person person = new Person() { Name = "Alex", Age = 19 };
            return Json(person);
        }

        public HtmlResult GetHtml()
        {
            return new HtmlResult("<h2>Hello world</h2>");
        }
        //public string Area()
        //{
        //    int Altitude = int.Parse(Request.Query.FirstOrDefault(p => p.Key == "altitude").Value);
        //    int Height = int.Parse(Request.Query.FirstOrDefault(p => p.Key == "height").Value);

        //    double result = Altitude * Height / 2;
        //    return $"{result}";
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // переадресация
        public IActionResult RedirectToMetanit()
        {
            return Redirect("https://metanit.com");
        }
        public IActionResult LocalRedirectToIndex()
        {
            return LocalRedirect("~/Home/Index");
        }
        public IActionResult RedirectToActionArea()
        {
            return RedirectToAction("Area", "Home", new { Altitude = 5, Height = 3 });
        }
        public IActionResult RedirectToRouteDefault()
        {
            return RedirectToRoute("default", new { controller = "home", action = "area", altitude = 3, height = 5 });
        }

        //отправка статусных кодов
        public IActionResult StatusCode()
        {
            return StatusCode(401);
        }
        public IActionResult Age()
        {

            if (Request.Query.FirstOrDefault(p => p.Key == "age").Value.Count == 0)
                return BadRequest("Возвраст не указан");
            int age = int.Parse(Request.Query.FirstOrDefault(p => p.Key == "age").Value);
            if (age < 18)
                return Unauthorized("Ваш возвраст меньше 18");
            else
                return Ok($"Вход успешно выполнен, Ваш возвраст {age}");
        }
        public IActionResult PageNotFound()
        {
            return NotFound("Запрашиваемая страница не найденa");
        }

        //Отправка файлов
        public IActionResult GetFile()
        {
            string filePath = Path.Combine(_env.ContentRootPath, "Files/TXTfile2.txt");
            string fileType = "text/plain";
            string fileName = "TXTfile2.txt";

            return PhysicalFile(filePath, fileType, fileName);
        }
        public IActionResult GetByteFile()
        {
            string filePath = Path.Combine(_env.ContentRootPath, "Files/TXTfile2.txt");
            byte[] arrayByte = System.IO.File.ReadAllBytes(filePath);
            string fileType = "text/plain";
            string fileName = "TXTfile2.txt";

            return File(arrayByte, fileType, fileName);
        }
        public IActionResult GetStreamFile()
        {
            string filePath = Path.Combine(_env.ContentRootPath, "Files/TXTfile2.txt");
            FileStream fs = new FileStream(filePath,FileMode.OpenOrCreate);
            string fileType = "application/octet-stream";

            return File(fs, fileType);
        }
        public IActionResult GetVirtualFile()
        {
            string filePath = Path.Combine("~/Files", "TXTfile1.txt");
            return File(filePath, "application/octet-stream");
        }

        public class Geometry
        {
            public int Altitude { get; set; }
            public int Height { get; set; }
            public double GetArea()
            {
                return Altitude * Height / 2;
            }
        }
    }
}
