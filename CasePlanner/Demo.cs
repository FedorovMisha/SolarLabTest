using System;

namespace CasePlanner
{
    class Demo
    {
        static void Main()
        {
            ActionList list = new ActionList();
            while(true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1 - Добавить запись");
                Console.WriteLine("2 - Показать записи");
                Console.WriteLine("3 - Удалить запись");
                Console.WriteLine("4 - Изменить запись");
                Console.WriteLine("5 - Exit");
                Console.WriteLine("Команда =>");
                var i = Console.ReadLine();
                Console.Clear();
                switch(i)
                {
                    case "1":
                        list.Add();
                        break;
                    case "2":
                        list.ShowAction();
                        break;
                    case "3":
                        list.Remove();
                        break;
                    case "4":
                        list.RemakeAction();
                        break;
                    case "5":
                        list.CloseList();
                        return;
                    default:
                        Console.WriteLine("Такой команды не существует");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }

        }
    }
}
