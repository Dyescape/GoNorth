using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace GoNorth.Data.Exporting
{
    /// <summary>
    /// Export Default Template Provider
    /// </summary>
    public class ExportDefaultTemplateProvider : IExportDefaultTemplateProvider
    {
        /// <summary>
        /// Default Export Template Folder
        /// </summary>
        private const string DefaultExportTemplateFolder = "DefaultExportTemplates";

        /// <summary>
        /// Default Export Template Extension
        /// </summary>
        private const string DefaultExportTemplateExtension = ".lua";

        /// <summary>
        /// Default Language Export Template Extension
        /// </summary>
        private const string DefaultLanguageExportTemplateExtension = ".ini";


        /// <summary>
        /// Default Template Code
        /// </summary>
        private class DefaultTemplateEntry
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="category">Category</param>
            /// <param name="templateType">Template Type</param>
            /// <param name="isLanguage">true if the template is for a language file, else false</param>
            public DefaultTemplateEntry(TemplateCategory category, TemplateType templateType, bool isLanguage = false)
            {
                Category = category;
                TemplateType = templateType;
                IsLanguage = isLanguage;
            }

            /// <summary>
            /// Template Category
            /// </summary>
            public TemplateCategory Category { get; set; }

            /// <summary>
            /// Template Type
            /// </summary>
            public TemplateType TemplateType { get; set; }

            /// <summary>
            /// true if the template is for language, else false
            /// </summary>
            public bool IsLanguage { get; set; }
        };

        /// <summary>
        /// Default Templates
        /// </summary>
        private readonly List<DefaultTemplateEntry> _DefaultTemplates = new List<DefaultTemplateEntry>
        {
            // Objects
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectNpc),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectItem),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectSkill),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectAttributeList),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectInventory),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectSkillList),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectDailyRoutineEventList),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectDailyRoutineFunctionList),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectDailyRoutineFunction),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectExportSnippetFunction),
            new DefaultTemplateEntry(TemplateCategory.Object, TemplateType.ObjectStateFunction),

            // Tale
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleDialogFunction),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleNpcTextLine),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TalePlayerTextLine),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleChoice),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleCondition),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionPlayerValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionNpcValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionPlayerInventory),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionNpcInventory),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionQuestValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionQuestState),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionNpcAliveState),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionCurrentSkillValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionGameTime),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionPlayerSkillValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionNpcSkillValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionPlayerLearnedSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionPlayerNotLearnedSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionNpcLearnedSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionNpcNotLearnedSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionRandomValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionDailyRoutineEventDisabled),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionDailyRoutineEventEnabled),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionCode),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleConditionItemValue),
            
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleAction),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChangePlayerValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChangeNpcValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionTransferItemToPlayer),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSpawnItemForPlayer),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionRemoveItemFromPlayer),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionTransferItemToNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSpawnItemForNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionRemoveItemFromNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSpawnItemForChooseNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionRemoveItemFromChooseNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionNpcUseItem),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionPlayerUseItem),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChooseNpcUseItem),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChangeQuestValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSetQuestState),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionAddQuestText),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChangeCurrentSkillValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionWait),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSetPlayerState),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSetNpcState),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSetTargetNpcState),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionPlayerLearnSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionPlayerForgetSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionNpcLearnSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionNpcForgetSkill),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChangePlayerSkillValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChangeNpcSkillValue),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionPersistDialogState),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionOpenShop),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionNpcPlayAnimation),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionPlayerPlayAnimation),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionCodeAction),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionShowFloatingTextAboveNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionShowFloatingTextAbovePlayer),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionShowFloatingTextAboveChooseNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionFadeToBlack),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionFadeFromBlack),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSetGameTime),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionDisableDailyRoutineEvent),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionEnableDailyRoutineEvent),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionTeleportNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionTeleportPlayer),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionTeleportChooseNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionWalkNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionWalkChooseNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionTeleportNpcToNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionTeleportChooseNpcToNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionWalkNpcToNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionWalkChooseNpcToNpc),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSpawnNpcAtMarker),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionSpawnItemAtMarker),
            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.TaleActionChangeItemValue),

            new DefaultTemplateEntry(TemplateCategory.Tale, TemplateType.ReferenceNode),
            
            // General
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralLogicGroup),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralLogicAnd),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralLogicOr),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorEqual),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorNotEqual),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorLess),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorLessOrEqual),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorBigger),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorBiggerOrEqual),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorContains),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorStartsWith),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralCompareOperatorEndsWith),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralChangeOperatorAssign),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralChangeOperatorAddTo),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralChangeOperatorSubstractFrom),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralChangeOperatorMultiply),
            new DefaultTemplateEntry(TemplateCategory.General, TemplateType.GeneralChangeOperatorDivide),

            // Language
            new DefaultTemplateEntry(TemplateCategory.Language, TemplateType.LanguageFile, true)
        };


        /// <summary>
        /// Hosting Environment
        /// </summary>
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Template Db Access
        /// </summary>
        private readonly IExportTemplateDbAccess _templateDbAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostingEnvironment">Hosting Environment</param>
        /// <param name="templateDbAccess">Template Db Access</param>
        public ExportDefaultTemplateProvider(IWebHostEnvironment hostingEnvironment, IExportTemplateDbAccess templateDbAccess)
        {
            _hostingEnvironment = hostingEnvironment;
            _templateDbAccess = templateDbAccess;
        }

        /// <summary>
        /// Returns all default export templates by the category
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="category">Template Category</param>
        /// <returns>List of export templates</returns>
        public async Task<List<ExportTemplate>> GetDefaultTemplatesByCategory(string projectId, TemplateCategory category)
        {
            List<ExportTemplate> templates = await _templateDbAccess.GetDefaultTemplatesByCategory(projectId, category);
            
            await FilterAndDeleteInvalidTemplates(templates);
            await AddMissingDefaultTemplates(projectId, templates, category);
            
            return templates;
        }

        /// <summary>
        /// Returns the default export template by its type
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="templateType">Template Type</param>
        /// <returns>Export Template, NULL if no saved template exists</returns>
        public async Task<ExportTemplate> GetDefaultTemplateByType(string projectId, TemplateType templateType)
        {
            ExportTemplate template = await _templateDbAccess.GetDefaultTemplateByType(projectId, templateType);
            if(template == null)
            {
                DefaultTemplateEntry defaultTemplate = _DefaultTemplates.First(t => t.TemplateType == templateType);
                template = await CreateDefaultTemplate(projectId, defaultTemplate.Category, defaultTemplate.TemplateType, true);
            }

            return template;
        }


        /// <summary>
        /// Filters invalid templates from a list of templates
        /// </summary>
        /// <param name="templates">Templates</param>
        /// <returns>Task</returns>        
        private async Task FilterAndDeleteInvalidTemplates(List<ExportTemplate> templates)
        {
            for(int curTemplate = templates.Count - 1; curTemplate >= 0; --curTemplate)
            {
                if(!Enum.IsDefined(typeof(TemplateType), templates[curTemplate].TemplateType))
                {
                    await _templateDbAccess.DeleteTemplate(templates[curTemplate]);
                    templates.RemoveAt(curTemplate);
                }
            }
        }

        /// <summary>
        /// Adds missing default templates to a list of templates without loading the code
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="templates">Loaded templates</param>
        /// <param name="category">Category for which to add the entry</param>
        /// <returns>Task</returns>
        private async Task AddMissingDefaultTemplates(string projectId, List<ExportTemplate> templates, TemplateCategory category)
        {
            foreach(DefaultTemplateEntry curDefaultTemplate in _DefaultTemplates)
            {
                if(curDefaultTemplate.Category == category && !templates.Any(t => t.TemplateType == curDefaultTemplate.TemplateType))
                {
                    ExportTemplate defaultTemplate = await CreateDefaultTemplate(projectId, category, curDefaultTemplate.TemplateType, false);
                    templates.Add(defaultTemplate);
                }
            }
        }

        /// <summary>
        /// Creates a default template
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="category">Category</param>
        /// <param name="templateType">Template Type</param>
        /// <param name="loadCode">true if the code must be loaded, else false</param>
        /// <returns>Default Template</returns>
        private async Task<ExportTemplate> CreateDefaultTemplate(string projectId, TemplateCategory category, TemplateType templateType, bool loadCode)
        {
            ExportTemplate defaultTemplate = new ExportTemplate();
            defaultTemplate.Id = string.Empty;
            defaultTemplate.ProjectId = projectId;
            defaultTemplate.CustomizedObjectId = string.Empty;
            defaultTemplate.Category = category;
            defaultTemplate.TemplateType = templateType;
            defaultTemplate.Code = "";
            defaultTemplate.ModifiedBy = string.Empty;
            defaultTemplate.ModifiedOn = DateTimeOffset.MinValue;
            defaultTemplate.RenderingEngine = ExportTemplateRenderingEngine.Scriban;

            if(loadCode)
            {
                defaultTemplate.Code = await LoadDefaultTemplateCode(category, templateType);
            }

            return defaultTemplate;
        }

        /// <summary>
        /// Loads the default template code
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="templateType">Template Type</param>
        /// <returns>Default Template code</returns>
        private async Task<string> LoadDefaultTemplateCode(TemplateCategory category, TemplateType templateType)
        {
            string extension = DefaultExportTemplateExtension;
            if(IsTemplateTypeLanguage(templateType))
            {
                extension = DefaultLanguageExportTemplateExtension;
            }
            string scriptFile = Path.Combine(_hostingEnvironment.ContentRootPath, DefaultExportTemplateFolder, category.ToString(), templateType.ToString()) + extension;
            
            using(FileStream fileStream = new FileStream(scriptFile, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        
        /// <summary>
        /// Returns true if a template type is for a language file, else true
        /// </summary>
        /// <param name="templateType">Template Type</param>
        /// <returns>true if the template type is for a language file, else false</returns>
        public bool IsTemplateTypeLanguage(TemplateType templateType)
        {
            DefaultTemplateEntry templateEntry = _DefaultTemplates.FirstOrDefault(d => d.TemplateType == templateType);
            if(templateEntry == null)
            {
                return false;
            }

            return templateEntry.IsLanguage;
        }
    }
}