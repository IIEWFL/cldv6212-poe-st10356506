using Microsoft.AspNetCore.Mvc;
using ABC_Retailers.Data;
using ABC_Retailers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace ABC_Retailers.Controllers
{
    //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions?view=aspnetcore-5.0&form=MG0AV3
    //controller for managing orders
    public class OrderController : Controller
    {
        //references the db context class instead of services to store data
        //https://stackoverflow.com/questions/62411410/is-there-any-better-way-to-add-the-dbcontext-to-a-asp-core-mvc-controller

        private readonly ABCRetailersDBContext _aBCRetailersDBContext;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ABCRetailersDBContext aBCRetailersDBContext, ILogger<OrderController> logger)
        {
            _aBCRetailersDBContext = aBCRetailersDBContext;
            _logger = logger;
        }

        //displays the new order page
        [HttpGet]
        public IActionResult Index()
        {


            return View("NewOrder");
        }

        [HttpGet]
        public async Task <IActionResult> OrderCart() 
        {
            //retrieve all orders
            var orders = await _aBCRetailersDBContext.OrdersTable.ToListAsync();
            return View("OrderCart", orders);
        }

        //method for receiving the new order
        [HttpGet]
        public IActionResult NewOrder(string productName, string description, double price)
        {
            //add order to the order table
            //https://docs.microsoft.com/en-us/ef/core/?form=MG0AV3

            var order = new OrderModel
            {
                ProductName = productName,
                ProductDescription = description,
                Price = price,
                Quantity = 1
            };
            _logger.LogInformation($"Product Description received: {order.ProductDescription}");

            return View(order);
        }

        //method for purchasing the product 
        //https://www.geeksforgeeks.org/basic-crud-create-read-update-delete-in-asp-net-mvc-using-c-sharp-and-entity-framework/
        [HttpPost]
        public async Task<IActionResult> PurchaseProduct(string productName, int quantity)
        {
            //error handling for minimum quantity
            if (quantity <= 0)
            {
                TempData["ErrorMessage"] = "Quantity must be greater than 0.";
                return RedirectToAction("Index");
            }

            _logger.LogInformation($"Attempting to purchase product: {productName}");

            //https://docs.microsoft.com/en-us/ef/core/?form=MG0AV3
            //retrieve the product from the SQL database
            var product = await _aBCRetailersDBContext.ProductsTable
            .FirstOrDefaultAsync(p => p.ProductName == productName);

            //error handling for retrieving products
            if (product == null)
            {
                _logger.LogWarning($"Product '{productName}' not found.");
                return View("Error", new ErrorViewModel { RequestId = "Product not available." });
            }

            if (product.Availability < quantity)
            {
                _logger.LogWarning($"Product '{productName}' out of stock.");
                return View("Error", new ErrorViewModel { RequestId = $"Insufficient stock. Only {product.Availability} available." });
            }

            try
            {
                //deduct the selected quantity and update inventory
                product.Availability -= quantity;
                _aBCRetailersDBContext.ProductsTable.Update(product);

                //https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-8.0

                var order = new OrderModel
                {
                    OrderID = Guid.NewGuid(),
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Price = product.Price,
                    Quantity = quantity
                };

                //add order to the order table and update the table
                _aBCRetailersDBContext.OrdersTable.Add(order);
                await _aBCRetailersDBContext.SaveChangesAsync();

                _logger.LogInformation($"Order for '{productName}' processed. Quantity: {quantity}. Remaining stock: {product.Availability}.");

                //redirect to the order success page after a successful order
                return View("OrderSuccess", order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing order for product '{productName}': {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = "An error occurred while processing the order." });
            }
        }

        //update the product inventory after the order
        //
        [HttpPost]
        public async Task<IActionResult> UpdateInventory(int productId, int quantity)
        {
            try
            {
                var product = await _aBCRetailersDBContext.ProductsTable.FindAsync(productId);
                if (product != null)
                {
                    product.Availability = quantity;
                    _aBCRetailersDBContext.ProductsTable.Update(product);
                    await _aBCRetailersDBContext.SaveChangesAsync();

                    _logger.LogInformation($"Inventory updated: Product ID {productId}, New Quantity: {quantity}");
                }
                else
                {
                    _logger.LogWarning($"Product ID {productId} not found for inventory update.");
                    return View("Error", new ErrorViewModel { RequestId = "Product not available for update." });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating inventory for product ID '{productId}': {ex}");
                return View("Error", new ErrorViewModel { RequestId = ex.ToString() });
            }
        }


    }
}

