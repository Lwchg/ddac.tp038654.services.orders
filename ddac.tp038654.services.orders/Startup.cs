using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ddac.tp038654.services.orders.Interface;
using ddac.tp038654.services.orders.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace ddac.tp038654.services.orders
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITableStorageService>(InitializeTableStorageClientInstanceAsync(Configuration.GetSection("TableStorage")).GetAwaiter().GetResult());
            services.AddScoped<ServiceBusSender>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new Info() { Title= "Orders API"});
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }


        private static async Task<TableStorageService> InitializeTableStorageClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string connectionString = configurationSection.GetSection("ConnectionString").Value;
            string orders = configurationSection.GetSection("Orders").Value;
            string ordersItems = configurationSection.GetSection("ordersItems").Value;

            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(connectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable ordersTable = tableClient.GetTableReference(orders);
            await ordersTable.CreateIfNotExistsAsync();

            CloudTable ordersItemsTable = tableClient.GetTableReference(ordersItems);
            await ordersItemsTable.CreateIfNotExistsAsync();

            TableStorageService tableStorageService = new TableStorageService(tableClient);
            return tableStorageService;
        }
    }
}
