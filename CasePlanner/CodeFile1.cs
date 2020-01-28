using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace CasePlanner
{
    public interface IActionInfo
    {
        string Label { get; set; }
        string Note { get; set; }
        DateTime date { get; set; }
        void ShowAction();
    }


    [Serializable]
    abstract class CompleteAction
    {
        public bool isComplete { get; protected set; }
        protected virtual bool CheckTimeToEnd() { return false; }
    }

    [Serializable]
    class Action : CompleteAction, IActionInfo
    {
        public string Label { get; set; }
        public string Note { get; set; }
        public DateTime date { get; set; }

        public Action()
        {
            Console.WriteLine("Добавить заголовок");
            Label = Console.ReadLine();
            SetDate();
            SetNote();
        }

        public void ShowAction()
        {
            Console.WriteLine("Заголовок: " + Label);
            Console.WriteLine("Дата: " + date);
            Console.WriteLine("Пометка к записи: " + Note);
            isComplete = CheckTimeToEnd();
            Console.WriteLine("Статус дела: {0} ",isComplete?"Непросроченно":"Просроченно");
        }

       
        protected override bool CheckTimeToEnd()
        {
            if (DateTime.Now <= date)
                return true;

            return false;
        }

        public void SetDate()
        {
            Console.WriteLine("Введите дату ");
            Console.WriteLine("Пример:  01.01.2001 10:10:25");
            bool result = false;
            while (!result)
            {
                try
                {
                    date = DateTime.Parse(Console.ReadLine());
                    result = true;
                }
                catch
                {
                    Console.WriteLine("Ошибка даты введите заново");
                }
            }
        }

        public void SetNote()
        {
            Note = "";
            Console.WriteLine("Введите заметку к делу для выхода Введите 'Exit'");
            while (true)
            {
                string str = Console.ReadLine();
                if (str.ToLower() == "exit")
                {
                   if(Note == "")
                   {
                        Note = "Пусто";
                   }
                    break;
                }

                Note += str + "\n";

            }
            Note = Note.TrimEnd('\n');
        }

    }

    [Serializable]
    class ActionList
    {
        static bool isChange;

        List<Action> ListAction = new List<Action>();


        public ActionList()
        {
            LoadFromFile();
            isChange = false;
        }

        private void LoadFromFile()
        {
            if (File.Exists("Base.dat") && File.ReadAllText("Base.dat").Length > 0)
            {
                var binFile = new SoapFormatter();
                using (var file = new FileStream("Base.dat", FileMode.OpenOrCreate))
                {
                    file.Seek(0, SeekOrigin.Begin);
                    var task = (Action[])binFile.Deserialize(file);
                    ListAction = new List<Action>(task);
                }
            }
        }
        
        private void SaveInFile()
        {
                var binFile = new SoapFormatter();
                using (var file = new FileStream("Base.dat", FileMode.OpenOrCreate))
                {
                    binFile.Serialize(file, ListAction.ToArray());
                }
                isChange = false;
            
        }

        public void Add()
        {
            isChange = true;
            ListAction.Add(new Action());
            Console.WriteLine("Запись добавлена");
            Sort();
        }
        
        public void ShowAction()
        {
            if(ListAction.Count == 0)
            {
                Console.WriteLine("Записей нет");
                return;
            }
            for(int i = 0;i < ListAction.Count;i++)
            {
                Console.WriteLine("{" + $"{i + 1}" + "}");
                ListAction[i].ShowAction();
                Console.WriteLine();
            }
        }

        public void Remove()
        {
            isChange = true;
            Console.WriteLine("введите номер записи которую хотите удалить иначе введите 0");
            Console.WriteLine();
            ShowAction();
            int num = Int32.Parse(Console.ReadLine());
            Console.WriteLine();

            if(num > 0)
            {
                ListAction.RemoveAt(num -1);
                Console.WriteLine("Запись удалена!");
            }
        }

        public void RemakeAction()
        {
            isChange = true;
            ShowAction();
            Console.WriteLine();
            Console.WriteLine("введите номер записи которую хотите изменить иначе введите 0");
            int num = Int32.Parse(Console.ReadLine());
            Console.WriteLine();
            if (num > 0)
            {
                Console.WriteLine("Введите пункты по порядку без пробелов котрые хотите изменить котрые хотите изменить \n" + "1 - Изменить заголовок \n" + "2 - изменить дату\n" + "3 - изменить даполнительную информацию\n");
                string point = Console.ReadLine();
                var arr = point.ToCharArray(); 
                for(int i =0;i < arr.Length-1;i++)
                    for(int j = i+1;j <arr.Length;j++)
                    {
                        if(arr[i]>arr[j])
                        {
                            var ch = arr[i];
                            arr[i] = arr[j];
                            arr[j] = ch;
                        }
                    }
                point = new string(arr);
                switch(point)
                {
                    case "1":
                        Console.WriteLine("Введите заголовок");
                        ListAction[num - 1].Label = Console.ReadLine(); 
                        break;
                    case "2":
                        ListAction[num - 1].SetDate();
                        break;
                    case "3":
                        ListAction[num - 1].SetNote();
                        break;
                    case "12":
                        Console.WriteLine("Введите заголовок");
                        ListAction[num - 1].Label = Console.ReadLine();
                        ListAction[num - 1].SetDate();
                        break;
                    case "13":
                        Console.WriteLine("Введите заголовок");
                        ListAction[num - 1].Label = Console.ReadLine();
                        ListAction[num - 1].SetNote();
                        break;
                    case "23":
                        ListAction[num - 1].SetDate();
                        ListAction[num - 1].SetNote();
                        break;
                    case "123":
                        Console.WriteLine("Введите заголовок");
                        ListAction[num - 1].Label = Console.ReadLine();
                        ListAction[num - 1].SetDate();
                        ListAction[num - 1].SetNote();
                        break;
                }
            }
        }

        public void CloseList()
        {
            if (isChange)
            {
                SaveInFile();
            }
        }

        private void Sort()
        {
            isChange = true;
            for (int i = 0; i < ListAction.Count - 1; i++)
                for (int j = i + 1; j < ListAction.Count; j++)
                {
                    if (ListAction[i].date < ListAction[j].date)
                    {
                        var ch = ListAction[i];
                        ListAction[i] = ListAction[j];
                        ListAction[j] = ch;
                    }
                }
        }
    }
}