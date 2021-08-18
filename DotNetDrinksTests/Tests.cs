using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetDrinks.Controllers;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DotNetDrinksTests
{
    [TestClass]
    public class Tests
    {
        // connect to the DB?? create a mock object to simulate our db connection when testing
        // This is an In-Memory db context > Microsoft.EntityFrameworkCore.InMemory
        private ApplicationDbContext _context;
        // empty list of products
        List<Product> products = new List<Product>();
        // declare the controller that will be tested
        ProductsController controller;

        // How do I fill _context with data? or when?
        // Create a constructor?? Rather, create an Initialize method
        [TestInitialize]
        public void TestInitialize()
        {
            // instantiate in-memory db > similar to startup.cs
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // create mock data in this db
            // Create 1 category
            var category = new Category { Id = 56, Name = "category test" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            // Create 1 brand
            var brand = new Brand { Id = 570, Name = "Brandless" };
            _context.Brands.Add(brand);
            _context.SaveChanges();

            // Create 3 products
            products.Add(new Product { Id = 230, Name = "a product", Price = 13, Category = category, Brand = brand });
            products.Add(new Product { Id = 320, Name = "on eproduct", Price = 22, Category = category, Brand = brand });
            products.Add(new Product { Id = 23, Name = "a singular prodict", Price = 333, Category = category, Brand = brand });

            foreach (var p in products)
            {
                _context.Products.Add(p);
            }
            _context.SaveChanges();

            // instanciate the controller class with mock db context
            controller = new ProductsController(_context);
        }

        [TestMethod]
        public void DeleteGet()
        {
            // Act
            var results = (ViewResult)controller.Delete(230).Result;

            // assert - could also be "Error" expected instead of 404 depending on logic in Details() 
            Assert.AreEqual("Delete", results.ViewName);
        }

        [TestMethod]
        public void DeleteConfirm()
        {
            // act - Get invalid detail
            var results = (ViewResult)controller.Details(230).Result;
            Console.WriteLine(results);

            // assert - return null
            Assert.AreEqual(null, results.ViewName);
        }
    }
}
