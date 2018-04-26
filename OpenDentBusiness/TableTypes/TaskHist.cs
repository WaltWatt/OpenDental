using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>Inherits from task. A historical copy of a task.  These are generated as a result of a task being edited.  When creating for insertion it needs a passed in Task object.</summary>
	//[IsHistTable=Task]
	[Serializable]
	public class TaskHist:Task {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long TaskHistNum;
		///<summary>FK to userod.UserNum  Identifies the user that changed this task from this state, not the person who originally wrote it.</summary>
		public long UserNumHist;
		///<summary>The date and time that this task was edited and added to the Hist table.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTStamp;
		///<summary>True if the note was changed when this historical copy was created.</summary>
		public bool IsNoteChange;

		///<summary>Pass in the old task that needs to be recorded.</summary>
		public TaskHist(Task task) {
			this.DateTask=task.DateTask;
			this.DateTimeEntry=task.DateTimeEntry;
			this.DateTimeFinished=task.DateTimeFinished;
			this.DateType=task.DateType;
			this.Descript=task.Descript;
			this.FromNum=task.FromNum;
			this.IsRepeating=task.IsRepeating;
			this.IsUnread=task.IsUnread;
			this.KeyNum=task.KeyNum;
			this.ObjectType=task.ObjectType;
			this.ParentDesc=task.ParentDesc;
			this.PatientName=task.PatientName;
			this.PriorityDefNum=task.PriorityDefNum;
			this.TaskListNum=task.TaskListNum;
			this.TaskNum=task.TaskNum;
			this.TaskStatus=task.TaskStatus;
			this.UserNum=task.UserNum;
		}

		public TaskHist() {
		}

		///<summary>Overrides Task.Copy() which is desired behavior because TaskHist extends Task.</summary>
		public new TaskHist Copy() {
			return (TaskHist)MemberwiseClone();
		}
	}
}
