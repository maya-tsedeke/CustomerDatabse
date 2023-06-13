using System;
using System.Collections.Generic;
using System.IO;

namespace fs15_12_Customer_Database
{
    public class CustomerDatabase
    {
        private Dictionary<int, Customer> customers;
        private List<Customer> customersDeleted;
        private string filePath = "customers.csv";
        private Stack<Action> undoStack;
        private Stack<Action> redoStack;

        public CustomerDatabase()
        {
            customers = new Dictionary<int, Customer>();
            customersDeleted = new List<Customer>();
            undoStack = new Stack<Action>();
            redoStack = new Stack<Action>();
            LoadCustomersFromFile();
        }

        public void AddCustomer(Customer customer)
        {
            if (customers.ContainsKey(customer.Id))
            {
                Console.WriteLine("Customer with the same ID already exists.");
                return;
            }

            Action addAction = () =>
            {
                customers.Add(customer.Id, customer);
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
        public void UpdateCustomer(Customer updatedCustomer)
        {
            if (customers.TryGetValue(updatedCustomer.Id, out Customer? existingCustomer))
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
                    existingCustomer.FirstName = updatedCustomer.FirstName;
                    existingCustomer.LastName = updatedCustomer.LastName;
                    existingCustomer.Email = updatedCustomer.Email;
                    existingCustomer.Address = updatedCustomer.Address;

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
            if (customers.ContainsKey(customerId))
            {
                Customer customerToRemove = customers[customerId];

                Action deleteAction = () =>
                {
                    customers.Remove(customerId);
                    customersDeleted.Add(customerToRemove);
                    SaveCustomersToFile();
                };

                Action undoAction = () =>
                {
                    UndoDeleteCustomer(customerId);
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
                    customers.Add(customerId, customerToAdd);
                    customersDeleted.Remove(customerToAdd);
                    SaveCustomersToFile();
                };

                Action undoAction = () =>
                {
                    DeleteCustomer(customerId);
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
            if (customers.ContainsKey(customerId))
            {
                return customers[customerId];
            }

            return null;
        }

        public List<Customer> SearchCustomersById(int customerId)
        {
            List<Customer> foundCustomers = new List<Customer>();

            foreach (Customer customer in customers.Values)
            {
                if (customer.Id == customerId)
                {
                    foundCustomers.Add(customer);
                }
            }

            return foundCustomers;
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
                        customers[customer.Id] = customer; // Update the customer in the dictionary
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
                foreach (Customer customer in customers.Values)
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
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                Action undoAction = undoStack.Pop();
                redoStack.Push(undoAction);
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
                undoStack.Push(redoAction);
                redoAction.Invoke();
            }
            else if (undoStack.Count > 0)
            {
                Action undoAction = undoStack.Pop();
                redoStack.Push(undoAction);
                undoAction.Invoke();
            }
            else
            {
                Console.WriteLine("No action to redo.");
            }
        }
    }
}
