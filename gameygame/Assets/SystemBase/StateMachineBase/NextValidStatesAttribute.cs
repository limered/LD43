using System;

namespace SystemBase.StateMachineBase
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NextValidStatesAttribute : Attribute
    {
        public NextValidStatesAttribute(params Type[] validStateChanges)
        {
            ValidStateChanges = validStateChanges;
        }

        public NextValidStatesAttribute()
        {
            ValidStateChanges = new Type[0];
        }

        public Type[] ValidStateChanges { get; set; }
    }
}