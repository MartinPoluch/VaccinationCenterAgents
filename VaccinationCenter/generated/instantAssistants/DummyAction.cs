using OSPABA;
using simulation;
using agents;
namespace instantAssistants
{
	//meta! id="121"
	public class DummyAction : Action
	{
		public DummyAction(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void Execute(MessageForm message)
		{
		}
		public new ModelAgent MyAgent
		{
			get
			{
				return (ModelAgent)base.MyAgent;
			}
		}
	}
}