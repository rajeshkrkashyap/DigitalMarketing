using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared
{
    public class APIs
    {
        public const string Login = "api/auth/login";
        public const string Register = "api/auth/register";
        public const string RefreshToken = "api/auth/refreshtoken";
        
        public const string MobileLogin = "api/Auth/MobileLogin";
        public const string SMSSend = "api/SMS/send";

        public const string ConfirmEmail = "api/auth/ConfirmEmail";
        public const string GetUserByEmail = "api/auth/GetUserByEmail";
        public const string IsEmailConfirmedAsync = "api/auth/IsEmailConfirmedAsync";
        public const string GeneratePasswordResetTokenAsync = "api/auth/GeneratePasswordResetTokenAsync";
        public const string ResetPasswordAsync = "api/auth/ResetPasswordAsync";
        public const string AddToRoleAsync = "api/auth/AddToRoleAsync";
 
        public const string AppUserSettingList = "api/AppUserSetting/List";
        public const string AppSettingQueryActiveByAppUserIdAsync = "api/AppUserSetting/QueryActiveByAppUserIdAsync";
        public const string AppUserSettingGetById = "api/AppUserSetting/GetById";
        public const string AppUserSettingCreate = "api/AppUserSetting/Create";
        public const string AppUserSettingDelete = "api/AppUserSetting/Delete";
        public const string AppUserSettingUpdate = "api/AppUserSetting/Update";
        public const string AppUserSettingActivateQuery = "api/AppUserSetting/ActivateQuery";
        public const string AppUserSettingSetInputType = "api/AppUserSetting/SetInputType";

        public const string ServiceList = "api/Service/List";
        public const string ServiceGetById = "api/Service/GetById";
        public const string ServiceGetByName = "api/Service/GetByName";
        public const string ServiceCreate = "api/Service/Create";
        public const string ServiceDelete = "api/Service/Delete";
        public const string ServiceUpdate = "api/Service/Update";

        public const string RechargeList = "api/Recharge/List";
        public const string RechargeGetById = "api/Recharge/GetById";
        public const string RechargeGetByUserId = "api/Recharge/GetByUserId";
        public const string RechargeGetByRazorpayPaymentId = "api/Recharge/GetByRazorpayPaymentId";
        public const string RechargeGetByCurrency = "api/Recharge/GetByCurrency";
        public const string RechargeCreate = "api/Recharge/Create";
        public const string RechargeDelete = "api/Recharge/Delete";
        public const string RechargeUpdate = "api/Recharge/Update";

        public const string AppUserList = "api/AppUser/List";
        public const string AppUserGetById = "api/AppUser/GetById";
        public const string AppUserUpdateTokens = "api/AppUser/UpdateTokens";
        public const string AppUserUpdate = "api/AppUser/Update";

        public const string ProjectList = "api/Project/List";
        public const string ProjectCount = "api/Project/ProjectCount";
        public const string ProjectGetByURL = "api/Project/GetByURL";
        public const string ProjectUpdateStatus = "api/Project/UpdateStatus";
        public const string ProjectUpdatePageCount = "api/Project/UpdatePageCount";
        public const string GetProjectsForAnalysisStatus = "api/Project/GetProjectsForAnalysisStatus";
        public const string ProjectGetNew = "api/Project/GetNew";
        public const string ProjectGetById = "api/Project/GetById";
        public const string ProjectGetByName = "api/Project/GetByName";
        public const string ProjectIsExist = "api/Project/IsExist";
        public const string ProjectCreate = "api/Project/Create";
        public const string ProjectDelete = "api/Project/Delete";
        public const string ProjectUpdate = "api/Project/Update";
        public const string ProjectUpdateParent = "api/Project/UpdateParent";

        public const string CrawledCreate = "api/Crawled/Create";
        public const string CrawledUpdate = "api/Crawled/Update";
        public const string CrawledUpdateAnalysisStatus = "api/Crawled/UpdateAnalysisStatus";

        public const string ContentAnalysisCreate = "api/ContentAnalysis/Create";
        public const string ContentAnalysisUpdateMetaTagKeywords = "api/ContentAnalysis/UpdateMetaTagKeywords";
        public const string ContentAnalysisUpdateHeadings = "api/ContentAnalysis/UpdateHeadings";
        public const string ContentAnalysisUpdateKeywordFrequency = "api/ContentAnalysis/UpdateKeywordFrequency";
        public const string ContentAnalysisUpdateMetaDescription = "api/ContentAnalysis/UpdateMetaDescription";
        public const string ContentAnalysisUpdateTitle = "api/ContentAnalysis/UpdateTitle";

        public const string ModelTrainingDelete = "api/ModelTraining/FineTune";
        public const string OCRReadImageData = "api/Ocr/ReadImageData";
    }
}
