using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BreakFastTasks;

namespace CoreCollectionsAsync
{
    class SimpleBreakfast
    {
        public static DateTime startTime;

        /// <summary>
        /// דוגמה להכנת ארוחת בוקר באופן סינכרוני (ללא אסינכרוניות)
        /// </summary>
        public static void MakeBreakfastDemo_1()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            DateTime start = DateTime.Now;

            // הכנת חביתה
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the Omlette at {startTime.ToString()}");
            Omlette myOmlette = new Omlette("myOmlette");
            myOmlette.OnProgress += Progress; // מאזינים להתקדמות

            myOmlette.Start(); // התחלת הכנת החביתה (סינכרוני)

            // הכנת טוסט
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the taskMakeToast at {startTime.ToString()}");

            Toast toast = new Toast("taskMakeToast");
            toast.OnProgress += Progress;
            toast.Start(); // התחלת הכנת הטוסט (סינכרוני)

            // הכנת מלפפון ראשון
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the first cucumber at {startTime.ToString()}");

            Cucumber cucumber1 = new Cucumber("first cucumber");
            cucumber1.OnProgress += Progress;
            cucumber1.Start();

            // הכנת מלפפון שני
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the second cucumber at {startTime.ToString()}");

            Cucumber cucumber2 = new Cucumber("second cucumber");
            cucumber1.OnProgress += Progress; // שימו לב: כאן יש טעות, צריך cucumber2.OnProgress
            cucumber2.Start();

            // הכנת עגבנייה
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the tomato at {startTime.ToString()}");
            Tomato tomato = new Tomato("tomato");
            tomato.OnProgress += Progress;
            Console.WriteLine();

            tomato.Start();

            stopWatch.Stop();
            Console.WriteLine($"Breakfast is ready after {stopWatch.Elapsed.TotalSeconds} seconds");
            DateTime end = DateTime.Now;
            TimeSpan length = end - start;
            Console.WriteLine($"Breakfast is ready at {end.ToString()}");
            Console.WriteLine($"Total time in seconds: {length.TotalSeconds}");
        }

        // פונקציה שמטפלת באירוע התקדמות של משימה
        static void Progress(object? sender, TaskEventArgs e)
        {
            if (sender is TaskExecutor)
            {
                TaskExecutor obj = (TaskExecutor)sender;
                Console.WriteLine($"Progress for {obj.Name}: {e.progressPercent}%");
            }
        }

        // פונקציה שמטפלת באירוע סיום של משימה
        static void Finish(Object? sender, EventArgs e)
        {
            if (sender is TaskExecutor)
            {
                TaskExecutor obj = (TaskExecutor)sender;
                Console.WriteLine($"\n{obj.Name} is ready!");
            }
        }

        /// <summary>
        /// דוגמה להכנת ארוחת בוקר באופן אסינכרוני (באמצעות async/await)
        /// </summary>
        public async static Task MakeBreakfastDemo_2_Async()
        {
            List<Task> tasks = new List<Task>();
            Console.WriteLine("Start Breakfast");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            DateTime start = DateTime.Now;

            // 1. הכנת חביתה בצורה אסינכרונית
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the omlette at {startTime.ToString()}");
            Task om = PrepareOmletteAsync(); // קריאה לפונקציה אסינכרונית שמחזירה משימה
            tasks.Add(om);

            // 2. במקביל, הכנת טוסט בצורה אסינכרונית
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the taskMakeToast at {startTime.ToString()}");
            Task<Toast> taskMakeToast = PrepareToastAsync(); // קריאה לפונקציה אסינכרונית שמחזירה טוסט
            tasks.Add(taskMakeToast);

            // 3. הכנת סלט (מלפפונים ועגבנייה) - כאן הפעולות סינכרוניות
            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the first cucumber at {startTime.ToString()}");
            PrepareCucumber();

            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the second cucumber at {startTime.ToString()}");
            PrepareCucumber();

            startTime = DateTime.Now;
            Console.WriteLine($"Start preparing the tomato at {startTime.ToString()}");
            PrepareTomato();

            Console.WriteLine();

            // המתנה לסיום כל המשימות האסינכרוניות (חביתה וטוסט)
            await Task.WhenAll(tasks);

            stopWatch.Stop();
            Console.WriteLine($"Breakfast is ready after {stopWatch.Elapsed.TotalSeconds} seconds");
            DateTime end = DateTime.Now;
            TimeSpan length = end - start;
            Console.WriteLine($"Breakfast is ready at {end.ToString()}");
            Console.WriteLine($"Total time in seconds: {length.TotalSeconds}");

            // קבלת תוצאה מהטוסט (await על משימה שמחזירה ערך)
            Toast t = await taskMakeToast;
            Console.WriteLine(t.Name);
        }

        /// <summary>
        /// הכנת מלפפון (סינכרוני)
        /// </summary>
        private static void PrepareCucumber()
        {
            Console.WriteLine("Start Cutting Cucumber");
            Cucumber cucumber = new Cucumber("myCucumber");
            cucumber.OnProgress += Progress;
            cucumber.Start();
            Console.WriteLine("Cucumber Finished");
        }

        /// <summary>
        /// הכנת עגבנייה (סינכרוני)
        /// </summary>
        private static void PrepareTomato()
        {
            Console.WriteLine("Start Cutting Tomato");
            Tomato tomato = new Tomato("myTomato");
            tomato.OnProgress += Progress;
            tomato.Start();
            Console.WriteLine("Tomato Finished");
        }

        /// <summary>
        /// הכנת טוסט בצורה אסינכרונית
        /// </summary>
        private static async Task<Toast> PrepareToastAsync()
        {
            Console.WriteLine("Start Prepare Toast");
            Toast toast = new Toast("myToast");
            toast.OnProgress += Progress;
            await toast.StartAsync(); // קריאה אסינכרונית
            Console.WriteLine("Toast Finished");
            return toast;
        }

        /// <summary>
        /// הכנת חביתה בצורה אסינכרונית
        /// </summary>
        private static async Task PrepareOmletteAsync()
        {
            Console.WriteLine("Start Prepare Omlette");
            Omlette omelette = new Omlette("myOmlete");
            omelette.OnProgress += Progress;
            await omelette.StartAsync(); // קריאה אסינכרונית
            Console.WriteLine("Omlette Finished");
        }
    }
}
