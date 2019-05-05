using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adz.User.CMS.Models
{
    public class CampaignModel
    {
        [Bind(Exclude = "Merchant")]
        public class Campaign
        {
            [ScaffoldColumn(false)]
            public int CampaignId { get; set; }

            [Required]
            [Display(Name = "Campaign Name")]
            public string Name { get; set; }

            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string Description { get; set; }
            public DateTime Create { get; set; }
            public DateTime Update { get; set; }
            public string LastAction { get; set; }
            public MerchantModel.Merchant Merchant { get; set; }

            public string SubImageId { get; set; }
            public string SubImageName { get; set; }
            [Display(Name = "Image")]
            public string SubImageUrl { get; set; }
            public string SubImageUrlLink { get; set; }

            [Display(Name = "URL")]
            [Required]
            [Url(ErrorMessage = "Invalid URL format (ex. http://www.google.com)")]
            public string LinkURL { get; set; }

            public List<Rule> Rules { get; set; }
            public List<Tag> PrimaryTags { get; set; }
            public List<Tag> SecondaryTags { get; set; }
            public int Views { get; set; }
            [Display(Name = "Last View")]
            public DateTime LastView { get; set; }
            public int Clicks { get; set; }
            [Display(Name = "Last Click")]
            public DateTime LastClick { get; set; }
        }

        public class Rule
        {
            public int RuleId { get; set; }
            public int Gender { get; set; }
            [Display(Name = "Number of View")]
            public int NumberOfView { get; set; }
            [Display(Name = "Start From")]
            public DateTime StartTime { get; set; }
            [Display(Name = "End At")]
            public DateTime EndTime { get; set; }
            [Display(Name = "Never End")]
            public Boolean NoEnd { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string TempLatLong { get; set; }
            public DateTime Create { get; set; }
            public DateTime Update { get; set; }
            public string LastAction { get; set; }
                        
            public bool Monday { get; set; }
            public bool Tuesday { get; set; }
            public bool Wednesday { get; set; }
            public bool Thursday { get; set; }
            public bool Friday { get; set; }
            public bool Saturday { get; set; }
            public bool Sunday { get; set; }

            public int AgeGroupStart { get; set; }
            public int AgeGroupEnd { get; set; }

            public ImageModel.Image Image { get; set; }
            public int ImageId { get; set; }
            public string ImageUrl { get; set; }

            public Campaign Campaign { get; set; }

            public List<ImageModel.Image> ExistingImages { get;set; }
        }
        
        public class CampaignHistory
        {
            public Campaign Campaign { get; set; }
            public UserModel.User User { get; set; }
            public DateTime DateTime { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Gender { get; set; }
            public string Action { get; set; }
        }

        public class CampaignHistoryGroup
        {
            public Campaign Campaign { get; set; }
            public int Views { get; set; }
            public int Clicks { get; set; }
            public DateTime LastViewTime { get; set; }
            public DateTime LastClickTime { get; set; }
        }
        
        public class CampaignHistoryGroupSummarize
        {
            public int Views { get; set; }
            public int Clicks { get; set; }
            public DateTime Date { get; set; }
            public string HourGroup { get; set; }
        }

        public class CampaignHistoryGroupByDate
        {
            public int Views { get; set; }
            public int Clicks { get; set; }
            public DateTime Date { get; set; }
            public bool isGetPoint { get; set; }

            public string Description { get; set; }
            public string Points { get; set; }
        }

        public class Tag
        {
            public int TagId { get; set; }
            [Required]
            [Display(Name = "Tag Name")]
            public string Name { get; set; }
            public DateTime Create { get; set; }
            public DateTime Update { get; set; }
            public string LastAction { get; set; }

            public bool Selected { get; set; }
        }
        
        public class CampaignHeatMap
        {
            public int CampaignId { get; set; }
            public List<CountryModel.Map> LatLng { get; set; }
        }

        public class CampaignChart
        {
            public int Views { get; set; }
            public int Clicks { get; set; }
            public DateTime Date { get; set; }
        }

        public class CampaignPieChart
        {
            public int Value { get; set; }
            public string Category { get; set; }
            public string Color { get; set; }
        }

        public class OverallChart
        {
            public decimal SensiteTotalUser { get; set; }
            public decimal AverageDaysActive { get; set; }
            public decimal AverageDailyViews { get; set; }
            public decimal AverageCTRinPercent { get; set; }
            public int XDaysActive { get; set; }
        }
        

        public class CampaignBarChart
        {
            public decimal Value { get; set; }
            public string Category { get; set; }
            public string Color { get; set; }
            public decimal Value2 { get; set; }
        }

    }
}