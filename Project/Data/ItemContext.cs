
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Project.Models;
using POSAPI.Models;

namespace Project.Data
{
	public class ItemContext : DbContext
	{
		public ItemContext(DbContextOptions<ItemContext> options) : base(options) { }
		
		public DbSet<Item> Items { get; set; }

	}
	public class CartContext : DbContext
	{
		public CartContext(DbContextOptions<CartContext> options) : base(options) { }

		public DbSet<Cart> CartItems { get; set; }

	}
	public class CustomerContext : DbContext
    {
		public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }
		public DbSet<Customer> Customers { get; set; }

	}

}

