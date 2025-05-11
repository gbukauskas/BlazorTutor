using System.Collections.Generic;
using System.Diagnostics;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class StateData: IStateData
    {
        private static readonly Lock _stateLock = new();
        private Dictionary<string, StateForComponent> States { get; set; } = new();

        public StateForComponent GetState(string index, int buttonCount = 10, int pageSize = 9)
        {
            StateForComponent rzlt;
            lock (_stateLock)
            {
                if (this.States.ContainsKey(index))    // TryGetValue(index, out rzlt))
                {
                    rzlt = (StateForComponent)this.States[index].Clone();
                }
                else
                {
                    rzlt = new StateForComponent(index, buttonCount, pageSize);
                    this.States[index] = rzlt;
                }
            }
            return rzlt;
        }

        public void SetState(StateForComponent currentState)
        {
            Debug.Assert(currentState != null && !String.IsNullOrEmpty(currentState.ComponentName));
            lock (_stateLock)
            {
                if (this.States.ContainsKey(currentState.ComponentName))
                {
                    States[currentState.ComponentName] = (StateForComponent)currentState.Clone();
                }
                else
                {
                    States.Add(currentState.ComponentName, (StateForComponent)currentState.Clone());
                }
            }
        }
    }

    public class StateForComponent: ICloneable
    {
        public string? ComponentName { get; set; }
        public string? SortState {  get; set; } = string.Empty;
        public PgPostData? PagerState { get; set; } = null;
        public string? FilterState { get; set; } = string.Empty;


        public StateForComponent(string componentName, int buttonCount = 10, int pageSize = 9) 
        {
            this.ComponentName = componentName;
            this.SortState = string.Empty;
            this.PagerState = new PgPostData(componentName, buttonCount, 1, 1, pageSize, "Customers/page");

        }

        public object Clone()
        {
            Debug.Assert(this.ComponentName != null);
            var rzlt = new StateForComponent(this.ComponentName)
            {
                SortState = this.SortState,
                FilterState = this.FilterState
            };
            if (this.PagerState != null)
            {
                rzlt.PagerState = (PgPostData)this.PagerState.Clone();
            }
            return rzlt;
        }
    }
}
