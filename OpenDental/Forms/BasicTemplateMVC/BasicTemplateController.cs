using CodeBase.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDental {
	public class BasicTemplateController:ODControllerAbs<BasicTemplateView> {
		///<summary>Required constructor so that the base class can associate our concrete view to the base _view variable.</summary>
		public BasicTemplateController(BasicTemplateView view) : base(view) {
		}
	}
}
