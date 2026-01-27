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
bool cikus = true;

while (cikus)
{
    Console.WriteLine("\n1 - listázás | 2 - hozzáadás | 3 - módosítás | 4 - törlés | 0 - kilépés");
    var be = Console.ReadLine();
    if (!int.TryParse(be, out int beInt))
    {
        Console.WriteLine("Számot adj meg!");
        continue;
    }
        
        switch (beInt)
        {
            case 0:
                cikus = false;
                break;

            case 1:
                foreach (var sugar in sugars)
                {
                    Console.WriteLine("ID: " + sugar.id + ", Neve: " + sugar.name + ", Ára: " + sugar.price + ", Készleten: " + sugar.quantity);
                }
                break;

            case 2:
                bool temp = true;
                bool temp2 = true;
            Console.WriteLine("Add meg a cukorka nevét: ");
                var newName = Console.ReadLine();
                var price = 0;
                while (temp)
                {
                    Console.WriteLine("Add meg a cukorka árát: ");
                    var newPriceString = Console.ReadLine();
                    if (!int.TryParse(newPriceString, out int newPrice))
                    {
                        Console.WriteLine("Számot adj meg!");
                    }
                    else
                    {
                        price = newPrice;
                        temp = false;
                        break;
                    }
                }
                var quantity = 0;
                while (temp2)
                {
                    Console.WriteLine("Add meg a cukorka készlet mennyiségét: ");
                    var newQuantityString = Console.ReadLine();
                    if (!int.TryParse(newQuantityString, out int newQuantity))
                    {
                        Console.WriteLine("Számot adj meg!");
                    }
                    else
                    {
                        quantity = newQuantity;
                        temp2 = false;
                        break;
                    }
                }
                var newId = sugars.Max(s => s.id) + 1;
                var newSugar = new Sugar
                {
                    id = newId,
                    name = newName,
                    price = price,
                    quantity = quantity
                };
                sugars.Add(newSugar);
                var rows = new List<string>();
                SaveSugar(sugars);
                break;

            case 3:
                Console.WriteLine("Add meg a módodítani kívánt cukor ID-át!");
                int editId = int.Parse(Console.ReadLine());
                var editSugar = sugars.Find(s => s.id == editId);
                if (editSugar != null)
                {
                    Console.WriteLine("Új neve: ");
                    editSugar.name = Console.ReadLine();

                    Console.WriteLine("Új ára: ");
                    var newPrice = Console.ReadLine();
                    if (!int.TryParse(newPrice, out int correctPrice))
                    {
                        Console.WriteLine("Számot adj meg!");
                        continue;
                    }
                    editSugar.price = correctPrice;

                    Console.WriteLine("Új mennyisége: ");
                    var newQuantity = Console.ReadLine();
                    if (!int.TryParse(newQuantity, out int correctQuantity))
                        {
                            Console.WriteLine("Számot adj meg!");
                            continue;
                        }
                    editSugar.quantity = correctQuantity;
 
                    SaveSugar(sugars);
                }
                else { Console.WriteLine("Nincs ilyen cukorka ID!"); }
            break;

            case 4:
                Console.WriteLine("Add meg a törölni kívánt cukor ID-át!");
                var deleteId = int.Parse(Console.ReadLine());
                var item = sugars.First(s => s.id == deleteId);
                if (item != null)
                {
                   var removeItem = sugars.FirstOrDefault(i => i.id == item.id);
                   sugars.Remove(removeItem);
                   SaveSugar(sugars);
                }
                else { Console.WriteLine("Nincs ilyen cukorka ID!"); }
                break;

            default: Console.WriteLine("0, 1, 2, 3, 4 az elfogadható számok!"); break;
        }
}
bool cikus2 = true;

while (cikus2) 
{
    Console.WriteLine("\n1 - Rendelések listázása | 2 - Új rendelés | 0 - kilépés");
    var be2 = Console.ReadLine();
    if (!int.TryParse(be2, out int beInt2))
    {
        Console.WriteLine("Számot adj meg!");
        continue;
    }

    switch (beInt2)
    {
        case 0:
            cikus2 = false;
            return;
        case 1:
            foreach (var order in orders)
            {
                Console.WriteLine("ID: " + order.orderID + ", Cukor ID: " + order.sugarID + ", Rendelt Mennyiség: " + order.quantity + ", Total: " + order.total);
            }
            break;
        case 2:
            Console.WriteLine("Add meg a rendelni kívánt cukorka ID-át!");
            var orderSugarId = Console.ReadLine();
            if (!int.TryParse(orderSugarId, out int correctSugarId))
            {
                Console.WriteLine("Számot adj meg!");
                break;
            }
            var sugar = sugars.FirstOrDefault(s => s.id == correctSugarId);
            if (sugar == null)
            {
                Console.WriteLine("Nincs ilyen cukorka ID!");
                continue;
            }

            Console.WriteLine("Mennyit?");
            var orderQuantity = Console.ReadLine();
            if (!int.TryParse(orderQuantity, out int correctQuantity))
            {
                Console.WriteLine("Számot adj meg!");
                break;
            }

            if (sugar.quantity < correctQuantity)
            {
                Console.WriteLine("Nincs ennyi készleten!");
                continue;
            }

            sugar.quantity -= correctQuantity;
            var newOrderId = orders.Max(o => o.orderID) + 1;
            var totalPrice = sugar.price * correctQuantity;

            var newOrder = new Order
            {
                orderID = newOrderId,
                sugarID = correctSugarId,
                quantity = correctQuantity,
                total = totalPrice
            };

            orders.Add(newOrder);

            SaveSugar(sugars);
            SaveOrder(orders);
            SaveFileToJson(sugars, orders);

            Console.WriteLine("Totál ár: " + totalPrice);
            break;

        default: Console.WriteLine("0, 1, 2 az elfogadható számok!"); break;
    }
}

void SaveSugar(List<Sugar> sugars)
{
    var lines = new List<string>();
    foreach (var sugar in sugars)
    {
        lines.Add($"{sugar.id}, {sugar.name}, {sugar.price}, {sugar.quantity} ");
    }
    File.WriteAllLines("Sugars.csv", lines);
}

void SaveOrder(List<Order> orders)
{
    var lines = new List<string>();
    foreach (var order in orders)
    {
        lines.Add($"{order.orderID}, {order.sugarID}, {order.quantity}, {order.total} ");
    }
    File.WriteAllLines("Orders.csv", lines);
}

void SaveFileToJson(List<Sugar> sugars, List<Order> orders)
{
    var value = new { orders, sugars };
    var json = JsonSerializer.Serialize(value);
    File.WriteAllText("teszt.json", json);
} 