using Microsoft.AspNetCore.Mvc;
using ABC_Retailers.Models;
using ABC_Retailers.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Azure.Storage.Blobs.Models;

namespace ABC_Retailers.Controllers
{
    //https://youtu.be/vq3pzXv6kG0?si=v2eQ9YPgu1wucB0Y 
    //customerController controls CRUD operations for the Customer table and stores image URLs
    public class CustomerController : Controller
    {
        //https://stackoverflow.com/questions/62411410/is-there-any-better-way-to-add-the-dbcontext-to-a-asp-core-mvc-controller
        //now instead of referncing the azure services, the db context class containing database tables has been referenced
        private readonly ABCRetailersDBContext _aBCRetailersDBContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerController(ABCRetailersDBContext aBCRetailersDBContext, IWebHostEnvironment webHostEnvironment)
        {
            _aBCRetailersDBContext = aBCRetailersDBContext;
            _webHostEnvironment = webHostEnvironment;
        }
        //https://www.geeksforgeeks.org/basic-crud-create-read-update-delete-in-asp-net-mvc-using-c-sharp-and-entity-framework/
        //get customers
        public async Task<IActionResult> Index()
        {
            return View(await _aBCRetailersDBContext.CustomersTable.ToListAsync());
        }

        //display form to add a new customer
        [HttpGet]
        public IActionResult NewCustomer()
        {
            return View();
        }

        //method for adding a new customer with image
        [HttpPost]
        public async Task<IActionResult> AddCustomer(string name, string surname, string email, int age, IFormFile image)
        {
            string imageURL = null;

            if (image != null)
            {
                //https://learn.microsoft.com/en-us/answers/questions/1202232/uploading-and-displaying-image-with-fileupload-con?form=MG0AV3
                //save image locally and get the path or URL
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", $"{Guid.NewGuid()}.jpg");

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                imageURL = $"/images/{Path.GetFileName(imagePath)}"; //url to access the image in wwwroot
            }
            //https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-8.0
            //insert data into the customers table
            var customer = new CustomerTable
            {
            CustomerID = Guid.NewGuid(),
            Name = name,
            Surname = surname,
            Email = email,
            Age = age,
            ImageURL = imageURL
        };
            //add and saves changes to the customer table
            _aBCRetailersDBContext.CustomersTable.Add(customer);
            await _aBCRetailersDBContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
} 


