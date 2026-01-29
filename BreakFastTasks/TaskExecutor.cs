using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakFastTasks
{
    class TaskExecutor
    {
        #region תכונות
        private string name;
        private int timeInMiliSec;
        #endregion

        #region Properties
        public string Name { get { return this.name; } }
        #endregion

        public event EventHandler<TaskEventArgs> OnProgress;

        //בנאי
        public TaskExecutor(string name, int ms)
        {
            this.timeInMiliSec = ms;
            this.name = name;
        }

      
       
        
        //מחקה זמן ביצוע פעולה
        public void Start()
        {
            for (int i = 1; i <= 10; i++)
            {
                Thread.Sleep(this.timeInMiliSec / 10);
                TaskEventArgs args= new TaskEventArgs { progressPercent = i * 10 };
				OnProgessUpdete(args);

			}
           
        }
		public async Task StartAsync()
		{
			for (int i = 1; i <= 10; i++)
			{
				TaskEventArgs args = new TaskEventArgs { progressPercent = i * 10 };
				OnProgessUpdete(args);
				await Task.Delay(this.timeInMiliSec / 10);

			}
		}


		private void OnProgessUpdete(TaskEventArgs args)
		{
            OnProgress?.Invoke(this, args);
		}
	}

    class Omlette : TaskExecutor
    {
        public Omlette(string name) : base(name, 12000)
        {

        }

		
	}

    class Cucumber : TaskExecutor
    {
        public Cucumber(string name) : base(name, 5000)
        { }
    }

    class Tomato : TaskExecutor
    {
        public Tomato(string name) : base(name, 1000)
        { }
    }

    class Toast : TaskExecutor
    {
        public Toast(string name) : base(name, 7000)
        {

        }
    }

}

