﻿using JJMasterData.Commons.Dao;
using JJMasterData.Commons.Extensions;
using JJMasterData.Core.DataDictionary;
using System.Collections;

namespace JJMasterData.Core.DataManager
{
    public class ExpressionOptions
    {
        /// <summary>
        /// User specified values.
        /// Use to replace values that support expressions.
        /// </summary>
        /// <remarks>
        /// Key = Field name, Value= Field value
        /// </remarks>
        public Hashtable UserValues { get; set; }

        public Hashtable FormValues { get; set; }

        public PageState PageState { get; set; }
        public IDataAccess DataAccess { get; set; }

        public ExpressionOptions(Hashtable userValues, Hashtable formValues, PageState pageState, IDataAccess dataAccess)
        {
            UserValues = userValues.DeepCopy();
            FormValues = formValues;
            PageState = pageState;
            DataAccess = dataAccess;
        }
    }
}
