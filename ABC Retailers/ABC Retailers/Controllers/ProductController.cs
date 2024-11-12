using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using ABC_Retailers.Models;
using System.IO;
using System.Collections.Concurrent;
using Azure.Storage.Queues;
using ABC_Retailers.Data;
using Microsoft.EntityFrameworkCore;

namespace ABC_Retailers.Controllers
{
    //controller to add new products to the product table with db context
    //https://stackoverflow.com/questions/62411410/is-there-any-better-way-to-add-the-dbcontext-to-a-asp-core-mvc-controller
    public class ProductController : Controller
    {
        private readonly ABCRetailersDBContext _aBCRetailersDBContext;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ABCRetailersDBContext aBCRetailersDBContext, ILogger<ProductController> logger)
        {
            _aBCRetailersDBContext = aBCRetailersDBContext;
            _logger = logger;
        }

        //https://www.geeksforgeeks.org/basic-crud-create-read-update-delete-in-asp-net-mvc-using-c-sharp-and-entity-framework/

        public async Task<IActionResult> Index()
        {
            var products = await _aBCRetailersDBContext.ProductsTable.ToListAsync();
            return View(products);
        }

        //this will display the form to add a new product
        [HttpGet]
        public IActionResult NewProduct()
        {
            return View();
        }
        //https://www.geeksforgeeks.org/basic-crud-create-read-update-delete-in-asp-net-mvc-using-c-sharp-and-entity-framework/
        //adds the new product to the web app and the storage account
        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, string description, double price, int availability, IFormFile image)
        {
            string imageURL = null;

            if (image != null)
            {
                //https://scrapingant.com/blog/download-image-c-sharp
                //save image locally and get the path or URL
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", $"{Guid.NewGuid()}.jpg");

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                imageURL = $"/images/{Path.GetFileName(imagePath)}"; //URL to access the image in wwwroot
            }
            else
            {
                return View("Error", new ErrorViewModel { RequestId = "Please upload product image." });
            }

            //create a new product instance and save it to the SQL database
            //https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-8.0
           
            var product = new ProductTable
            {
                ProductID = Guid.NewGuid(),
                ProductName = name,
                ProductDescription = description,
                Price = price,
                Availability = availability,
                ImageURL = imageURL
            };

            //add and save product to the product table
            _aBCRetailersDBContext.ProductsTable.Add(product);
            await _aBCRetailersDBContext.SaveChangesAsync();

            _logger.LogInformation($"Product '{name}' added with ID {product.ProductID}, Description: {description}, and image URL: {imageURL}");

            //redirect to product index after product is added
            return RedirectToAction("Index");
        }
    }
}