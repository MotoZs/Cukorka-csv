using Cukorka;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var sugars = new List<Sugar>();
var lines = File.ReadAllLines("Sugars.csv");
foreach (var line in lines)
{   
    var data = line.Split(", ");
    var sugar = new Sugar{id = int.Parse(data[0]), name = data[1], price = int.Parse(data[2]), quantity = int.Parse(data[3]) };
    sugars.Add(sugar);
}

var orders = new List<Order>();
var lines2 = File.ReadAllLines("Orders.csv");
foreach (var line in lines2.Skip(1))
{
    var data = line.Split(", ");
    var order = new Order { orderID = int.Parse(data[0]), sugarID = int.Parse(data[1]), quantity = int.Parse(data[2]), total = int.Parse(data[3]),  };
    orders.Add(order);
}

while (true)
{
    Console.WriteLine("1 - listázás | 2 - hozzáadás | 3 - módosítás | 4 - törlés | 0 - kilépés");
    var be = int.Parse(Console.ReadLine());

    switch (be)
    {
        case 0:
            break;

        case 1:
            foreach (var sugar in sugars)
            {
                Console.WriteLine("ID: " + sugar.id + ", Neve: " + sugar.name + ", Ára: " + sugar.price + ", Készleten: " + sugar.quantity);
            }
            break;

        case 2:
            Console.WriteLine("Add meg a cukorka nevét: ");
            var newName = Console.ReadLine();
            Console.WriteLine("Add meg a cukorka árát: ");
            var newPrice = int.Parse(Console.ReadLine());
            Console.WriteLine("Add meg a cukorka készlet mennyiségét: ");
            var newQuantity = int.Parse(Console.ReadLine());
            var newId = sugars.Max(s => s.id) + 1;
            var newSugar = new Sugar 
            { 
                id = newId, name = newName, price = newPrice, quantity = newQuantity
            };
            sugars.Add(newSugar);
            var rows = new List<string>();
            Save(sugars);
            break;

        case 3:
            Console.WriteLine("Add meg a módodítani kívánt cukor ID-átÍ!");
            int editId = int.Parse(Console.ReadLine());
            var editSugar = sugars.Find(s => s.id == editId);
            if (editSugar != null)
            {
                Console.WriteLine("Neve: ");
                editSugar.name = Console.ReadLine();
                Console.WriteLine("Ára: ");
                editSugar.price = int.Parse(Console.ReadLine());
                Console.WriteLine("Mennyiség: ");
                editSugar.quantity = int.Parse(Console.ReadLine());
                Save(sugars);
            }
            break;

        case 4:
            Console.WriteLine("Add meg a törölni kívánt cukor ID-át!");
            var deleteId = int.Parse(Console.ReadLine());
            var item = sugars.First(s => s.id == deleteId);
            var removeItem = sugars.FirstOrDefault(i => i.id == item.id);
            sugars.Remove(removeItem);
            Save(sugars);
            break;

        default: Console.WriteLine("Rossz szám!"); break;
    }

    Console.WriteLine("1 - Rendelések listázása | 2 - Új rendelés | 0 - kilépés");
    var be2 = int.Parse(Console.ReadLine());

    switch (be2) 
    {
        case 0:
            SaveFileToJson(sugars, orders);
            return;
        case 1:
            foreach (var order in orders)
            {
                Console.WriteLine("ID: " + order.orderID + ", Cukor ID: " + order.sugarID + ", Rendelt Mennyiség: " + order.quantity + ", Total: " + order.total);
            }
            break;
        case 2:

            break;
    }
}

void Save(List<Sugar> sugars)
{
    var lines = new List<string>();
    foreach (var sugar in sugars)
    {
        lines.Add($"{sugar.id}, {sugar.name}, {sugar.price}, {sugar.quantity} ");
    }
    File.WriteAllLines("Sugars.csv", lines);
}

 void SaveFileToJson(List<Sugar> sugars, List<Order> orders)
{
    var value = new { orders, sugars };
    var json = JsonSerializer.Serialize(value);
    File.WriteAllText("teszt.json", json);
} 