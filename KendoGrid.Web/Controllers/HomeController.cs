using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KendoGrid.Web.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace KendoGrid.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetProducts([FromBody]Request request)
        {
            var context = GetContext();

            var result = await context.Products.ToDataSourceAsync(request);

            return Json(result);
        }


        /// <summary>
        /// این تابع یک بانک اطلاعاتی را در حافظه ایجاد کرده و چند رکورد برای استفاده در مثال درج می کند
        /// </summary>
        /// <returns></returns>
        private MyDbContext GetContext()
        {
            var builder = new DbContextOptionsBuilder<MyDbContext>();
            builder.UseInMemoryDatabase("TestDb", optionsBuilder => { });
            var options = builder.Options;

            var context = new MyDbContext(options);

            if (!context.Products.Any())
            {
                for (var i = 0; i < 100; i++)
                {
                    context.Products.Add(new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Product " + (i+1),
                        Date = DateTime.Now.AddDays(i),
                        Price = (i+1) * 1000
                    });
                }

                context.SaveChanges();
            }

            return context;
        }


    }
}
