﻿namespace Ark.StepRunner.CustomAttribute
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotNullAttribute : Attribute
    {

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public NotNullAttribute()
        {
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}