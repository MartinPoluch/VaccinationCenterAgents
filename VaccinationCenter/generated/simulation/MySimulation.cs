using OSPABA;
using agents;
using VaccinationCenter.models;

namespace simulation
{
	public class MySimulation : Simulation
	{
		public MySimulation()
		{
			Init();
		}

		private SimParameter SimParameter { get; set; }

		protected override void PrepareSimulation()
		{
			base.PrepareSimulation();
			//RegistrationAgent.Initialization(SimParameter);
		}

		protected override void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
		}

		protected override void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();
		}

		protected override void SimulationFinished()
		{
			// Dysplay simulation results
			base.SimulationFinished();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			ModelAgent = new ModelAgent(SimId.ModelAgent, this, null);
			SurroundingsAgent = new SurroundingsAgent(SimId.SurroundingsAgent, this, ModelAgent);
			VacCenterAgent = new VacCenterAgent(SimId.VacCenterAgent, this, ModelAgent);
			VaccinationAgent = new VaccinationAgent(SimId.VaccinationAgent, this, VacCenterAgent);
			ExaminationAgent = new ExaminationAgent(SimId.ExaminationAgent, this, VacCenterAgent);
			RegistrationAgent = new RegistrationAgent(SimId.RegistrationAgent, this, VacCenterAgent);
			LunchAgent = new LunchAgent(SimId.LunchAgent, this, VacCenterAgent);
			WaitingAgent = new WaitingAgent(SimId.WaitingAgent, this, VacCenterAgent);
			MovementAgent = new MovementAgent(SimId.MovementAgent, this, VacCenterAgent);
		}
		public ModelAgent ModelAgent
		{ get; set; }
		public SurroundingsAgent SurroundingsAgent
		{ get; set; }
		public VacCenterAgent VacCenterAgent
		{ get; set; }
		public VaccinationAgent VaccinationAgent
		{ get; set; }
		public ExaminationAgent ExaminationAgent
		{ get; set; }
		public RegistrationAgent RegistrationAgent
		{ get; set; }
		public LunchAgent LunchAgent
		{ get; set; }
		public WaitingAgent WaitingAgent
		{ get; set; }
		public MovementAgent MovementAgent
		{ get; set; }
		//meta! tag="end"
	}
}