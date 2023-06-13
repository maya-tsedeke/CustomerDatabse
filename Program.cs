using System;
using System.Collections.Generic;
using fs15_12_Customer_Database;

public class Program
{
    static void Main(string[] args)
    {
        // Create an instance of CustomerDatabase and demonstrate the functionality
        CustomerDatabase database = new CustomerDatabase();
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Customer Database Management");
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. Update Customer");
            Console.WriteLine("3. Delete Customer");
            Console.WriteLine("4. Search Customers by ID");
            Console.WriteLine("5. Undo");
            Console.WriteLine("6. Redo");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");
            string? option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    AddCustomer(database);
                    break;
                case "2":
                    UpdateCustomer(database);
                    break;
                case "3":
                    DeleteCustomer(database);
                    break;
                case "4":
                    SearchCustomers(database);
                    break;
                case "5":
                    database.Undo();
                    Console.WriteLine("Undo operation executed.");
                    break;
                case "6":
                    database.Redo();
                    Console.WriteLine("Redo operation executed.");
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            Console.WriteLine();
        }
    }

    static void AddCustomer(CustomerDatabase database)
    {
        Console.WriteLine("Add Customer");
        Console.WriteLine();
        Console.Write("Enter Customer ID: ");
        string? input = Console.ReadLine();
        int id;
        if (input != null && int.TryParse(input, out id))
        {
            Console.Write("Enter First Name: ");
            string? firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string? lastName = Console.ReadLine();
            Console.Write("Enter Email: ");
            string? email = Console.ReadLine();
            Console.Write("Enter Address: ");
            string? address = Console.ReadLine();
            Customer customer = new Customer
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Address = address
            };
            database.AddCustomer(customer);
            Console.WriteLine("Customer added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid input for Customer ID. Please try again.");
        }
    }

    static void UpdateCustomer(CustomerDatabase database)
    {
        Console.WriteLine("Update Customer");
        Console.WriteLine();
        Console.Write("Enter Customer ID: ");
        string? input = Console.ReadLine();
        int id;
        if (input != null && int.TryParse(input, out id))
        {
            Customer? existingCustomer = database.GetCustomerById(id);
            if (existingCustomer != null)
            {
                Console.Write("Enter New First Name: ");
                string? firstName = Console.ReadLine();
                Console.Write("Enter New Last Name: ");
                string? lastName = Console.ReadLine();
                Console.Write("Enter New Email: ");
                string? email = Console.ReadLine();
                Console.Write("Enter New Address: ");
                string? address = Console.ReadLine();
                Customer updatedCustomer = new Customer
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Address = address
                };

                database.UpdateCustomer(updatedCustomer);
                Console.WriteLine("Customer updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Customer ID. Please try again.");
        }
    }

    static void DeleteCustomer(CustomerDatabase database)
    {
        Console.WriteLine("Delete Customer");
        Console.WriteLine();
        Console.Write("Enter Customer ID: ");
        string? input = Console.ReadLine();
        int id;
        if (input != null && int.TryParse(input, out id))
        {
            Customer? existingCustomer = database.GetCustomerById(id);
            if (existingCustomer != null)
            {
                Console.WriteLine("Are you sure you want to delete the following customer?");
                Console.WriteLine($"ID: {existingCustomer.Id}");
                Console.WriteLine($"Name: {existingCustomer.FirstName} {existingCustomer.LastName}");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");
                Console.Write("Select an option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        database.DeleteCustomer(id);
                        Console.WriteLine("Customer deleted successfully.");
                        break;
                    case "2":
                        Console.WriteLine("Delete operation canceled.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Delete operation canceled.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Customer ID. Please try again.");
        }
    }

    static void SearchCustomers(CustomerDatabase database)
    {
        Console.WriteLine("Search Customers by ID");
        Console.WriteLine();
        Console.Write("Enter Customer ID: ");
        string? input = Console.ReadLine();
        int id;
        if (input != null && int.TryParse(input, out id))
        {
            List<Customer> searchedCustomers = database.SearchCustomersById(id);
            if (searchedCustomers.Count > 0)
            {
                Console.WriteLine("Search results:");
                foreach (Customer customer in searchedCustomers)
                {
                    Console.WriteLine($"ID: {customer.Id}");
                    Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");
                    Console.WriteLine($"Email: {customer.Email}");
                    Console.WriteLine($"Address: {customer.Address}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No customers found with the specified ID.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Customer ID. Please try again.");
        }
    }






}
