using System;
using System.Collections.Generic;

namespace WowSuite.Language
{
    public class ResourceProvider
    {
        private readonly static object InstantiateSync = new object();
        private static ResourceProvider _instance;

        private Dictionary<TextResource, string> _currentDictionary;
        private Languages _language;

        private bool _dictionarySelected;
        private IResourceSet<Languages, Dictionary<TextResource, string>> _textResourcesSet;

        public ResourceProvider()
        {
        }

        public IResourceSet<Languages, Dictionary<TextResource, string>> TextResourcesSet
        {
            get { return _textResourcesSet; }
            set
            {
                if (!value.Validate())
                {
                    throw new InvalidOperationException("Набор ресурсов не прошёл валидацию.");
                }
                _textResourcesSet = value;
                _dictionarySelected = false;
            }
        }

        public Languages Languages
        {
            get { return _language; }
            set
            {
                _language = value;
                SelectDictionary(value);
            }
        }

        /// <summary>
        /// Возвращает единственный экземпляр класса. Свойство потокобезопасно.
        /// </summary>
        public static ResourceProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstantiateSync)
                    {
                        if (_instance == null)
                        {
                            _instance = new ResourceProvider();
                        }
                    }
                }

                return _instance;
            }
        }

        public string Get(TextResource resource)
        {
            if (!_dictionarySelected)
            {
                SelectDictionary(_language);
            }
            return _currentDictionary[resource];
        }

        public string Get(TextResource resource, Languages language)
        {
            return _textResourcesSet.Get(language)[resource];
        }

        private void SelectDictionary(Languages language)
        {
            if (TextResourcesSet != null)
            {
                _currentDictionary = TextResourcesSet.Get(language);
                _dictionarySelected = true;
            }
            else
            {
                _dictionarySelected = false;
            }
        }
    }
}