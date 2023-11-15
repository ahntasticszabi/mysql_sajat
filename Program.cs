using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace mysql
{
    class EmployeesTable
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }

    }
    class ProductsTable
    {
        public string name { get; set; }
        public string type { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }
    class PaymentsTable
    {
        public int customerNumber { get; set; }
        public string paymentDate { get; set; }
        public double amount { get; set; }
    }
    class Program
    {
        static string connString = "server=localhost;database=classicmodels;uid=root;password=12345678";
        static MySqlConnection conn = new MySqlConnection(connString);
        



        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Kapcsolat létrehozása..");
            conn.Open();
            Console.WriteLine("Kapcsolat létrehozva!");
            Console.ResetColor();

            Employees();
            Products();
            Payments();

            Console.ReadKey();
        }

        static void Employees()
        {
            List<EmployeesTable> employees = new List<EmployeesTable>();
            string query = "SELECT * FROM employees";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var employee = new EmployeesTable
                {
                    id = Convert.ToInt32(reader["employeeNumber"]),
                    firstname = Convert.ToString(reader["firstName"]),
                    lastname = Convert.ToString(reader["lastName"]),
                    email = Convert.ToString(reader["email"])
                };
                employees.Add(employee);
            }
            reader.Close();
            //foreach (var item in employees)
            //{
            //Console.WriteLine($"{item.id} | {item.firstname} | {item.lastname} | {item.email}");
            //}
        }



        static void Products()
        {
            List<ProductsTable> products = new List<ProductsTable>();
            string query = "SELECT * FROM products";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var product = new ProductsTable
                {
                    name = Convert.ToString(reader["productName"]),
                    type = Convert.ToString(reader["productLine"]),
                    quantity = Convert.ToInt32(reader["quantityInStock"]),
                    price = Convert.ToDouble(reader["buyPrice"])
                };
                products.Add(product);
            }
            reader.Close();
            //foreach (var item2 in products)
            //{
            //    Console.WriteLine($"{item2.name} | {item2.type} | {item2.quantity} | {item2.price}");
            //}
            reader.Close();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nÜdv! Szeretnéd lefuttatni a kódot?\n\t Ha igen, válaszd ki, hogy a Linq-s vagy Lambda-s megoldásd használnád. A Linq-shoz írd a \"Linq\"-t\n\t A Lambda-s megoldáshoz írd a \"Lambda\"-t\n\tHa egyiket se választod, írd a \"Tovább\" szót");
            Console.Write("Válaszod : ");
            Console.ForegroundColor = ConsoleColor.White;
            string valasztas = Console.ReadLine();

            switch (valasztas)
            {
                //Linq-s megoldás
                case "Linq":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //1. feladat: Hány darab elem van a listában
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"1. feladat : {products.Count()} darab termék van a listában");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //2. feladat: Típusonként hány darab van
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("2. feladat : Típusonként hány darab van");

                    var darab_linq = (
                        from sor in products
                        group sor by sor.type
                    );
                    foreach (var item in darab_linq)
                    {
                        Console.WriteLine($"\t{item.Key} | {item.Count()}");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //3. feladat: Csak a megadott típusúakat írja
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("3. feladat: Adj meg egy típust: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    string tipusneve = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;

                    var tipus_linq = (
                        from sor in products
                        where sor.type == tipusneve
                        select sor
                    );

                    if (tipus_linq.Any())
                    {
                        foreach (var item in tipus_linq)
                        {
                            Console.WriteLine($"\t{item.name} | {item.price:.}$ | {item.type}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nincsen ilyen típus");
                    }


                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //4. feladat: Az összes "Cars"-ra végződő típus
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("4. feladat : Az összes \"Cars\"-ra végződő típus");

                    var cars_linq = (
                        from sor in products
                        where sor.type.EndsWith("Cars")
                        select sor
                    );

                    if (cars_linq.Any())
                    {
                        foreach (var item in cars_linq)
                        {
                            Console.WriteLine($"\t{item.name} | {item.price:.}$ | {item.type}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tNincsenek ilyen típus/típusok");
                    }


                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //5. feladat: Legdrágább típus adatai 
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    var legdragabb = (
                        from sor in products
                        orderby sor.price
                        select sor
                    ).Last();
                    Console.WriteLine($"5. feladat : Legdrágább típus:\n\t{legdragabb.name} | {legdragabb.price:.$} | {legdragabb.type}");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //6. feladat: Legdrágább típusok adatai 
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"6. feladat : A legdrágább típusok");

                    Double max = (
                        from sor in products
                        select sor.price
                    ).Max();

                    var legdragabbak = (
                        from sor in products
                        where sor.price == max
                        select sor
                    );
                    if (legdragabbak.Any())
                    {
                        foreach (var item in legdragabbak)
                        {
                            Console.WriteLine($"\t{item.name} | {item.price:.}$ | {item.type}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tNincs több ugyanolyan árú autó");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //7. feladat : Minden típusból a legdrágább autó
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("7. feladat : Minden típusból a legdrágább autó:");
                    var minden_legdragabb = (
                        from sor in products
                        orderby sor.price
                        group sor.price by sor.type
                    );
                    foreach (var item in minden_legdragabb)
                    {
                        Console.WriteLine($"\t{item.Key} | {item.Max()}");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //8. feladat : Hány darab típus van
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var dis_linq = (
                        from sor in products
                        select sor.type
                    ).Distinct();

                    Console.WriteLine($"8. feladat : {dis_linq.Count()} darab típus van. Ezek az alábbiak:");
                    foreach (var item in dis_linq)
                    {
                        Console.WriteLine($"\t{item}");
                    }

                    Console.ResetColor();




                    break;

                //Lambda-s megoldás
                case "Lambda":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //1. feladat: Hány darab elem van a listában
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"1. feladat : {products.Count()} darab termék van a listában");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //2. feladat: Típusonként hány darab van
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("2. feladat : Típusonként hány darab van");

                    var darab_lambda = products.GroupBy(x => x.type);
                    foreach (var item in darab_lambda)
                    {
                        Console.WriteLine($"\t{item.Key} | {item.Count()}");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //3. feladat: Csak a megadott típusúakat írja
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("3. feladat: Adj meg egy típust: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    string tipusneve_lambda = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;

                    var tipus_lambda = products.Where(x => x.type == tipusneve_lambda);

                    if (tipus_lambda.Any())
                    {
                        foreach (var item in tipus_lambda)
                        {
                            Console.WriteLine($"\t{item.name} | {item.price:.}$ | {item.type}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nincsenek ilyen típusok");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //4. feladat: Az összes "Cars"-ra végződő típus
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("4. feladat : Az összes \"Cars\"-ra végződő típus");

                    var cars_lambda = products.Where(x => x.type.EndsWith("Cars"));

                    if (cars_lambda.Any())
                    {
                        foreach (var item in cars_lambda)
                        {
                            Console.WriteLine($"\t{item.name} | {item.price:.}$ | {item.type}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nincsenek ilyen típusok");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //5. feladat: Legdrágább típus adatai 
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    var legdragabb_lambda = (
                        from sor in products
                        orderby sor.price
                        select sor
                    ).Last();
                    Console.WriteLine($"5. feladat : Legdrágább típus:\n\t{legdragabb_lambda.name} | {legdragabb_lambda.price:.$} | {legdragabb_lambda.type}");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //6. feladat: Legdrágább típusok adatai 
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"6. feladat : A legdrágább típusok");

                    Double max_lambda = (
                        from sor in products
                        select sor.price
                    ).Max();

                    var legdragabbak_lambda = (
                        from sor in products
                        where sor.price == max_lambda
                        select sor
                    );
                    if (legdragabbak_lambda.Any())
                    {
                        foreach (var item in legdragabbak_lambda)
                        {
                            Console.WriteLine($"\t{item.name} | {item.price:.}$ | {item.type}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tNincs több ugyanolyan árú autó");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //7. feladat : Minden típusból a legdrágább autó
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("7. feladat : Minden típusból a legdrágább autó:");
                    var minden_legdragabb_lambda = (
                        from sor in products
                        orderby sor.price
                        group sor.price by sor.type
                    );
                    foreach (var item in minden_legdragabb_lambda)
                    {
                        Console.WriteLine($"\t{item.Key} | {item.Max()}");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n------------------------------------\n");

                    //8. feladat : Hány darab típus van
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var dis_lambda = products.Select(x => x.type).Distinct();
                    Console.WriteLine($"8. feladat : {dis_lambda.Count()} darab típus van. Ezek az alábbiak:");
                    foreach (var item in dis_lambda)
                    {
                        Console.WriteLine($"\t{item}");
                    }

                    Console.ResetColor();
                    break;

                case "Tovább":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("\"Linq\" vagy \"Lambda\", nem nagy feladat banyek");
                    Console.ResetColor();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Tovább mentél a következő táblára");
                    Console.ResetColor();
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n------------------------------------\n");
            Console.ResetColor();
        }

        static void Payments()
        {
            List<PaymentsTable> payments = new List<PaymentsTable>();
            string query = "SELECT * FROM payments";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var payment = new PaymentsTable
                {
                    customerNumber = Convert.ToInt32(reader["customerNumber"]),
                    paymentDate = Convert.ToString(reader["paymentDate"]),
                    amount = Convert.ToDouble(reader["amount"]),
                };
                payments.Add(payment);
            }
            reader.Close();
            //foreach (var item in payments)
            //{
              //Console.WriteLine($"{item.customerNumber} | {item.paymentDate} | {item.amount}");
            //}

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n------------------------------------\n");
            Console.ResetColor();

            //3. feladat: What is the total of payments received?
            Console.WriteLine($"3. feladat : Összes fizetések száma:");
            var allpayments = (
                from sor in payments
                select sor.amount
            ).Sum();
            Console.WriteLine($"\t{allpayments}$");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n------------------------------------\n");
            Console.ResetColor();

            //5. feladat: Report total payments for October 28, 2004.
            Console.WriteLine("5. feladat : A 2004. Október 28.-i fizetés adatai:");
            var total_payments = (
                        from sor in payments
                        where sor.paymentDate == "2004. 10. 28. 0:00:00"
                        select sor
            );
            foreach (var item in total_payments)
            {
                Console.WriteLine($"\t{item.customerNumber} | {item.amount}$");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n------------------------------------\n");
            Console.ResetColor();

            //6. feladat: Report those payments greater than $100,000.
            Console.WriteLine("6. feladat : Az 100,000$-nál nagyobb fizetések adatai:");
            var greater_than = (
                        from sor in payments
                        where sor.amount > 100000
                        select sor
            );
            foreach (var item in greater_than)
            {
                Console.WriteLine($"\t{item.customerNumber} | {item.paymentDate} | {item.amount}$");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n------------------------------------\n");
            Console.ResetColor();

            //9. feladat: What is the minimum payment received?
            var min_payment = (
                from sor in payments
                orderby sor.amount
                select sor.amount
            ).Min();
            Console.WriteLine($"9. feladat : Legkisebb fizetett összeg: {min_payment}$");


        }
    }
}
