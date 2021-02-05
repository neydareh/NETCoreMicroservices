using AutoMapper;
using Ecommerce.Api.Orders.Db;
using Ecommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order 
                { 
                    Id = 1, 
                    OrderDate = DateTime.Now,
                    CustomerId = 1,
                    Total = 40,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem(){Id = 1, OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 100},
                        new Db.OrderItem(){Id = 2, OrderId = 1, ProductId = 2, Quantity = 10, UnitPrice = 50},
                        new Db.OrderItem(){Id = 3, OrderId = 1, ProductId = 3, Quantity = 10, UnitPrice = 2000},
                        new Db.OrderItem(){Id = 4, OrderId = 1, ProductId = 4, Quantity = 10, UnitPrice = 120},
                    }
                });

                dbContext.Orders.Add(new Db.Order
                {
                    Id = 2,
                    OrderDate = DateTime.Now.AddDays(2),
                    CustomerId = 3,
                    Total = 400,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem(){Id = 5, OrderId = 2, ProductId = 1, Quantity = 100, UnitPrice = 100},
                        new Db.OrderItem(){Id = 6, OrderId = 2, ProductId = 2, Quantity = 100, UnitPrice = 50},
                        new Db.OrderItem(){Id = 7, OrderId = 2, ProductId = 3, Quantity = 100, UnitPrice = 2000},
                        new Db.OrderItem(){Id = 8, OrderId = 2, ProductId = 4, Quantity = 100, UnitPrice = 120},
                    }
                });

                dbContext.Orders.Add(new Db.Order
                {
                    Id = 3,
                    OrderDate = DateTime.Now.AddDays(-1),
                    CustomerId = 2,
                    Total = 420,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem(){Id = 9, OrderId = 3, ProductId = 1, Quantity = 50, UnitPrice = 100},
                        new Db.OrderItem(){Id = 10, OrderId = 3, ProductId = 2, Quantity = 100, UnitPrice = 50},
                        new Db.OrderItem(){Id = 11, OrderId = 3, ProductId = 3, Quantity = 220, UnitPrice = 2000},
                        new Db.OrderItem(){Id = 12, OrderId = 3, ProductId = 4, Quantity = 150, UnitPrice = 120}
                    }
                });

                dbContext.SaveChanges();
            }
        }

        public async Task<(bool isSuccess, IEnumerable<Models.Order> Orders, string errorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                logger?.LogInformation($"Querying orders table");
                var orders = await dbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(oi => oi.Items)
                    .ToListAsync();

                if (orders != null && orders.Any())
                {
                    logger?.LogInformation($"{orders.Count} order(s) found");
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }


    }
}
