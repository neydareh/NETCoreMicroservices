﻿using Ecommerce.Api.Search.Interfaces;
using Ecommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Services
{
  public class CustomersService : ICustomersService
  {
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<ICustomersService> logger;

    public CustomersService(IHttpClientFactory httpClientFactory, ILogger<ICustomersService> logger)
    {
      this.httpClientFactory = httpClientFactory;
      this.logger = logger;
    }

    public async Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
    {
      try
      {
        var client = httpClientFactory.CreateClient("CustomersService");
        var response = await client.GetAsync($"api/customers/{id}");
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsByteArrayAsync();
          var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
          var result = JsonSerializer.Deserialize<dynamic>(content, options);

          return (true, result, null);
        }
        return (false, null, response.ReasonPhrase);
      }
      catch (Exception ex)
      {
        logger?.LogError(ex.ToString());
        return (false, null, ex.Message);
      }
    }

    public Task<(bool isSuccess, IEnumerable<Customer> Customers, string errorMessage)> GetCustomersAsync()
    {
      throw new NotImplementedException();
    }
  }
}
