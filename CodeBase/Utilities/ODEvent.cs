using System;

namespace CodeBase {
	///<summary>Helper class to allow multiple areas of the program to subscribe to various events which they care about.</summary>
	public class ODEvent {
		///<summary>Occurs when any developer calls Fire().  Can happen from anywhere in the program.
		///Consumers of "global" ODEvents need to register for this handler because this will be the event that mainly gets fired.</summary>
		public static event ODEventHandler Fired;

		///<summary>Triggers the global Fired event to get called with the passed in ODEventArgs.</summary>
		public static void Fire(ODEventArgs e) {
			Fired?.Invoke(e);
		}
	}

	///<summary>Arguments specifically designed for use in ODEvent.</summary>
	public class ODEventArgs {
		private string _name;
		private object _tag;

		///<summary>Used to uniquly identify this ODEvent for consumers.</summary>
		public string Name {
			get {
				return _name;
			}
		}

		///<summary>A generic object related to the event, such as a Commlog object.  Can be null.</summary>
		public object Tag {
			get {
				return _tag;
			}
		}

		///<summary>Used when an ODEvent is needed but no object is needed in the consuming class.  
		///Set name to something unique that the consumer can explicitly look for.  E.g. a consuming form might look for "FormProgressStatus".</summary>
		public ODEventArgs(string name) : this(name,null) {
		}

		///<summary>Creates an ODEventArg with the name and object.
		///Set name to something unique that the consumer can explicitly look for.  E.g. a consuming form might look for "FormProgressStatus".
		///Tag can be set to anything that the consumer may need.  E.g. a string for FormProgressStatus to show to users.</summary>
		public ODEventArgs(string name,object tag) {
			_name=name;
			_tag=tag;
		}
	}

	///<summary>Only used for ODEvent.  Not necessary to reference this delegate directly.</summary>
	public delegate void ODEventHandler(ODEventArgs e);

}
