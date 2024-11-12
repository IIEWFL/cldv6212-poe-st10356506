using Azure;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retailers.Models
{
    //https://www.techtarget.com/whatis/definition/model-view-controller-MVC#:~:text=The%20MVC%20methodology%20separates%20an,out%20other%20data%2Drelated%20tasks.
    //order model contains the variables for the product order
    public class OrderModel
    {
        [Key]
       public Guid OrderID { get; set; }
       public string ProductName { get; set; }
       public string ProductDescription { get; set; }
       public double Price { get; set; }
       public int Quantity { get; set; }
       public OrderModel() { }

       public OrderModel(string name, string description, double price, int quantity)
       {
           OrderID = Guid.NewGuid();
           ProductName = name;
           ProductDescription = description;
           Price = price;
          Quantity = quantity;
       }

    }

}
