using System;
using System.Collections.Generic;
using System.IO;

namespace fs15_12_Customer_Database
{
    public class CustomerDatabase
    {
        private List<Customer> customers;
        private List<Customer> customersDeleted; // List to store deleted customers
        private string filePath = "customers.csv";
        private Stack<Action> undoStack;
        private Stack<Action> redoStack;

        public CustomerDatabase()
        {
            customers = new List<Customer>();
            customersDeleted = new List<Customer>(); // Initialize the list of deleted customers
            undoStack = new Stack<Action>();
            redoStack = new Stack<Action>();
            LoadCustomersFromFile();
        }
        public List<Customer> Customers => customers;
        public void AddCustomer(Customer customer)
        {
            if (customers.Exists(c => c.Email == customer.Email))
            {
                Console.WriteLine("Customer with the same email already exists.");
                return;
            }

            Action addAction = () =>
            {
                customers.Add(customer);
                SaveCustomersToFile();
            };

            Action undoAction = () =>
            {
                DeleteCustomer(customer.Id);
                SaveCustomersToFile();
            };

            PerformAction(addAction, undoAction);

            SaveCustomersToFile();
        }
        public void UpdateCustomer(Customer customer)
        {
            Customer? existingCustomer = customers.Find(c => c.Id == customer.Id);
            if (existingCustomer != null)
            {
                Customer previousCustomer = new Customer
                {
                    Id = existingCustomer.Id,
                    FirstName = existingCustomer.FirstName,
                    LastName = existingCustomer.LastName,
                    Email = existingCustomer.Email,
                    Address = existingCustomer.Address
                };

                Action updateAction = () =>
                {
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Address = customer.Address;

                    SaveCustomersToFile();
                };

                Action undoAction = () =>
                {
                    UpdateCustomer(previousCustomer);
                    SaveCustomersToFile();
                };

                PerformAction(updateAction, undoAction);

                SaveCustomersToFile();
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        public void DeleteCustomer(int customerId)
        {
            Customer? customerToRemove = customers.Find(c => c.Id == customerId);
            if (customerToRemove != null)
            {
                Action deleteAction = () =>
                {
                    customers.Remove(customerToRemove);
                    customersDeleted.Add(customerToRemove); // Add the customer to the list of deleted customers
                    SaveCustomersToFile();
                };

                Action undoAction = () =>
                {
                    UndoDeleteCustomer(customerToRemove.Id); // Call separate method to undo deletion
                    SaveCustomersToFile();
                };

                PerformAction(deleteAction, undoAction);

                SaveCustomersToFile();
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        private void UndoDeleteCustomer(int customerId)
        {
            Customer? customerToAdd = customersDeleted.Find(c => c.Id == customerId);
            if (customerToAdd != null)
            {
                Action addAction = () =>
                {
                    customers.Add(customerToAdd); // Add the customer back to the main list
                    customersDeleted.Remove(customerToAdd); // Remove the customer from the list of deleted customers
                    SaveCustomersToFile();
                };

                Action undoAction = () =>
                {
                    DeleteCustomer(customerToAdd.Id); // Call the separate method to delete the customer again
                    SaveCustomersToFile();
                };

                PerformAction(addAction, undoAction);

                SaveCustomersToFile();
            }
            else
            {
                Console.WriteLine("Customer not found in the deleted list.");
            }
        }
        public Customer? GetCustomerById(int customerId)
        {
            return customers.Find(c => c.Id == customerId);
        }
        public List<Customer> SearchCustomersById(int customerId)
        {
            return customers.FindAll(c => c.Id == customerId);
        }
        private void LoadCustomersFromFile()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] values = line.Split(',');
                        Customer customer = new Customer()
                        {
                            Id = int.Parse(values[0]),
                            FirstName = values[1],
                            LastName = values[2],
                            Email = values[3],
                            Address = values[4]
                        };
                        customers.Add(customer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading customers: " + ex.Message);
                }
            }
        }
        private void SaveCustomersToFile()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (Customer customer in customers)
                {
                    string line = $"{customer.Id},{customer.FirstName},{customer.LastName},{customer.Email},{customer.Address}";
                    lines.Add(line);
                }
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving customers: " + ex.Message);
            }
        }
        private void PerformAction(Action addAction, Action undoAction)
        {
            addAction.Invoke();
            undoStack.Push(undoAction);
            redoStack.Clear(); // Clear redo stack when a new action is performed
        }
        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                Action undoAction = undoStack.Pop();
                redoStack.Push(undoAction); // Store the undo action in the redo stack before executing it
                undoAction.Invoke();
            }
            else
            {
                Console.WriteLine("No action to undo.");
            }
        }
        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                Action redoAction = redoStack.Pop();
                undoStack.Push(redoAction); // Store the redo action in the undo stack before executing it
                redoAction.Invoke();
            }
            else if (undoStack.Count > 0)
            {
                Action undoAction = undoStack.Pop();
                redoStack.Push(undoAction); // Store the undo action in the redo stack before executing it
                undoAction.Invoke();
            }
            else
            {
                Console.WriteLine("No action to redo.");
            }
        }
    }
}
