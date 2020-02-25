using System;
using System.Collections.Generic;

namespace WowSuite.Language
{
    public enum Languages
    {
        enUS,
        frFRA,
        esESP,
        itITA,
        deDEU,
        czCZ,
        trTR,
        ruRU
    }

    public enum TextResource
    {
        PLAY,
        STARTING,
        ERROR_BATTLENET,
        ONLINE,
        OFFLINE,
        UPDATING,
        DOWNLOAD_STATUS,
        REGISTER,
        ACCOUNT,
        FORUM,
        REALMLISTBTN,
        CLEARCACHEBTN,
        HOTNEWS,
        UPDSTART,
        UPDCOMPLETE,
        NEWSBANNERERROR,
        EXENOTFOUND,
        ERRORSTARTINGGAME,
        CACHEOK,
        CACHENO,
        REALMLISTOK,
        REALMLISTNO,
        CHARINFOERR,
        CHARLISTERR,
        SETTINGOK,
        SETTINGERR,
        TOOLSTATONLINE,
        TOOLSETTINGS,
        TOOLMINI,
        TOOLCLOSE,
        SAVEBTN,
        SETTLABEL1,
        SETTLABEL2,
        LABELAUTOLOG,
        SETLOGIN,
        SETPASSWORD,
        SETHEADER,
        ENTLOG,
        ENTPASS,
        TABMENU,
        TABMENU1,
        TABMENU2,
        CLEARTOOLTIP,
        CHANGELOC,
        ENTWOWEXE,
        BTNCLEAR,
        EXITBTN,
        SEARCHLABEL,
        SEARCHCHAR,
        HIDELIST,
        SHOWLIST,
        DOUBLECLICKCHAR,
        GENINFO,
        LEVELCHAR,
        FACTIONCHAR,
        CLASSCHAR,
        PVPCHAR,
        QUESTCHAR,
        ERRORLOGIN,
        NOTLOGORPASS,
        LOGINBTN,
        FORGOTPWD,
        REMEMBER,
        NOTACC,
        CREATEACC,
        ENTERLOGIN,
        LOGINEMPTY,
        PASSEMPTY,
        RESETSETT,
        CANCELBTN,
        POPUPMSG,
        RESETMSG,
        RESETMSGERR,
        RESTARTBTN,
        RESTARTLATERBTN,
        POPUPMSGRESTART,
        WELCBLOCK,
        WELCDESCR,
        WELCBTN,
        WELCBTNSET,
        GAMESBTN,
        NEWSBTN,
        ACCMENU,
        FORUMMENU,
        SETTMENU,
        RELOGMENU,
        EXITMENU,
        LANGUAGEMENU,
        GENERALSETLABEL,
        COPYAPP,
        ALLREALMS,
        CONFTEXT,
        OPENGAMECONF,
        SETTINGSBTN,
        SHOPBTN, 
        FORUMBTN,
        CHANGEGAMELANG,
        ERRORLOGINORPASSWORD,
        CHECKONECHEKBOX,
        BANNED,
        CHECKFILE
    }

    public abstract class TextResourceSetBase : IResourceSet<Languages, Dictionary<TextResource, string>>
    {
        private readonly Dictionary<Languages, Dictionary<TextResource, string>> _dictionaries;

        protected TextResourceSetBase()
        {
            _dictionaries = new Dictionary<Languages, Dictionary<TextResource, string>>();
        }

        public Dictionary<TextResource, string> Get(Languages language)
        {
            return _dictionaries[language];
        }

        public void Add(Languages language, Dictionary<TextResource, string> set)
        {
            _dictionaries.Add(language, set);
        }

        public bool Validate()
        {
            var names = Enum.GetValues(typeof(Languages));
            bool success = true;
            foreach (var name in names)
            {
                if (!_dictionaries.ContainsKey((Languages)name))
                {
                    success = false;
                }
            }

            return success;
        }
    }
}