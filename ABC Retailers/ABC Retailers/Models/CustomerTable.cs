using Azure;
using Azure.Data.Tables;
using System;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retailers.Models
{
    //customer model will store all the variables for the customer table
    //https://www.c-sharpcorner.com/article/azure-storage-crud-operations-in-mvc-using-c-sharp-azure-table-storage-part-one/ 
    public class CustomerTable 
    {
        [Key]
        public Guid CustomerID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string? ImageURL { get; set; }

        public CustomerTable() { }
    
        public CustomerTable(string name, string surname, string email, int age, string imageURL)
        {
            CustomerID = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Email = email;
            Age = age;
            ImageURL = imageURL;
        }
    }
}
