﻿using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retailers.Models
{
    //product model contains the product variables
    //https://www.c-sharpcorner.com/article/azure-storage-crud-operations-in-mvc-using-c-sharp-azure-table-storage-part-one/ 
    //https://stackoverflow.com/questions/60617190/how-do-i-store-a-picture-in-azure-blob-storage-in-asp-net-mvc-application 
    public class ProductTable
    {
        [Key]
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
        public int Availability { get; set; }
        public string ImageURL { get; set; }

        public ProductTable() { }

        public ProductTable(string name, string description, double price, int availability, string imageUrl)
        {
            ProductID = Guid.NewGuid();
            ProductName = name;
            ProductDescription = description;
            Price = price;
            Availability = availability;
            ImageURL = imageUrl;
        }

    }
}
