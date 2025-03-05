using System.Collections;

// Import UnityHFSM

namespace UnityHFSM.Samples.GuardAI
{
    public class GuardAI
    {
        public bool IsFight;
        public bool A2B_Condition;
        public bool B2A_Condition;
        
        // Declare the finite state machine
        public StateMachine fsm = new StateMachine();

        public float searchTime = 3; // in seconds


        void Start()
        {
            fsm.AddState("Apple", onEnter: (state => { ; } ));
            fsm.AddState("Banana", onEnter: (state => { ; } ));
            
            // fsm.OnLogic();
            if (true)
            {
                // 변화 조건
                fsm.AddTransition(
                    from: "Apple",
                    to: "Banana",
                    condition: A2B_GetCondition,
                    onTransition: t => {; },
                    afterTransition: t => {; }
                );
               
                // 변화 조건
                fsm.AddTransition(
                    from: "Banana",
                    to: "Apple",
                    condition: B2A_GetCondition,
                    onTransition: t => {; },
                    afterTransition: t => {; }
                );
            }

            if (true)
            {
                fsm.AddTriggerTransition("Event_1", "Apple", "Banana");
                fsm.AddTriggerTransition("Event_2", "Banana", "Apple");
            }

            fsm.SetStartState("Apple");
            fsm.Init();
            
        }
    
        private bool A2B_GetCondition(Transition<string> t)
        {
            return A2B_Condition;
        }

        private bool B2A_GetCondition(Transition<string> t)
        {
            return B2A_Condition;
        }

        void Update()
        {
            //fsm.OnLogic();
        }

        public void TriggerEnter()
        {
            fsm.Trigger("Event_1");
        }
        
        public void TriggerEnter2()
        {
            fsm.Trigger("Event_2");
        }
    }
}

