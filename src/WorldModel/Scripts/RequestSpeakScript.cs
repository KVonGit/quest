﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextAdventures.Quest.Functions;

/*
 * This script command is an alternative to request (Speak, "some text"), and is added as part of deprecating
 * request. It is called requestspeak to replace RequestSpeak (an earlier step to deprecation), and
 * to hopefully avoid name clashes in existing games.
 */

namespace TextAdventures.Quest.Scripts
{
    public class RequestSpeakScriptConstructor : ScriptConstructorBase
    {
        #region ScriptConstructorBase Members

        public override string Keyword
        {
            get { return "requestspeak"; }
        }

        protected override IScript CreateInt(List<string> parameters, ScriptContext scriptContext)
        {
            return new RequestSpeakScript(scriptContext, new ExpressionGeneric(parameters[0], scriptContext));
        }

        protected override int[] ExpectedParameters
        {
            get { return new int[] { 1 }; }
        }
        #endregion
    }

    public class RequestSpeakScript : ScriptBase
    {
        private ScriptContext m_scriptContext;
        private WorldModel m_worldModel;
        private IFunctionGeneric m_function;

        public RequestSpeakScript(ScriptContext scriptContext, IFunctionGeneric function)
        {
            m_scriptContext = scriptContext;
            m_worldModel = scriptContext.WorldModel;
            m_function = function;
        }

        protected override ScriptBase CloneScript()
        {
            return new RequestSpeakScript(m_scriptContext, m_function.Clone());
        }

        public override void Execute(Context c)
        {
            object result = m_function.Execute(c);
            m_worldModel.PlayerUI.Speak(result.ToString());
        }

        public override string Save()
        {
            return SaveScript("requestspeak", m_function.Save());
        }

        public override object GetParameter(int index)
        {
            return m_function.Save();
        }

        public override void SetParameterInternal(int index, object value)
        {
            m_function = new ExpressionGeneric((string)value, m_scriptContext);
        }

        public override string Keyword
        {
            get
            {
                return "requestspeak";
            }
        }
    }
}
