using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers
{
	//meta! id="2"
	public class SurroundingsManager : Manager
	{
		public SurroundingsManager(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="ModelAgent", id="49", type="Notice"
		public void ProcessPatientExit(MessageForm message)
		{
		}

		//meta! sender="ArrivalsScheduler", id="32", type="Notice"
		public void ProcessNewArrival(MessageForm message)
		{
			message.Addressee = MySim.FindAgent(SimId.ModelAgent);
			message.Code = Mc.PatientArrival;
			Notice(message);
		}

		//meta! sender="ArrivalsScheduler", id="31", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code) {
				case Mc.Initialization: {
					message.Addressee = MyAgent.FindAssistant(SimId.ArrivalsScheduler);
					StartContinualAssistant(message);
					break;
				}
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.NewArrival:
				ProcessNewArrival(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			case Mc.PatientExit:
				ProcessPatientExit(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new SurroundingsAgent MyAgent
		{
			get
			{
				return (SurroundingsAgent)base.MyAgent;
			}
		}
	}
}