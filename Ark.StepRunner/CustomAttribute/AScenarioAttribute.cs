﻿using System;

namespace Ark.StepRunner.CustomAttribute
{
    //[AttributeUsage(AttributeTargets.Parameter)]
    [AttributeUsage(AttributeTargets.Class)]
    public class AScenarioAttribute : System.Attribute
    {
        private readonly string _description;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public AScenarioAttribute(string description)
        {
            _description = description;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}